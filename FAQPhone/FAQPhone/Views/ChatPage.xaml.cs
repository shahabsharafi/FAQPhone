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
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();
            var factory = App.Resolve<ChatViewModelFactory>();
            BindingContext = factory.Create(Navigation);
        }
    }

    public class ChatViewModelFactory
    {
        IDiscussionService discussionService;
        public ChatViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public ChatViewModel Create(INavigation navigation)
        {
            return new ChatViewModel(this.discussionService, navigation);
        }
    }

    public class ChatViewModel : BaseViewModel
    {

        public ChatViewModel(IDiscussionService discussionService, INavigation navigation) : base(navigation)
        {
            this.discussionService = discussionService;
            this.SendCommand = new Command(async (model) => await sendCommand());
            this.List = new ObservableCollection<DiscussionDetailModel>();
            Task.Run(async () => await loadItems());
        }
        private IDiscussionService discussionService { get; set; }


        ObservableCollection<DiscussionDetailModel> _list;
        public ObservableCollection<DiscussionDetailModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public async Task loadItems()
        {
            this.setList(new List<DiscussionDetailModel>()
            {
                new DiscussionDetailModel()
                {
                    createDate = DateTime.Now,
                    text = "aaaaa"
                },
                new DiscussionDetailModel()
                {
                    createDate = DateTime.Now,
                    text = "bbbb"
                },
                new DiscussionDetailModel()
                {
                    createDate = DateTime.Now,
                    text = "ccccccc"
                }
            });
        }

        private void setList(List<DiscussionDetailModel> list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                this.List.Add(item);
            }
        }

        public ICommand SendCommand { protected set; get; }

        public async Task sendCommand()
        {
            ///
        }
    }
}