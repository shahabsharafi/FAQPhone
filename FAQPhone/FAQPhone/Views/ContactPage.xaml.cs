using FAQPhone.Inferstructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
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

        string _province;
        public string province
        {
            get { return _province; }
            set { _province = value; OnPropertyChanged(); }
        }

        string _city;
        public string city
        {
            get { return _city; }
            set { _city = value; OnPropertyChanged(); }
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
            this.model.email = "";
            this.model.sms = "";
            if (this.model.contact == null)
            {
                this.model.contact = new Contact();
            }            
            this.model.contact.house= this.house;
            this.model.contact.work = this.work;
            this.model.contact.province = this.province;
            this.model.contact.city = this.city;
            this.model.contact.address = this.address;
            this.model.contact.pcode = this.pcode;
            await this.accountService.Save(this.model);
        }
    }
}