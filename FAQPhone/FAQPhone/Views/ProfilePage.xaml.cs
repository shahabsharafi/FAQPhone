using FAQPhone.Inferstructure;
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
            BindingContext = factory.Create(Navigation, model);
        }
    }

    public class ProfileViewModelFactory
    {
        IAccountService accountService;
        public ProfileViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public ProfileViewModel Create(INavigation navigation, AccountModel model)
        {
            return new ProfileViewModel(this.accountService, navigation, model);
        }
    }

    public class ProfileViewModel : BaseViewModel
    {

        public ProfileViewModel(IAccountService accountService, INavigation navigation, AccountModel model) : base(navigation)
        {
            this.accountService = accountService;
            this.model = model;
            this.SaveCommand = new Command(async () => await saveCommand());
        }
        private IAccountService accountService { get; set; }
        AccountModel model { get; set; }        

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

        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            if (this.model.profile == null)
            {
                this.model.profile = new ProfileModel();
            }
            this.model.profile.firstName = this.firstName;
            this.model.profile.lastName = this.lastName;
            this.model.profile.fatherName = this.fatherName;
            this.model.profile.no = this.no;
            this.model.profile.placeOfIssues = this.placeOfIssues;
            this.model.profile.nationalCode = this.nationalCode;
            this.model.profile.birthPlace = this.birthPlace;
            await this.accountService.Save(this.model);
            await this.RootNavigate(new ContactPage(new AccountModel()));
        }
    }
}