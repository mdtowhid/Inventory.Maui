using InventorySystem.Application.Orders;
using InventorySystem.Application.Products;
using Refit;

namespace Inventory.Maui.Services.Api;

public interface IOrderApiClient
{
    [Get("/orders")]
    Task<IApiResponse<PagedResult<OrderDto>>> GetOrdersAsync(
        [Query] int pageNumber = 1,
        [Query] int pageSize = 20,
        [Query] string? orderNumber = null,
        [Query] int? status = null,
        [Query] DateTime? fromDate = null,
        [Query] DateTime? toDate = null);

    [Get("/orders/{id}")]
    Task<IApiResponse<OrderDto>> GetOrderAsync(int id);
}