using Awesome;
using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services;
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
    public partial class ConnectingPage : ContentPage
    {
        public ConnectingPage()
        {
            InitializeComponent();
            var factory = App.Resolve<ConnectingViewModelFactory>();
            BindingContext = factory.Create(this);            
        }
    }

    public class ConnectingViewModelFactory
    {
        IAccountService accountService;
        public ConnectingViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public ConnectingViewModel Create(ContentPage page)
        {
            return new ConnectingViewModel(accountService, page);
        }
    }

    public class ConnectingViewModel : BaseViewModel
    {
        public ConnectingViewModel(IAccountService accountService, ContentPage page) : base(page)
        {
            this.accountService = accountService;
            NotConnected = true;
            CommandCaption = ResourceManagerHelper.GetValue(Constants.COMMAND_CONNECT);
            this.TryCommand = new Command(async () => await tryCommand());
            this.ResetPasswordCommand = new Command(async () => await resetPasswordCommand());
        }

        IAccountService accountService;

        string _StateCaption;
        public string StateCaption
        {
            get { return _StateCaption; }
            set { _StateCaption = value; OnPropertyChanged(); }
        }

        bool _NotConnected;
        public bool NotConnected
        {
            get { return _NotConnected; }
            set { _NotConnected = value; OnPropertyChanged(); }
        }

        string _CommandCaption;
        public string CommandCaption
        {
            get { return _CommandCaption; }
            set { _CommandCaption = value; OnPropertyChanged(); }
        }
        public ICommand ResetPasswordCommand { protected set; get; }
        public async Task resetPasswordCommand()
        {
            await this.RootNavigate(new SendCodePage());
        }

        public ICommand TryCommand { protected set; get; }
        public async Task tryCommand()
        {
            this.StateCaption = FontAwesome.FASpinner;
            NotConnected = false;
            SigninModel model = new SigninModel()
            {
                username = Settings.Username,
                password = Settings.Password
            };
            try
            {
                var flag = await accountService.SignIn(model);
                if (flag)
                {
                    await this.RootNavigate(new MainPage());
                }
                else
                {
                    await this.RootNavigate(new SendCodePage());
                }
            }
            catch (Exception ex)
            {
                this.StateCaption = "";
                NotConnected = true;
                CommandCaption = ResourceManagerHelper.GetValue(Constants.COMMAND_TRY);
            }            
        }
    }
}
