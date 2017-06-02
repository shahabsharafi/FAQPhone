using FAQPhone.Inferstructure;
using FAQPhone.Infrastructure;
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
        public SecurityCodePage(FlowType flow, string mobile, string code)
        {
            InitializeComponent();
            BindingContext = new SecurityCodeViewModel(Navigation, flow, mobile, code);
        }
    }

    

    public class SecurityCodeViewModel : BaseViewModel
    {
        public SecurityCodeViewModel(INavigation navigation, FlowType flow, string mobile, string code) : base(navigation)
        {
            this.code = code;
            this.flow = flow;
            this.mobile = mobile;
            this.CheckCodeCommand = new Command(async () => await checkCodeCommand());
        }
        private string code { get; set; }
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
            if (this.securitycode == this.code)
            {
                if (this.flow == FlowType.Signup)
                {
                    await this.Navigation.PushAsync(new SignupPage(this.mobile));
                }
                else if (this.flow == FlowType.ForgetPassword)
                {
                    await this.Navigation.PushAsync(new ResetPasswordPage(this.mobile));
                }
            }
            else
            {
                this.message = "err_securitycode";
            }
        }
    }
}
