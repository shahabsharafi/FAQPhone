using FAQPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> SendCode(string mobile);

        Task SignUp(SignupModel model);

        Task SignIn(SigninModel model);

        void SignOut();

        bool IsAuthenticated();

        string GetToken();
    }
}
