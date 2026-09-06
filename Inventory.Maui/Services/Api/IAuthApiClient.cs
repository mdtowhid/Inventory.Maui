using Refit;
using InventorySystem.Application.Auth;

namespace Inventory.Maui.Services.Api;

public interface IAuthApiClient
{
    [Post("/auth/login")]
    Task<IApiResponse<LoginResponse>> LoginAsync([Body] LoginRequest request);

    [Post("/auth/refresh")]
    Task<IApiResponse<LoginResponse>> RefreshTokenAsync([Body] RefreshTokenRequest request);

    [Post("/auth/logout")]
    Task<IApiResponse> LogoutAsync();
}