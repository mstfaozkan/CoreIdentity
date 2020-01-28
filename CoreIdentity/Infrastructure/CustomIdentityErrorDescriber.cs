using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreIdentity.Infrastructure
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return (new IdentityError()
            {
                Code = "UserNameExist",
                Description = "Bu mail adresi daha önce kullanılmış"
            });
        }


    }
}
