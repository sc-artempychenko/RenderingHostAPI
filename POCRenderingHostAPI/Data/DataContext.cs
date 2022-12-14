using Microsoft.EntityFrameworkCore;
using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
    }
}
