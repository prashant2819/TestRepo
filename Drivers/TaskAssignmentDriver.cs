using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.ProjectManagement.Models;

namespace Orchard.ProjectManagement.Drivers
{
    public class TaskAssignmentDriver: ContentPartDriver<TaskAssignment>
    {
        protected override DriverResult Editor(TaskAssignment part, dynamic shapeHelper)
        {
            return ContentShape("Parts_AssignmentPart_Edit",
                () => shapeHelper.EditorTemplate(TemplateName: "Parts/AssignmentPart", Model: part, Prefix: Prefix));
            
        }
        protected override DriverResult Editor(TaskAssignment part, ContentManagement.IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}