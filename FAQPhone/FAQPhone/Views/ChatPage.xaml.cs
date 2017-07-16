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
        public ChatPage(DiscussionModel model)
        {
            InitializeComponent();
            var factory = App.Resolve<ChatViewModelFactory>();
            BindingContext = factory.Create(Navigation, model);
        }
    }

    public class ChatViewModelFactory
    {
        IDiscussionService discussionService;
        public ChatViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public ChatViewModel Create(INavigation navigation, DiscussionModel model)
        {
            return new ChatViewModel(this.discussionService, navigation, model);
        }
    }

    public class ChatViewModel : BaseViewModel
    {

        public ChatViewModel(IDiscussionService discussionService, INavigation navigation, DiscussionModel model) : base(navigation)
        {
            this.discussionService = discussionService;
            this.SendCommand = new Command(async () => await sendCommand());
            this.FinishCommand = new Command(async () => await finishCommand());
            this.ReportCommand = new Command(async () => await reportCommand());
            this.model = model;
            this.List = new ObservableCollection<DiscussionDetailModel>();
            this.setList(this.model.items.ToList());
        }
        private IDiscussionService discussionService { get; set; }
        private DiscussionModel model { get; set; }
        string _replay;
        public string replay
        {
            get { return _replay; }
            set { _replay = value; OnPropertyChanged(); }
        }

        ObservableCollection<DiscussionDetailModel> _list;
        public ObservableCollection<DiscussionDetailModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
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
            if (!string.IsNullOrWhiteSpace(this.replay))
            {
                var l = this.model.items.ToList();
                l.Add(new DiscussionDetailModel()
                {
                    createDate = DateTime.Now,
                    owner = new AccountModel() { username = App.Username },
                    text = this.replay
                });
                this.model.items = l.ToArray();
                await this.discussionService.Save(model);
                await this.RootNavigate(new MainPage());
            }
        }

        public ICommand FinishCommand { protected set; get; }

        public async Task finishCommand()
        {
            ///// 
            if (!string.IsNullOrWhiteSpace(this.replay))
            {
                var l = this.model.items.ToList();
                l.Add(new DiscussionDetailModel
                {
                    createDate = DateTime.Now,
                    owner = new AccountModel() { username = App.Username },
                    text = this.replay
                });
                model.items = l.ToArray();
            }
            model.state = 2;
            await this.discussionService.Save(model);
            await this.RootNavigate(new MainPage());
        }

        public ICommand ReportCommand { protected set; get; }

        public async Task reportCommand()
        {
            /////     
            model.state = 9;
            model.to = new AccountModel() { username = App.Username };
            await this.discussionService.Save(model);
            await this.RootNavigate(new MainPage());
        }
    }
}