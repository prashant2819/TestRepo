using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.ProjectManagement.Models;
using Orchard.Data;

namespace Orchard.ProjectManagement.Handlers
{
    public class TaskAssignmentHandler:ContentHandler
    {
        public TaskAssignmentHandler(IRepository<TaskAssignPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}