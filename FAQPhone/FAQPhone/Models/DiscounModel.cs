using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class DiscountModel
    {
        public string _id { get; set; }
        public AccountModel owner { get; set; }
        public DepartmentModel category { get; set; }
        public decimal price { get; set; }
        public int count { get; set; }
        public decimal total { get; set; }
        public decimal used { get; set; }
        public AttributeModel type { get; set; }
        public DateTime beginDate { get; set; }
        public string Caption { get; set; }
    }
}
