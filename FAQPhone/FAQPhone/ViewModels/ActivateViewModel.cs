using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FAQPhone.ViewModels
{
    public class ActivateViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ActivateViewModel(INavigation navigation): base (navigation)
        {
            this.ActivateCommand = new Command(async () => await activateCommand());
        }
        string _username;
        public string username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        string _activation;
        public string activation
        {
            get { return _activation; }
            set { _activation = value; OnPropertyChanged(); }
        }
        public ICommand ActivateCommand { protected set; get; }

        public async Task activateCommand()
        {
            /////
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
