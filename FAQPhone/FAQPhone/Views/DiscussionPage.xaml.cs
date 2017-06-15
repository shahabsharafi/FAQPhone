using FAQPhone.Inferstructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            this.SelectItemCommand = new Command<DiscussionModel>(async (model) => await selectItemCommand(model));
            Task.Run(async () => await loadItems());
        }
        private IDiscussionService discussionService { get; set; }

        public ICommand SelectItemCommand { protected set; get; }

        public async Task selectItemCommand(DiscussionModel model)
        {
            
        }
        ObservableCollection<DiscussionModel> _list;
        public ObservableCollection<DiscussionModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }
        private void setList(List<DiscussionModel> list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                this.List.Add(item);
            }
        }
        public async Task loadItems()
        {
            var list = await this.discussionService.GetList();
            this.setList(list);
        }
    }
}
