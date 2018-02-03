using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class AttributeModel
    {
        public string _id { get; set; }
        public string type { get; set; }
        public string caption { get; set; }
        public string parentId { get; set; }
        public string value { get; set; }
        public override string ToString()
        {
            return caption;
        }
    }
}
