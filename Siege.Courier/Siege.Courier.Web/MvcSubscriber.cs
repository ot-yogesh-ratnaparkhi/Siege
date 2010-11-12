using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Siege.Courier.Web.Responses;

namespace Siege.Courier.Web
{
    public class MvcSubscriber
    {
        private ControllerContext controllerContext;
        protected dynamic QueryString;
        protected dynamic TempData;
        protected dynamic ViewModel;
        private readonly ViewModel viewModel;

        public MvcSubscriber(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
            QueryString = new QueryString(controllerContext.RequestContext.HttpContext.Request.QueryString);
            TempData = new TempData(controllerContext.Controller.TempData);
            ViewModel = viewModel = new ViewModel(controllerContext.Controller.ViewData);
        }

        protected void View(object model)
        {
            var response = new ViewResponse();

            response.Execute(model, controllerContext);

            viewModel.ClearModelState();
        }

        protected void Json(object model)
        {
            new JsonResponse().Execute(model, controllerContext);
        }

        protected void Redirect(string url)
        {
            new RedirectResponse(url).Execute(null, controllerContext);
        }

        protected void RedirectTo<T>(Expression<Action<T>> destination)
        {
            new RedirectToResponse<T>(destination).Execute(null, controllerContext);
        }

        protected void AddModelError(string key, string errorMessage)
        {
            controllerContext.Controller.ViewData.ModelState.AddModelError(key, errorMessage);
        }
    }
}