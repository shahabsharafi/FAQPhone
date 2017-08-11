using FAQPhone.Inferstructure;
using FAQPhone.Infrastructure;
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
    public partial class AccountPage : ContentPage
    {
        public AccountPage(AccountModel model)
        {
            InitializeComponent();
            var factory = App.Resolve<AccountViewModelFactory>();
            var vm = factory.Create(this, model);
            this.Appearing += (sender, e) => {
                Task.Run(async () => await vm.Load()).Wait();
            };
            BindingContext = vm;
        }
    }

    public class AccountViewModelFactory
    {
        IAccountService accountService;
        public AccountViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public AccountViewModel Create(ContentPage page, AccountModel model)
        {
            return new AccountViewModel(this.accountService, page, model);
        }
    }

    public class AccountViewModel : BaseViewModel
    {

        public AccountViewModel(IAccountService accountService, ContentPage page, AccountModel model) : base(page)
        {
            this.accountService = accountService;
            this.EditCommand = new Command(async () => await editCommand());
            this.model = model;            
        }

        bool _loaded = false;
        public async Task Load()
        {
            if (this._loaded)
            {
                var m = await this.accountService.GetMe();
                this.setFields(m);
            }
            else
            {
                this._loaded = true;
                this.setFields(this.model);
            }
        }

        void setFields(AccountModel model)
        {
            if (model.profile != null)
            {
                this.fullName = model.profile.firstName + " " + model.profile.lastName;
            }
            this.mobile = model.mobile;
            this.email = model.email;
            this.credit = 
                ResourceManagerHelper.GetValue("account_credit") + ":" + 
                (model.credit).ToString() + " " + 
                ResourceManagerHelper.GetValue("unit_of_mony_caption");
        }

        private IAccountService accountService { get; set; }
        AccountModel model { get; set; }

        string _fullName;
        public string fullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }

        string _mobile;
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; OnPropertyChanged(); }
        }

        string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        string _credit;
        public string credit
        {
            get { return _credit; }
            set { _credit = value; OnPropertyChanged(); }
        }

        public ICommand EditCommand { protected set; get; }

        public async Task editCommand()
        {            
            await this.Navigation.PushAsync(new ProfilePage(this.model));
        }
    }
}

