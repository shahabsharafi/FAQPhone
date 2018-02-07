using FAQPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services.Interfaces
{
    public interface IAccountService: IRestService<AccountModel>
    {
        Task<List<AccountModel>> GetOperatoreList();
        Task<CodeResultModel> SendCode(string mobile);

        Task<bool> SignUp(AccountChangeModel model);

        Task<bool> SignIn(SigninModel model);
        Task<bool> ResetPassword(AccountChangeModel model);

        void SignOut();
        Task<AccountModel> GetByUsername(string username);
        Task<AccountModel> GetMe();
        Task<ResultModel> GetVersion();

        bool IsAuthenticated();
    }
}
