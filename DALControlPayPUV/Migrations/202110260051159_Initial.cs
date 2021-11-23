namespace DALControlPayPUV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Childs_OZPEV",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        DocumentPayId = c.Long(nullable: false),
                        Surname = c.String(maxLength: 150, storeType: "nvarchar"),
                        Name = c.String(maxLength: 150, storeType: "nvarchar"),
                        Patr = c.String(maxLength: 150, storeType: "nvarchar"),
                        act_Data = c.DateTime(precision: 0),
                        act_Number = c.String(maxLength: 50, storeType: "nvarchar"),
                        act_NameZaks = c.String(maxLength: 1000, storeType: "nvarchar"),
                        DateBirthday = c.DateTime(precision: 0),
                        SNILS = c.String(maxLength: 20, storeType: "nvarchar"),
                        DateStartPay = c.DateTime(precision: 0),
                        DateEndPay = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DocumentPays", t => t.DocumentPayId, cascadeDelete: true)
                .Index(t => t.DocumentPayId);
            
            CreateTable(
                "dbo.DocumentPays",
                c => new
                    {
                        DocumentPayId = c.Long(nullable: false, identity: true),
                        StatementId = c.Long(nullable: false),
                        DocumentName = c.String(maxLength: 150, storeType: "nvarchar"),
                        TypePay = c.String(maxLength: 20, storeType: "nvarchar"),
                        z_Surname = c.String(maxLength: 150, storeType: "nvarchar"),
                        z_Name = c.String(maxLength: 150, storeType: "nvarchar"),
                        z_Patr = c.String(maxLength: 150, storeType: "nvarchar"),
                        z_SNILS = c.String(maxLength: 20, storeType: "nvarchar"),
                        ud_Type = c.String(maxLength: 100, storeType: "nvarchar"),
                        ud_Seriya = c.String(maxLength: 30, storeType: "nvarchar"),
                        ud_Number = c.String(maxLength: 50, storeType: "nvarchar"),
                        ud_DataVidachi = c.DateTime(precision: 0),
                        ud_Vidan = c.String(maxLength: 200, storeType: "nvarchar"),
                        ud_Kod = c.String(maxLength: 20, storeType: "nvarchar"),
                        Address = c.String(maxLength: 1000, storeType: "nvarchar"),
                        Phone = c.String(maxLength: 20, storeType: "nvarchar"),
                        Email = c.String(maxLength: 100, storeType: "nvarchar"),
                        DateFirstPay = c.DateTime(precision: 0),
                        DateLastPay = c.DateTime(precision: 0),
                        Operation = c.String(maxLength: 20, storeType: "nvarchar"),
                        AmountPay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NameDeliveryOrg = c.String(maxLength: 1000, storeType: "nvarchar"),
                        BIKDeliveryOrg = c.String(maxLength: 20, storeType: "nvarchar"),
                        KCSchetDeliveryOrg = c.String(maxLength: 100, storeType: "nvarchar"),
                        p_Surname = c.String(maxLength: 150, storeType: "nvarchar"),
                        p_Name = c.String(maxLength: 150, storeType: "nvarchar"),
                        p_Patr = c.String(maxLength: 150, storeType: "nvarchar"),
                        p_AccountNumber = c.String(maxLength: 50, storeType: "nvarchar"),
                        GUID = c.String(maxLength: 100, storeType: "nvarchar"),
                        GlobalID = c.String(maxLength: 100, storeType: "nvarchar"),
                        RegNumber = c.String(maxLength: 100, storeType: "nvarchar"),
                        DateStartPay = c.DateTime(precision: 0),
                        DateEndPay = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.DocumentPayId)
                .ForeignKey("dbo.Statements", t => t.StatementId, cascadeDelete: true)
                .Index(t => t.StatementId);
            
            CreateTable(
                "dbo.Statements",
                c => new
                    {
                        StatementId = c.Long(nullable: false, identity: true),
                        PersonId = c.Long(nullable: false),
                        GUID = c.String(maxLength: 50, storeType: "nvarchar"),
                        RegNumber = c.String(maxLength: 50, storeType: "nvarchar"),
                        Vid_Pay = c.String(maxLength: 200, storeType: "nvarchar"),
                        Osn_Pay = c.String(maxLength: 100, storeType: "nvarchar"),
                        DateInnings = c.DateTime(precision: 0),
                        DateRegistration = c.DateTime(precision: 0),
                        Status = c.String(maxLength: 100, storeType: "nvarchar"),
                        DateResolved = c.DateTime(precision: 0),
                        TypeResolved = c.String(maxLength: 100, storeType: "nvarchar"),
                        RejectionReason = c.String(maxLength: 1200, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.StatementId)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.DocumentPUVs",
                c => new
                    {
                        DocumentPUVId = c.Long(nullable: false, identity: true),
                        StatementId = c.Long(nullable: false),
                        DocumentPUVName = c.String(maxLength: 150, storeType: "nvarchar"),
                        DateLoaded = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.DocumentPUVId)
                .ForeignKey("dbo.Statements", t => t.StatementId, cascadeDelete: true)
                .Index(t => t.StatementId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Long(nullable: false, identity: true),
                        SNILS = c.String(maxLength: 20, storeType: "nvarchar"),
                        Surname = c.String(maxLength: 150, storeType: "nvarchar"),
                        Name = c.String(maxLength: 150, storeType: "nvarchar"),
                        Patr = c.String(maxLength: 150, storeType: "nvarchar"),
                        DateBirthday = c.DateTime(precision: 0),
                        PhoneNumber = c.String(maxLength: 20, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Statements", "PersonId", "dbo.People");
            DropForeignKey("dbo.DocumentPUVs", "StatementId", "dbo.Statements");
            DropForeignKey("dbo.DocumentPays", "StatementId", "dbo.Statements");
            DropForeignKey("dbo.Childs_OZPEV", "DocumentPayId", "dbo.DocumentPays");
            DropIndex("dbo.DocumentPUVs", new[] { "StatementId" });
            DropIndex("dbo.Statements", new[] { "PersonId" });
            DropIndex("dbo.DocumentPays", new[] { "StatementId" });
            DropIndex("dbo.Childs_OZPEV", new[] { "DocumentPayId" });
            DropTable("dbo.People");
            DropTable("dbo.DocumentPUVs");
            DropTable("dbo.Statements");
            DropTable("dbo.DocumentPays");
            DropTable("dbo.Childs_OZPEV");
        }
    }
}
