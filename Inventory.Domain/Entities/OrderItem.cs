using Inventory.Domain.Common;
using Inventory.Domain.Entities;

namespace Inventory.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal TotalPrice { get; set; }
    public string? Notes { get; set; }

    // Navigation Properties
    public virtual Order? Order { get; set; }
    public virtual Product? Product { get; set; }
}