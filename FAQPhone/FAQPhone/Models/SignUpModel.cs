using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class SignUpModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string confirm { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
    }
}
