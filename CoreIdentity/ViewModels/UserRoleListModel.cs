using CoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreIdentity.ViewModels
{
    public class UserRoleListModel
    {
        public ApplicationUser User { get; set; }
        public List<string> RoleName { get; set; }
    }
}
