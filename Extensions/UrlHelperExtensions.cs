using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Mvc.Extensions;
using System.Web.Mvc;

namespace Orchard.ProjectManagement.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string ProjectForAdmin(this UrlHelper urlHelper) {
            return urlHelper.Action("List", "ProjectAdmin", new {area = "Orchard.ProjectManagement"});
        }
    }
}