using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using FilePicker;
using Newtonsoft.Json;
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
            EventHandler h = null;
            h = (sender, e) =>
            {
                for (var i = 0; i < pushCount; i++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
                Appearing -= h;
            };
            Appearing += h;
            var factory = App.Resolve<ChatViewModelFactory>();
            BindingContext = factory.Create(this, state, model);
        }
    }

    public class ChatViewModelFactory
    {
        IDiscussionService discussionService;
        public ChatViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public ChatViewModel Create(ContentPage page, string state, DiscussionModel model)
        {
            return new ChatViewModel(this.discussionService, page, state, model);
        }
    }

    public class ChatViewModel : BaseViewModel
    {

        public ChatViewModel(IDiscussionService discussionService, ContentPage page, string state, DiscussionModel model) : base(page)
        {
            this.discussionService = discussionService;
            this.SelectItemCommand = new Command<DiscussionDetailModel>((d) => selectItemCommand(d));
            this.SendCommand = new Command(async () => await sendCommand());
            this.AttachCommand = new Command(async () => await attachCommand());
            this.FinishCommand = new Command(async () => await finishCommand());
            this.ReportCommand = new Command(async () => await reportCommand());
            this.model = model;
            this.List = new ObservableCollection<DiscussionDetailModel>();
            this._state = state;
            this.IsOperator = this._state == Constants.OPERATOR_INPROGRESS_FAQ;
            this.Editable = (this._state == Constants.OPERATOR_INPROGRESS_FAQ || (this._state == Constants.USER_INPROGRESS_FAQ && model.state < 2));
            this.HasFinishing = this.IsOperator && model.state != Constants.DISCUSSION_STATE_REPORT && model.state != Constants.DISCUSSION_STATE_FINISHED;
            this.HasReporting = this.IsOperator && model.state != Constants.DISCUSSION_STATE_REPORT && model.state != Constants.DISCUSSION_STATE_FINISHED;
            this.setList(this.model.items.ToList());
            _downloadService = DependencyService.Get<IDownloadService>();
            _downloadService.Downloaded += (s, e) =>
            {
                if(_currentDetail != null) _currentDetail.Mode = 3;
            };
            _downloadService.Failed += (s, e) =>
            {
                if (_currentDetail != null) _currentDetail.Mode = 1;
            };
            //this.ShowProgress = false;
        }
        private string _state;
        private DiscussionDetailModel _currentDetail;
        private IDownloadService _downloadService;
        private IDiscussionService discussionService { get; set; }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
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
                if (!string.IsNullOrEmpty(item.attachment))
                {
                    var fileService = DependencyService.Get<IFileService>();
                    string documentsPath = fileService.GetDocumentsPath();
                    item.Mode = fileService.Exists(documentsPath + "/" + item.attachment) ? 3: 1;                    
                }
                else
                {
                    item.Mode = 0;
                }
                
                this.List.Add(item);
            }
        }

        public ICommand AttachCommand { protected set; get; }
        public async Task attachCommand()
        {
            FilePicker.FilePickerPage filePicker = new FilePicker.FilePickerPage(this.Navigation);
            await filePicker.Open();
            filePicker.Select += (sender, e) =>
            {
                upload(filePicker.Path, filePicker.FileName);
            };
        }

        public override void ChangeIsBusy(bool state)
        {
            this.Editable = state
                ? false
                : (this._state == Constants.OPERATOR_INPROGRESS_FAQ || (this._state == Constants.USER_INPROGRESS_FAQ && model.state < 2));
        }

        //bool _ShowProgress;
        //public bool ShowProgress
        //{
        //    get { return _ShowProgress; }
        //    set {
        //        _ShowProgress = value;
        //        if (value)
        //            this.Editable = false;
        //        else
        //            this.Editable = (this._state == Constants.OPERATOR_INPROGRESS_FAQ || (this._state == Constants.USER_INPROGRESS_FAQ && model.state < 2));
        //        OnPropertyChanged();
        //    }
        //}

        public async void upload(string path, string fileName)
        {
            Dictionary <string, string> dic = new Dictionary<string, string>();
            var json = JsonConvert.SerializeObject(model);
            dic.Add("EntityName", "discussion");
            dic.Add("Entity", json);
            var d = await UploadHelper.UploadFile<DiscussionDetailModel>(
                Constants.UploadUrl, 
                path, 
                fileName, 
                (state) => {
                    //this.ShowProgress = state;
                    this.IsBusy = state;
                }, 
                dic
            );
            var l = this.model.items.ToList();
            l.Add(d);
            this.setList(l);
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

        public ICommand SelectItemCommand { protected set; get; }

        public void selectItemCommand(DiscussionDetailModel d)
        {
            if (d == null)
                return;
            if (d.Mode == 1)
            {
                _currentDetail = d;
                _downloadService.Start(d.attachment);
                d.Mode = 2;
            }
            else if (d.Mode == 3)
            {
                DependencyService.Get<IFileService>().OpenFile(d.attachment);
            }
            this.SelectedItem = null;
        }
    }
}