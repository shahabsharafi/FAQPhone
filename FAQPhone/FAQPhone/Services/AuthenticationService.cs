using FAQPhone.Inferstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
        public async Task<string> SendCode(string mobile)
        {
            var result = await this.get<ResultModel>(string.Format("sendcode/{0}", mobile));
            return result.data;
        }

        public async Task SignUp(SignupModel model)
        {
            await this.post<SignupModel>("signup", model);
            setAutenticationInfo(null);
        }

        public async Task SignIn(SigninModel model)
        {
            var result = await this.post<SigninModel, AuthenticationResultModel>("signin", model);
            setAutenticationInfo(result);
        }

        public void SignOut()
        {
            setAutenticationInfo(null);
        }

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(App.Bag.token);
        }

        private static void setAutenticationInfo(AuthenticationResultModel info)
        {
            App.Bag.username = info?.username;
            App.Bag.token  = info?.token;
            App.Bag.firstName  = info?.firstName;
            App.Bag.lastName  = info?.lastName;
        }        
    }
}
