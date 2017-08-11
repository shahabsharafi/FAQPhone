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
        public ChatPage(string state, DiscussionModel model, int pushCount)
        {
            InitializeComponent();
            this.Appearing += (sender, e) => {
                for (var i = 0; i < pushCount; i++)
                {
                    this.Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
            };
            var factory = App.Resolve<ChatViewModelFactory>();
            BindingContext = factory.Create(Navigation, state, model);
        }
    }

    public class ChatViewModelFactory
    {
        IDiscussionService discussionService;
        public ChatViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public ChatViewModel Create(INavigation navigation, string state, DiscussionModel model)
        {
            return new ChatViewModel(this.discussionService, navigation, state, model);
        }
    }

    public class ChatViewModel : BaseViewModel
    {

        public ChatViewModel(IDiscussionService discussionService, INavigation navigation, string state, DiscussionModel model) : base(navigation)
        {
            this.discussionService = discussionService;
            this.SendCommand = new Command(async () => await sendCommand());
            this.FinishCommand = new Command(async () => await finishCommand());
            this.ReportCommand = new Command(async () => await reportCommand());
            this.model = model;
            this.List = new ObservableCollection<DiscussionDetailModel>();
            this.IsOperator = state == Constants.OPERATOR_INPROGRESS_FAQ;
            this.Editable = (state == Constants.OPERATOR_INPROGRESS_FAQ || (state == Constants.USER_INPROGRESS_FAQ && model.state < 2));
            this.HasFinishing = this.IsOperator && model.state != Constants.DISCUSSION_STATE_REPORT && model.state != Constants.DISCUSSION_STATE_FINISHED;
            this.HasReporting = this.IsOperator && model.state != Constants.DISCUSSION_STATE_REPORT && model.state != Constants.DISCUSSION_STATE_FINISHED;
            this.setList(this.model.items.ToList());
        }
        private IDiscussionService discussionService { get; set; }
        private DiscussionModel model { get; set; }

        bool IsOperator { get; set; }

        bool _HasReporting;
        public bool HasReporting
        {
            get { return _HasReporting; }
            set { _HasReporting = value; OnPropertyChanged(); }
        }

        public string ReportingStyle
        {
            get { return _HasReporting ? "Info" : ""; }
        }

        bool _HasFinishing;
        public bool HasFinishing
        {
            get { return _HasFinishing; }
            set { _HasFinishing = value; OnPropertyChanged(); }
        }

        public string FinishingStyle
        {
            get { return _HasFinishing ? "Info" : ""; }
        }

        bool _CanSending;
        public bool CanSending
        {
            get { return _CanSending; }
            set { _CanSending = value; OnPropertyChanged(); }
        }

        public string SendingStyle
        {
            get { return _CanSending ? "Info" : ""; }
        }

        bool _Editable;
        public bool Editable
        {
            get { return _Editable; }
            set { _Editable = value; OnPropertyChanged(); }
        }

        string _replay;
        public string replay
        {
            get { return _replay; }
            set
            {
                _replay = value;
                OnPropertyChanged();
                CanSending = !string.IsNullOrWhiteSpace(_replay);
            }
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
                if (this.IsOperator)
                {
                    model.operatorRead = false;
                }
                else
                {
                    model.userRead = false;
                }
                var l = this.model.items.ToList();
                l.Add(new DiscussionDetailModel()
                {
                    createDate = DateTime.Now,
                    owner = new AccountModel() { username = App.Username },
                    text = this.replay
                });
                this.model.items = l.ToArray();                
                await this.discussionService.Save(model);
                this.replay = string.Empty;
                this.setList(l);
                //await this.RootNavigate(new MainPage());
            }
        }

        public ICommand FinishCommand { protected set; get; }

        public async Task finishCommand()
        {
            ///// 
            if (!string.IsNullOrWhiteSpace(this.replay))
            {
                if (this.IsOperator)
                {
                    model.userRead = false;
                }
                else
                {
                    model.operatorRead = false;
                }
                var l = this.model.items.ToList();
                l.Add(new DiscussionDetailModel
                {
                    createDate = DateTime.Now,
                    owner = new AccountModel() { username = App.Username },
                    text = this.replay
                });
                model.items = l.ToArray();
            }
            model.state = Constants.DISCUSSION_STATE_FINISHED;
            await this.discussionService.Save(model);
            await this.Navigation.PopAsync();
        }

        public ICommand ReportCommand { protected set; get; }

        public async Task reportCommand()
        {
            /////     
            await this.Navigation.PushAsync(new CancelationPage(model));
        }
    }
}