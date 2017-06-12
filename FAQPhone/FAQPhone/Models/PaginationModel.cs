using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class PaginationModel<T>
    {
        public T[] docs { get; set; }
        public int total { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
    }
}
