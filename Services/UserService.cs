using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Data;
using Orchard.Users.Models;

namespace Orchard.ProjectManagement.Services
{
    
    public class UserService:IUserService
    {
        private readonly IRepository<UserPartRecord> _userRepository;
        public UserService(IRepository<UserPartRecord> userRepository)
        {
            _userRepository = userRepository;
        }



        public IEnumerable<UserPartRecord> GetUsers()
        {
            return _userRepository.Table.ToList();
        }
    }
}