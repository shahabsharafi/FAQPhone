using FAQPhone.Inferstructure;
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
        public ResetPasswordPage(string mobile)
        {
            InitializeComponent();
            BindingContext = new ResetPasswordPageViewModel(Navigation, mobile);
        }
    }

    class ResetPasswordPageViewModel : BaseViewModel
    {

        public ResetPasswordPageViewModel(INavigation navigation, string mobile) : base (navigation)
        {
            this.mobile = mobile;
            this.ResetPasswordCommand = new Command(async () => await resetPasswordCommand());
        }

        private string mobile { get; set; }

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

            await this.Navigation.PushAsync(new MainPage());

        }
    }
}
