using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using FilePicker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using TackPicture;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        public AccountPage(AccountModel model, string parm)
        {
            InitializeComponent();
            var factory = App.Resolve<AccountViewModelFactory>();
            var vm = factory.Create(this, model, parm);
            this.Appearing += (sender, e) => {
                Task.Run(async () => await vm.Load()).Wait();
            };
            BindingContext = vm;
        }
    }

    public class AccountViewModelFactory
    {
        IAccountService accountService;
        public AccountViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public AccountViewModel Create(ContentPage page, AccountModel model, string parm)
        {
            return new AccountViewModel(this.accountService, page, model, parm);
        }
    }

    public class AccountViewModel : BaseViewModel
    {

        public AccountViewModel(IAccountService accountService, ContentPage page, AccountModel model, string parm) : base(page)
        {
            this._parm = parm;
            this.accountService = accountService;
            this.EditCommand = new Command(async () => await editCommand());
            this.TackPictureCommand = new Command(async () => await tackPictureCommand());
            this.model = model;
            var fileService = DependencyService.Get<IFileService>();
            string documentsPath = fileService.GetDocumentsPath();
            _downloadService = DependencyService.Get<IDownloadService>();
            _downloadService.Downloaded += (s, e) =>
            {
                this.SourceImage = Path.Combine(documentsPath, this.model.PictureName);
            };
            _downloadService.Failed += (s, e) =>
            {
                this.SourceImage = Utility.GetImage("man.png");
            };
        }

        string _SourceImage;
        public string SourceImage
        {
            get { return _SourceImage; }
            set { _SourceImage = value; OnPropertyChanged(); }
        }

        private IDownloadService _downloadService;
        public ICommand TackPictureCommand { protected set; get; }

        public async Task tackPictureCommand()
        {
            Action<string> action = (path) =>
            {
                upload(path, this.model.PictureName);
            };
            DependencyService.Get<IPictureService>().TakeAPicture(action);
        }

        public async void upload(string path, string fileName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("EntityName", "none");
            var d = await UploadHelper.UploadFile<ResultModel>(
                Constants.UploadUrl,
                path,
                fileName,
                (state) => {
                    //this.ShowProgress = state;
                    this.IsBusy = state;
                },
                dic
            );
        }

        string _parm { get; set; }

        bool _loaded = false;
        public async Task Load()
        {
            var fileService = DependencyService.Get<IFileService>();
            string documentsPath = fileService.GetDocumentsPath();
            var path = Path.Combine(documentsPath, this.model.PictureName);
            if (fileService.Exists(path))
            {
                this.SourceImage = path;
            }
            else
            {
                this._downloadService.Start(this.model.PictureName);
            }
            if (this._loaded)
            {
                var m = await this.accountService.GetMe();
                this.setFields(m);
            }
            else
            {
                this._loaded = true;
                this.setFields(this.model);
            }
        }

        void setFields(AccountModel model)
        {
            if (model.profile != null)
            {
                this.fullName = model.profile.firstName + " " + model.profile.lastName;
            }
            this.mobile = model.mobile;
            this.email = model.email;
            this.credit = 
                ResourceManagerHelper.GetValue("account_credit") + ":" + 
                (model.credit).ToString() + " " + 
                ResourceManagerHelper.GetValue("unit_of_mony_caption");
        }

        private IAccountService accountService { get; set; }
        AccountModel model { get; set; }

        string _fullName;
        public string fullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }

        string _mobile;
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; OnPropertyChanged(); }
        }

        string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        string _credit;
        public string credit
        {
            get { return _credit; }
            set { _credit = value; OnPropertyChanged(); }
        }

        public ICommand EditCommand { protected set; get; }

        public async Task editCommand()
        {
            if (this._parm == Constants.ACCESS_OPERATOR)
            {
                await this.Navigation.PushAsync(new ProfilePage(this.model));
            }
            else
            {
                await this.Navigation.PushAsync(new UserProfilePage(this.model));
            }
        }
    }
}

