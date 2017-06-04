using FAQPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services.Interfaces
{
    public interface IAccountService: IRestService<Account>
    {
        Task<string> SendCode(string mobile);

        Task<bool> SignUp(SignupModel model);

        Task<bool> SignIn(SigninModel model);

        void SignOut();

        bool IsAuthenticated();
    }
}
