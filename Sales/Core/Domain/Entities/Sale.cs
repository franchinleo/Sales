using System;
using System.Collections.Generic;

namespace Sales.Core.Domain.Entities
{
    public class Sale
    {
        public int SaleId { get; set; } 
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; } 
        public decimal TotalAmount { get; set; } 
        public string Branch { get; set; } 
        public List<Item> Itens { get; set; } = []; 
        public bool IsCancelled { get; set; } 
    }

}
