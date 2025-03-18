namespace Business.Services.Abstract;

public interface IRazorViewRenderer
{
    Task<string?> RenderViewToStringAsync(string viewName, object model);

}
