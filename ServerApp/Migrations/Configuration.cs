using System.Data.Entity.Migrations;
using ServerApp.Models;

namespace ServerApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MobileServerSyncDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MobileServerSyncDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
