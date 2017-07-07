using FAQPhone.Inferstructure;
using FAQPhone.Infrastructure;
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
        public DiscussionPage(string state)
        {
            InitializeComponent();
            var factory = App.Resolve<DiscussionPageViewModelFactory>();
            BindingContext = factory.Create(Navigation, state);
        }
    }

    public class DiscussionPageViewModelFactory
    {
        IDiscussionService discussionService;
        public DiscussionPageViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public DiscussionPageViewModel Create(INavigation navigation, string state)
        {
            return new DiscussionPageViewModel(this.discussionService, navigation, state);
        }
    }

    public class DiscussionPageViewModel : BaseViewModel
    {

        public DiscussionPageViewModel(IDiscussionService discussionService, INavigation navigation, string state) : base(navigation)
        {
            this.Title = ResourceManagerHelper.GetValue(state);
            this.IsUser = (state == "user_inprogress_faq" || state == "user_archived_faq");
            this.States = (state == "user_inprogress_faq" || state == "operator_inprogress_faq") ? new int[] { 0, 1 } : new int[] { 2 };
            this.discussionService = discussionService;
            this.SelectItemCommand = new Command<DiscussionModel>(async (model) => await selectItemCommand(model));
            this.List = new ObservableCollection<DiscussionModel>();
            Task.Run(async () => await loadItems());
        }
        private IDiscussionService discussionService { get; set; }
        bool IsUser { get; set; }
        int[] States { get; set; }
        public string Title { get; set; }

        public ICommand SelectItemCommand { protected set; get; }

        public async Task selectItemCommand(DiscussionModel model)
        {
            await this.Navigation.PushAsync(new ChatPage(model));
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
            var list = await this.discussionService.GetList(this.IsUser, this.States);
            this.setList(list);
        }
    }
}
