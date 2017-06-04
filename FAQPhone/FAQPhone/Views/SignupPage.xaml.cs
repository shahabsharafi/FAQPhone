using FAQPhone.Inferstructure;
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
    public partial class SignupPage : ContentPage
    {
        public SignupPage(string mobile)
        {
            InitializeComponent();
            var factory = App.Resolve<SignupViewModelFactory>();
            BindingContext = factory.Create(Navigation, mobile);
        }
    }

    public class SignupViewModelFactory
    {
        IAccountService accountService;
        public SignupViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public SignupViewModel Create(INavigation navigation, string mobile)
        {
            return new SignupViewModel(this.accountService, navigation, mobile);
        }
    }

    public class SignupViewModel : BaseViewModel
    {
        public SignupViewModel(IAccountService accountService, INavigation navigation, string mobile) : base(navigation)
        {
            this.accountService = accountService;
            this.mobile = mobile;
            this.SignupCommand = new Command(async () => await signupCommand());
        }
        private IAccountService accountService { get; set; }
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
        string _confirm;
        public string confirm
        {
            get { return _confirm; }
            set { _confirm = value; OnPropertyChanged(); }
        }
        string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        private string mobile { get; set; }
        public ICommand SignupCommand { protected set; get; }

        public async Task signupCommand()
        {
            /////
            SignupModel model = new SignupModel()
            {
                mobile = this.mobile,
                username = this.username,
                password = this.password,
                email = this.email
            };
            await this.accountService.SignUp(model);
            await this.Navigation.PushAsync(new MainPage());
            
        }
    }
}
