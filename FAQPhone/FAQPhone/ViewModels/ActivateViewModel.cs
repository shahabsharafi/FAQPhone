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
    public class ActivateViewModel : BaseViewModel
    {
        public ActivateViewModel(INavigation navigation, string username): base (navigation)
        {
            this.username = username;
            this.ActivateCommand = new Command(async () => await activateCommand());
        }
        public string username { get; private set; }
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
            await this.Navigation.PushAsync(new SigninPage());
        }
    }
}
