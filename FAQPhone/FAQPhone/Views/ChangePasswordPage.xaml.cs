using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
            var factory = App.Resolve<ChangePasswordFactory>();
            BindingContext = factory.Create(this);
        }
    }

    public class ChangePasswordFactory
    {
        IAccountService accountService;
        public ChangePasswordFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public ChangePasswordViewModel Create(ContentPage page)
        {
            return new ChangePasswordViewModel(this.accountService, page);
        }
    }

    public class ChangePasswordViewModel : BaseViewModel
    {

        public ChangePasswordViewModel(IAccountService accountService, ContentPage page) : base(page)
        {
            this.accountService = accountService;
            this.SendCommand = new Command(async () => await sendCommand());
        }
        private IAccountService accountService { get; set; }

        string _password;
        public string password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        string _confirm;
        public string confirm
        {
            get { return _confirm; }
            set
            {
                _confirm = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendCommand { protected set; get; }

        public async Task sendCommand()
        {
            if (this.password == this.confirm)
            {
                Settings.LocalPassword = this.password;
                await this.Navigation.PopAsync();
            }
            else
            {
                await Utility.Alert("message_password_confirmation");
            }
        }
    }

}