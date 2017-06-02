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
        public async Task<string> SendActivation(string mobile)
        {
            var result = await this.get<ResultModel>(string.Format("sendactivation/{0}", mobile));
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
            return Application.Current.Properties.Any(o => o.Key == "token" && o.Value != null);
        }

        public string GetToken()
        {
            string token = "";
            object obj;
            if (Application.Current.Properties.TryGetValue("token", out obj))
            {
                token = obj.ToString();
            }
            return token;
        }

        private static void setAutenticationInfo(AuthenticationResultModel info)
        {
            Application.Current.Properties["username"] = info?.username;
            Application.Current.Properties["token"] = info?.token;
            Application.Current.Properties["firstName"] = info?.firstName;
            Application.Current.Properties["lastName"] = info?.lastName;
        }        
    }
}
