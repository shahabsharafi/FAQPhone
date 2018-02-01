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
    public partial class BrowserPage : ContentPage
    {
        public BrowserPage(string title, string url)
        {
            InitializeComponent();
            var factory = App.Resolve<BrowserViewModelFactory>();
            BindingContext = factory.Create(this, title, url);
        }
    }

    public class BrowserViewModelFactory
    {
        public BrowserViewModelFactory()
        {
            
        }
        public BrowserViewModel Create(ContentPage page, string title, string url)
        {
            return new BrowserViewModel(page, title, url);
        }
    }

    public class BrowserViewModel : BaseViewModel
    {
        public BrowserViewModel(ContentPage page, string title, string url) : base(page)
        {
            this.Title = title;
            this.Source = url;
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

        string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged(); }
        }
    }
}


