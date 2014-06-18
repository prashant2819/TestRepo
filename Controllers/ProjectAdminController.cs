using Orchard.Core.Contents;
using Orchard.DisplayManagement;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.ProjectManagement.Models;
using Orchard.ProjectManagement.Services;
using Orchard.UI.Admin;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.UI.Navigation;
using Orchard.Settings;
using Orchard.Core.Contents.Controllers;
using Orchard.ProjectManagement.Extensions;
using Orchard.UI.Notify;

namespace Orchard.ProjectManagement.Controllers
{
    [ValidateInput(false), Admin]
    public class ProjectAdminController : Controller,IUpdateModel
    {
        private readonly IProjectTaskService _taskService;
        private readonly IContentManager _contentManager;
        private readonly ITransactionManager _transactionManager;
        private readonly ISiteService _siteService;

        public ProjectAdminController(
            IProjectTaskService taskService,
            IContentManager contenetManager,
            ITransactionManager transactionManager,
            ISiteService siteService,
            IOrchardServices services,
            IShapeFactory shape)
        {
            _taskService = taskService;
            _contentManager = contenetManager;
            _transactionManager = transactionManager;
            _siteService = siteService;
            Services = services;
            T = NullLocalizer.Instance;
            Shape = shape;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        dynamic Shape { get; set; }


        public ActionResult Create()
        {
            if (!Services.Authorizer.Authorize(Permissions.EditContent, T("Cannot create Project")))
                return new HttpUnauthorizedResult();

            var task = Services.ContentManager.New<ProjectPart>("ProjectTaskMgmt");
            if (task == null)
                return HttpNotFound();

            dynamic model = Services.ContentManager.BuildEditor(task);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);

        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST()
        {
            if (!Services.Authorizer.Authorize(Permissions.EditContent, T("Cannot create Project")))
                return new HttpUnauthorizedResult();

            var task = Services.ContentManager.New<ProjectPart>("ProjectTaskMgmt");
            _contentManager.Create(task, VersionOptions.Draft);
            dynamic model = _contentManager.UpdateEditor(task, this);

            task.ProjectId = task.Id;
            task.Task_Level = task.Id.ToString() + "_";

            if (!ModelState.IsValid)
            {
                _transactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            _contentManager.Publish(task.ContentItem);
            return Redirect(Url.Action("List","ProjectAdmin"));
        }

        public ActionResult Edit(int projectId)
        {
            var project = _taskService.Get(projectId, VersionOptions.Latest);

            if (!Services.Authorizer.Authorize(Permissions.EditContent, project, T("Not allowed to edit Project")))
                return new HttpUnauthorizedResult();

            if (project == null)
                return HttpNotFound();

            dynamic model = Services.ContentManager.BuildEditor(project);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Save")]
        public ActionResult EditTask(int projectId)
        {
            var project = _taskService.Get(projectId, VersionOptions.DraftRequired);

            if (!Services.Authorizer.Authorize(Permissions.EditContent, project, T("Couldn't edit Project")))
                return new HttpUnauthorizedResult();

            if (project == null)
                return HttpNotFound();

            dynamic model = Services.ContentManager.UpdateEditor(project, this);
            if (!ModelState.IsValid)
            {
                Services.TransactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            _contentManager.Publish(project);
            Services.Notifier.Information(T("Project information updated"));

            return Redirect(Url.ProjectForAdmin());
        }

        //[HttpPost, ActionName("Edit")]
        //[FormValueRequired("submit.Delete")]
        //public ActionResult EditDeleteProject(int projectId)
        //{
        //    if (!Services.Authorizer.Authorize(Permissions.EditContent, T("Couldn't delete blog")))
        //        return new HttpUnauthorizedResult();

        //    var project = _taskService.Get(projectId, VersionOptions.DraftRequired);
        //    if (project == null)
        //        return HttpNotFound();
        //    _taskService.Delete(project);

        //    Services.Notifier.Information(T("Project deleted"));

        //    return RedirectToAction("List");
        //}

        //[HttpPost]
        //public ActionResult Remove(int projectId)
        //{
        //    if (!Services.Authorizer.Authorize(Permissions.EditContent, T("Couldn't delete Project")))
        //        return new HttpUnauthorizedResult();

        //    var project = _taskService.Get(projectId, VersionOptions.Latest);

        //    if (project == null)
        //        return HttpNotFound();

        //    _contentManager.Remove(project);

        //    Services.Notifier.Information(T("Blog was successfully deleted"));
        //    return Redirect(Url.ProjectForAdmin());
        //}
        public ActionResult List()
        {
            var list =  Services.New.List();
            list.AddRange(_taskService.Get(VersionOptions.Latest).Where(ta=>ta.Level==0)
                           .Select(t =>
                               {
                                   var task = Services.ContentManager.BuildDisplay(t, "SummaryAdmin");
                                   return task;
                               }));
            dynamic viewModel =  Services.New.ViewModel()
                .ContentItems(list);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)viewModel);
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