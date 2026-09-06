using Inventory.Domain.Common;
using Inventory.Domain.Entities;

namespace InventorySystem.Domain.Entities;

public class Product : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public decimal UnitPrice { get; set; }
    public int MinStock { get; set; } = 0;
    public int MaxStock { get; set; } = 100;
    public string? Barcode { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; } = true;
    public string? SKU { get; set; }
    public decimal? Weight { get; set; }
    public string? UnitOfMeasure { get; set; }

    // Navigation Properties
    public virtual Category? Category { get; set; }
    public virtual Supplier? Supplier { get; set; }
    public virtual InventoryItem? InventoryItem { get; set; }  // ← This property exists
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}