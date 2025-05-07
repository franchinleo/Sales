using Sales.Core.Domain.Entities;

namespace Sales.Core.Domain.Interfaces
{
    public interface IDiscountService
    {
        decimal CalculateDiscount(Item item);
        decimal ApplyDiscounts(List<Item> items);
    }
}
