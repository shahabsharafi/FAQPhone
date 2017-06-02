using FAQPhone.Inferstructure;
using FAQPhone.Models;
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
    public partial class ActivatePage : ContentPage
    {
        public ActivatePage(string mobile)
        {
            InitializeComponent();
            BindingContext = new ActivateViewModel(Navigation, mobile);
        }
    }

    public class ActivateViewModel : BaseViewModel
    {
        public ActivateViewModel(INavigation navigation, string mobile) : base(navigation)
        {
            this.mobile = mobile;
            this.ActivateCommand = new Command(async () => await activateCommand());
        }
        public string mobile { get; private set; }
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
            await this.Navigation.PushAsync(new SignupPage(this.mobile));
        }
    }
}
