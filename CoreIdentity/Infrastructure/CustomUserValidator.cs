using CoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreIdentity.Infrastructure
{
    public class CustomUserValidator : IUserValidator<ApplicationUser>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (!user.Email.ToLower().EndsWith(".com"))
            {
                errors.Add(new IdentityError()
                {
                    Code = "EmailMustBeEndsWithCom",
                    Description = "Email adresi .com uzantılı olmalıdır."
                });
            }

            var emailUser = await manager.FindByEmailAsync(user.Email);
            
            //if (emailUser != null)
            //{
            //    if (emailUser.Id != user.Id)
            //    {
            //        errors.Add(new IdentityError()
            //        {
            //            Code = "EmailExist",
            //            Description = "Bu email adresi daha önce kullanılmış"
            //        });
            //    }
            //}


            var usernameUser = await manager.FindByNameAsync(user.UserName);

            //if (usernameUser != null)
            //{
            //    if (usernameUser.Id != user.Id)
            //    {
            //        errors.Add(new IdentityError()
            //        {
            //            Code = "UserNameExist",
            //            Description = "Bu kullanıcı adı daha önce kullanılmış"
            //        });
            //    }
            //}

            var addr = new System.Net.Mail.MailAddress(user.Email);
            if (addr.Address != user.Email)
            {
                errors.Add(new IdentityError()
                {
                    Code = "EmailAddressNotValid",
                    Description = "Email adresinizi uygun formatta giriniz"
                });
            };

            string[] Digits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            foreach (var item in Digits)
            {
                if (user.UserName[0].ToString()==item)
                {
                    errors.Add(new IdentityError()
                    {
                        Code = "UserNameContainsFirstLetterDigit",
                        Description = "Kullanıcı adının ilk karakteri sayısal karakter içeremez."
                    });
                }
            }

            return await Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }

        public async Task<IdentityResult> ValidateAsyncUpdate(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (!user.Email.ToLower().EndsWith(".com"))
            {
                errors.Add(new IdentityError()
                {
                    Code = "EmailMustBeEndsWithCom",
                    Description = "Email adresi .com uzantılı olmalıdır."
                });
            }

            var CurrentUser = await manager.FindByIdAsync(user.Id);
            if (CurrentUser.Email != user.Email && CurrentUser != null && user.Email != null)
            {
                errors.Add(new IdentityError()
                {
                    Code = "EmailExist",
                    Description = "Bu email adresi daha önce kullanılmış"
                });
            }

            var username = await manager.FindByNameAsync(user.UserName);

            if (username != null && (CurrentUser.UserName != user.UserName))
            {
                errors.Add(new IdentityError()
                {
                    Code = "UserNameExist",
                    Description = "Bu kullanıcı adı daha önce kullanılmış"
                });
            }

            var addr = new System.Net.Mail.MailAddress(user.Email);
            if (addr.Address != user.Email && CurrentUser.Email != user.Email)
            {
                errors.Add(new IdentityError()
                {
                    Code = "EmailAddressNotValid",
                    Description = "Email adresinizi uygun formatta giriniz"
                });
            };

            return await Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
