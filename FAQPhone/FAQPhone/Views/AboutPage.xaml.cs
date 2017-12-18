using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
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
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            var factory = App.Resolve<AboutViewModelFactory>();
            BindingContext = factory.Create(this);
        }
    }

    public class AboutViewModelFactory
    {
        public AboutViewModelFactory()
        {
            
        }
        public AboutViewModel Create(ContentPage page)
        {
            return new AboutViewModel(page);
        }
    }

    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel(ContentPage page) : base(page)
        {     
        }
    }
}


