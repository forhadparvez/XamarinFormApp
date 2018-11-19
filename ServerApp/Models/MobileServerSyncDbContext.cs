using System.Data.Entity;

namespace ServerApp.Models
{
    public class MobileServerSyncDbContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}