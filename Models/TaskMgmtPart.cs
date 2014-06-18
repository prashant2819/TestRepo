using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using System.ComponentModel.DataAnnotations;
using Orchard.Core.Title.Models;
using Orchard.Core.Common.Models;



namespace Orchard.ProjectManagement.Models
{
    public class TaskMgmtPartRecord : ContentPartRecord
    {
        public virtual string TaskCode { get; set; }
        public virtual string Status { get; set; }
        public virtual double Duration { get; set; }
        public virtual bool StartIsMilestone { get; set; }
        public virtual bool EndIsMilestone { get; set; }
        public virtual double Progress { get; set; }
        public virtual string Dependancy { get; set; }
        public virtual DateTime? ProjStartDate { get; set; }
        public virtual DateTime? ProjEndDate { get; set; }
        public virtual string Task_Level { get; set; }
        public virtual int Level { get; set; }
       // public virtual bool HasChild { get; set; }
        public virtual int ParentTaskId { get; set; }
        public virtual int ProjectId { get; set; }

    }

    public class TaskMgmtPart : ContentPart<TaskMgmtPartRecord>
    {
        public string Name { get { return this.As<TitlePart>().Title; } }
        public string Description { get { return this.As<BodyPart>().Text; } }
        [Required]
        public string TaskCode { get { return Record.TaskCode; } set { Record.TaskCode = value; } }
        [Required]
        public string Status { get { return Record.Status; } set { Record.Status = value; } }
        public double Duration { get { return Record.Duration; } set { Record.Duration = value; } }
        public bool StartIsMilestone { get { return Record.StartIsMilestone; } set { Record.StartIsMilestone = value; } }
        public bool EndIsMilestone { get { return Record.EndIsMilestone; } set { Record.EndIsMilestone = value; } }
        [Range(0.0, 100.0)]
        public double Progress { get { return Record.Progress; } set { Record.Progress = value; } }
        public string Dependancy { get { return Record.Dependancy; } set { Record.Dependancy = value; } }
        [Required]
        public DateTime? ProjStartDate { get { return Record.ProjStartDate; } set { Record.ProjStartDate = value; } }
        public DateTime? ProjEndDate { get { return Record.ProjEndDate; } set { Record.ProjEndDate = value; } }
        public string Task_Level { get { return Record.Task_Level; } set { Record.Task_Level = value; } }
        public int Level { get { return Record.Level; } set { Record.Level = value; } }
        // public bool HasChild { get { return Record.HasChild; } set { Record.HasChild = value; } }
        public int ParentTaskId { get { return Record.ParentTaskId; } set { Record.ParentTaskId = value; } }
        public int ProjectId { get { return Record.ProjectId; } set { Record.ProjectId = value; } }
    }
}