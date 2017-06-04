using FAQPhone.Inferstructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services
{
    public class AccountService: RestService<AccountModel>, IAccountService
    {
        public AccountService(): base()
        {
            this._relativeUrl = "accounts/{0}";
        }

        public async Task<CodeResultModel> SendCode(string mobile)
        {
            var url = this.getUrl(string.Format("sendcode/{0}", mobile));
            var result = await this.get<CodeResultModel>(url);
            return result;
        }

        public async Task<bool> SignUp(AccountChangeModel model)
        {
            var url = this.getUrl("signup");
            await this.post<AccountChangeModel>(url, model);
            SigninModel m = new SigninModel()
            {
                username = model.username,
                password = model.password
            };
            return await this.SignIn(m);
        }

        public async Task<bool> SignIn(SigninModel model)
        {
            string url = string.Format(Constants.RestUrl, "accounts/authenticate");
            var result = await this.post<SigninModel, AutResultModel>(url, model);
            if (result.success == true)
            {
                setAutenticationInfo(result);
                return true;
            }
            return false;
        }

        public Task<bool> ResetPasswordIn(AccountChangeModel model)
        {
            throw new NotImplementedException();
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
