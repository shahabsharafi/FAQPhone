using FAQPhone.Infarstructure;
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
    public partial class UserProfilePage : ContentPage
    {
        public UserProfilePage(AccountModel model)
        {
            InitializeComponent();
            var factory = App.Resolve<UserProfileViewModelFactory>();
            BindingContext = factory.Create(this, model);
        }
    }

    public class UserProfileViewModelFactory
    {
        IAccountService accountService;
        public UserProfileViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public UserProfileViewModel Create(ContentPage page, AccountModel model)
        {
            return new UserProfileViewModel(this.accountService, page, model);
        }
    }

    public class UserProfileViewModel : BaseViewModel
    {

        public UserProfileViewModel(IAccountService accountService, ContentPage page, AccountModel model) : base(page)
        {
            this.accountService = accountService;
            this.model = model;
            this.SaveCommand = new Command(async () => await saveCommand());
            this.SexList = new ObservableCollection<AttributeModel>();
            this.StatusList = new ObservableCollection<AttributeModel>();
            this.JobStateList = new ObservableCollection<AttributeModel>();
            this.ReligionList = new ObservableCollection<AttributeModel>();
            this.SectList = new ObservableCollection<AttributeModel>();
            this.ReferenceList = new ObservableCollection<AttributeModel>();
            this.CountryList = new ObservableCollection<AttributeModel>();
            this.ProvinceList = new ObservableCollection<AttributeModel>();
            this.CityList = new ObservableCollection<AttributeModel>();
            this.GradeList = new ObservableCollection<AttributeModel>();
            var sexList = App.AttributeList.Where(o => o.type == "sex");
            foreach (var item in sexList)
            {
                this.SexList.Add(item);
            }
            var statusList = App.AttributeList.Where(o => o.type == "status");
            foreach (var item in statusList)
            {
                this.StatusList.Add(item);
            }
            var jobStateList = App.AttributeList.Where(o => o.type == "job_state");
            foreach (var item in jobStateList)
            {
                this.JobStateList.Add(item);
            }
            var religionList = App.AttributeList.Where(o => o.type == "religion");
            foreach (var item in religionList)
            {
                this.ReligionList.Add(item);
            }
            var sectList = App.AttributeList.Where(o => o.type == "sect");
            foreach (var item in sectList)
            {
                this.SectList.Add(item);
            }
            var referenceList = App.AttributeList.Where(o => o.type == "reference");
            foreach (var item in referenceList)
            {
                this.ReferenceList.Add(item);
            }
            var countryList = App.AttributeList.Where(o => o.type == "province");
            foreach (var item in sexList)
            {
                this.CountryList.Add(item);
            }
            var gradeList = App.AttributeList.Where(o => o.type == "grade");
            foreach (var item in gradeList)
            {
                this.GradeList.Add(item);
            }
            if (this.model.profile != null)
            {
                if (model.profile.sex != null)
                {
                    this.SelectedSex = App.AttributeList.Find(o => o._id == model.profile.sex);
                }
                if (model.profile.status != null)
                {
                    this.SelectedStatus = App.AttributeList.Find(o => o._id == model.profile.status);
                }
                if (model.profile.jobState != null)
                {
                    this.SelectedJobState = App.AttributeList.Find(o => o._id == model.profile.jobState);
                }
                if (model.profile.religion != null)
                {
                    this.SelectedReligion = App.AttributeList.Find(o => o._id == model.profile.religion);
                }
                if (model.profile.sect != null)
                {
                    this.SelectedSect = App.AttributeList.Find(o => o._id == model.profile.sect);
                }
                if (model.profile.reference != null)
                {
                    this.SelectedReference = App.AttributeList.Find(o => o._id == model.profile.reference);
                }
            }
            this.email = this.model.email;
            if (this.model.contact != null)
            {
                if (model.contact.country != null)
                {
                    this.SelectedCountry = App.AttributeList.Find(o => o._id == model.contact.country);
                    this.CountryText = this.SelectedCountry.caption;
                }
                if (model.contact.province != null)
                {
                    this.SelectedProvince = App.AttributeList.Find(o => o._id == model.contact.province);
                    this.ProvinceText = this.SelectedProvince.caption;
                }
                if (model.contact.city != null)
                {
                    this.SelectedCity = App.AttributeList.Find(o => o._id == model.contact.city);
                    this.CityText = this.SelectedCity.caption;
                }
            }
            if (this.model.education != null)
            {
                if (this.model.education.grade != null)
                {
                    this.SelectedGrade = App.AttributeList.Find(o => o._id == this.model.education.grade);
                    this.GradeText = this.SelectedGrade.caption;
                }
            }
        }

        private IAccountService accountService { get; set; }
        AccountModel model { get; set; }

        string _house;
        public string house
        {
            get { return _house; }
            set { _house = value; OnPropertyChanged(); }
        }

        string _work;
        public string work
        {
            get { return _work; }
            set { _work = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AttributeModel> _SexList;
        public ObservableCollection<AttributeModel> SexList
        {
            get { return _SexList; }
            set { _SexList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedSex;
        public AttributeModel SelectedSex
        {
            get { return _SelectedSex; }
            set
            {
                _SelectedSex = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AttributeModel> _StatusList;
        public ObservableCollection<AttributeModel> StatusList
        {
            get { return _StatusList; }
            set { _StatusList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedStatus;
        public AttributeModel SelectedStatus
        {
            get { return _SelectedStatus; }
            set
            {
                _SelectedStatus = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AttributeModel> _JobStateList;
        public ObservableCollection<AttributeModel> JobStateList
        {
            get { return _JobStateList; }
            set { _JobStateList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedJobState;
        public AttributeModel SelectedJobState
        {
            get { return _SelectedJobState; }
            set
            {
                _SelectedJobState = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AttributeModel> _ReligionList;
        public ObservableCollection<AttributeModel> ReligionList
        {
            get { return _ReligionList; }
            set { _ReligionList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedReligion;
        public AttributeModel SelectedReligion
        {
            get { return _SelectedReligion; }
            set
            {
                _SelectedReligion = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AttributeModel> _SectList;
        public ObservableCollection<AttributeModel> SectList
        {
            get { return _SectList; }
            set { _SectList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedSect;
        public AttributeModel SelectedSect
        {
            get { return _SelectedSect; }
            set
            {
                _SelectedSect = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AttributeModel> _ReferenceList;
        public ObservableCollection<AttributeModel> ReferenceList
        {
            get { return _ReferenceList; }
            set { _ReferenceList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedReference;
        public AttributeModel SelectedReference
        {
            get { return _SelectedReference; }
            set
            {
                _SelectedReference = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AttributeModel> _CountryList;
        public ObservableCollection<AttributeModel> CountryList
        {
            get { return _CountryList; }
            set { _CountryList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedCountry;
        public AttributeModel SelectedCountry
        {
            get { return _SelectedCountry; }
            set
            {
                _SelectedCountry = value;
                OnPropertyChanged();
                var provinceList = App.AttributeList.Where(o => o.parentId == _SelectedCountry._id);
                this.ProvinceList.Clear();
                foreach (var item in provinceList)
                {
                    this.ProvinceList.Add(item);
                }
            }
        }

        public string _CountryText;
        public string CountryText
        {
            get { return _CountryText; }
            set { _CountryText = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AttributeModel> _ProvinceList;
        public ObservableCollection<AttributeModel> ProvinceList
        {
            get { return _ProvinceList; }
            set { _ProvinceList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedProvince;
        public AttributeModel SelectedProvince
        {
            get { return _SelectedProvince; }
            set
            {
                _SelectedProvince = value;
                OnPropertyChanged();
                var cityList = App.AttributeList.Where(o => o.parentId == _SelectedProvince._id);
                this.CityList.Clear();
                foreach (var item in cityList)
                {
                    this.CityList.Add(item);
                }
            }
        }

        public string _ProvinceText;
        public string ProvinceText
        {
            get { return _ProvinceText; }
            set { _ProvinceText = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AttributeModel> _CityList;
        public ObservableCollection<AttributeModel> CityList
        {
            get { return _CityList; }
            set { _CityList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedCity;
        public AttributeModel SelectedCity
        {
            get { return _SelectedCity; }
            set { _SelectedCity = value; OnPropertyChanged(); }
        }

        public string _CityText;
        public string CityText
        {
            get { return _CityText; }
            set { _CityText = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AttributeModel> _GradeList;
        public ObservableCollection<AttributeModel> GradeList
        {
            get { return _GradeList; }
            set { _GradeList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedGrade;
        public AttributeModel SelectedGrade
        {
            get { return _SelectedGrade; }
            set { _SelectedGrade = value; OnPropertyChanged(); }
        }

        public string _GradeText;
        public string GradeText
        {
            get { return _GradeText; }
            set { _GradeText = value; OnPropertyChanged(); }
        }

        string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        string _address;
        public string address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }

        string _pcode;
        public string pcode
        {
            get { return _pcode; }
            set { _pcode = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            this.model.email = this.email;

            if (this.model.profile == null)
            {
                this.model.profile = new ProfileModel();
            }
            if (this.SelectedSex != null)
            {
                this.model.profile.sex = this.SelectedSex._id;
            }
            if (this.SelectedStatus != null)
            {
                this.model.profile.status = this.SelectedStatus._id;
            }
            if (this.SelectedJobState != null)
            {
                this.model.profile.jobState = this.SelectedJobState._id;
            }
            if (this.SelectedReligion != null)
            {
                this.model.profile.religion = this.SelectedReligion._id;
            }
            if (this.SelectedSect != null)
            {
                this.model.profile.sect = this.SelectedSect._id;
            }
            if (this.SelectedReference != null)
            {
                this.model.profile.reference = this.SelectedReference._id;
            }

            if (this.model.contact == null)
            {
                this.model.contact = new ContactModel();
            }
            if (this.SelectedCountry != null)
            {
                this.model.contact.country = this.SelectedCountry._id;
            }
            if (this.SelectedProvince != null)
            {
                this.model.contact.province = this.SelectedProvince._id;
            }
            if (this.SelectedCity != null)
            {
                this.model.contact.city = this.SelectedCity._id;
            }

            if (this.model.education == null)
            {
                this.model.education = new EducationModel();
            }
            if (this.SelectedGrade != null)
            {
                this.model.education.grade = this.SelectedGrade._id;
            }

            await this.accountService.Save(this.model);
            await this.Navigation.PushAsync(new EducationPage(this.model));
        }
    }
}