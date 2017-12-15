using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(AccountModel model)
        {
            InitializeComponent();
            var factory = App.Resolve<ProfileViewModelFactory>();
            BindingContext = factory.Create(this, model);
        }
    }

    public class ProfileViewModelFactory
    {
        IAccountService accountService;
        public ProfileViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public ProfileViewModel Create(ContentPage page, AccountModel model)
        {
            return new ProfileViewModel(this.accountService, page, model);
        }
    }

    public class ProfileViewModel : BaseViewModel
    {

        public ProfileViewModel(IAccountService accountService, ContentPage page, AccountModel model) : base(page)
        {
            this.accountService = accountService;
            this.model = model;
            this.SexList = new ObservableCollection<AttributeModel>();
            this.StatusList = new ObservableCollection<AttributeModel>();
            this.YearList = new ObservableCollection<AttributeModel>();
            this.MonthList = new ObservableCollection<AttributeModel>();
            this.DayList = new ObservableCollection<AttributeModel>();
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
            var d = Utility.MiladiToShamsi(DateTime.Now);
            var y = d[0];
            for (int i = y; i >= 1320; i--)
            {
                this.YearList.Add(new AttributeModel { caption = i.ToString() });
            }
            for (int i = 1; i <= 12; i++)
            {
                this.MonthList.Add(new AttributeModel { caption = i.ToString() });
            }
            for (int i = 1; i <= 31; i++)
            {
                this.DayList.Add(new AttributeModel { caption = i.ToString() });
            }
            this.sexPrevention = model.sexPrevention;
            if (this.model.profile != null)
            {
                this.SaveCommand = new Command(async () => await saveCommand());
                this.firstName = model.profile.firstName;
                this.lastName = model.profile.lastName;
                this.fatherName = model.profile.fatherName;
                this.no = model.profile.no;
                this.placeOfIssues = model.profile.placeOfIssues;
                this.nationalCode = model.profile.nationalCode;
                this.birthPlace = model.profile.birthPlace;                
                if (model.profile.birthDay != null)
                {
                    var birthDay = Utility.MiladiToShamsi(model.profile.birthDay.Value);
                    this.SelectedYear = this.YearList.ToList().Find(o => o.caption == birthDay[0].ToString());
                    this.SelectedMonth = this.MonthList.ToList().Find(o => o.caption == birthDay[1].ToString());
                    this.SelectedDay = this.DayList.ToList().Find(o => o.caption == birthDay[2].ToString());
                }
                if (model.profile.sex != null)
                {
                    this.SelectedSex = App.AttributeList.Find(o => o._id == model.profile.sex);
                }
                if (model.profile.status != null)
                {
                    this.SelectedStatus = App.AttributeList.Find(o => o._id == model.profile.status);
                }
            }
        }
        private IAccountService accountService { get; set; }
        AccountModel model { get; set; }

        private ObservableCollection<AttributeModel> _YearList;
        public ObservableCollection<AttributeModel> YearList
        {
            get { return _YearList; }
            set { _YearList = value; }
        }

        private AttributeModel _SelectedYear;
        public AttributeModel SelectedYear
        {
            get { return _SelectedYear; }
            set
            {
                _SelectedYear = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AttributeModel> _MonthList;
        public ObservableCollection<AttributeModel> MonthList
        {
            get { return _MonthList; }
            set { _MonthList = value; }
        }

        private AttributeModel _SelectedMonth;
        public AttributeModel SelectedMonth
        {
            get { return _SelectedMonth; }
            set
            {
                _SelectedMonth = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AttributeModel> _DayList;
        public ObservableCollection<AttributeModel> DayList
        {
            get { return _DayList; }
            set { _DayList = value; }
        }

        private AttributeModel _SelectedDay;
        public AttributeModel SelectedDay
        {
            get { return _SelectedDay; }
            set
            {
                _SelectedDay = value;
                OnPropertyChanged();
            }
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

        string _firstName;
        public string firstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(); }
        }

        string _lastName;
        public string lastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(); }
        }

        string _fatherName;
        public string fatherName
        {
            get { return _fatherName; }
            set { _fatherName = value; OnPropertyChanged(); }
        }

        string _no;
        public string no
        {
            get { return _no; }
            set { _no = value; OnPropertyChanged(); }
        }

        string _placeOfIssues;
        public string placeOfIssues
        {
            get { return _placeOfIssues; }
            set { _placeOfIssues = value; OnPropertyChanged(); }
        }

        string _nationalCode;
        public string nationalCode
        {
            get { return _nationalCode; }
            set { _nationalCode = value; OnPropertyChanged(); }
        }

        string _birthPlace;
        public string birthPlace
        {
            get { return _birthPlace; }
            set { _birthPlace = value; OnPropertyChanged(); }
        }

        bool _sexPrevention;
        public bool sexPrevention
        {
            get { return _sexPrevention; }
            set { _sexPrevention = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            if (this.model.profile == null)
            {
                this.model.profile = new ProfileModel();
            }
            if (this.SelectedYear != null &&
               this.SelectedMonth != null &&
               this.SelectedDay != null)
            {
                int year, month, day;
                if (int.TryParse(this.SelectedYear.caption, out year) &&
                    int.TryParse(this.SelectedMonth.caption, out month) &&
                    int.TryParse(this.SelectedDay.caption, out day))
                {
                    this.model.profile.birthDay = Utility.ShamsiToMiladi(year, month, day);
                }
            }
            if (this.SelectedSex != null)
            {
                this.model.profile.sex = this.SelectedSex._id;
            }
            if (this.SelectedStatus != null)
            {
                this.model.profile.status = this.SelectedStatus._id;
            }
            this.model.profile.firstName = this.firstName;
            this.model.profile.lastName = this.lastName;
            this.model.profile.fatherName = this.fatherName;
            this.model.profile.no = this.no;
            this.model.profile.placeOfIssues = this.placeOfIssues;
            this.model.profile.nationalCode = this.nationalCode;
            this.model.profile.birthPlace = this.birthPlace;
            this.model.sexPrevention = this.sexPrevention;
            await this.accountService.Save(this.model);
            await this.Navigation.PushAsync(new ContactPage(this.model));
        }
    }
}