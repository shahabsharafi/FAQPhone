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
    public partial class ResetPasswordPage : ContentPage
    {
        public ResetPasswordPage(string mobile, CodeResultModel codeResult)
        {
            InitializeComponent();
            var factory = App.Resolve<ResetPasswordPageViewModelFactory>();
            BindingContext = factory.Create(Navigation, mobile, codeResult);
        }
    }

    public class ResetPasswordPageViewModelFactory
    {
        IAccountService accountService;
        public ResetPasswordPageViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public ResetPasswordPageViewModel Create(INavigation navigation, string mobile, CodeResultModel codeResult)
        {
            return new ResetPasswordPageViewModel(this.accountService, navigation, mobile, codeResult);
        }
    }

    public class ResetPasswordPageViewModel : BaseViewModel
    {

        public ResetPasswordPageViewModel(IAccountService accountService, INavigation navigation, string mobile, CodeResultModel codeResult) : base (navigation)
        {
            this.accountService = accountService;
            this.codeResult = codeResult;
            this.mobile = mobile;
            this.ResetPasswordCommand = new Command(async () => await resetPasswordCommand());
        }
        private IAccountService accountService { get; set; }
        private string mobile { get; set; }

        CodeResultModel codeResult { get; set; }

        string _password;
        public string password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand ResetPasswordCommand { get; }

        public async Task resetPasswordCommand()
        {
            /////
            AccountChangeModel model = new AccountChangeModel()
            {
                code = codeResult.code,
                mobile = this.mobile,
                password = this.password
            };
            bool flag = await this.accountService.ResetPassword(model);
            if (flag)
            {
                await RootNavigate<MainPage>();
            }

        }
    }
}
