using GraphQL.Common.Response;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using POCRenderingHostAPI.Models;
using POCRenderingHostAPI.Models.DTO;
using POCRenderingHostAPI.Repositories;
using POCRenderingHostAPI.Services;

namespace POCRenderingHostAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RenderingHostController : ControllerBase
    {
        private readonly IRenderingHostQueryRunnerService _rhQueryRunner;
        private readonly ISiteQueryRunnerService _siteQueryRunner;
        private readonly IRenderingHostRepository _renderingHostRepository;
        private readonly HttpClient _httpClient = new();

        public RenderingHostController(IRenderingHostQueryRunnerService rhQueryRunner,
            ISiteQueryRunnerService siteQueryRunner,
            IRenderingHostRepository renderingHostRepository)
        {
            _rhQueryRunner = rhQueryRunner;
            _siteQueryRunner = siteQueryRunner;
            _renderingHostRepository = renderingHostRepository;
            _httpClient.BaseAddress = new Uri("https://sitecoreservicescloudidegitpodregistration.azurewebsites.net");
        }

        [HttpPost("CreateRenderingHost")]
        public async Task<ActionResult> CreateRenderingHost([FromBody] CreateRenderingHostPayload renderingHostPayload)
        {
            var rawJwtToken = Request.Headers[HeaderNames.Authorization];
            var jwtTokenResponse = new TokenResponse(rawJwtToken);
            
            var siteData = await _siteQueryRunner.GetSiteRoot(renderingHostPayload.SiteName);
            if (siteData.Errors != null && siteData.Errors.Length > 0)
            {
                return BadRequest(siteData.Errors);
            }

            var createdRhDefinitionItemData = await _rhQueryRunner.CreateRenderingHostDefinitionItem(
                renderingHostPayload.Name,
                renderingHostPayload.RenderingHostUrl, 
                renderingHostPayload.RenderingHostUrl, 
                renderingHostPayload.SiteName);

            if (createdRhDefinitionItemData.Errors != null && createdRhDefinitionItemData.Errors.Length > 0)
            {
                return BadRequest(createdRhDefinitionItemData.Errors);
            }

            var updatedSiteGroupingWithNewRenderingHost = await _rhQueryRunner.SwitchRenderingHostForSite(
                renderingHostPayload.Name,
                renderingHostPayload.SiteName,
                siteData.DTO.RootPath);

            if (updatedSiteGroupingWithNewRenderingHost.Errors != null && updatedSiteGroupingWithNewRenderingHost.Errors.Length > 0)
            {
                return BadRequest(updatedSiteGroupingWithNewRenderingHost.Errors);
            }

            var renderingHostDto = PayloadToDto(renderingHostPayload);
            renderingHostDto.DefinitionItemId = createdRhDefinitionItemData.DTO.Item.ItemId;

            await _renderingHostRepository.AddRenderingHost(renderingHostDto);

            return Ok();
        }

        [HttpGet("GetAllRenderingHosts")]
        public async Task<ActionResult<List<ShortRenderingHostInfo>>> GetAllRenderingHosts()
        {
            var result = await MatchRenderingHostsWithWorkspaces();

            return Ok(result);
        }

        [HttpGet("GetRenderingHost/{id}")]
        public async Task<ActionResult<RenderingHostWithWorkspaceDTO>> GetRenderingHostById(string id)
        {
            var result = await MatchRenderingHostWithWorkspace(id);

            return Ok(result);
        }

        [HttpDelete("RemoveAllRenderingHosts")]
        public async Task<IActionResult> RemoveAll()
        {
            var renderingHosts = await _renderingHostRepository.GetAllRenderingHosts();
            var errors = new List<GraphQLError>();
            foreach (var renderingHost in renderingHosts)
            {
                var removalResult = await _rhQueryRunner.RemoveRenderingHost(renderingHost.DefinitionItemId);
                if (removalResult.Errors != null && removalResult.Errors.Length > 0)
                {
                    errors.AddRange(removalResult.Errors);
                    continue;
                }

                await _renderingHostRepository.RemoveRenderingHost(renderingHost);
            }

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            return NoContent();
        }

        [HttpDelete("RemoveRenderingHost/{id}")]
        public async Task<IActionResult> RemoveById(string id)
        {
            var renderingHostToRemove = await _renderingHostRepository.GetRenderingHostById(id);

            if (renderingHostToRemove == null)
            {
                return BadRequest($"Rendering host with id {id} does not exist.");
            }

            var removalResult = await _rhQueryRunner.RemoveRenderingHost(renderingHostToRemove.DefinitionItemId);
            if (removalResult.Errors != null && removalResult.Errors.Length > 0)
            {
                return BadRequest(removalResult.Errors);
            }

            await _renderingHostRepository.RemoveRenderingHost(renderingHostToRemove);

            return NoContent();
        }

        private RenderingHostDTO PayloadToDto(CreateRenderingHostPayload payload)
        {
            return new RenderingHostDTO
            {
                RenderingHostId = payload.RenderingHostId,
                Name = payload.Name,
                SiteName = payload.SiteName,
                EnvironmentName = payload.EnvironmentName,
                PlatformTenantName = payload.PlatformTenantName,
                RepositoryUrl = payload.RepositoryUrl,
                SourceControlIntegrationName = payload.SourceControlIntegrationName,
                RenderingHostHostingMethod = payload.RenderingHostHostingMethod,
                RenderingHostUrl = payload.RenderingHostHostingMethod != HostingMethods.Gitpod ? payload.RenderingHostUrl : ""
            };
        }

        private string GetHostingMethod(HostingMethod hostingMethod)
        {
            if (hostingMethod.Gitpod)
            {
                return nameof(hostingMethod.Gitpod);
            }

            if (hostingMethod.Local)
            {
                return nameof(hostingMethod.Local);
            }

            return nameof(hostingMethod.External);
        }

        private async Task<RenderingHostWithWorkspaceDTO> MatchRenderingHostWithWorkspace(string id)
        {
            var renderingHostDto = await _renderingHostRepository.GetRenderingHostById(id);
            var workspace = await GetWorkspaceById(id);

            var renderingHostWithWorkspacesDto =
                MapRenderingHostDtoToRenderingHostWithWorkspaceDto(renderingHostDto);
            if (renderingHostDto.RenderingHostHostingMethod == HostingMethods.Gitpod)
            {
                renderingHostWithWorkspacesDto =
                    MapWorkspaceDtoToRenderingHostWithWorkspaceDto(workspace, renderingHostWithWorkspacesDto);
            }

            return renderingHostWithWorkspacesDto;
        }

        private async Task<Workspace> GetWorkspaceById(string id)
        {
            var result = await _httpClient.GetAsync($"api/v1/workspace/{id}");
            var content = await result.Content.ReadAsStringAsync();
            var workspace = JsonConvert.DeserializeObject<Workspace>(content);

            return workspace;
        }

        private async Task<List<ShortRenderingHostInfo>> MatchRenderingHostsWithWorkspaces()
        {
            var shortRenderingHostInfos = new List<ShortRenderingHostInfo>();
            var renderingHostDtos = await _renderingHostRepository.GetAllRenderingHosts();
            var workspaces = await GetAllWorkspaces();

            foreach (var renderingHostDto in renderingHostDtos)
            {
                var workspace = workspaces.FirstOrDefault(w => w.RenderingHostId == renderingHostDto.RenderingHostId);

                var renderingHostWithWorkspacesDto =
                    MapRenderingHostDtoToShortRenderingHostInfo(renderingHostDto);

                if (renderingHostDto.RenderingHostHostingMethod == HostingMethods.Gitpod)
                {
                    renderingHostWithWorkspacesDto =
                        MapWorkspaceDtoToShortRenderingHostInfo(workspace, renderingHostWithWorkspacesDto);
                }

                shortRenderingHostInfos.Add(renderingHostWithWorkspacesDto);
            }

            return shortRenderingHostInfos;
        }

        private async Task<List<Workspace>> GetAllWorkspaces()
        {
            var result = await _httpClient.GetAsync("api/v1/workspace");
            var content = await result.Content.ReadAsStringAsync();
            var workspaces = JsonConvert.DeserializeObject<List<Workspace>>(content);

            return workspaces;
        }

        private RenderingHostWithWorkspaceDTO MapRenderingHostDtoToRenderingHostWithWorkspaceDto(RenderingHostDTO dto)
        {
            return new RenderingHostWithWorkspaceDTO()
            {
                RenderingHostId = dto.RenderingHostId,
                Name = dto.Name,
                RepositoryUrl = dto.RepositoryUrl,
                SiteName = dto.SiteName,
                EnvironmentName = dto.EnvironmentName,
                PlatformTenantName = dto.PlatformTenantName,
                SourceControlIntegrationName = dto.SourceControlIntegrationName,
                RenderingHostUrl = dto.RenderingHostUrl
            };
        }

        private ShortRenderingHostInfo MapRenderingHostDtoToShortRenderingHostInfo(RenderingHostDTO dto)
        {
            return new ShortRenderingHostInfo()
            {
                RenderingHostId = dto.RenderingHostId,
                Name = dto.Name
            };
        }

        private RenderingHostWithWorkspaceDTO MapWorkspaceDtoToRenderingHostWithWorkspaceDto(Workspace workspace,
            RenderingHostWithWorkspaceDTO renderingHostWithWorkspaceDto)
        {
            renderingHostWithWorkspaceDto.RenderingHostUrl = string.IsNullOrEmpty(renderingHostWithWorkspaceDto.RenderingHostUrl) ? workspace.RenderingHostUrl : string.Empty;
            renderingHostWithWorkspaceDto.WordspaceUrl = workspace.WorkspaceUrl ?? string.Empty;
            renderingHostWithWorkspaceDto.WorkspaceId = workspace.WorkspaceId ?? string.Empty;
            renderingHostWithWorkspaceDto.Status = workspace.IsRenderingHostActive.ToString();

            return renderingHostWithWorkspaceDto;
        }

        private ShortRenderingHostInfo MapWorkspaceDtoToShortRenderingHostInfo(Workspace workspace,
            ShortRenderingHostInfo shortRenderingHostInfo)
        {
            shortRenderingHostInfo.Status = workspace.IsRenderingHostActive.ToString();
            return shortRenderingHostInfo;
        }
    }
}
