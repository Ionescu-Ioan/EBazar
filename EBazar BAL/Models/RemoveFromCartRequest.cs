using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Models
{
    public class RemoveFromCartRequest
    {
        public string Username { get; set; }
        public string ProductId { get; set; }
    }
}
