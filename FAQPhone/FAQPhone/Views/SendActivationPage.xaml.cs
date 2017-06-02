using FAQPhone.Inferstructure;
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
    public partial class SendActivationPage : ContentPage
    {
        public SendActivationPage()
        {
            InitializeComponent();
            BindingContext = new SendActivationViewModel(Navigation);
        }
    }

    public class SendActivationViewModel : BaseViewModel
    {
        public SendActivationViewModel(INavigation navigation) : base(navigation)
        {
            this.SendActivationCommand = new Command(async () => await sendActivationCommand());
        }
        string _mobile;
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; OnPropertyChanged(); }
        }
        public ICommand SendActivationCommand { protected set; get; }

        public async Task sendActivationCommand()
        {
            /////
            await this.Navigation.PushAsync(new ActivatePage(this.mobile));
        }
    }
}
