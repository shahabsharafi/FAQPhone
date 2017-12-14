using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecurityCodePage : ContentPage
    {
        public SecurityCodePage(string mobile, CodeResultModel codeResult)
        {
            InitializeComponent();
            var factory = App.Resolve<SecurityCodeViewModelFactory>();
            BindingContext = factory.Create(this, mobile, codeResult);
        }
    }

    public class SecurityCodeViewModelFactory
    {
        IAccountService accountService;
        public SecurityCodeViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public SecurityCodeViewModel Create(ContentPage page, string mobile, CodeResultModel codeResult)
        {
            return new SecurityCodeViewModel(this.accountService, page, mobile, codeResult);
        }
    }

    public class SecurityCodeViewModel : BaseViewModel
    {
        public SecurityCodeViewModel(IAccountService accountService, ContentPage page, string mobile, CodeResultModel codeResult) : base(page)
        {
            this.accountService = accountService;
            this.codeResult = codeResult;
            this.mobile = mobile;
            this.CheckCodeCommand = new Command(async () => await checkCodeCommand());
        }
        IAccountService accountService { get; set; }
        private CodeResultModel codeResult { get; set; }
        private string mobile { get; set; }
        string _securitycode;
        public string securitycode
        {
            get { return _securitycode; }
            set { _securitycode = value; OnPropertyChanged(); }
        }
        public ICommand CheckCodeCommand { protected set; get; }

        public async Task checkCodeCommand()
        {
            /////
            string securitycode = this.securitycode.ToEnglishNumber();
            if (securitycode == this.codeResult.code)
            {
                if (this.codeResult.username == "")
                {
                    //await this.RootNavigate(new SignupPage(this.mobile, this.codeResult.code));
                    AccountChangeModel model = new AccountChangeModel()
                    {
                        code = this.codeResult.code,
                        mobile = this.mobile,
                        username = this.mobile,
                        password = this.codeResult.code
                    };
                    if (await this.accountService.SignUp(model))
                    {
                        if (Utility.CompareVersion() >= 0)
                        {
                            Settings.Username = this.mobile;
                            Settings.Password = this.codeResult.code;
                            await RootNavigate(new MainPage());
                        }
                        else
                        {
                            this.message = "message_unsuported_version";
                        }
                        
                    }
                    else
                    {
                        this.message = "err_securitycode_failed";
                    }
                }
                else
                {
                    AccountChangeModel model = new AccountChangeModel()
                    {
                        code = this.codeResult.code,
                        mobile = this.mobile,
                        username = this.codeResult.username,
                        password = this.codeResult.code
                    };
                    if (await this.accountService.ResetPassword(model))
                    {
                        if (Utility.CompareVersion() >= 0)
                        {
                            Settings.Username = this.codeResult.username;
                            Settings.Password = this.codeResult.code;
                            await RootNavigate(new MainPage());
                        }
                        else
                        {
                            this.message = "message_unsuported_version";
                        }
                    }
                    else
                    {
                        this.message = "err_securitycode_failed";
                    }
                }
            }
            else
            {
                this.message = "err_securitycode_notmatch";
            }
        }
    }
}
