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
    }
    public class AccountModel
    {
        public string _id { get; set; }
        public string username { get; set; }
        public ProfileModel profile { get; set; }

        public string DisplayName
        {
            get
            {
                return this.profile.firstName + " " + this.profile.lastName;
            }
        }
    }
}
