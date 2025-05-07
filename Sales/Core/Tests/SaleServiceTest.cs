using Moq;
using Sales.Core.Application.DTOs;
using Sales.Core.Domain.Entities;
using Sales.Core.Domain.Interfaces;
using Sales.Core.Domain.Services;
using Sales.Core.Infra.Interfaces;
using Xunit;

namespace Sales.Core.Tests
{
    public class SaleServiceTests
    {
        private readonly Mock<ISaleRepository<Sale>> _saleRepositoryMock;
        private readonly Mock<IDiscountService> _discountServiceMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly SaleService _saleService;

        public SaleServiceTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository<Sale>>();
            _discountServiceMock = new Mock<IDiscountService>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _saleService = new SaleService(
                _saleRepositoryMock.Object,
                _discountServiceMock.Object,
                _loggerServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateSale_ShouldCreateSaleSuccessfully()
        {
            // Arrange
            var createSaleDto = new CreateSaleDto
            {
                SaleDate = DateTime.Now,
                Customer = "John Doe",
                Branch = "Main Branch",
                Itens = new List<CreateSaleItemDto>
                {
                    new CreateSaleItemDto { ItemName = "Product 1", Quantity = 5, UnitPrice = 10.0m }
                }
            };

            var expectedSale = new Sale
            {
                SaleId = 1,
                SaleDate = createSaleDto.SaleDate,
                Customer = createSaleDto.Customer,
                Branch = createSaleDto.Branch,
                Itens = createSaleDto.Itens.Select(i => new Item
                {
                    ItemName = i.ItemName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = 5.0m
                }).ToList(),
                TotalAmount = 45.0m, // (5 * 10) - 5
                IsCancelled = false
            };

            _discountServiceMock
                .Setup(d => d.ApplyDiscounts(It.IsAny<List<Item>>()))
                .Returns(5.0m);

            _saleRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Sale>()))
                .ReturnsAsync(expectedSale);

            // Act
            var result = await _saleService.CreateSale(createSaleDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSale.SaleId, result.SaleId);
            Assert.Equal(expectedSale.TotalAmount, result.TotalAmount);
            _loggerServiceMock.Verify(l => l.LogInfo(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CancelSale_ShouldCancelSaleSuccessfully()
        {
            // Arrange
            var saleId = 1;
            var sale = new Sale
            {
                SaleId = saleId,
                IsCancelled = false
            };

            _saleRepositoryMock
                .Setup(r => r.GetByIdAsync(saleId))
                .ReturnsAsync(sale);

            _saleRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Sale>()))
                .ReturnsAsync(true);

            // Act
            var result = await _saleService.CancelSale(saleId);

            // Assert
            Assert.True(result);
            Assert.True(sale.IsCancelled);
            _loggerServiceMock.Verify(l => l.LogInfo(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CancelSale_ShouldThrowException_WhenSaleNotFound()
        {
            // Arrange
            var saleId = 1;

            _saleRepositoryMock
                .Setup(r => r.GetByIdAsync(saleId))
                .ReturnsAsync((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _saleService.CancelSale(saleId));
        }

        [Fact]
        public async Task CancelSaleItem_ShouldRemoveItemSuccessfully()
        {
            // Arrange
            var saleId = 1;
            var saleItemId = 101;
            var sale = new Sale
            {
                SaleId = saleId,
                Itens = new List<Item>
                {
                    new Item { ItemId = saleItemId, ItemName = "Product 1", Quantity = 2, UnitPrice = 10.0m }
                }
            };

            _saleRepositoryMock
                .Setup(r => r.GetByIdAsync(saleId))
                .ReturnsAsync(sale);

            _saleRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Sale>()))
                .ReturnsAsync(true);

            // Act
            var result = await _saleService.CancelSaleItem(saleId, saleItemId);

            // Assert
            Assert.True(result);
            Assert.Empty(sale.Itens);
            _loggerServiceMock.Verify(l => l.LogInfo(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CancelSaleItem_ShouldThrowException_WhenItemNotFound()
        {
            // Arrange
            var saleId = 1;
            var saleItemId = 101;
            var sale = new Sale
            {
                SaleId = saleId,
                Itens = new List<Item>()
            };

            _saleRepositoryMock
                .Setup(r => r.GetByIdAsync(saleId))
                .ReturnsAsync(sale);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _saleService.CancelSaleItem(saleId, saleItemId));
        }

        [Fact]
        public async Task GetAllSales_ShouldReturnAllSales()
        {
            // Arrange
            var sales = new List<Sale>
            {
                new Sale { SaleId = 1, Customer = "John Doe" },
                new Sale { SaleId = 2, Customer = "Jane Doe" }
            };

            _saleRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(sales);

            // Act
            var result = await _saleService.GetAllSales();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
