using Inventory.Application.Inventory;
using Inventory.Application.Products;
using Refit;

namespace Inventory.Maui.Services.Api;

public interface IInventoryApiClient
{
    [Get("/inventory")]
    Task<IApiResponse<PagedResult<InventoryDto>>> GetInventoryAsync(
        [Query] int pageNumber = 1,
        [Query] int pageSize = 20,
        [Query] int? productId = null);

    [Get("/inventory/{productId}")]
    Task<IApiResponse<InventoryDto>> GetInventoryByProductAsync(int productId);

    [Get("/inventory/{productId}/movements")]
    Task<IApiResponse<List<InventoryMovementDto>>> GetMovementsAsync(
        int productId,
        [Query] DateTime? fromDate = null,
        [Query] DateTime? toDate = null);

    [Get("/inventory/low-stock")]
    Task<IApiResponse<List<InventoryDto>>> GetLowStockItemsAsync([Query] int threshold = 10);
}