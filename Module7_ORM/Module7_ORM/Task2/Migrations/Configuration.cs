namespace Task2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Task2.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<Task2.Entities.NorthwindDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Task2.Entities.NorthwindDB context)
        {
            context.Categories.AddOrUpdate(
                c => c.CategoryID,
                new Category { CategoryID = 1, CategoryName = "Category1" },
                new Category { CategoryID = 2, CategoryName = "Category2" });

            context.Regions.AddOrUpdate(
                r => r.RegionID,
                new Region { RegionID = 1, RegionDescription = "Region1" },
                new Region { RegionID = 2, RegionDescription = "Region2" });

            context.Territories.AddOrUpdate(
                t => t.TerritoryID,
                new Territory { TerritoryID = "T1", TerritoryDescription = "Territory1", RegionID = 1 },
                new Territory { TerritoryID = "T2", TerritoryDescription = "Territory2", RegionID = 1 });
            context.SaveChanges();
        }
    }
}
