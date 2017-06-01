using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services
{
    public class AccountService: RestService<Account>, IAccountService
    {
        public AccountService(): base()
        {
            this._relativeUrl = "";
        }
    }
}
