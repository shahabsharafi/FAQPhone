using FAQPhone.Helpers;
using FAQPhone.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FAQPhone.Infarstructure
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                this.ChangeIsBusy(value);
                OnPropertyChanged();
            }
        }
        public virtual void ChangeIsBusy(bool state) { }
        string _message;
        public string message
        {
            get { return ResourceManagerHelper.GetValue(_message); }
            set { _message = value; OnPropertyChanged(); }
        }
        public ContentPage Page { get; set; }
        public BaseViewModel(ContentPage page)
        {
            this.Page = page;
        }
        public INavigation Navigation { get { return this.Page.Navigation; } }
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
