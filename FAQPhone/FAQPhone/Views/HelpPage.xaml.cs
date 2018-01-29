using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
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
    public partial class HelpPage : ContentPage
    {
        public HelpPage()
        {
            InitializeComponent();
            var factory = App.Resolve<HelpViewModelFactory>();
            BindingContext = factory.Create(this);
        }
    }

    public class HelpViewModelFactory
    {
        public HelpViewModelFactory()
        {

        }
        public HelpViewModel Create(ContentPage page)
        {
            return new HelpViewModel(page);
        }
    }

    public class HelpViewModel : BaseViewModel
    {
        public HelpViewModel(ContentPage page) : base(page)
        {
            this.Source = ResourceManagerHelper.GetValue(Constants.INFO_URL); ;
            this.NavigatingCommand = new Command(() => navigatingCommand());
            this.NavigatedCommand = new Command(() => navigatedCommand());
        }

        public ICommand NavigatingCommand { protected set; get; }
        public async void navigatingCommand()
        {
            this.IsBusy = true;
        }

        public ICommand NavigatedCommand { protected set; get; }
        public async void navigatedCommand()
        {
            this.IsBusy = false;
        }

        string _Source;
        public string Source
        {
            get { return _Source; }
            set { _Source = value; OnPropertyChanged(); }
        }
    }
}


