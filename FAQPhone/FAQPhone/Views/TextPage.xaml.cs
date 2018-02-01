using FAQPhone.Infarstructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextPage : ContentPage
    {
        public TextPage(string title, string text)
        {
            InitializeComponent();
            var factory = App.Resolve<TextViewModelFactory>();
            BindingContext = factory.Create(this, title, text);
        }
    }

    public class TextViewModelFactory
    {
        public TextViewModelFactory()
        {

        }
        public TextViewModel Create(ContentPage page, string title, string text)
        {
            return new TextViewModel(page, title, text);
        }
    }

    public class TextViewModel : BaseViewModel
    {
        public TextViewModel(ContentPage page, string title, string text) : base(page)
        {
            this.Title = title;
            this.Text = text;            
        }

        string _Text;
        public string Text
        {
            get { return _Text; }
            set { _Text = value; OnPropertyChanged(); }
        }

        string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged(); }
        }
    }
}


