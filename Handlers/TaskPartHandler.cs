using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ProjectManagement.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Orchard.ProjectManagement.Handlers
{
    public class TaskPartHandler: ContentHandler
    {
        public TaskPartHandler(IRepository<TaskMgmtPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}