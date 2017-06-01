using FAQPhone.Inferstructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services
{
    public class AuthenticationService: BaseRestService, IAuthenticationService
    {
        string _relativeUrl { get; set; }
        public AuthenticationService(): base()
        {
            this._relativeUrl = "token/";
        }
        private string getUrl(string param = "")
        {
            return string.Format(Constants.RestUrl, string.Format(this._relativeUrl, param));
        }
        public async Task SignUp(SignupModel model)
        {
            await this.post<SignupModel>("signup", model);
        }

        public async Task Activate(ActivateModel model)
        {
            await this.post<ActivateModel>("activate", model);
        }

        public async Task SignIn(SigninModel model)
        {
            await this.post<SigninModel>("signin", model);
        }

        public void SignOut()
        {
            
        }
    }
}
