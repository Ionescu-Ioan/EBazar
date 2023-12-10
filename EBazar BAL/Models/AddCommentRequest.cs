using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Models
{
    public class AddCommentRequest
    {
        public string username { get; set; }
        public int productId { get; set; }
        public string text { get; set; }
    }
}
