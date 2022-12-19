using Microsoft.EntityFrameworkCore;
using POCRenderingHostAPI.Models.DTO;

namespace POCRenderingHostAPI.Data;
public class DataContext: DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<RenderingHostDTO> RenderingHosts { get; set; }
}
