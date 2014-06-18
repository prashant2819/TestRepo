using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.ProjectManagement.Models;
using Orchard.Forms.Services;
using Orchard.ProjectManagement.Services;
using Orchard.ContentManagement;
using Newtonsoft.Json.Linq;

namespace Orchard.ProjectManagement.Drivers
{
    public class ProjectPartDriver:ContentPartDriver<ProjectPart>
    {
        private readonly IProjectTaskService _projTasks;

        public ProjectPartDriver(IProjectTaskService projTask)
        {
            _projTasks = projTask;
        }
        protected override DriverResult Display(ProjectPart part, string displayType, dynamic shapeHelper)
        {
            IEnumerable<TaskMgmtPart> projTasks = _projTasks.Get(VersionOptions.Latest).Where(x => x.Id == part.Id);
            IEnumerable<TaskMgmtPart> task = _projTasks.GetTask(VersionOptions.Latest).Where(x => x.ProjectId == part.Id).OrderBy(x => x.Task_Level);
            var tasks = projTasks.Concat((task));
            var jsonTasks = ProjectManagementTaskJson(tasks);
            // var json = new JavaScriptSerializer().Serialize(_taskPartVM);
            return Combined(
            ContentShape("Parts_ProjectPart",
                () => shapeHelper.Parts_TaskPart(jsonData: jsonTasks)),
            ContentShape("Parts_ProjectPart_ProjectPart_SummaryAdmin",
                () => shapeHelper.Parts_TaskPart_TaskPart_SummaryAdmin())
            );
        }

        protected override DriverResult Editor(ProjectPart part, dynamic shapeHelper)
        {
           // part.Tasks = _projTasks.GetTasks().Where(t => t.Id != part.Id);
            return ContentShape("Parts_ProjectPart_Edit",
                             () => shapeHelper.EditorTemplate(TemplateName: "Parts/ProjectPart", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(ProjectPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        private string ProjectManagementTaskJson<T>(IEnumerable<T> _tasks) where T : TaskMgmtPart
        {
            //IEnumerable<TaskMgmtPartRecord> _tasks = _projTasks.GetTasks();
           // ProjectTaskViewModel _taskPartVM = new ProjectTaskViewModel(_tasks);
            dynamic jsonTask = new JObject();
            jsonTask.tasks = new JArray(_tasks.Select(_task =>
                {
                    dynamic task = new JObject();
                    task.id = _task.Id;
                    task.name = _task.Name;
                    task.description = _task.Description;
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
                    if (_task.ProjEndDate != null)
                    {
                        DateTime end = (DateTime)_task.ProjEndDate;
                        task.end = (long)(end - new DateTime(1970, 1, 1)).TotalMilliseconds;
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