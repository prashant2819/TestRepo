using Orchard.ContentManagement;
using Orchard.Core.Contents;
using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.ProjectManagement.Models;
using Orchard.ProjectManagement.Services;
using Orchard.UI.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.UI.Notify;
using Orchard.Core.Contents.Controllers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orchard.Forms.Services;

namespace Orchard.ProjectManagement.Controllers
{
    [ValidateInput(false), Admin]
    public class TaskAdminController : Controller, IUpdateModel
    {
        private readonly IUserService _userService;
        private readonly IProjectTaskService _taskService;
        private readonly IContentManager _contentManager;
        private readonly ITransactionManager _transactionManager;
        public TaskAdminController(
            IUserService userService,
            IProjectTaskService taskService,
            IContentManager contenetManager,
            ITransactionManager transactionManager,
            IOrchardServices services,
            IShapeFactory shape)
        {
            _userService = userService;
            _taskService = taskService;
            _contentManager = contenetManager;
            _transactionManager = transactionManager;
            Services = services;
            T = NullLocalizer.Instance;
            Shape = shape;
        }
        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        dynamic Shape { get; set; }

        public ActionResult Create(int Id)
        {

            if (!Services.Authorizer.Authorize(Permissions.EditContent, T("Cannot create Project")))
                return new HttpUnauthorizedResult();
            var task = Services.ContentManager.New<TaskPart>("Task");
            if (task == null)
                return HttpNotFound();

            dynamic model = Services.ContentManager.BuildEditor(task);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreateTask(int Id)
        {
            if (!Services.Authorizer.Authorize(Permissions.EditContent, T("Cannot create Project")))
                return new HttpUnauthorizedResult();

            var parentTask = _taskService.GetTasks().Where(t => t.Id == Id).FirstOrDefault();
            var task = Services.ContentManager.New<TaskPart>("Task");

            
            _contentManager.Create(task, VersionOptions.Draft);

            dynamic model = _contentManager.UpdateEditor(task, this);

            task.ParentTaskId = parentTask.Id;
            task.ProjectId = parentTask.ProjectId;
            task.Level = parentTask.Level + 1;
            task.Task_Level = parentTask.Task_Level + task.Id.ToString() + "_";

            if (!ModelState.IsValid)
            {
                _transactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

           _contentManager.Publish(task.ContentItem);

          
           return RedirectToAction("ListTask", "TaskAdmin",new { Id = parentTask.Id });
            //return Redirect(Url.Action("List", "TaskAdmin"));
        }

        public ActionResult ListTask(int Id)
        {
            var list = Services.New.List();


            list.AddRange(_taskService.GetTask(VersionOptions.Latest).Where(ta => ta.ParentTaskId == Id)//.Where(ta => ta.ParentTaskId == Id)
                           .Select(t =>
                           {
                               var task = Services.ContentManager.BuildDisplay(t, "SummaryAdmin");
                               return task;
                           }));
            dynamic viewModel = Services.New.ViewModel()
                .ContentItems(list)
                .parentId(Id);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)viewModel);
        }

        public ActionResult Edit(int taskId)
        {
            var task = _taskService.GetTask(taskId, VersionOptions.Latest);

            if (!Services.Authorizer.Authorize(Permissions.EditContent, task, T("Not allowed to edit Project")))
                return new HttpUnauthorizedResult();

            if (task == null)
                return HttpNotFound();

            dynamic model = Services.ContentManager.BuildEditor(task);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Save")]
        public ActionResult EditTask(int taskId)
        {
            //var item = _taskService.
            var task = _taskService.GetTask(taskId, VersionOptions.DraftRequired);

            TaskPart taskPart = (TaskPart)task.Get(typeof(TaskPart));

            if (!Services.Authorizer.Authorize(Permissions.EditContent, task, T("Couldn't edit Task")))
                return new HttpUnauthorizedResult();

            if (task == null)
                return HttpNotFound();

            dynamic model = Services.ContentManager.UpdateEditor(task, this);
            if (!ModelState.IsValid)
            {
                Services.TransactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            _contentManager.Publish(task);
            Services.Notifier.Information(T("Task is updated successfully"));

            return Redirect(Url.Action("ListTask", "TaskAdmin", new {Id =taskPart.ParentTaskId }));
        }

      //  [HttpPost]
        public ActionResult Remove(int taskid)
        {
            var task = _taskService.GetTask(taskid, VersionOptions.Latest);

            TaskPart taskPart = (TaskPart)task.Get(typeof(TaskPart));

            //Get the child tasks of the task to be deleted
            var childTask = _taskService.GetTask(VersionOptions.Latest).Where(t => t.ParentTaskId == taskid);
            if (!Services.Authorizer.Authorize(Permissions.DeleteContent, task, T("Couldn't remove content")))
                return new HttpUnauthorizedResult();

            if (task != null)
            {
                _contentManager.Remove(task);
                foreach(var item in childTask)
                    _contentManager.Remove(item.ContentItem);

                Services.Notifier.Information(string.IsNullOrWhiteSpace(task.TypeDefinition.DisplayName)
                     ? T("That content has been removed.")
                     : T("That {0} has been removed.", task.TypeDefinition.DisplayName));
            }

            return Redirect(Url.Action("ListTask", "TaskAdmin", new {Id =taskPart.ParentTaskId }));
        }
        public ActionResult AddAssignment(int taskId)
        {
            TaskAssignment _taskAssignment = new TaskAssignment();
            var users = _userService.GetUsers();
            _taskAssignment.Users = users;
            dynamic jObj = new JObject();
            jObj.Users = new JArray(users.Select(x =>
             {
                 dynamic user = new JObject();
                 user.id = x.Id;
                 user.Name = x.UserName;

                 return user;
             }));
            _taskAssignment.jsonData = FormParametersHelper.ToJsonString(jObj);
            return View("AddAssignment", _taskAssignment);
        }

        public ActionResult GetUserList()
        {
            var users = _userService.GetUsers();

            dynamic jObj = new JObject();
            jObj.Users = new JArray(users.Select(x =>
            {
                dynamic user = new JObject();
                user.id = x.Id;
                user.Name = x.UserName;

                return user;
            }));

           
            return Json(FormParametersHelper.ToJsonString(jObj), JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult SaveProjectData(object projData)
        {
            return null;
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}