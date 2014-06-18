using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ProjectManagement.Models;

namespace Orchard.ProjectManagement.ViewModels
{
    public class ProjectTaskViewModel
    {
        public ProjectTaskViewModel(IEnumerable<TaskMgmtPartRecord> _tasks)
        {
            this.Tasks = _tasks;
        }
        public IEnumerable<TaskMgmtPartRecord> Tasks { get; set; }
        public List<int> DeletedTaskId { get; set; }
        public int SelectedRow { get; set; }
        public bool CanWrite { get; set; }
        public bool CanWriteOnParent { get; set; }
        
        
    }
}