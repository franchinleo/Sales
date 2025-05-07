using Sales.Core.Application.DTOs;
using Sales.Core.Domain.Entities;
using Sales.Core.Domain.Interfaces;
using Sales.Core.Infra.Interfaces;

namespace Sales.Core.Domain.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository<Sale> _saleRepository;
        private readonly IDiscountService _discountService;
        private readonly ILoggerService _loggerService;
        public SaleService(ISaleRepository<Sale> saleRepository, IDiscountService discountService, ILoggerService loggerService)
        {
            _saleRepository = saleRepository;
            _discountService = discountService;
            _loggerService = loggerService;
        }
        public async Task<Sale> CreateSale(CreateSaleDto createSaleDto)
        {
            ArgumentNullException.ThrowIfNull(createSaleDto);

            if (createSaleDto.Itens == null || createSaleDto.Itens.Count == 0)
                throw new ArgumentException("At least one product must be sold");


            var sale = new Sale
            {
                SaleDate = createSaleDto.SaleDate,
                Customer = createSaleDto.Customer,
                Branch = createSaleDto.Branch,
                Itens = createSaleDto.Itens.Select(item => new Item
                {
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                }).ToList(),
                TotalAmount = createSaleDto.Itens.Sum(item => (item.UnitPrice * item.Quantity) - item.Discount),
                IsCancelled = false
            };

            sale.Itens.Select(i => i.Discount = _discountService.ApplyDiscounts(sale.Itens));


            var createdSale = await _saleRepository.CreateAsync(sale);
            _loggerService.LogInfo($"Sale created with ID: {createdSale.SaleId}");
            return createdSale;


        }

        public async Task<bool> CancelSale(int saleId)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);

            if (sale == null)
            {
                throw new KeyNotFoundException($"Sale with ID {saleId} not found.");
            }

            sale.IsCancelled = true;
            var canceledSale = await Task.FromResult(_saleRepository.UpdateAsync(sale).Result);
            _loggerService.LogInfo($"Sale {saleId} cancelled");
            return canceledSale;
        }
        public async Task<bool> CancelSaleItem(int saleId,int saleItemId)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);

            if (sale == null)
            {
                throw new KeyNotFoundException($"Sale with ID {saleId} not found.");
            }

            var item = sale.Itens.FirstOrDefault(i => i.ItemId == saleItemId);

            if(item == null)
            {
                throw new KeyNotFoundException($"Item with ID {saleItemId} not found in sale with ID {saleId}.");
            }

            sale.Itens.Remove(item);

            var updatedSale = await Task.FromResult(_saleRepository.UpdateAsync(sale).Result);
            _loggerService.LogInfo($"Item {saleItemId} cancelled in sale {saleId}");
            return updatedSale;

        }
        public async Task<bool> DeleteSale(int id)
        {
            return await Task.FromResult(_saleRepository.DeleteAsync(id).Result);

        }

        public async Task<IEnumerable<Sale>> GetAllSales()
        {
            return await _saleRepository.GetAllAsync();
        }

        public async Task<Sale> GetSaleById(int id)
        {
            return await _saleRepository.GetByIdAsync(id);

        }

        public async Task<bool> UpdateSale(int id, UpdateSaleDto updateSaleDto)
        {
            var sale = new Sale
            {
                SaleDate = updateSaleDto.SaleDate,
                Customer = updateSaleDto.Customer,
                Branch = updateSaleDto.Branch,
                Itens = updateSaleDto.Products.Select(item => new Item
                {
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount
                }).ToList(),
                TotalAmount = updateSaleDto.Products.Sum(item => (item.UnitPrice * item.Quantity) - item.Discount),
                IsCancelled = false
            };

            var updatedSale = await _saleRepository.UpdateAsync(sale);
            _loggerService.LogInfo($"Sale updated with ID: {updateSaleDto.SaleId}");
            return updatedSale;

        }
    }
}
