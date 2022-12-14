using GraphQL.Common.Response;
using Microsoft.AspNetCore.Mvc;
using POCRenderingHostAPI.Models;
using POCRenderingHostAPI.Repositories;
using POCRenderingHostAPI.Services;

namespace POCRenderingHostAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RenderingHostController : ControllerBase
    {
        private readonly IRenderingHostQueryRunnerService _rhQueryRunner;
        private readonly ISiteQueryRunnerService _siteQueryRunner;
        private readonly IItemRepository _itemRepository;

        public RenderingHostController(IRenderingHostQueryRunnerService rhQueryRunner,
            ISiteQueryRunnerService siteQueryRunner,
            IItemRepository itemRepository)
        {
            _rhQueryRunner = rhQueryRunner;
            _siteQueryRunner = siteQueryRunner;
            _itemRepository = itemRepository;
        }

        [HttpPost("CreateRenderingHostAndUpdateSiteGrouping")]
        public async Task<IActionResult> CreateAndUpdate([FromBody] RenderingHostDefinitionItem definitionItem)
        {
            var siteData = await _siteQueryRunner.GetSiteRoot(definitionItem.AppName);
            if (siteData.Errors != null && siteData.Errors.Length > 0)
            {
                return BadRequest(siteData.Errors);
            }

            var createdRhDefinitionItemData = await _rhQueryRunner.CreateRenderingHostDefinitionItem(
                definitionItem.Name, 
                definitionItem.ServerSideRenderingEngineEndpointUrl, 
                definitionItem.ServerSideRenderingEngineApplicationUrl, 
                definitionItem.AppName);

            if (createdRhDefinitionItemData.Errors != null && createdRhDefinitionItemData.Errors.Length > 0)
            {
                return BadRequest(createdRhDefinitionItemData.Errors);
            }

            var updatedSiteGroupingWithNewRenderingHost = await _rhQueryRunner.SwitchRenderingHostForSite(
                definitionItem.Name, 
                definitionItem.AppName, 
                siteData.DTO.RootPath);

            if (updatedSiteGroupingWithNewRenderingHost.Errors != null && updatedSiteGroupingWithNewRenderingHost.Errors.Length > 0)
            {
                return BadRequest(updatedSiteGroupingWithNewRenderingHost.Errors);
            }
            await _itemRepository.AddItem(createdRhDefinitionItemData.DTO.Item.ItemId, createdRhDefinitionItemData.DTO.Item.Name);

            return Ok("Site grouping updated successfully with newly created rendering host");
        }

        [HttpPost("CreateRenderingHost")]
        public async Task<IActionResult> Create([FromBody] RenderingHostDefinitionItem definitionItem)
        {
            var createdRhDefinitionItemData = await _rhQueryRunner.CreateRenderingHostDefinitionItem(
                definitionItem.Name,
                definitionItem.ServerSideRenderingEngineEndpointUrl,
                definitionItem.ServerSideRenderingEngineApplicationUrl,
                definitionItem.AppName);

            if (createdRhDefinitionItemData.Errors != null && createdRhDefinitionItemData.Errors.Length > 0)
            {
                return BadRequest(createdRhDefinitionItemData.Errors);
            }

            await _itemRepository.AddItem(createdRhDefinitionItemData.DTO.Item.ItemId, createdRhDefinitionItemData.DTO.Item.Name);

            return Ok($"Rendering host with name {definitionItem.Name} created successfully.");
        }

        [HttpPut("UpdateSiteGroupingWithRenderingHost")]
        public async Task<IActionResult> Update([FromBody] SiteGrouping siteGrouping)
        {
            var siteData = await _siteQueryRunner.GetSiteRoot(siteGrouping.AppName);
            if (siteData.Errors != null && siteData.Errors.Length > 0)
            {
                return BadRequest(siteData.Errors);
            }

            var updatedSiteGroupingWithNewRenderingHost = await _rhQueryRunner.SwitchRenderingHostForSite(siteGrouping.RenderingHost, siteGrouping.AppName, siteData.DTO.RootPath);
            if (updatedSiteGroupingWithNewRenderingHost.Errors != null && updatedSiteGroupingWithNewRenderingHost.Errors.Length > 0)
            {
                return BadRequest(updatedSiteGroupingWithNewRenderingHost.Errors);
            }

            return Ok("Site grouping updated successfully with newly created rendering host");
        }

        [HttpDelete("RemoveAllCreatedRenderingHosts")]
        public async Task<IActionResult> RemoveAll()
        {
            var items = _itemRepository.GetItems();
            var errors = new List<GraphQLError>();
            foreach (var item in items)
            {
                var removalResult = await _rhQueryRunner.RemoveRenderingHost(item.Id);
                if (removalResult.Errors != null && removalResult.Errors.Length > 0)
                {
                    errors.AddRange(removalResult.Errors);
                    continue;
                }

                await _itemRepository.RemoveItem(item);
            }

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            return Ok("Removal is successful.");
        }

        [HttpDelete("RemoveRenderingHostById")]
        public async Task<IActionResult> RemoveById([FromBody] string id)
        {
            var removalResult = await _rhQueryRunner.RemoveRenderingHost(id);
            if (removalResult.Errors != null && removalResult.Errors.Length > 0)
            {
                return BadRequest(removalResult.Errors);
            }

            var itemToRemove = await _itemRepository.GetItemById(id);
            await _itemRepository.RemoveItem(itemToRemove);

            return Ok($"Removal of item with id {id} is successful.");
        }
    }
}
