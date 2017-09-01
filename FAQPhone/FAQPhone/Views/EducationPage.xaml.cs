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
    public partial class EducationPage : ContentPage
    {
        public EducationPage(AccountModel model)
        {
            InitializeComponent();
            this.Appearing += (sender, e) => {
                this.Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            };
            var factory = App.Resolve<EducationViewModelFactory>();
            BindingContext = factory.Create(this, model);
        }
    }

    public class EducationViewModelFactory
    {
        IAccountService accountService;
        public EducationViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public EducationViewModel Create(ContentPage page, AccountModel model)
        {
            return new EducationViewModel(this.accountService, page, model);
        }
    }

    public class EducationViewModel : BaseViewModel
    {

        public EducationViewModel(IAccountService accountService, ContentPage page, AccountModel model) : base(page)
        {
            this.accountService = accountService;
            this.model = model;
            this.SaveCommand = new Command(async () => await saveCommand());
            this.GradeList = new ObservableCollection<AttributeModel>();
            this.MajorList = new ObservableCollection<AttributeModel>();
            this.UniversityList = new ObservableCollection<AttributeModel>();
            this.LevelList = new ObservableCollection<AttributeModel>();
            var gradeList = App.AttributeList.Where(o => o.type == "grade");
            foreach (var item in gradeList)
            {
                this.GradeList.Add(item);
            }
            var majorList = App.AttributeList.Where(o => o.type == "major");
            foreach (var item in majorList)
            {
                this.MajorList.Add(item);
            }
            var universityList = App.AttributeList.Where(o => o.type == "university");
            foreach (var item in universityList)
            {
                this.UniversityList.Add(item);
            }
            var levelList = App.AttributeList.Where(o => o.type == "level");
            foreach (var item in levelList)
            {
                this.LevelList.Add(item);
            }
            if (this.model.education != null)
            {
                if (this.model.education.grade != null)
                {
                    this.SelectedGrade = App.AttributeList.Find(o => o._id == this.model.education.grade);
                    this.GradeText = this.SelectedGrade.caption;
                }
                if (this.model.education.major != null)
                {
                    this.SelectedMajor = App.AttributeList.Find(o => o._id == this.model.education.major);
                    this.MajorText = this.SelectedMajor.caption;
                }
                if (this.model.education.university != null)
                {
                    this.SelectedUniversity = App.AttributeList.Find(o => o._id == this.model.education.university);
                    this.UniversityText = this.SelectedUniversity.caption;
                }
                if (this.model.education.level != null)
                {
                    this.SelectedLevel = App.AttributeList.Find(o => o._id == this.model.education.level);
                    this.LevelText = this.SelectedLevel.caption;
                }
            }
        }
        private IAccountService accountService { get; set; }
        AccountModel model { get; set; }

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

        private ObservableCollection<AttributeModel> _MajorList;
        public ObservableCollection<AttributeModel> MajorList
        {
            get { return _MajorList; }
            set { _MajorList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedMajor;
        public AttributeModel SelectedMajor
        {
            get { return _SelectedMajor; }
            set { _SelectedMajor = value; OnPropertyChanged(); }
        }

        public string _MajorText;
        public string MajorText
        {
            get { return _MajorText; }
            set { _MajorText = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AttributeModel> _UniversityList;
        public ObservableCollection<AttributeModel> UniversityList
        {
            get { return _UniversityList; }
            set { _UniversityList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedUniversity;
        public AttributeModel SelectedUniversity
        {
            get { return _SelectedUniversity; }
            set { _SelectedUniversity = value; OnPropertyChanged(); }
        }

        public string _UniversityText;
        public string UniversityText
        {
            get { return _UniversityText; }
            set { _UniversityText = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AttributeModel> _LevelList;
        public ObservableCollection<AttributeModel> LevelList
        {
            get { return _LevelList; }
            set { _LevelList = value; OnPropertyChanged(); }
        }

        private AttributeModel _SelectedLevel;
        public AttributeModel SelectedLevel
        {
            get { return _SelectedLevel; }
            set { _SelectedLevel = value; OnPropertyChanged(); }
        }

        public string _LevelText;
        public string LevelText
        {
            get { return _LevelText; }
            set { _LevelText = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            if (this.model.education == null)
            {
                this.model.education = new EducationModel();
            }
            if (this.SelectedGrade != null)
            {
                this.model.education.grade = this.SelectedGrade._id;
            }
            if (this.SelectedMajor != null)
            {
                this.model.education.major = this.SelectedMajor._id;
            }
            if (this.SelectedUniversity != null)
            {
                this.model.education.university = this.SelectedUniversity._id;
            }
            if (this.SelectedLevel != null)
            {
                this.model.education.level = this.SelectedLevel._id;
            }
            await this.accountService.Save(this.model);
            await this.Navigation.PopAsync();
        }
    }
}
