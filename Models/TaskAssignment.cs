using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Users.Models;
using Newtonsoft.Json.Linq;

namespace Orchard.ProjectManagement.Models
{
    
    public class TaskAssignment 
    {
        public int TaskId { get; set; }
        public int ResourceId { get; set; }
       // public int RoleId { get; set; }
        public double Efforts { get; set; }
        public IEnumerable<UserPartRecord> Users { get; set; }
        public string jsonData { get; set; }
    }
}