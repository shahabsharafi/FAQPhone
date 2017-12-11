using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountFeedbackPage : ContentPage
    {
        public AccountFeedbackPage(AccountModel model)
        {
            InitializeComponent();
            var factory = App.Resolve<AccountFeedbackViewModelFactory>();
            var vm = factory.Create(this, model);
            BindingContext = vm;
        }
    }

    public class AccountFeedbackViewModelFactory
    {
        IAccountService accountService;
        public AccountFeedbackViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public AccountFeedbackViewModel Create(ContentPage page, AccountModel model)
        {
            return new AccountFeedbackViewModel(this.accountService, page, model);
        }
    }

    public class AccountFeedbackViewModel : BaseViewModel
    {

        public AccountFeedbackViewModel(IAccountService accountService, ContentPage page, AccountModel model) : base(page)
        {
            this.accountService = accountService;
            this.List = new ObservableCollection<AccountComment>();
            this.SelectItemCommand = new Command<AccountComment>((d) => selectItemCommand(d));
            this.SaveCommand = new Command(async () => await saveCommand());
            this.model = model;
            this.setFields(this.model);
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
            if (model.comments != null)
            {
                foreach (var item in model.comments)
                {
                    this.List.Add(item);
                }
            }            
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

        string _replay;
        public string replay
        {
            get { return _replay; }
            set
            {
                _replay = value;
                OnPropertyChanged();
                CanSending = !string.IsNullOrWhiteSpace(_replay);
            }
        }

        bool _CanSending;
        public bool CanSending
        {
            get { return _CanSending; }
            set { _CanSending = value; OnPropertyChanged(); }
        }

        ObservableCollection<AccountComment> _list;
        public ObservableCollection<AccountComment> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        public ICommand SelectItemCommand { protected set; get; }

        public void selectItemCommand(AccountComment d)
        {
            if (d == null)
                return;
            this.SelectedItem = null;
        }

        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            if (!string.IsNullOrWhiteSpace(this.replay))
            {
                var l = this.model.comments.ToList();
                l.Add(new AccountComment()
                {
                    createDate = DateTime.Now,
                    text = this.replay
                });
                model.comments = l.ToArray();
                await this.accountService.Save(this.model);
                await this.Navigation.PopAsync();
            }
        }
    }
}

