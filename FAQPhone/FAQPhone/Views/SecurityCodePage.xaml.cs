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
    public partial class SecurityCodePage : ContentPage
    {
        public SecurityCodePage(string flow, string mobile)
        {
            InitializeComponent();
            BindingContext = new SecurityCodeViewModel(Navigation, flow, mobile);
        }
    }

    public class SecurityCodeViewModel : BaseViewModel
    {
        public SecurityCodeViewModel(INavigation navigation, string flow, string mobile) : base(navigation)
        {
            this.flow = flow;
            this.mobile = mobile;
            this.CheckCodeCommand = new Command(async () => await checkCodeCommand());
        }
        private string flow { get; set; }
        private string mobile { get; set; }
        string _activation;
        public string activation
        {
            get { return _activation; }
            set { _activation = value; OnPropertyChanged(); }
        }
        public ICommand CheckCodeCommand { protected set; get; }

        public async Task checkCodeCommand()
        {
            /////
            if (this.flow == "signup")
            {
                await this.Navigation.PushAsync(new SignupPage(this.mobile));
            }
            else if (this.flow == "forgetpassword")
            {
                await this.Navigation.PushAsync(new ResetPasswordPage(this.mobile));
            }
        }
    }
}
