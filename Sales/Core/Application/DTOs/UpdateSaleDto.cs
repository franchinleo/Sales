namespace Sales.Core.Application.DTOs
{
    public class UpdateSaleDto
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; } 
        public string Customer { get; set; }
        public string Branch { get; set; }
        public List<CreateSaleItemDto> Products { get; set; } = [];
    }
     
    public class UpdateSaleItemDto
    {
        public string ProductName { get; set; } 
        public int Quantity { get; set; } 
        public decimal UnitPrice { get; set; } 
        public decimal Discount { get; set; } 
    }
}
