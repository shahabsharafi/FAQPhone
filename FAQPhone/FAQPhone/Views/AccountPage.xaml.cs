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
        ICommonService commonServic;
        public AccountViewModelFactory(IAccountService accountService, ICommonService commonServic)
        {
            this.accountService = accountService;
            this.commonServic = commonServic;
        }
        public AccountViewModel Create(ContentPage page, AccountModel model, string parm)
        {
            return new AccountViewModel(this.accountService, this.commonServic, page, model, parm);
        }
    }

    public class AccountViewModel : BaseViewModel
    {

        public AccountViewModel(IAccountService accountService, ICommonService commonServic, ContentPage page, AccountModel model, string parm) : base(page)
        {
            this._parm = parm;
            this.accountService = accountService;
            this.commonServic = commonServic;
            this.EditCommand = new Command(async () => await editCommand());
            this.TackPictureCommand = new Command(async () => await tackPictureCommand());
            this.model = model;
            var fileService = DependencyService.Get<IFileService>();
            string documentsPath = fileService.GetDocumentsPath();
            _downloader = DependencyService.Get<IDownloadService>().GetDownloader();
            _downloader.Downloaded += (s, e) =>
            {
                this.SourceImage = Path.Combine(documentsPath, this.model.PictureName);
            };
            _downloader.Failed += (s, e) =>
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

        string _grade;
        public string grade
        {
            get { return _grade; }
            set { _grade = value; OnPropertyChanged(); }
        }

        string _university;
        public string university
        {
            get { return _university; }
            set { _university = value; OnPropertyChanged(); }
        }

        bool _isOperator;
        public bool isOperator
        {
            get { return _isOperator; }
            set { _isOperator = value; OnPropertyChanged(); }
        }

        private IDownloader _downloader;
        public ICommand TackPictureCommand { protected set; get; }

        private async Task tackPictureCommand()
        {
            this.IsBusy = true;
            Action<byte[]> action = (data) =>
            {
                upload(data, this.model.PictureName);
            };
            DependencyService.Get<IPictureService>().TakeAPicture(action);
        }

        public async void upload(byte[] data, string fileName)
        {
            await UploadHelper.UploadPicture(data, fileName,
                (state) =>
                {
                    if (state == false)
                    {
                        this.IsBusy = state;
                        this._downloader.Start(this.model.PictureName);
                    }
                }, 150, 150);
        }

        string _parm { get; set; }

        bool _loaded = false;
        public async Task Load()
        {
            if (!this.IsBusy)
            {
                this._downloader.Start(this.model.PictureName);
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
                this.fullName = "";
                if (!string.IsNullOrWhiteSpace(this.model.profile.sex))
                {
                    var sex = App.AttributeList.Find(o => o._id == this.model.profile.sex);
                    if (sex != null)
                    {
                        this.fullName += sex.value == "male"
                            ? ResourceManagerHelper.GetValue("perfix_male")
                            : ResourceManagerHelper.GetValue("perfix_female");
                    }
                }
                if (!string.IsNullOrWhiteSpace(this.model.profile.firstName))
                {
                    if (!string.IsNullOrWhiteSpace(this.fullName))
                    {
                        this.fullName += " ";
                    }
                    this.fullName += model.profile.firstName;
                }
                if (!string.IsNullOrWhiteSpace(model.profile.lastName))
                {
                    if (!string.IsNullOrWhiteSpace(this.fullName))
                    {
                        this.fullName += " ";
                    }
                    this.fullName += model.profile.lastName;
                }
                if (model.profile.birthDay.HasValue)
                {
                    var bd = Utility.MiladiToShamsi(model.profile.birthDay.Value);
                    this.birthDay = 
                        ResourceManagerHelper.GetValue("profile_birthDay") + 
                        " " + bd[0] + "/" + bd[1] + "/" + bd[2];

                }                
            }

            this.isOperator = (App.EnterAsOperator == true);

            if (this._parm == Constants.ACCESS_OPERATOR)
            {
                if (model.education != null)
                {
                    if (model.education.grade != null)
                    {
                        var grade = App.AttributeList.Find(o => o._id == this.model.education.grade);
                        this.grade = ResourceManagerHelper.GetValue("education_grade") + " " + grade.caption;
                    }

                    if (model.education.university != null)
                    {
                        var university = App.AttributeList.Find(o => o._id == this.model.education.university);
                        this.university = ResourceManagerHelper.GetValue("education_university") + " " + university.caption;
                    }
                }
            }

            this.mobile = model.mobile;            
            this.credit = 
                ResourceManagerHelper.GetValue("account_credit") + ":" + 
                (model.credit).ToString() + " " + 
                ResourceManagerHelper.GetValue("unit_of_mony_caption");
        }

        private IAccountService accountService { get; set; }
        private ICommonService commonServic { get; set; }
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

        string _birthDay;
        public string birthDay
        {
            get { return _birthDay; }
            set { _birthDay = value; OnPropertyChanged(); }
        }

        string _credit;
        public string credit
        {
            get { return _credit; }
            set { _credit = value; OnPropertyChanged(); }
        }

        public ICommand ShowReportBalanceCommand { protected set; get; }
        public async Task showReportBalanceCommand()
        {
            await this.Navigation.PushAsync(new ReportBalancePage());
        }
        public ICommand ChargeCommand { protected set; get; }
        public async Task chargeCommand()
        {
            var titleCharge = ResourceManagerHelper.GetValue(Constants.CHARGE);
            var keyResult = await accountService.SetUserKey();
            var urlCharge = string.Format(Constants.ChargeUrl, keyResult.message, App.Username);
            await this.Navigation.PushAsync(new BrowserPage(titleCharge, urlCharge));
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

