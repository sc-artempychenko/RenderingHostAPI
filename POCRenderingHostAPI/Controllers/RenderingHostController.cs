using Microsoft.AspNetCore.Mvc;
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
        private readonly IHostConfigurationProvider _hostConfigurationProvider;

        public RenderingHostController(IRenderingHostQueryRunnerService rhQueryRunner,
            ISiteQueryRunnerService siteQueryRunner,
            IRenderingHostRepository renderingHostRepository,
            IHostConfigurationProvider hostConfigurationProvider)
        {
            _rhQueryRunner = rhQueryRunner;
            _siteQueryRunner = siteQueryRunner;
            _renderingHostRepository = renderingHostRepository;
            _hostConfigurationProvider = hostConfigurationProvider;
        }

        [HttpPost("CreateRenderingHost")]
        public async Task<ActionResult> CreateRenderingHost([FromBody] CreateRenderingHostPayload renderingHostPayload)
        {
            var token = HttpContext.Request.Headers.Authorization;
            var client = _hostConfigurationProvider.GetGraphQlClient(renderingHostPayload.Host);
            _hostConfigurationProvider.SetJwtToken(token);
            _rhQueryRunner.SetGraphQlClient(client);
            _siteQueryRunner.SetGraphQlClient(client);
            GraphQLEndpointResponse<CreateItemResponse> createdRhDefinitionItemData;

            try
            {
                var siteData = await _siteQueryRunner.GetSiteRoot(renderingHostPayload.SiteName);
                if (siteData.Errors != null && siteData.Errors.Length > 0)
                {
                    return BadRequest(siteData.Errors);
                }

                createdRhDefinitionItemData = await _rhQueryRunner.CreateRenderingHostDefinitionItem(
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

                if (updatedSiteGroupingWithNewRenderingHost.Errors != null &&
                    updatedSiteGroupingWithNewRenderingHost.Errors.Length > 0)
                {
                    return BadRequest(updatedSiteGroupingWithNewRenderingHost.Errors);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }

            var renderingHostDto = PayloadToDto(renderingHostPayload);
            renderingHostDto.DefinitionItemId = createdRhDefinitionItemData.DTO.Item.ItemId;

            await _renderingHostRepository.AddRenderingHost(renderingHostDto);

            return Ok();
        }

        [HttpGet("GetAllRenderingHosts")]
        public async Task<ActionResult<List<RenderingHostWithWorkspaceDTO>>> GetAllRenderingHosts()
        {
            var result = await MatchRenderingHosts();

            return Ok(result);
        }

        [HttpGet("GetRenderingHost/{id}")]
        public async Task<ActionResult<RenderingHostWithWorkspaceDTO>> GetRenderingHostById(string id)
        {
            var parsingResult = int.TryParse(id, out var hostId);
            if (!parsingResult)
            {
                return BadRequest($"{id} is not valid id.");
            }

            var result = await MatchRenderingHostWithWorkspace(hostId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("RemoveRenderingHost/{id}")]
        public async Task<IActionResult> RemoveById(string id)
        {
            var result = int.TryParse(id, out var hostId);
            if (!result)
            {
                return BadRequest($"{id} is not valid id.");
            }

            var renderingHostToRemove = await _renderingHostRepository.GetRenderingHostById(hostId);

            if (renderingHostToRemove == null)
            {
                return BadRequest($"Rendering host with id {id} does not exist.");
            }

            var token = HttpContext.Request.Headers.Authorization;
            var client = _hostConfigurationProvider.GetGraphQlClient(renderingHostToRemove.Host);
            _hostConfigurationProvider.SetJwtToken(token);
            _rhQueryRunner.SetGraphQlClient(client);
            _siteQueryRunner.SetGraphQlClient(client);

            try
            {
                var removalResult = await _rhQueryRunner.RemoveRenderingHost(renderingHostToRemove.DefinitionItemId);
                if (removalResult.Errors != null && removalResult.Errors.Length > 0)
                {
                    return BadRequest(removalResult.Errors);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (HttpRequestException)
            {
                
            }

            await _renderingHostRepository.RemoveRenderingHost(renderingHostToRemove);

            return NoContent();
        }

        private RenderingHostDTO PayloadToDto(CreateRenderingHostPayload payload)
        {
            return new RenderingHostDTO
            {
                Name = payload.Name,
                SiteName = payload.SiteName,
                EnvironmentName = payload.EnvironmentName,
                RepositoryUrl = payload.RepositoryUrl,
                RenderingHostHostingMethod = payload.RenderingHostHostingMethod,
                RenderingHostUrl = payload.RenderingHostUrl,
                Host = payload.Host,
                WorkspaceUrl = payload.WorkspaceUrl,
                WorkspaceId = payload.WorkspaceId
            };
        }

        private async Task<RenderingHostWithWorkspaceDTO> MatchRenderingHostWithWorkspace(int id)
        {
            var renderingHostDto = await _renderingHostRepository.GetRenderingHostById(id);
            if (renderingHostDto == null)
            {
                return null;
            }

            var renderingHostWithWorkspacesDto =
                MapRenderingHostToRenderingHostDto(renderingHostDto);

            return renderingHostWithWorkspacesDto;
        }

        private async Task<List<RenderingHostWithWorkspaceDTO>> MatchRenderingHosts()
        {
            var fullRenderingHostInfos = new List<RenderingHostWithWorkspaceDTO>();
            var renderingHostDtos = await _renderingHostRepository.GetAllRenderingHosts();

            foreach (var renderingHostDto in renderingHostDtos)
            {
                var renderingHostWithWorkspacesDto =
                    MapRenderingHostToRenderingHostDto(renderingHostDto);

                fullRenderingHostInfos.Add(renderingHostWithWorkspacesDto);
            }

            return fullRenderingHostInfos;
        }

        private RenderingHostWithWorkspaceDTO MapRenderingHostToRenderingHostDto(RenderingHostDTO dto)
        {
            return new RenderingHostWithWorkspaceDTO()
            {
                Id = dto.Id,
                Name = dto.Name,
                RepositoryUrl = dto.RepositoryUrl,
                SiteName = dto.SiteName,
                EnvironmentName = dto.EnvironmentName,
                RenderingHostHostingMethod = dto.RenderingHostHostingMethod,
                Host = dto.Host,
                RenderingHostUrl = dto.RenderingHostUrl,
                WorkspaceUrl = dto.WorkspaceUrl,
                WorkspaceId = dto.WorkspaceId
            };
        }
    }
}
