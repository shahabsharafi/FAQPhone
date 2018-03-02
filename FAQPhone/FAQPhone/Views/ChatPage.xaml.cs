using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using FilePicker;
using Newtonsoft.Json;
using Plugin.AudioRecorder;
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
            if (state != Constants.OPERATOR_INPROGRESS_FAQ && model.to == null)
            {
                this.ToolbarItems.RemoveAt(1);
            }
            var factory = App.Resolve<ChatViewModelFactory>();
            BindingContext = factory.Create(this, state, model);
        }
    }

    public class ChatViewModelFactory
    {
        IDiscussionService discussionService;
        IDepartmentService departmentService;
        IAccountService accountService;
        public ChatViewModelFactory(IAccountService accountService, IDiscussionService discussionService, IDepartmentService departmentService)
        {
            this.discussionService = discussionService;
            this.departmentService = departmentService;
            this.accountService = accountService;
        }
        public ChatViewModel Create(ContentPage page, string state, DiscussionModel model)
        {
            return new ChatViewModel(this.accountService, this.discussionService, this.departmentService, page, state, model);
        }
    }

    public class ChatViewModel : BaseViewModel
    {
        AudioRecorderService recorder;
        public ChatViewModel(IAccountService accountService, IDiscussionService discussionService, IDepartmentService departmentService, ContentPage page, string state, DiscussionModel model) : base(page)
        {
            this.discussionService = discussionService;
            this.departmentService = departmentService;
            this.accountService = accountService;
            this.SelectItemCommand = new Command<DiscussionDetailModel>((d) => selectItemCommand(d));
            this.SendCommand = new Command(async () => await sendCommand());
            this.AttachCommand = new Command(async () => await attachCommand());
            this.FinishCommand = new Command(async () => await finishCommand());
            this.ReportCommand = new Command(async () => await reportCommand());
            this.ProfileCommand = new Command(async () => await profileCommand());
            this.RuleCommand = new Command(async () => await ruleCommand());
            this.model = model;
            this.List = new ObservableCollection<DiscussionDetailModel>();
            this._state = state;
            this.IsOperator = this._state == Constants.OPERATOR_INPROGRESS_FAQ;
            this.Editable = (this._state == Constants.OPERATOR_INPROGRESS_FAQ || (this._state == Constants.USER_INPROGRESS_FAQ && model.state < 2));
            this.HasFinishing = this.IsOperator && model.state != Constants.DISCUSSION_STATE_REPORT && model.state != Constants.DISCUSSION_STATE_FINISHED;
            this.HasReporting = this.IsOperator && model.state != Constants.DISCUSSION_STATE_REPORT && model.state != Constants.DISCUSSION_STATE_FINISHED;
            this.setList(this.model.items.ToList());
            _downloader = DependencyService.Get<IDownloadService>().GetDownloader();
            _downloader.Downloaded += (s, e) =>
            {
                if (_currentDetail != null) _currentDetail.Mode = 3;
            };
            _downloader.Failed += (s, e) =>
            {
                if (_currentDetail != null) _currentDetail.Mode = 1;
            };
            //this.ShowProgress = false;
            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(15) //audio will stop recording after 15 seconds
            };
            recorder.AudioInputReceived += Recorder_AudioInputReceived;
            this.RecordCommand = new Command(() => recordCommand());
        }

        private async void Recorder_AudioInputReceived(object sender, string audioFile)
        {
            this.IsRecording = false;
            upload(audioFile, "sount.wav");
        }
        private string _state;
        private DiscussionDetailModel _currentDetail;
        private IDownloader _downloader;
        IAccountService accountService { get; set; }
        private IDiscussionService discussionService { get; set; }
        private IDepartmentService departmentService { get; set; }

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

        bool _CanMic;
        public bool CanMic
        {
            get { return _CanMic; }
            set { _CanMic = value; OnPropertyChanged(); }
        }

        bool _IsRecording;
        public bool IsRecording
        {
            get { return _IsRecording; }
            set
            {
                _IsRecording = value;
                OnPropertyChanged();
                this.CheckState();
            }
        }

        bool _CanSending;
        public bool CanSending
        {
            get { return _CanSending; }
            set { _CanSending = value; OnPropertyChanged(); }
        }

        bool _CanAttach;
        public bool CanAttach
        {
            get { return _CanAttach; }
            set { _CanAttach = value; OnPropertyChanged(); }
        }

        public string SendingStyle
        {
            get { return _CanSending ? "Info" : ""; }
        }

        bool _Editable;
        public bool Editable
        {
            get { return _Editable; }
            set
            {
                _Editable = value;
                OnPropertyChanged();
                this.CheckState();
            }
        }

        string _replay;
        public string replay
        {
            get { return _replay; }
            set
            {
                _replay = value;
                OnPropertyChanged();
                this.CheckState();
            }
        }

        private void CheckState()
        {
            CanSending = !this._IsRecording && this._Editable && !string.IsNullOrWhiteSpace(_replay);
            CanAttach = !this._IsRecording && string.IsNullOrWhiteSpace(_replay);
            CanMic = !this._IsRecording && string.IsNullOrWhiteSpace(_replay);
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
                    item.Mode = fileService.Exists(documentsPath + "/" + item.attachment) ? 3 : 1;
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

        public async void upload(string path, string fileName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var json = JsonConvert.SerializeObject(model);
            //dic.Add("has_encode", "true");
            dic.Add("EntityName", "discussion");
            dic.Add("Entity", json);
            var d = await UploadHelper.UploadFile<DiscussionDetailModel>(
                Constants.UploadUrl,
                path,
                fileName,
                (state) => {
                    this.IsBusy = state;
                },
                dic
            );
            d.isAnswer = (this._state != Constants.OPERATOR_INPROGRESS_FAQ);
            var l = this.model.items.ToList();
            l.Add(d);
            this.model.items = l.ToArray();
            this.setList(l);
        }

        public ICommand RecordCommand { protected set; get; }
        public async void recordCommand()
        {
            try
            {
                if (!recorder.IsRecording)
                {
                    this.IsRecording = true;
                    await recorder.StartRecording();
                }
                else
                {
                    await recorder.StopRecording();
                }
            }
            catch (Exception ex)
            {
                this.IsRecording = false;
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
                    isAnswer = (this._state != Constants.OPERATOR_INPROGRESS_FAQ),
                    text = this.replay
                });
                this.model.items = l.ToArray();
                await this.discussionService.Save(model);
                this.replay = string.Empty;
                this.setList(l);
            }
        }

        public ICommand FinishCommand { protected set; get; }

        public async Task finishCommand()
        {
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
                    isAnswer = true,
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
            await this.Navigation.PushAsync(new CancelationPage(model));
        }

        public ICommand RuleCommand { protected set; get; }

        public async Task ruleCommand()
        {
            if (this.model.department != null)
            {
                var department = await this.departmentService.Get(model.department._id);
                var title = ResourceManagerHelper.GetValue(Constants.RULES);
                var text = this.IsOperator
                    ? department.operatorRule
                    : department.userRule;
                await this.Navigation.PushAsync(new TextPage(title, text));
            }
        }

        public ICommand ProfileCommand { protected set; get; }
        public async Task profileCommand()
        {
            string username;
            if (this.IsOperator)
                username = model.from.username;
            else
                username = model.to.username;
            var role = this.IsOperator ? Constants.ACCESS_OPERATOR : Constants.ACCESS_USER;
            var user = await this.accountService.GetByUsername(username);
            if (user != null)
            {
                await this.Navigation.PushAsync(new ProfileInfoPage(user, role, false));
            }
        }

        public ICommand SelectItemCommand { protected set; get; }

        public void selectItemCommand(DiscussionDetailModel d)
        {
            if (d == null)
                return;
            if (d.Mode == 1)
            {
                _currentDetail = d;
                _downloader.Start(d.attachment);
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
