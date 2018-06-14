namespace Task2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version_1_3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Region", newName: "Regions");
            AddColumn("dbo.Customers", "FoudationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "FoudationDate");
            RenameTable(name: "dbo.Regions", newName: "Region");
        }
    }
}
