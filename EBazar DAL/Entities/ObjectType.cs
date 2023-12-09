using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Entities
{
    public class ObjectType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
