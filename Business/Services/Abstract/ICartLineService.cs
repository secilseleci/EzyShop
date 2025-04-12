namespace Business.Services.Abstract;

public interface ICartLineService
{
    
    Task<int>GetTotalCartLinesAsync(Guid userId);

}
