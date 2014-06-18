using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Users.Models;
using Orchard.ProjectManagement.Models;

namespace Orchard.ProjectManagement.ViewModels
{
    public class AssignmentTaskViewModel
    {
        IEnumerable<TaskAssignment> tasks { get; set; }
        IEnumerable<UserPartRecord> Users { get; set; }
    }
}