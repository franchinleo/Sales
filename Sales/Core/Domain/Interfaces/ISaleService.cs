using Sales.Core.Application.DTOs;
using Sales.Core.Domain.Entities;

namespace Sales.Core.Domain.Interfaces
{
    public interface ISaleService
    {
        Task<Sale> CreateSale(CreateSaleDto createSaleDto);
        Task<bool> CancelSale(int saleId);
        Task<bool> CancelSaleItem(int saleId, int saleItemId);
        Task<IEnumerable<Sale>> GetAllSales();
        Task<Sale> GetSaleById(int saleId);
        Task<bool> UpdateSale(int saleId, UpdateSaleDto updateSaleDto);
        Task<bool> DeleteSale(int saleId);

    }
}