using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
