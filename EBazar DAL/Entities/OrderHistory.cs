using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Entities
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Username { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }

    }
}
