using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUI.Services;

public class RazorViewRenderer(
    IRazorViewEngine _razorViewEngine,
    IServiceProvider _serviceProvider,
    ITempDataProvider _tempDataProvider,
    ILogger<RazorViewRenderer> _logger) : IRazorViewRenderer
{
    public async Task<string?> RenderViewToStringAsync(string viewName, object model)
    {
        try
        {
            var actionContext = new ActionContext(
            new DefaultHttpContext { RequestServices = _serviceProvider },
            new RouteData(),
            new ActionDescriptor());

            using var sw = new StringWriter();

            var viewResult = _razorViewEngine.GetView("", viewName, false);
            if (!viewResult.Success)
            {
                _logger.LogError(message: $"Class: RazorViewRenderer, Hata: {viewName} isimli View dosyası bulunamadı");
                return null;
            }

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var tempData = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                tempData,
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            return sw.ToString();
        }
        catch (Exception e)
        {
            var locationDefinition = "Inside RazorViewRenderer class RenderViewToStringAsync method's catch section!!!";
            _logger.LogError(message: $" {locationDefinition} Hata Mesajı: {e.Message}, StackTrace: {e.StackTrace}");
            return null;
        }
    }
}