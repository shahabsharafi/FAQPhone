using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class ProfileModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fatherName { get; set; }
        public string no { get; set; }
        public string placeOfIssues { get; set; }
        public string nationalCode { get; set; }
        public string birthPlace { get; set; }
    }
    public class Contact {
        public string house { get; set; }
        public string work { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string pcode { get; set; }
    }
    public class AccountModel
    {
        public string _id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string sms { get; set; }
        public ProfileModel profile { get; set; }
        public Contact contact { get; set; }
    }
}
