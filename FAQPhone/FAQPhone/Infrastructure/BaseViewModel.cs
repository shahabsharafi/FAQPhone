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
        public BaseViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
        }

        public TextAlignment Direction { get { return ResourceManagerHelper.Direction; } }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
