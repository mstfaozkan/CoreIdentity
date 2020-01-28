using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreIdentity.Infrastructure;
using CoreIdentity.Models;
using CoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreIdentity.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IPasswordValidator<ApplicationUser> passwordValidator;
        private IUserValidator<ApplicationUser> userValidator;
        private IPasswordHasher<ApplicationUser> passwordHasher;

        public AdminController(UserManager<ApplicationUser> _userManager, IPasswordValidator<ApplicationUser> _passwordValidator, IPasswordHasher<ApplicationUser> _passwordHasher, IUserValidator<ApplicationUser> _userValidator)
        {
            userManager = _userManager;
            passwordValidator = _passwordValidator;
            passwordHasher = _passwordHasher;
            userValidator = _userValidator;
        }

        public async Task<IActionResult> Index()
        {
            var UserRoleList = new List<UserRoleListModel>();
            foreach (var user in userManager.Users)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var list = new List<string>();
                foreach (var role in userRoles)
                {
                    list.Add(role);
                }
                var userRole = new UserRoleListModel()
                {
                    User = user,
                    RoleName = list

                };

                UserRoleList.Add(userRole);
            }
            return View(UserRoleList);
        }

        //[Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel entity)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = entity.UserName,
                    Email = entity.Email
                };
                var result = await userManager.CreateAsync(user, entity.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);

            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("UserNotFound", "User not found");
            }


            return View("Index", userManager.Users);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {

                return View(user);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Update(ApplicationUser entity, string password)
        {
            var user = await userManager.FindByIdAsync(entity.Id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(password))
                {
                    //user.Email = entity.Email;
                    //var userValid = await userValidator.ValidateAsync(userManager, user);

                    var passwordValid = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (passwordValid.Succeeded)
                    {
                        user.Email = entity.Email;
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                        var result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in passwordValid.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }

                    }

                }
                else
                {
                    ModelState.AddModelError("", "Şifre alanı boş geçilemez");
                }

            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
            }
            return View(entity);
        }
    }
}