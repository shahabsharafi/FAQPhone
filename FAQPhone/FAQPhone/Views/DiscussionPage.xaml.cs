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
        public DiscussionPage(bool isUser, int[] states)
        {
            InitializeComponent();
            var factory = App.Resolve<DiscussionPageViewModelFactory>();
            BindingContext = factory.Create(Navigation, isUser, states);
        }
    }

    public class DiscussionPageViewModelFactory
    {
        IDiscussionService discussionService;
        public DiscussionPageViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public DiscussionPageViewModel Create(INavigation navigation, bool isUser, int[] states)
        {
            return new DiscussionPageViewModel(this.discussionService, navigation, isUser, states);
        }
    }

    public class DiscussionPageViewModel : BaseViewModel
    {

        public DiscussionPageViewModel(IDiscussionService discussionService, INavigation navigation, bool isUser, int[] states) : base(navigation)
        {
            this.IsUser = isUser;
            this.States = states;
            this.discussionService = discussionService;
            this.SelectItemCommand = new Command<DiscussionModel>(async (model) => await selectItemCommand(model));
            this.List = new ObservableCollection<DiscussionModel>();
            Task.Run(async () => await loadItems());
        }
        private IDiscussionService discussionService { get; set; }
        bool IsUser { get; set; }
        int[] States { get; set; }

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
