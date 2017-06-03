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

        Task SignUp(SignupModel model);

        Task SignIn(SigninModel model);

        void SignOut();

        bool IsAuthenticated();
    }
}
