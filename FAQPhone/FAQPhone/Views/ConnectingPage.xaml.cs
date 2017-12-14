using Awesome;
using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var vm = factory.Create(this);
            this.Appearing += (sender, e) => {
                try
                {
                    vm.tryCommand();
                }
                catch
                {

                }
                //Task.Run(async () => await vm.tryCommand()).Wait();
            };
            BindingContext = vm;            
        }
    }

    public class ConnectingViewModelFactory
    {
        IAccountService accountService;
        IMessageService messageService;
        public ConnectingViewModelFactory(IAccountService accountService, IMessageService messageService)
        {
            this.accountService = accountService;
            this.messageService = messageService;
        }
        public ConnectingViewModel Create(ContentPage page)
        {
            return new ConnectingViewModel(accountService, messageService, page);
        }
    }

    public class ConnectingViewModel : BaseViewModel
    {
        public ConnectingViewModel(IAccountService accountService, IMessageService messageService, ContentPage page) : base(page)
        {
            this.accountService = accountService;
            this.messageService = messageService;
            CommandCaption = ResourceManagerHelper.GetValue(Constants.COMMAND_CONNECT);
            this.TryCommand = new Command(() => tryCommand());
            this.ResetPasswordCommand = new Command(async () => await resetPasswordCommand());            
        }        

        IAccountService accountService;
        IMessageService messageService;

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
        public async void tryCommand()
        {
            this.IsBusy = true;
            SigninModel model = new SigninModel()
            {
                username = Settings.Username,
                password = Settings.Password
            };
            var err = false;
            var flag = false;
            try
            {
                flag = await accountService.SignIn(model);
            }
            catch
            {
                err = true;
                CommandCaption = ResourceManagerHelper.GetValue(Constants.COMMAND_TRY);
            }
            this.IsBusy = false;
            if (!err)
            {
                if (flag)
                {
                    if (Utility.CompareVersion() >= 0)
                    {
                        var p = new MainPage();
                        await this.RootNavigate(p);
                        await p.Navigation.PushAsync(new MessagePage(true));
                    }
                    else
                    {
                        await Utility.Alert("message_unsuported_version");
                    }
                }
                else
                {
                    await this.RootNavigate(new SendCodePage());
                }
            }    
        }
    }
}
