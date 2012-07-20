﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Admin.Security.Models;
using Siege.Security.Principals;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.Admin.Security.Controllers
{
    public class ApplicationsController : SecurityController<Application, IApplicationProvider>
    {
        public ApplicationsController(IApplicationProvider provider, IAuthenticationProvider authenticationProvider) : base(provider, authenticationProvider)
        {
        }

        public JsonResult List(int consumerID, JqGridConfiguration configuration, IConsumerProvider consumerProvider)
        {
            var applications = consumerProvider.Find(consumerID).Applications;

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = applications.Count,
                rows = applications.Select(application => new
                {
                    id = application.ID,
                    cell = new object[]
                    {
                        application.Name,
                        application.Description,
                        application.IsActive ? "Yes" : "No",
                        application.ID.ToString()
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(ApplicationModel model)
        {
            var application = model.IsNew ? new Application() : this.provider.Find(model.ApplicationID);
            
            application.Name = model.Name;
            application.Description = model.Description;
            application.IsActive = model.IsActive;


            this.provider.Save(application);

            return Json(new { result = true });
        }

        public ActionResult Edit(int? id)
        {
            var application = this.provider.Find(id);

            var model = new ApplicationModel
            {
                ApplicationID = application.ID,
                Description = application.Description,
                IsActive = application.IsActive,
                Name = application.Name
            };

            return View(model);
        }

        public ActionResult GetForConsumer(int? id, IConsumerProvider consumerProvider)
        {
            var applications = consumerProvider.Find(id).Applications;
         
            var result = new
            {
                total = applications.Count,
                rows = applications.Select(application => new
                {
                    id = application.ID,
                    Name = application.Name,
                })
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(int id, IConsumerProvider consumerProvider)
        {
            var application = provider.Find(id);

            var jsonData = new
            {
                total = 1,
                rows = new[]
                {
                    new 
                    {
                        id = application.ID,
                        Name = application.Name,
                    }
                }
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ForConsumer(int? id, JqGridConfiguration configuration, IConsumerProvider consumerProvider)
        {
            IList<Application> applications;
            IList<Application> consumerApplications = new List<Application>();
           
            if (id != null)
            {
                var consumer = consumerProvider.Find(id);
                applications = provider.GetAllApplications();
                consumerApplications = consumer.Applications;
            }
            else
            {
                applications = provider.GetAllApplications();
            }

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = applications.Count,
                rows = applications.Select(application => new
                {
                    id = application.ID,
                    cell = new object[]
                    {
                           application.ID,
                           consumerApplications.Any(g => g.ID == application.ID) ? "true" : "false",
                           application.Name
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ForUser(User user, Consumer consumer, JqGridConfiguration configuration)
        {
            IList<Application> applications;
            IList<Application> userApplications = new List<Application>();

            if (user != null)
            {
                applications = user.Consumer.Applications;
                userApplications = user.Applications;
            }
            else
            {
                applications = consumer.Applications;
            }

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = applications.Count,
                rows = applications.Select(application => new
                {
                    id = application.ID,
                    cell = new object[]
                    {
                           application.ID,
                           userApplications.Any(g => g.ID == application.ID) ? "true" : "false",
                           application.Name
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}