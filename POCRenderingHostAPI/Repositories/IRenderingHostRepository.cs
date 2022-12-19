using POCRenderingHostAPI.Models.DTO;

namespace POCRenderingHostAPI.Repositories;

public interface IRenderingHostRepository
{
    Task AddRenderingHost(RenderingHostDTO renderingHost);
    Task<List<RenderingHostDTO>> GetAllRenderingHosts();
    Task<RenderingHostDTO> GetRenderingHostById(string id);
    Task RemoveRenderingHost(RenderingHostDTO renderingHost);
}