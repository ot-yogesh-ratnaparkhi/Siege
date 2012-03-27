using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Providers;
using Siege.Security.SampleApplication.Models;

namespace Siege.Security.SampleApplication.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthenticationProvider authenticationProvider;
        private readonly IIdentityProvider identityProvider;

        public AccountController(IIdentityProvider identityProvider, IAuthenticationProvider authenticationProvider)
        {
            this.identityProvider = identityProvider;
            this.authenticationProvider = authenticationProvider;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return ContextDependentView();
        }
        
        [AllowAnonymous]
        [HttpPost]
        public JsonResult JsonLogin(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authenticationProvider.Authenticate(model.UserName, model.Password, model.RememberMe))
                    return Json(new {success = true, redirect = returnUrl});

                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return Json(new {errors = GetErrorsFromModelState()});
            }

            return Json(new {errors = GetErrorsFromModelState()});
        }
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authenticationProvider.Authenticate(model.UserName, model.Password, model.RememberMe))
                {
                    if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        public ActionResult LogOff()
        {
            authenticationProvider.Clear();

            return RedirectToAction("Index", "Home");
        }
        
        [AllowAnonymous]
        public ActionResult Register()
        {
            return ContextDependentView();
        }
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult JsonRegister(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.UserName, 
                    Password = model.Password,
                    Email = model.Email,
                    SecretQuestion = "What is the meaning of life?",
                    SecretAnswer = "43",
                    IsActive = true
                };
                
                identityProvider.Create(user);

                return
                    JsonLogin(
                        new LoginModel {Password = model.Password, RememberMe = false, UserName = model.UserName}, "");
            }

            return Json(new {errors = GetErrorsFromModelState()});
        }
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.UserName,
                    Password = model.Password,
                    Email = model.Email,
                    SecretQuestion = "What is the meaning of life?",
                    SecretAnswer = "43",
                    IsActive = true
                };

                identityProvider.Create(user);

                return Login(new LoginModel {Password = model.Password, RememberMe = false, UserName = model.UserName}, "");
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }
        
        public ActionResult ChangePassword()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = identityProvider.ChangePassword(User.Identity.Name, model.OldPassword,
                                                                              model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            return View(model);
        }
        
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        private ActionResult ContextDependentView()
        {
            string actionName = ControllerContext.RouteData.GetRequiredString("action");
            if (Request.QueryString["content"] != null)
            {
                ViewBag.FormAction = "Json" + actionName;
                return PartialView();
            }
            else
            {
                ViewBag.FormAction = actionName;
                return View();
            }
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
    }
}