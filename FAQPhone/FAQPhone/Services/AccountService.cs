using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
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

        public async Task<ResultModel> SetUserKey()
        {
            var url = this.getUrl("setuserkey");
            var result = await this.get<ResultModel>(url);
            return result;
        }

        public async Task<List<AccountModel>> GetOperatoreList()
        {
            //var url = string.Format(Constants.RestUrl, "accounts?$filter=isOperator eq 'true'");
            var url = this.getUrl("online");
            var result = await this.get<List<AccountModel>>(url);
            return result;
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
            AutResultModel result = await this.post<AccountChangeModel, AutResultModel>(url, model);
            if (result.success == true)
            {
                setAutenticationInfo(result);
                return true;
            }
            return false;
        }

        public async Task<bool> SignIn(SigninModel model)
        {
            //string url = string.Format(Constants.RestUrl, "accounts/authenticate");
            var url = this.getUrl("authenticate");
            var result = await this.UnmanagedPost<SigninModel, AutResultModel>(url, model);
            if (result.success == true)
            {
                setAutenticationInfo(result);
                return true;
            }
            return false;
        }

        public async Task<bool> ResetPassword(AccountChangeModel model)
        {
            var url = this.getUrl("resetpassword");
            var result = await this.post<AccountChangeModel, AutResultModel>(url, model);
            if (result.success == true)
            {
                setAutenticationInfo(result);
                return true;                
            }
            return false;
        }

        public void SignOut()
        {
            setAutenticationInfo(null);
        }
        public async Task<AccountModel> GetMe()
        {
            string url = this.getUrl("me");
            return await this.get<AccountModel>(url);
        }

        public async Task<ResultModel> GetVersion()
        {
            string url = this.getUrl("version");
            return await this.get<ResultModel>(url);
        }

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(App.Token);
        }

        private static void setAutenticationInfo(AutResultModel info)
        {
            App.SuportVersion = info?.suportVersion;
            App.Blocked = info?.blocked ?? false;
            App.Username = info?.username;
            App.Token = info?.token;
            App.Access = info?.access;
        }

        public async Task<AccountModel> GetByUsername(string username)
        {
            string url = this.getUrl(string.Format("byusername/{0}", username));
            return await this.get<AccountModel>(url);
        }
    }
}
