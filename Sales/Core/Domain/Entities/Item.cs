namespace Sales.Core.Domain.Entities
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; } 
        public decimal UnitPrice { get; set; } 
        public decimal Discount { get; set; } 
        public decimal TotalItemAmount => (UnitPrice * Quantity) - Discount;
        
    }
}
