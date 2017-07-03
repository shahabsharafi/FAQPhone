using FAQPhone.Inferstructure;
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
        public SecurityCodePage(FlowType flow, string mobile, CodeResultModel codeResult)
        {
            InitializeComponent();
            var factory = App.Resolve<SecurityCodeViewModelFactory>();
            BindingContext = factory.Create(Navigation, flow, mobile, codeResult);
        }
    }

    public class SecurityCodeViewModelFactory
    {
        IAccountService accountService;
        public SecurityCodeViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public SecurityCodeViewModel Create(INavigation navigation, FlowType flow, string mobile, CodeResultModel codeResult)
        {
            return new SecurityCodeViewModel(this.accountService, navigation, flow, mobile, codeResult);
        }
    }

    public class SecurityCodeViewModel : BaseViewModel
    {
        public SecurityCodeViewModel(IAccountService accountService, INavigation navigation, FlowType flow, string mobile, CodeResultModel codeResult) : base(navigation)
        {
            this.accountService = accountService;
            this.codeResult = codeResult;
            this.flow = flow;
            this.mobile = mobile;
            this.CheckCodeCommand = new Command(async () => await checkCodeCommand());
        }
        IAccountService accountService { get; set; }
        private CodeResultModel codeResult { get; set; }
        private FlowType flow { get; set; }
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
            if (this.securitycode == this.codeResult.code)
            {
                if (this.flow == FlowType.Signup)
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
                        await this.accountService.SignUp(model);
                        await RootNavigate(new MainPage());
                    }
                    else
                    {
                        this.message = "err_securitycode_userexists";
                    }
                }
                else if (this.flow == FlowType.ForgetPassword)
                {
                    if (this.codeResult.username != "")
                    {
                        await this.RootNavigate(new ResetPasswordPage(this.mobile, this.codeResult));
                    }
                    else
                    {
                        this.message = "err_securitycode_mobilenotfound";
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
