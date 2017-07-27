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
    public partial class FilePickerPage : ContentPage
    {
        public FilePickerPage()
        {
            InitializeComponent();
            var factory = App.Resolve<FilePickerViewModelFactory>();
            BindingContext = factory.Create(Navigation);
        }
    }

    public class FilePickerViewModelFactory
    {
        public FilePickerViewModelFactory()
        {
            
        }
        public FilePickerViewModel Create(INavigation navigation)
        {
            return new FilePickerViewModel(navigation);
        }
    }

    public class FilePickerViewModel : BaseViewModel
    {
        public FilePickerViewModel(INavigation navigation) : base(navigation)
        {
            this.SelectCommand = new Command(async () => await selectCommand());
        }
        

        public ICommand SelectCommand { protected set; get; }

        public async Task selectCommand()
        {
            /////     
            
            await this.Navigation.PopAsync();
        }

    }
}
