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
            this._relativeUrl = "accounts/{0}";
        }

        public async Task<string> SendCode(string mobile)
        {
            var url = this.getUrl(string.Format("sendcode/{0}", mobile));
            var result = await this.get<ResultModel>(url);
            return result.data;
        }

        public async Task SignUp(SignupModel model)
        {
            await this.post<SignupModel>("signup", model);
            setAutenticationInfo(null);
        }

        public async Task SignIn(SigninModel model)
        {
            var result = await this.post<SigninModel, AutResultModel>("signin", model);
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

        private static void setAutenticationInfo(AutResultModel info)
        {
            App.Bag.username = info?.username;
            App.Bag.token = info?.token;
            App.Bag.firstName = info?.firstName;
            App.Bag.lastName = info?.lastName;
        }
    }
}
