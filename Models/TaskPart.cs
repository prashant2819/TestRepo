using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orchard.ProjectManagement.Models
{
    public class TaskPart : TaskMgmtPart//ContentPart<TaskMgmtPartRecord>
    {
        
        //public string Name { get { return this.As<TitlePart>().Title; } }
        //public string Description { get { return this.As<BodyPart>().Text; } }
        //[Required]
        //public string TaskCode { get { return Record.TaskCode; } set { Record.TaskCode = value; } }
        //[Required]
        //public string Status { get { return Record.Status; } set { Record.Status = value; } }
        //public double Duration { get { return Record.Duration; } set { Record.Duration = value; } }
        //public bool StartIsMilestone { get { return Record.StartIsMilestone; } set { Record.StartIsMilestone = value; } }
        //public bool EndIsMilestone { get { return Record.EndIsMilestone; } set { Record.EndIsMilestone = value; } }
        //[Range(0.0, 100.0)]
        //public double Progress { get { return Record.Progress; } set { Record.Progress = value; } }
        //public string Dependancy { get { return Record.Dependancy; } set { Record.Dependancy = value; } }
        //[Required]
        //public DateTime? ProjStartDate { get { return Record.ProjStartDate; } set { Record.ProjStartDate = value; } }
        //public DateTime? ProjEndDate { get { return Record.ProjEndDate; } set { Record.ProjEndDate = value; } }
        //public string Task_Level { get { return Record.Task_Level; } set { Record.Task_Level = value; } }
        //public int Level { get { return Record.Level; } set { Record.Level = value; } }
        ////public bool HasChild { get { return Record.HasChild; } set { Record.HasChild = value; } }
        //public int ParentTaskId { get { return Record.ParentTaskId; } set { Record.ParentTaskId = value; } }
        //public int ProjectId { get { return Record.ProjectId; } set { Record.ProjectId = value; } }
        public IEnumerable<TaskPart> Tasks { get; set; }
    }
}