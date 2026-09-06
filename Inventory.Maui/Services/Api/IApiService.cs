using Refit;

namespace Inventory.Maui.Services.Api;

public interface IApiService
{
    IProductApiClient Products { get; }
    IInventoryApiClient Inventory { get; }
    IOrderApiClient Orders { get; }
    IAuthApiClient Auth { get; }
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private IProductApiClient? _productApi;
    private IInventoryApiClient? _inventoryApi;
    private IOrderApiClient? _orderApi;
    private IAuthApiClient? _authApi;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public IProductApiClient Products =>
        _productApi ??= RestService.For<IProductApiClient>(_httpClient);

    public IInventoryApiClient Inventory =>
        _inventoryApi ??= RestService.For<IInventoryApiClient>(_httpClient);

    public IOrderApiClient Orders =>
        _orderApi ??= RestService.For<IOrderApiClient>(_httpClient);

    public IAuthApiClient Auth =>
        _authApi ??= RestService.For<IAuthApiClient>(_httpClient);
}