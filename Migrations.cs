using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Orchard.ProjectManagement {
    public class Migrations : DataMigrationImpl {

        public int Create() {
			// Creating table Orch_Orchard_ProjectManagement_TaskPartRecord
            //SchemaBuilder.DropTable("TaskPartRecord");
			SchemaBuilder.CreateTable("TaskMgmtPartRecord", table => table
				.ContentPartRecord()
				.Column("TaskCode", DbType.String)
				.Column("Status", DbType.Int32)
				.Column("Duration", DbType.Double)
				.Column("StartIsMilestone", DbType.Boolean)
				.Column("EndIsMilestone", DbType.Boolean)
				.Column("Progress", DbType.Double)
				.Column("Dependancy", DbType.String)
				.Column("Level", DbType.Int32)
				.Column("ProjStartDate", DbType.DateTime)
				.Column("ProjEndDate", DbType.DateTime)
				.Column("HasChild", DbType.Boolean)
				.Column("ParentTaskId", DbType.Int32)
				.Column("ProjectId", DbType.Int32)
			);



            return 1;
        }
        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("ProjectTask", builder => builder
                       .DisplayedAs("Project Management")
                       .WithPart("TaskPart")
                       .WithPart("CommonPart")
                       .WithPart("TitlePart")
                       .WithPart("BodyPart")
                       .Creatable()
                       .Draftable());
            return 2;

        }

        public int UpdateFrom2()
        {
            SchemaBuilder.CreateTable("TaskMgmtPartRecord", table => table
                .ContentPartRecord()
                .Column("TaskCode", DbType.String)
                .Column("Status", DbType.Int32)
                .Column("Duration", DbType.Double)
                .Column("StartIsMilestone", DbType.Boolean)
                .Column("EndIsMilestone", DbType.Boolean)
                .Column("Progress", DbType.Double)
                .Column("Dependancy", DbType.String)
                .Column("Level", DbType.Int32)
                .Column("ProjStartDate", DbType.DateTime)
                .Column("ProjEndDate", DbType.DateTime)
                .Column("HasChild", DbType.Boolean)
                .Column("ParentTaskId", DbType.Int32)
                .Column("ProjectId", DbType.Int32)
            );
            return 3;
        }
        public int UpdateFrom3()
        {
            SchemaBuilder.DropTable("TaskPartRecord");
            return 4;
        }
        public int UpdateFrom4()
        {
            ContentDefinitionManager.AlterTypeDefinition("ProjectTask",builder=>builder
                .RemovePart("TaskPart")
                .WithPart("TaskMgmtPart")
                
                );
           

            return 5;
        }
        public int UpdateFrom5()
        {
            ContentDefinitionManager.DeleteTypeDefinition("ProjectTask");
            ContentDefinitionManager.AlterTypeDefinition("ProjectTaskMgmt", builder => builder
                       .DisplayedAs("ProjectManagement")
                       .WithPart("TaskMgmtPart")
                       .WithPart("CommonPart")
                       .WithPart("TitlePart")
                       .WithPart("BodyPart")
                       .Creatable()
                       .Draftable());
            return 6;
        }
        public int UpdateFrom6()
        {
            SchemaBuilder.CreateTable("TaskAssigPartRecord", builder => builder
                .Column("TaskId", DbType.Int32)
                .Column("ResourceId", DbType.Int32)
                .Column("RoleId", DbType.Int32)
                .Column("Efforts", DbType.Double));

            ContentDefinitionManager.AlterTypeDefinition("ProjectTask", builder => builder
                .WithPart("TaskAssignmentPart"));
            return 7;
        }
        public int UpdateFrom7()
        {
            
            ContentDefinitionManager.AlterTypeDefinition("ProjectTask", builder => builder
                .RemovePart("TaskAssignmentPart"));
            return 8;
        }

        public int UpdateFrom8()
        {
            ContentDefinitionManager.AlterPartDefinition("ProjectPart", builder => builder
                .WithDescription("Turns content types into a Project"));
            ContentDefinitionManager.AlterTypeDefinition("ProjectTaskMgmt", builder => builder
                .RemovePart("TaskMgmtPart")
                .WithPart("ProjectPart"));
                
                
            return 9;
        }

        public int UpdateFrom9()
        {
            ContentDefinitionManager.AlterPartDefinition("TaskPart", builder => builder
                .WithDescription("Turns content types into a Task"));
            ContentDefinitionManager.AlterTypeDefinition("Task", builder => builder
                .DisplayedAs("Task")
                       .WithPart("TaskPart")
                       .WithPart("CommonPart")
                       .WithPart("TitlePart")
                       .WithPart("BodyPart")
                       .Draftable());


            return 10;
        }

        public int UpdateFrom10()
        {
            ContentDefinitionManager.AlterTypeDefinition("Task", builder => builder
                .Draftable());
            return 11;
        }

        public int UpdateFrom11()
        {
           
            SchemaBuilder.AlterTable("TaskMgmtPartRecord", table => table
                .AddColumn("Status", DbType.String));
                
            
            return 12;
        }

        public int UpdateFrom12()
        {

            SchemaBuilder.AlterTable("TaskMgmtPartRecord", table => table
                .DropColumn("HasChild"));
            SchemaBuilder.AlterTable("TaskMgmtPartRecord", table => table
                .AddColumn("Task_Level",DbType.String));


            return 13;
        }
        public int UpdateFrom13()
        {
            SchemaBuilder.CreateTable("TaskAssingment",builder=>builder
                .Column("Id",DbType.Int32,column => column.PrimaryKey().Identity())
                .Column("TaskId",DbType.Int32)
                .Column("ResourceId",DbType.Int32)
                .Column("Efforts",DbType.Double)
                );

            return 14;
        }
    }

}