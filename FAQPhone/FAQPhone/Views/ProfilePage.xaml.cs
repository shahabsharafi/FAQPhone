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
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfilePageViewModel(Navigation);
        }
    }

    class ProfilePageViewModel : BaseViewModel
    {

        public ProfilePageViewModel(INavigation navigation) : base(navigation)
        {
            this.SaveCommand = new Command(async () => await saveCommand());
        }

        public ICommand SaveCommand { get; }

        public async Task saveCommand()
        {
            /////
            //await this.Navigation.PushAsync(new SignupPage(this.mobile));
        }
    }
}
