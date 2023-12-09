using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Models
{
    public class AddToCartRequest
    {
        public string Username { get; set; }
        public string ProductId { get; set; }
        public string Quantity { get; set; }
    }
}

