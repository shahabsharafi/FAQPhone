using FAQPhone.Inferstructure;
using FAQPhone.Services.Interfaces;
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
    public partial class DiscussionPage : ContentPage
    {
        public DiscussionPage()
        {
            InitializeComponent();
            var factory = App.Resolve<DiscussionPageViewModelFactory>();
            BindingContext = factory.Create(Navigation);
        }
    }

    public class DiscussionPageViewModelFactory
    {
        IDiscussionService discussionService;
        public DiscussionPageViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public DiscussionPageViewModel Create(INavigation navigation)
        {
            return new DiscussionPageViewModel(this.discussionService, navigation);
        }
    }

    public class DiscussionPageViewModel : BaseViewModel
    {

        public DiscussionPageViewModel(IDiscussionService discussionService, INavigation navigation) : base(navigation)
        {
            this.discussionService = discussionService;
            this.SelectCommand = new Command(async () => await selectCommand());
            Task.Run(async () => await loadItems());
        }
        private IDiscussionService discussionService { get; set; }

        public ICommand SelectCommand { get; }

        public async Task selectCommand()
        {
            ///// 
            await loadItems();
        }
        public async Task loadItems()
        {
            
        }
    }
}
