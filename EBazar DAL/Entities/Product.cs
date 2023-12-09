using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int ObjectTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public ObjectType ObjectType { get; set; }
        public ICollection<CartItem> CartItems { get; set; }


    }
}
