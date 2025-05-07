using System;
using System.Collections.Generic;

namespace Sales.Core.Application.DTOs
{
    public class CreateSaleDto
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; } 
        public string Customer { get; set; }
        public string Branch { get; set; } 
        public List<CreateSaleItemDto> Itens { get; set; } = new(); 
    }

    public class CreateSaleItemDto
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
    }
}
