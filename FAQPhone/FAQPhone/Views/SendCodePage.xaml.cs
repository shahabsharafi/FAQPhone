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
    public partial class SendCodePage : ContentPage
    {
        public SendCodePage(string flow)
        {
            InitializeComponent();
            BindingContext = new SendCodeViewModel(Navigation, flow);
        }
    }

    public class SendCodeViewModel : BaseViewModel
    {
        public SendCodeViewModel(INavigation navigation, string flow) : base(navigation)
        {
            this.flow = flow;
            this.SendCodeCommand = new Command(async () => await sendCodeCommand());
        }
        private string flow { get; set; }
        string _mobile;
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; OnPropertyChanged(); }
        }
        public ICommand SendCodeCommand { protected set; get; }

        public async Task sendCodeCommand()
        {
            /////
            await this.Navigation.PushAsync(new SecurityCodePage(this.flow, this.mobile));
        }
    }
}
