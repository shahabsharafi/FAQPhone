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
            }
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
            await this.Navigation.PushAsync(new ContactPage(this.model));
        }
    }
}