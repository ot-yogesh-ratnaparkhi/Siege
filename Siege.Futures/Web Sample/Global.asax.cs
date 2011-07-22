using System.Web.Mvc;
using Siege.ServiceLocator;
using Siege.ServiceLocator.Extensions.Conventions;
using Siege.ServiceLocator.Extensions.ExtendedRegistrationSyntax;
using Siege.Integration.WCF;
using Siege.Eventing.Web;
using Siege.Eventing.Web.ViewEngine;
using Siege.Eventing;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Native;
using Siege.ServiceLocator.Web.Conventions;
using Web.Sample.Controllers;
using Web.Sample.Conventions;
using Web.Sample.Messages;
using Web.Sample.Models;
using Web.Sample.Subscribers;

namespace Web.Sample
{
    public class MvcApplication : EventfulHttpApplication
    {
        protected override IServiceLocatorAdapter GetServiceLocatorAdapter()
        {
            return new SiegeAdapter();
        }

        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            ServiceLocator
                .Register(Using.Convention<TemplateConvention>())
                .Register(Using.Convention<ControllerConvention<HomeController>>())
                .Register(Given<WCFAdapter>.Then<WCFAdapter>())
                .Register(Given<IConfigurationManager>.Then<ServiceBusConfigurationManager>())
                .Register(Given<IChannelManagerFactory>.Then<WCFChannelManagerFactory>())
                .Register(Given<IChannelManager<IWCFProtocol>>.ConstructWith(x =>
                {
                    var config = x.GetInstance<IConfigurationManager>();
                    return x.GetInstance<WCFChannelManagerFactory>().Create<IWCFProtocol>(config.ServiceBusEndPoint);
                }))
                .Register(Given<WCFProxy<IWCFProtocol>>.Then<WCFProxy<IWCFProtocol>>())
                .Register(Given<AccountSubscriber>.Then<AccountSubscriber>())
                .Register(Given<IMessageBucket>.Then<HttpMessageBucket>())
                .Register(Given<IFormsAuthenticationService>.Then<FormsAuthenticationService>());
            
            MapMessage<LogOnAccountMessage, WCFAdapter>();
            
            AddSubscriber<AccountSubscriber, MemberAuthenticatedMessage>();
            AddSubscriber<AccountSubscriber, MemberFailedAuthenticationMessage>();
            AddSubscriber<AccountSubscriber, MessageValidationFailedMessage<LogOnAccountMessage>>();

            var engine = new TemplateViewEngine(() => ServiceLocator.Store.Get<IContextStore>().Items);

            engine.UseConvention(new SampleConvention());
            
            //engine
            //    .For<HomeController>(controller => controller.Index())
            //    .Map(
            //             To.Path("LOL").When<bool>(x => x)
            //        );
            //engine
            //    .ForPartial("LogOnUserControl")
            //    .Map(
            //            To.Path("~/Views/Shared/LogOnUserControl.ascx"),
            //            To.Path("~/Views/LOL/LOLUserControl.ascx").When<bool>(x => x)
            //        );

            //engine
            //    .Map(
            //            To.Path("~/Views/Home/"),
            //            To.Path("~/Views/LOL/").When<bool>(x => x),
            //            To.Master("~/Views/Shared/Site.master"),
            //            To.Master("~/Views/LOL/LOL.master").When<bool>(x => x)
            //        );
            
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(engine);

            ServiceLocator.AddContext(true);
        }
    }
}