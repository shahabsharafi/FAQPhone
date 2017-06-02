using FAQPhone.Inferstructure;
using FAQPhone.Infrastructure;
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
    public partial class SigninPage : ContentPage
    {
        public SigninPage()
        {
            InitializeComponent();
            BindingContext = new SigninViewModel(Navigation);
        }
    }

    public class SigninViewModel : BaseViewModel
    {
        public SigninViewModel(INavigation navigation) : base(navigation)
        {
            this.SigninCommand = new Command(async () => await signinCommand());
            this.SignupCommand = new Command(async () => await signupCommand());
            this.ForgetPasswordCommand = new Command(async () => await forgetPasswordCommand());
        }
        string _username;
        public string username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        string _password;
        public string password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        public ICommand SigninCommand { protected set; get; }

        public async Task signinCommand()
        {
            /////
            await this.Navigation.PushAsync(new MainPage());
        }

        public ICommand SignupCommand { protected set; get; }

        public async Task signupCommand()
        {
            /////
            await this.Navigation.PushAsync(new SendCodePage(FlowType.Signup));
        }

        public ICommand ForgetPasswordCommand { protected set; get; }

        public async Task forgetPasswordCommand()
        {
            /////
            await this.Navigation.PushAsync(new SendCodePage(FlowType.ForgetPassword));
        }
    }
}
