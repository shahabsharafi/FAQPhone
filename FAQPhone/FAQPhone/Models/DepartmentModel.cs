using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class DepartmentModel
    {
        public string _id { get; set; }
        public string type { get; set; }
        public string caption { get; set; }
        public string parentId { get; set; }
        public string description { get; set; }
        public string language { get; set; }
        public long price { get; set; }
        public DepartmentModel[] children { get; set; }
        public string[] accounts { get; set; }
    }
}
