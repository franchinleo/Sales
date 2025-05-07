using Moq;
using Sales.Core.Domain.Entities;
using Sales.Core.Domain.Services;
using Sales.Core.Infra.Interfaces;
using Xunit;

namespace Sales.Core.Tests
{
    public class DiscountServiceTests
    {
        private readonly Mock<ISaleRepository<Sale>> _saleRepositoryMock;
        private readonly DiscountService _discountService;

        public DiscountServiceTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository<Sale>>();
            _discountService = new DiscountService(_saleRepositoryMock.Object);
        }

        [Fact]
        public void CalculateDiscount_ShouldApply10PercentDiscount_WhenQuantityIsGreaterThan4()
        {
            // Arrange
            var item = new Item
            {
                Quantity = 5,
                UnitPrice = 10.0m
            };

            // Act
            var discount = _discountService.CalculateDiscount(item);

            // Assert
            Assert.Equal(5.0m, discount); // 10% de desconto em 5 * 10
        }

        [Fact]
        public void CalculateDiscount_ShouldApply20PercentDiscount_WhenQuantityIsBetween10And20()
        {
            // Arrange
            var item = new Item
            {
                Quantity = 15,
                UnitPrice = 10.0m
            };

            // Act
            var discount = _discountService.CalculateDiscount(item);

            // Assert
            Assert.Equal(30.0m, discount); // 20% de desconto em 15 * 10
        }

        [Fact]
        public void CalculateDiscount_ShouldThrowException_WhenQuantityIsGreaterThan20()
        {
            // Arrange
            var item = new Item
            {
                Quantity = 25,
                UnitPrice = 10.0m
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _discountService.CalculateDiscount(item));
            Assert.Equal("It's not possible to sell more than 20 identical items.", exception.Message);
        }

        [Fact]
        public void CalculateDiscount_ShouldReturnZero_WhenQuantityIsLessThanOrEqualTo4()
        {
            // Arrange
            var item = new Item
            {
                Quantity = 3,
                UnitPrice = 10.0m
            };

            // Act
            var discount = _discountService.CalculateDiscount(item);

            // Assert
            Assert.Equal(0.0m, discount); // Sem desconto para quantidade <= 4
        }

        [Fact]
        public void ApplyDiscounts_ShouldReturnTotalDiscount_ForMultipleItems()
        {
            // Arrange
            var items = new List<Item>
            {
                new Item { Quantity = 5, UnitPrice = 10.0m },  // 10% de desconto
                new Item { Quantity = 15, UnitPrice = 20.0m }, // 20% de desconto
                new Item { Quantity = 3, UnitPrice = 30.0m }   // Sem desconto
            };

            // Act
            var totalDiscount = _discountService.ApplyDiscounts(items);

            // Assert
            Assert.Equal(65.0m, totalDiscount); // (5 * 10 * 0.10) + (15 * 20 * 0.20) + 0
        }

        [Fact]
        public void ApplyDiscounts_ShouldReturnZero_WhenNoItemsProvided()
        {
            // Arrange
            var items = new List<Item>();

            // Act
            var totalDiscount = _discountService.ApplyDiscounts(items);

            // Assert
            Assert.Equal(0.0m, totalDiscount); // Sem itens, sem desconto
        }
    }
}
