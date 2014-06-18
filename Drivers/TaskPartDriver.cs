using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using Orchard.ProjectManagement.Models;
using Orchard.ProjectManagement.ViewModels;
using Orchard.ProjectManagement.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orchard.Forms.Services;


namespace Orchard.ProjectManagement.Drivers
{
    public class TaskPartDriver : ContentPartDriver<TaskPart>
    {
        private readonly IProjectTaskService _projTasks;

        public TaskPartDriver(IProjectTaskService projTask)
        {
            _projTasks = projTask;
        }
        protected override DriverResult Display(TaskPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Task_Task_SummaryAdmin",
                             () => shapeHelper.Parts_Task_Task_SummaryAdmin());
            //return null;
           
        }

        protected override DriverResult Editor(TaskPart part, dynamic shapeHelper)
        {
            part.Tasks = _projTasks.GetTask(VersionOptions.Latest).Where(t => t.ProjectId == part.ProjectId && t.Level == part.Level && t.Id != part.Id);
            return ContentShape("Parts_TaskPart_Edit",
                             () => shapeHelper.EditorTemplate(TemplateName: "Parts/TaskPart", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(TaskPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        private string ProjectManagementTaskJson()
        {
            IEnumerable<TaskMgmtPartRecord> _tasks = _projTasks.GetTasks();
            ProjectTaskViewModel _taskPartVM = new ProjectTaskViewModel(_tasks);
            dynamic jsonTask = new JObject();
            jsonTask.tasks = new JArray(_tasks.Select(_task =>
                {
                    dynamic task = new JObject();
                    task.id = _task.Id;
                    task.name = "Demo";
                    task.status = _task.Status;
                    task.code = _task.TaskCode;
                    task.level = _task.Level;
                    task.duration = _task.Duration;
                    task.startIsMilestone = _task.StartIsMilestone;
                    task.endIsMilestone = _task.EndIsMilestone;
                    task.progress = _task.Progress;
                    if (_task.Dependancy != null)
                    {
                        task.depends = _task.Dependancy;
                    }
                    //task.start = DateTime.Now.AddDays(10).Millisecond;
                    if (_task.ProjStartDate != null)
                    {
                        DateTime start = (DateTime)_task.ProjStartDate;
                        task.start = (long)(start - new DateTime(1970, 1, 1)).TotalMilliseconds;
                    }
                    return task;
                }));
            jsonTask.selectedRow = 0;
            jsonTask.canWrite = true;
            jsonTask.canWriteOnParent = true;

            return FormParametersHelper.ToJsonString(jsonTask);

        }
    }
}