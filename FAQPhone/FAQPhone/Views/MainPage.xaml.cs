using FAQPhone.Inferstructure;
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
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var factory = App.Resolve<MainPageViewModelFactory>();
            BindingContext = factory.Create(Navigation);
        }
    }

    public class MainPageViewModelFactory
    {
        IAccountService accountService;
        public MainPageViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public MainPageViewModel Create(INavigation navigation)
        {
            return new MainPageViewModel(this.accountService, navigation);
        }
    }

    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel(IAccountService accountService, INavigation navigation): base (navigation)
        {
            this.CreateFAQCommand = new Command(async () => await createFAQCommand());
            this.ReceiveFAQCommand = new Command(async () => await receiveFAQCommand());
            this.ShowInprogressCommand = new Command(async () => await showInprogressCommand());
            this.ShowArchivedCommand = new Command(async () => await showArchivedCommand());
            this.SignoutCommand = new Command(async () => await signoutCommand());
            this.accountService = accountService;
        }

        IAccountService accountService { get; set; }

        public ICommand CreateFAQCommand { protected set; get; }

        public async Task createFAQCommand()
        {
            /////           
           

        }

        public ICommand ReceiveFAQCommand { protected set; get; }

        public async Task receiveFAQCommand()
        {
            /////           
            

        }

        public ICommand ShowInprogressCommand { protected set; get; }

        public async Task showInprogressCommand()
        {
            /////           
            

        }

        public ICommand ShowArchivedCommand { protected set; get; }

        public async Task showArchivedCommand()
        {
            /////           
           

        }

        public ICommand SignoutCommand { protected set; get; }

        public async Task signoutCommand()
        {
            /////       
            this.accountService.SignOut();
            await this.Navigation.PushAsync(new SigninPage());

        }

    }
}
