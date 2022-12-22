using Microsoft.EntityFrameworkCore;
using POCRenderingHostAPI.Data;
using POCRenderingHostAPI.Models.DTO;

namespace POCRenderingHostAPI.Repositories
{
    public class RenderingHostRepository : IRenderingHostRepository
    {
        private readonly DataContext _dataContext;

        public RenderingHostRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddRenderingHost(RenderingHostDTO renderingHost)
        {
            await _dataContext.RenderingHosts.AddAsync(renderingHost);

            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<RenderingHostDTO>> GetAllRenderingHosts()
        {
            return await _dataContext.RenderingHosts.ToListAsync();
        }

        public async Task<RenderingHostDTO> GetRenderingHostById(int id)
        {
            return await _dataContext.RenderingHosts.FirstOrDefaultAsync(host => host!.Id == id);
        }

        public async Task RemoveRenderingHost(RenderingHostDTO renderingHost)
        {
            if (renderingHost != null)
            {
                _dataContext.RenderingHosts.Remove(renderingHost);
            }
            
            await _dataContext.SaveChangesAsync();
        }
    }
}
