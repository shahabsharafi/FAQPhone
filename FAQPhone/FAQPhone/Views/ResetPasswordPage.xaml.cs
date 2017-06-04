using FAQPhone.Inferstructure;
using FAQPhone.Models;
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
            BindingContext = new ResetPasswordPageViewModel(Navigation, mobile, codeResult);
        }
    }

    class ResetPasswordPageViewModel : BaseViewModel
    {

        public ResetPasswordPageViewModel(INavigation navigation, string mobile, CodeResultModel codeResult) : base (navigation)
        {
            this.codeResult = codeResult;
            this.mobile = mobile;
            this.ResetPasswordCommand = new Command(async () => await resetPasswordCommand());
        }

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
            await this.Navigation.PushAsync(new MainPage());

        }
    }
}
