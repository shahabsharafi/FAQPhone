using FAQPhone.Inferstructure;
using FAQPhone.Infrastructure;
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
    public partial class SendCodePage : ContentPage
    {
        public SendCodePage(FlowType flow)
        {
            InitializeComponent();
            var factory = App.Resolve<SendCodeViewModelFactory>();
            BindingContext = factory.Create(Navigation, flow);
        }
    }
    
    public class SendCodeViewModelFactory
    {
        IAuthenticationService authenticationService;
        public SendCodeViewModelFactory(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        public SendCodeViewModel Create(INavigation navigation, FlowType flow)
        {
            return new SendCodeViewModel(this.authenticationService, navigation, flow);
        }
    }
    
    public class SendCodeViewModel : BaseViewModel
    {
        public SendCodeViewModel(IAuthenticationService authenticationService, INavigation navigation, FlowType flow) : base(navigation)
        {
            this.authenticationService = authenticationService;
            this.flow = flow;
            this.SendCodeCommand = new Command(async () => await sendCodeCommand());
        }
        private IAuthenticationService authenticationService { get; set; }
        private FlowType flow { get; set; }
        string _mobile;
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; OnPropertyChanged(); }
        }
        public ICommand SendCodeCommand { protected set; get; }

        public async Task sendCodeCommand()
        {
            /////
            //string code = await this.authenticationService.SendCode(this.mobile);            
            string code = "123456";
            await this.Navigation.PushAsync(new SecurityCodePage(this.flow, this.mobile, code));
        }
    }
}
