using Orchard.Data;
using Orchard.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orchard.ProjectManagement.Services
{
    public interface IUserService:IDependency
    {
        IEnumerable<UserPartRecord> GetUsers();
        
    }
}