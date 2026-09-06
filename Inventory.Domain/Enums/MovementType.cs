namespace Inventory.Domain.Enums;

public enum MovementType
{
    Purchase = 1,
    Sale = 2,
    Adjustment = 3,
    Return = 4,
    Transfer = 5,
    StockTakeAdjustment = 6,
    Damage = 7,
    Expired = 8
}