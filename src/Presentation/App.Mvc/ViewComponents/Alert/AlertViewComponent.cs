using App.Infra.CrossCutting.Extensions;
using App.Mvc.ViewComponents.Alert.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Mvc.ViewComponents.Alert
{
    public class AlertViewComponent : ViewComponent
    {
        private Controller _controller;

        public AlertViewComponent(): base() {}

        protected AlertViewComponent(
            Controller context)
        {
            _controller = context;
        }

        public static AlertViewComponent Initialize(Controller context)
            => new AlertViewComponent(context);

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AlertViewComponentModel model = new();
            //Save
            if (ViewData.ContainsKey(nameof(AlertViewComponent)))
                model = (AlertViewComponentModel)await Task.FromResult(ViewData[nameof(AlertViewComponent)]);

            //ForRedirect
            if (await HttpContext.Session.GetObjectAsync<AlertViewComponentModel>(nameof(AlertViewComponent)) != null)
                model = await HttpContext.Session.GetUniqueAsync<AlertViewComponentModel>(nameof(AlertViewComponent));

            return View(model);
        }

        public async Task ShowAsync(string title, string subtitle, AlertViewComponentModel.Types type, AlertViewComponentModel.Layouts layout, bool redirect = false)
        {
            await SaveAsync(new AlertViewComponentModel() { 
                 Title= title,
                 Message = subtitle,            
                 Type = type,
                 Layout = layout
            }, redirect);
        }

        private async Task SaveAsync(AlertViewComponentModel model, bool redirect = false)
        {
            using (model)
            {
                model.Visible = true;
                model.Redirect = redirect;

                if (redirect)
                {
                    await _controller.HttpContext.Session.SetObjectAsync(nameof(AlertViewComponent), model);
                }
                else
                {
                    _controller.ViewData.Remove(nameof(AlertViewComponent));
                    _controller.ViewData.Add(nameof(AlertViewComponent), model);
                }
            }
        }
    }
}
