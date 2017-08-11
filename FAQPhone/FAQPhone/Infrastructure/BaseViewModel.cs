using FAQPhone.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FAQPhone.Inferstructure
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        string _message;
        public string message
        {
            get { return ResourceManagerHelper.GetValue(_message); }
            set { _message = value; OnPropertyChanged(); }
        }
        public INavigation Navigation { get; set; }
        public BaseViewModel(ContentPage page)
        {
            this.Navigation = page.Navigation;
        }

        public TextAlignment Direction { get { return ResourceManagerHelper.Direction; } }
        public LayoutOptions Layout { get { return ResourceManagerHelper.Layout; } }

        public async Task RootNavigate<T>() where T : Page, new()
        {
            await this.RootNavigate(new T());
        }

        public async Task RootNavigate(Page page)
        {
            this.Navigation.InsertPageBefore(page, this.Navigation.NavigationStack.First());
            await this.Navigation.PopToRootAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
