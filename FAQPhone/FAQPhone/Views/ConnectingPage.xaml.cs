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
            var languageList = new string[] { "Fa", "Ar" };
            this.LanguageList = new ObservableCollection<AttributeModel>();
            foreach (var item in languageList)
            {
                string key = string.Format("setting_language_{0}", item.ToLower());
                string val = ResourceManagerHelper.GetValue(key);
                this.LanguageList.Add(new AttributeModel() { caption = val, _id = key });
            }
        }

        private ObservableCollection<AttributeModel> _LanguageList;
        public ObservableCollection<AttributeModel> LanguageList
        {
            get { return _LanguageList; }
            set { _LanguageList = value; }
        }

        private AttributeModel _SelectedLanguage;
        public AttributeModel SelectedLanguage
        {
            get { return _SelectedLanguage; }
            set
            {
                _SelectedLanguage = value;
                OnPropertyChanged();
            }
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
            if (!string.IsNullOrEmpty(this.SelectedLanguage?._id))
            {
                Settings.Language = this.SelectedLanguage._id;
            }
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
                    var p = new MainPage();
                    await this.RootNavigate(p);
                    //var list = await this.messageService.GetNewMessages();
                    //if (list != null && list.Count > 0)
                    //{
                        await p.Navigation.PushAsync(new MessagePage(true));
                    //}                   
                }
                else
                {
                    await this.RootNavigate(new SendCodePage());
                }
            }    
        }
    }
}
