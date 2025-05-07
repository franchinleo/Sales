using Sales.Core.Domain.Entities;
using Sales.Core.Domain.Interfaces;
using Sales.Core.Infra.Interfaces;

namespace Sales.Core.Domain.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ISaleRepository<Sale> _saleRepository;
        public DiscountService(ISaleRepository<Sale> saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public decimal CalculateDiscount(Item item)
        {
            if (item.Quantity > 4)
                return (item.UnitPrice * item.Quantity) * 0.10m;

            if (item.Quantity >= 10 && item.Quantity <= 20)
                return (item.UnitPrice * item.Quantity) * 0.20m;

            if (item.Quantity > 20)
                throw new InvalidOperationException("It's not possible to sell more than 20 identical items.");

            return 0;
        }

        public decimal ApplyDiscounts(List<Item> items)
        {
            decimal totalDiscount = 0;

            foreach (var item in items)
            {
                totalDiscount += CalculateDiscount(item);
            }
       
            return totalDiscount;
        }

    }
}
