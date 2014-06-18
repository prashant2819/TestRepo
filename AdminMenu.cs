using System.Linq;
using Orchard.ProjectManagement.Services;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Orchard.ProjectManagement {
    public class AdminMenu : INavigationProvider {
        private readonly IProjectTaskService _taskService;

        public AdminMenu(IProjectTaskService taskService)
        {
            _taskService = taskService;
        }

        public Localizer T { get; set; }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder.AddImageSet("Project Management")
                .Add(item => item
                .Caption(T("Project Management"))
                .Position("2")
                .Action("List", "ProjectAdmin", new { area = "Orchard.ProjectManagement" })       
            );
        }

        //private void BuildMenu(NavigationItemBuilder menu) {
        //    var tasks = _taskService.Get();
        //    var taskCount = tasks.Count();
        //    var singleTask = taskCount == 1 ? tasks.ElementAt(0) : null;

        //    if (taskCount > 0 && singleTask == null)
        //    {
        //        menu.Add(T("Manage Projects"), "3",
        //                 item => item.Action("List", "BlogAdmin", new { area = "Orchard.Blogs" }).Permission(Permissions.MetaListBlogs));
        //    }
        //    else if (singleTask != null)
        //        menu.Add(T("Manage Blog"), "1.0",
        //            item => item.Action("Item", "BlogAdmin", new { area = "Orchard.Blogs", blogId = singleTask.Id }).Permission(Permissions.MetaListOwnBlogs));

        //    if (singleTask != null)
        //        menu.Add(T("New Post"), "1.1",
        //                 item =>
        //                 item.Action("Create", "BlogPostAdmin", new { area = "Orchard.Blogs", blogId = singleTask.Id }).Permission(Permissions.MetaListOwnBlogs));

        //    menu.Add(T("New Blog"), "1.2",
        //             item =>
        //             item.Action("Create", "BlogAdmin", new { area = "Orchard.Blogs" }).Permission(Permissions.ManageBlogs));

        //}
    }
}

