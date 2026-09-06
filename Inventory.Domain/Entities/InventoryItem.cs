using Inventory.Domain.Common;
using Inventory.Domain.Entities;

namespace Inventory.Domain.Entities;

public class InventoryItem : BaseEntity
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableQuantity => Quantity - ReservedQuantity;
    public string? WarehouseLocation { get; set; }
    public string? BinLocation { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual Product? Product { get; set; }
    public virtual ICollection<InventoryMovement> Movements { get; set; } = new List<InventoryMovement>();
}