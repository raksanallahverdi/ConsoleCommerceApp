using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Order:BaseEntity
    {
      
        public int ProductQuantity { get; set; }
        public int ProductId { get; set; }
        public int SellerId {  get; set; }
        public int CustomerId { get; set; }
        public Seller Seller { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public decimal TotalPrice {  get; set; }
       
        
    }
}
