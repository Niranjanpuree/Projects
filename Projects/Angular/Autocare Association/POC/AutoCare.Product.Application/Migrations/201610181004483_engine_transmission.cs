namespace AutoCare.Product.Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class engine_transmission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aspiration",
                c => new
                    {
                        AspirationID = c.Int(nullable: false),
                        AspirationName = c.String(nullable: false, maxLength: 30, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AspirationID);
            
            CreateTable(
                "dbo.EngineConfig",
                c => new
                    {
                        EngineConfigID = c.Int(nullable: false),
                        EngineDesignationID = c.Int(nullable: false),
                        EngineVINID = c.Int(nullable: false),
                        ValvesID = c.Int(nullable: false),
                        EngineBaseID = c.Int(nullable: false),
                        FuelDeliveryConfigID = c.Int(nullable: false),
                        AspirationID = c.Int(nullable: false),
                        CylinderHeadTypeID = c.Int(nullable: false),
                        FuelTypeID = c.Int(nullable: false),
                        IgnitionSystemTypeID = c.Int(nullable: false),
                        EngineMfrID = c.Int(nullable: false),
                        EngineVersionID = c.Int(nullable: false),
                        PowerOutputID = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EngineConfigID)
                .ForeignKey("dbo.Aspiration", t => t.AspirationID)
                .ForeignKey("dbo.CylinderHeadType", t => t.CylinderHeadTypeID)
                .ForeignKey("dbo.EngineBase", t => t.EngineBaseID)
                .ForeignKey("dbo.EngineDesignation", t => t.EngineDesignationID)
                .ForeignKey("dbo.EngineVersion", t => t.EngineVersionID)
                .ForeignKey("dbo.EngineVIN", t => t.EngineVINID)
                .ForeignKey("dbo.FuelDeliveryConfig", t => t.FuelDeliveryConfigID)
                .ForeignKey("dbo.FuelType", t => t.FuelTypeID)
                .ForeignKey("dbo.IgnitionSystemType", t => t.IgnitionSystemTypeID)
                .ForeignKey("dbo.Mfr", t => t.EngineMfrID)
                .ForeignKey("dbo.PowerOutput", t => t.PowerOutputID)
                .ForeignKey("dbo.Valves", t => t.ValvesID)
                .Index(t => t.EngineDesignationID)
                .Index(t => t.EngineVINID)
                .Index(t => t.ValvesID)
                .Index(t => t.EngineBaseID)
                .Index(t => t.FuelDeliveryConfigID)
                .Index(t => t.AspirationID)
                .Index(t => t.CylinderHeadTypeID)
                .Index(t => t.FuelTypeID)
                .Index(t => t.IgnitionSystemTypeID)
                .Index(t => t.EngineMfrID)
                .Index(t => t.EngineVersionID)
                .Index(t => t.PowerOutputID);
            
            CreateTable(
                "dbo.CylinderHeadType",
                c => new
                    {
                        CylinderHeadTypeID = c.Int(nullable: false),
                        CylinderHeadTypeName = c.String(nullable: false, maxLength: 30, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CylinderHeadTypeID);
            
            CreateTable(
                "dbo.EngineBase",
                c => new
                    {
                        EngineBaseID = c.Int(nullable: false),
                        Liter = c.String(nullable: false, maxLength: 6),
                        CC = c.String(nullable: false, maxLength: 8),
                        CID = c.String(maxLength: 7),
                        Cylinders = c.String(nullable: false, maxLength: 2, fixedLength: true, unicode: false),
                        BlockType = c.String(nullable: false, maxLength: 2, fixedLength: true, unicode: false),
                        EngBoreIn = c.String(nullable: false, maxLength: 10),
                        EngBoreMetric = c.String(nullable: false, maxLength: 10),
                        EngStrokeIn = c.String(nullable: false, maxLength: 10),
                        EngStrokeMetric = c.String(nullable: false, maxLength: 10),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EngineBaseID);
            
            CreateTable(
                "dbo.EngineDesignation",
                c => new
                    {
                        EngineDesignationID = c.Int(nullable: false),
                        EngineDesignationName = c.String(nullable: false, maxLength: 30, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EngineDesignationID);
            
            CreateTable(
                "dbo.EngineVersion",
                c => new
                    {
                        EngineVersionID = c.Int(nullable: false),
                        EngineVersion = c.String(nullable: false, maxLength: 20, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EngineVersionID);
            
            CreateTable(
                "dbo.EngineVIN",
                c => new
                    {
                        EngineVINID = c.Int(nullable: false),
                        EngineVINName = c.String(nullable: false, maxLength: 5, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EngineVINID);
            
            CreateTable(
                "dbo.FuelDeliveryConfig",
                c => new
                    {
                        FuelDeliveryConfigID = c.Int(nullable: false),
                        FuelDeliveryTypeID = c.Int(nullable: false),
                        FuelDeliverySubTypeID = c.Int(nullable: false),
                        FuelSystemControlTypeID = c.Int(nullable: false),
                        FuelSystemDesignID = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FuelDeliveryConfigID)
                .ForeignKey("dbo.FuelDeliverySubType", t => t.FuelDeliverySubTypeID)
                .ForeignKey("dbo.FuelDeliveryType", t => t.FuelDeliveryTypeID)
                .ForeignKey("dbo.FuelSystemControlType", t => t.FuelSystemControlTypeID)
                .ForeignKey("dbo.FuelSystemDesign", t => t.FuelSystemDesignID)
                .Index(t => t.FuelDeliveryTypeID)
                .Index(t => t.FuelDeliverySubTypeID)
                .Index(t => t.FuelSystemControlTypeID)
                .Index(t => t.FuelSystemDesignID);
            
            CreateTable(
                "dbo.FuelDeliverySubType",
                c => new
                    {
                        FuelDeliverySubTypeID = c.Int(nullable: false),
                        FuelDeliverySubTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ChangeRequestId = c.Long(),
                        FuelDeliveryConfigCount = c.Int(nullable: false),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FuelDeliverySubTypeID);
            
            CreateTable(
                "dbo.FuelDeliveryType",
                c => new
                    {
                        FuelDeliveryTypeID = c.Int(nullable: false),
                        FuelDeliveryTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FuelDeliveryTypeID);
            
            CreateTable(
                "dbo.FuelSystemControlType",
                c => new
                    {
                        FuelSystemControlTypeID = c.Int(nullable: false),
                        FuelSystemControlTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FuelSystemControlTypeID);
            
            CreateTable(
                "dbo.FuelSystemDesign",
                c => new
                    {
                        FuelSystemDesignID = c.Int(nullable: false),
                        FuelSystemDesignName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FuelSystemDesignID);
            
            CreateTable(
                "dbo.FuelType",
                c => new
                    {
                        FuelTypeID = c.Int(nullable: false),
                        FuelTypeName = c.String(nullable: false, maxLength: 30, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FuelTypeID);
            
            CreateTable(
                "dbo.IgnitionSystemType",
                c => new
                    {
                        IgnitionSystemTypeID = c.Int(nullable: false),
                        IgnitionSystemTypeName = c.String(nullable: false, maxLength: 30, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IgnitionSystemTypeID);
            
            CreateTable(
                "dbo.Mfr",
                c => new
                    {
                        MfrID = c.Int(nullable: false),
                        MfrName = c.String(nullable: false, maxLength: 30, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MfrID);
            
            CreateTable(
                "dbo.Transmission",
                c => new
                    {
                        TransmissionID = c.Int(nullable: false),
                        TransmissionBaseID = c.Int(nullable: false),
                        TransmissionMfrCodeID = c.Int(nullable: false),
                        TransmissionElecControlledID = c.Int(nullable: false),
                        TransmissionMfrID = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransmissionID)
                .ForeignKey("dbo.ElecControlled", t => t.TransmissionElecControlledID)
                .ForeignKey("dbo.Mfr", t => t.TransmissionMfrID)
                .ForeignKey("dbo.TransmissionBase", t => t.TransmissionBaseID)
                .ForeignKey("dbo.TransmissionMfrCode", t => t.TransmissionMfrCodeID)
                .Index(t => t.TransmissionBaseID)
                .Index(t => t.TransmissionMfrCodeID)
                .Index(t => t.TransmissionElecControlledID)
                .Index(t => t.TransmissionMfrID);
            
            CreateTable(
                "dbo.ElecControlled",
                c => new
                    {
                        ElecControlledID = c.Int(nullable: false),
                        ElecControlled = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ElecControlledID);
            
            CreateTable(
                "dbo.TransmissionBase",
                c => new
                    {
                        TransmissionBaseID = c.Int(nullable: false),
                        TransmissionTypeID = c.Int(nullable: false),
                        TransmissionNumSpeedsID = c.Int(nullable: false),
                        TransmissionControlTypeID = c.Int(nullable: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransmissionBaseID)
                .ForeignKey("dbo.TransmissionControlType", t => t.TransmissionControlTypeID)
                .ForeignKey("dbo.TransmissionNumSpeeds", t => t.TransmissionNumSpeedsID)
                .ForeignKey("dbo.TransmissionType", t => t.TransmissionTypeID)
                .Index(t => t.TransmissionTypeID)
                .Index(t => t.TransmissionNumSpeedsID)
                .Index(t => t.TransmissionControlTypeID);
            
            CreateTable(
                "dbo.TransmissionControlType",
                c => new
                    {
                        TransmissionControlTypeID = c.Int(nullable: false),
                        TransmissionControlTypeName = c.String(nullable: false, maxLength: 30, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransmissionControlTypeID);
            
            CreateTable(
                "dbo.TransmissionNumSpeeds",
                c => new
                    {
                        TransmissionNumSpeedsID = c.Int(nullable: false),
                        TransmissionNumSpeeds = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransmissionNumSpeedsID);
            
            CreateTable(
                "dbo.TransmissionType",
                c => new
                    {
                        TransmissionTypeID = c.Int(nullable: false),
                        TransmissionTypeName = c.String(nullable: false, maxLength: 30, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransmissionTypeID);
            
            CreateTable(
                "dbo.TransmissionMfrCode",
                c => new
                    {
                        TransmissionMfrCodeID = c.Int(nullable: false),
                        TransmissionMfrCode = c.String(nullable: false, maxLength: 30, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransmissionMfrCodeID);
            
            CreateTable(
                "dbo.VehicleToTransmission",
                c => new
                    {
                        VehicleToTransmissionID = c.Int(nullable: false),
                        VehicleID = c.Int(nullable: false),
                        TransmissionID = c.Int(nullable: false),
                        Source = c.String(maxLength: 10),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleToTransmissionID)
                .ForeignKey("dbo.Transmission", t => t.TransmissionID)
                .ForeignKey("dbo.Vehicle", t => t.VehicleID)
                .Index(t => t.VehicleID)
                .Index(t => t.TransmissionID);
            
            CreateTable(
                "dbo.VehicleToEngineConfig",
                c => new
                    {
                        VehicleToEngineConfigID = c.Int(nullable: false),
                        VehicleID = c.Int(nullable: false),
                        EngineConfigID = c.Int(nullable: false),
                        Source = c.String(maxLength: 10),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VehicleToEngineConfigID)
                .ForeignKey("dbo.EngineConfig", t => t.EngineConfigID)
                .ForeignKey("dbo.Vehicle", t => t.VehicleID)
                .Index(t => t.VehicleID)
                .Index(t => t.EngineConfigID);
            
            CreateTable(
                "dbo.PowerOutput",
                c => new
                    {
                        PowerOutputID = c.Int(nullable: false, identity: true),
                        HorsePower = c.String(nullable: false, maxLength: 10, unicode: false),
                        KilowattPower = c.String(nullable: false, maxLength: 10, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PowerOutputID);
            
            CreateTable(
                "dbo.Valves",
                c => new
                    {
                        ValvesID = c.Int(nullable: false),
                        ValvesPerEngine = c.String(nullable: false, maxLength: 3, fixedLength: true, unicode: false),
                        ChangeRequestId = c.Long(),
                        InsertDate = c.DateTime(),
                        LastUpdateDate = c.DateTime(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ValvesID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EngineConfig", "ValvesID", "dbo.Valves");
            DropForeignKey("dbo.EngineConfig", "PowerOutputID", "dbo.PowerOutput");
            DropForeignKey("dbo.EngineConfig", "EngineMfrID", "dbo.Mfr");
            DropForeignKey("dbo.VehicleToTransmission", "VehicleID", "dbo.Vehicle");
            DropForeignKey("dbo.VehicleToEngineConfig", "VehicleID", "dbo.Vehicle");
            DropForeignKey("dbo.VehicleToEngineConfig", "EngineConfigID", "dbo.EngineConfig");
            DropForeignKey("dbo.VehicleToTransmission", "TransmissionID", "dbo.Transmission");
            DropForeignKey("dbo.Transmission", "TransmissionMfrCodeID", "dbo.TransmissionMfrCode");
            DropForeignKey("dbo.Transmission", "TransmissionBaseID", "dbo.TransmissionBase");
            DropForeignKey("dbo.TransmissionBase", "TransmissionTypeID", "dbo.TransmissionType");
            DropForeignKey("dbo.TransmissionBase", "TransmissionNumSpeedsID", "dbo.TransmissionNumSpeeds");
            DropForeignKey("dbo.TransmissionBase", "TransmissionControlTypeID", "dbo.TransmissionControlType");
            DropForeignKey("dbo.Transmission", "TransmissionMfrID", "dbo.Mfr");
            DropForeignKey("dbo.Transmission", "TransmissionElecControlledID", "dbo.ElecControlled");
            DropForeignKey("dbo.EngineConfig", "IgnitionSystemTypeID", "dbo.IgnitionSystemType");
            DropForeignKey("dbo.EngineConfig", "FuelTypeID", "dbo.FuelType");
            DropForeignKey("dbo.EngineConfig", "FuelDeliveryConfigID", "dbo.FuelDeliveryConfig");
            DropForeignKey("dbo.FuelDeliveryConfig", "FuelSystemDesignID", "dbo.FuelSystemDesign");
            DropForeignKey("dbo.FuelDeliveryConfig", "FuelSystemControlTypeID", "dbo.FuelSystemControlType");
            DropForeignKey("dbo.FuelDeliveryConfig", "FuelDeliveryTypeID", "dbo.FuelDeliveryType");
            DropForeignKey("dbo.FuelDeliveryConfig", "FuelDeliverySubTypeID", "dbo.FuelDeliverySubType");
            DropForeignKey("dbo.EngineConfig", "EngineVINID", "dbo.EngineVIN");
            DropForeignKey("dbo.EngineConfig", "EngineVersionID", "dbo.EngineVersion");
            DropForeignKey("dbo.EngineConfig", "EngineDesignationID", "dbo.EngineDesignation");
            DropForeignKey("dbo.EngineConfig", "EngineBaseID", "dbo.EngineBase");
            DropForeignKey("dbo.EngineConfig", "CylinderHeadTypeID", "dbo.CylinderHeadType");
            DropForeignKey("dbo.EngineConfig", "AspirationID", "dbo.Aspiration");
            DropIndex("dbo.VehicleToEngineConfig", new[] { "EngineConfigID" });
            DropIndex("dbo.VehicleToEngineConfig", new[] { "VehicleID" });
            DropIndex("dbo.VehicleToTransmission", new[] { "TransmissionID" });
            DropIndex("dbo.VehicleToTransmission", new[] { "VehicleID" });
            DropIndex("dbo.TransmissionBase", new[] { "TransmissionControlTypeID" });
            DropIndex("dbo.TransmissionBase", new[] { "TransmissionNumSpeedsID" });
            DropIndex("dbo.TransmissionBase", new[] { "TransmissionTypeID" });
            DropIndex("dbo.Transmission", new[] { "TransmissionMfrID" });
            DropIndex("dbo.Transmission", new[] { "TransmissionElecControlledID" });
            DropIndex("dbo.Transmission", new[] { "TransmissionMfrCodeID" });
            DropIndex("dbo.Transmission", new[] { "TransmissionBaseID" });
            DropIndex("dbo.FuelDeliveryConfig", new[] { "FuelSystemDesignID" });
            DropIndex("dbo.FuelDeliveryConfig", new[] { "FuelSystemControlTypeID" });
            DropIndex("dbo.FuelDeliveryConfig", new[] { "FuelDeliverySubTypeID" });
            DropIndex("dbo.FuelDeliveryConfig", new[] { "FuelDeliveryTypeID" });
            DropIndex("dbo.EngineConfig", new[] { "PowerOutputID" });
            DropIndex("dbo.EngineConfig", new[] { "EngineVersionID" });
            DropIndex("dbo.EngineConfig", new[] { "EngineMfrID" });
            DropIndex("dbo.EngineConfig", new[] { "IgnitionSystemTypeID" });
            DropIndex("dbo.EngineConfig", new[] { "FuelTypeID" });
            DropIndex("dbo.EngineConfig", new[] { "CylinderHeadTypeID" });
            DropIndex("dbo.EngineConfig", new[] { "AspirationID" });
            DropIndex("dbo.EngineConfig", new[] { "FuelDeliveryConfigID" });
            DropIndex("dbo.EngineConfig", new[] { "EngineBaseID" });
            DropIndex("dbo.EngineConfig", new[] { "ValvesID" });
            DropIndex("dbo.EngineConfig", new[] { "EngineVINID" });
            DropIndex("dbo.EngineConfig", new[] { "EngineDesignationID" });
            DropTable("dbo.Valves");
            DropTable("dbo.PowerOutput");
            DropTable("dbo.VehicleToEngineConfig");
            DropTable("dbo.VehicleToTransmission");
            DropTable("dbo.TransmissionMfrCode");
            DropTable("dbo.TransmissionType");
            DropTable("dbo.TransmissionNumSpeeds");
            DropTable("dbo.TransmissionControlType");
            DropTable("dbo.TransmissionBase");
            DropTable("dbo.ElecControlled");
            DropTable("dbo.Transmission");
            DropTable("dbo.Mfr");
            DropTable("dbo.IgnitionSystemType");
            DropTable("dbo.FuelType");
            DropTable("dbo.FuelSystemDesign");
            DropTable("dbo.FuelSystemControlType");
            DropTable("dbo.FuelDeliveryType");
            DropTable("dbo.FuelDeliverySubType");
            DropTable("dbo.FuelDeliveryConfig");
            DropTable("dbo.EngineVIN");
            DropTable("dbo.EngineVersion");
            DropTable("dbo.EngineDesignation");
            DropTable("dbo.EngineBase");
            DropTable("dbo.CylinderHeadType");
            DropTable("dbo.EngineConfig");
            DropTable("dbo.Aspiration");
        }
    }
}
