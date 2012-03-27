using System.Linq;
using System.Web.Mvc;
using Siege.Security.Providers;

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
    }
}