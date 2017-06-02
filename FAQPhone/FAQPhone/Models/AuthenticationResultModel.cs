using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class AuthenticationResultModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
