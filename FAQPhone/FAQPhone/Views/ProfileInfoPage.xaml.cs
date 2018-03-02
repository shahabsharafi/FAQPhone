using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfileInfoPage : ContentPage
	{
		public ProfileInfoPage (AccountModel model, string role, bool hasFAQ = true)
		{
			InitializeComponent ();
            if (!hasFAQ)
            {
                this.ToolbarItems.RemoveAt(1);
            }
            var factory = App.Resolve<ProfileInfoViewModelFactory>();
            var vm = factory.Create(this, model, role);
            this.Appearing += (sender, e) => {
                Task.Run(async () => await vm.Load()).Wait();
            };
            BindingContext = vm;
        }
    }

    public class ProfileInfoViewModelFactory
    {
        public ProfileInfoViewModelFactory()
        {
            
        }
        public ProfileInfoViewModel Create(ContentPage page, AccountModel model, string role)
        {
            return new ProfileInfoViewModel(page, model, role);
        }
    }

    public class ProfileInfoViewModel : BaseViewModel
    {

        public ProfileInfoViewModel(ContentPage page, AccountModel model, string role) : base(page)
        {
            this._role = role;
            this.CreateFAQCommand = new Command(async () => await createFAQCommand());
            this.CommentCommand = new Command(async () => await commentCommand());
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

        string _role { get; set; }

        public async Task Load()
        {
            if (!this.IsBusy)
            {
                this._downloader.Start(this.model.PictureName);
            }
            this.setFields(this.model);
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

            if (this._role == Constants.ACCESS_OPERATOR)
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

        public ICommand CreateFAQCommand { protected set; get; }
        public async Task createFAQCommand()
        {
            await this.Navigation.PushAsync(new DiscussionNewPage(null,this.model , null, 0));
        }

        public ICommand CommentCommand { protected set; get; }

        public async Task commentCommand()
        {
            await this.Navigation.PushAsync(new CommentPage(this.model));
        }
    }
}

