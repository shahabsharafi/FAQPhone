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
    public partial class SigninPage : ContentPage
    {
        public SigninPage()
        {
            InitializeComponent();
            var factory = App.Resolve<SigninFactory>();
            BindingContext = factory.Create(this);
        }
    }

    public class SigninFactory
    {
        IAccountService accountService;
        public SigninFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public SigninViewModel Create(ContentPage page)
        {
            return new SigninViewModel(this.accountService, page);
        }
    }

    public class SigninViewModel : BaseViewModel
    {

        public SigninViewModel(IAccountService accountService, ContentPage page) : base(page)
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

        public ICommand SendCommand { protected set; get; }

        public async Task sendCommand()
        {

            await this.Navigation.PopAsync();
        }
    }

}