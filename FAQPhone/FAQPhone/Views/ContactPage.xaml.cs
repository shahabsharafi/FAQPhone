using FAQPhone.Inferstructure;
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
    public partial class ContactPage : ContentPage
    {
        public ContactPage(AccountModel model)
        {
            InitializeComponent();
            this.Appearing += (sender, e) => {
                this.Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            };
            var factory = App.Resolve<ContactViewModelFactory>();
            BindingContext = factory.Create(Navigation, model);
        }
    }

    public class ContactViewModelFactory
    {
        IAccountService accountService;
        public ContactViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public ContactViewModel Create(INavigation navigation, AccountModel model)
        {
            return new ContactViewModel(this.accountService, navigation, model);
        }
    }

    public class ContactViewModel : BaseViewModel
    {

        public ContactViewModel(IAccountService accountService, INavigation navigation, AccountModel model) : base(navigation)
        {
            this.accountService = accountService;
            this.model = model;
            this.SaveCommand = new Command(async () => await saveCommand());
            this.ProvinceList = new ObservableCollection<AttributeModel>();
            this.CityList = new ObservableCollection<AttributeModel>();
            var provinceList = App.AttributeList.Where(o => o.type == "province");
            foreach (var item in provinceList)
            {
                this.ProvinceList.Add(item);
            }
            this.email = this.model.email;
            if (this.model.contact != null)
            {
                this.house = model.contact.house;
                this.work = model.contact.work;
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
                this.address = model.contact.address;
                this.pcode = model.contact.pcode;
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
            if (this.model.contact == null)
            {
                this.model.contact = new ContactModel();
            }
            this.model.email = this.email;
            this.model.contact.house= this.house;
            this.model.contact.work = this.work;
            if (this.SelectedProvince != null)
            {
                this.model.contact.province = this.SelectedProvince._id;
            }
            if (this.SelectedCity != null)
            {
                this.model.contact.city = this.SelectedCity._id;
            }
            this.model.contact.address = this.address;
            this.model.contact.pcode = this.pcode;
            await this.accountService.Save(this.model);
            await this.Navigation.PushAsync(new EducationPage(this.model));
        }
    }
}
