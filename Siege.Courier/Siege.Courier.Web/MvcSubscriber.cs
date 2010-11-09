using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Siege.Courier.Web.Responses;

namespace Siege.Courier.Web
{
    public class MvcSubscriber
    {
        private TempDataDictionary tempDataDictionary;
        private ViewDataDictionary viewDataDictionary;
        private readonly RequestContext requestContext;
        private readonly DummyController controller = new DummyController();
        protected dynamic QueryString;
        protected dynamic TempData;
        protected dynamic ViewModel;
        private readonly ViewModel viewModel;

        public MvcSubscriber(RequestContext requestContext)
        {
            this.requestContext = requestContext;
            QueryString = new QueryString(requestContext.HttpContext.Request.QueryString);
            TempData = new TempData(TempDataDictionary);
            ViewModel = viewModel = new ViewModel(ViewData);

            controller.ViewData = viewDataDictionary;
            controller.TempData = tempDataDictionary;
        }

        protected void View(object model)
        {
            var response = new ViewResponse {ViewData = this.ViewData, TempData = this.TempDataDictionary };
            
            response.Execute(model, ControllerContext);

            viewDataDictionary.ModelState.Clear();
        }

        protected void Json(object model)
        {
            new JsonResponse().Execute(model, ControllerContext);
        }

        protected void Redirect(string url)
        {
            new RedirectResponse(url).Execute(null, ControllerContext);
        }

        protected void RedirectTo<T>(Expression<Action<T>> destination)
        {
            new RedirectToResponse<T>(destination).Execute(null, ControllerContext);
        }

        protected ControllerContext ControllerContext
        {
            get { return GetControllerContext(); }
        }

        protected ControllerContext GetControllerContext()
        {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            return new ControllerContext(context, requestContext.RouteData, controller);
        }

        protected void AddModelError(string key, string errorMessage)
        {
            viewModel.AddModelError(key, errorMessage);
        }

        private TempDataDictionary TempDataDictionary
        {
            get
            {
                if ((this.ControllerContext != null) && this.ControllerContext.IsChildAction)
                {
                    return this.ControllerContext.ParentActionViewContext.TempData;
                }
                return this.tempDataDictionary ?? (this.tempDataDictionary = new TempDataDictionary());
            }
        }

        private ViewDataDictionary ViewData
        {
            get { return this.viewDataDictionary ?? (this.viewDataDictionary = new ViewDataDictionary()); }
        }
    }
}