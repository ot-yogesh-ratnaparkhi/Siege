using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Siege.Security.Admin.Security.Models;
using Siege.Security.Providers;
using Siege.Security.Web;

namespace Siege.Security.Admin.Security.Controllers
{
    public class ConsumersController : SecurityController<Consumer, IConsumerProvider>
    {
        public ConsumersController(IConsumerProvider provider, IAuthenticationProvider authenticationProvider) : base(provider, authenticationProvider)
        {
        }

        public ActionResult Get(int? id, IUserProvider userProvider)
        {

            if(id != null)
            {
                var consumer = provider.Find(id);

                var jsonData = new
                {
                    total = 1,
                    rows = new[]
                    {
                        new {
                            id = consumer.ID,
                            Name = consumer.Name,
                        }
                    }
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            var principal = authenticationProvider.GetAuthenticatedUser();
            var user = userProvider.FindByUserName(principal.Identity.Name);

            if (!user.Can("CanAdministerAllSecurity"))
            {
                var jsonData = new
                {
                    total = 1,
                    rows = new[]
                    {
                        new {
                            id = user.Consumer.ID,
                            Name = user.Consumer.Name
                        }
                    }
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            var consumers = provider.All();

            var result = new
            {
                total = consumers.Count,
                rows = consumers.Select(consumer => new
                {
                    id = consumer.ID,
                    Name = consumer.Name,
                })
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult List(JqGridConfiguration configuration)
        {
            var consumers = provider.All();

            var jsonData = new
            {
                total = 1,
                page = configuration.PageIndex,
                records = consumers.Count,
                rows = consumers.Select(consumer => new
                {
                    id = consumer.ID,
                    cell = new object[]
                    {
                        consumer.Name,
                        consumer.Description,
                        consumer.IsActive ? "Yes" : "No",
                        consumer.ID.ToString()
                    }
                })
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            var consumer = this.provider.Find(id);

            var model = new ConsumerModel
            {
                ConsumerID = consumer.ID,
                Description = consumer.Description,
                IsActive = consumer.IsActive,
                Name = consumer.Name
            };

            return View(model);
        }

        public ActionResult Save(ConsumerModel model, List<Application> selectedApplications, IApplicationProvider applicationProvider)
        {
            var consumer = model.IsNew ? new Consumer() : this.provider.Find(model.ConsumerID);

            consumer.Name = model.Name;
            consumer.Description = model.Description;
            consumer.IsActive = model.IsActive;

            consumer.Applications.Clear();
            selectedApplications.ForEach(a => consumer.Applications.Add(a));

            this.provider.Save(consumer);

            return Json(new { result = true });
        }
    }
}