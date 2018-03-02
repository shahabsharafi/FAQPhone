using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
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
    public partial class DiscussionNewPage : ContentPage
    {
        public DiscussionNewPage(DepartmentModel departmentModel, AccountModel owner, DiscountModel discount, int pushCount)
        {
            InitializeComponent();                      
            if (departmentModel == null)
            {
                this.ToolbarItems.RemoveAt(0);
            }
            var factory = App.Resolve<DiscussionEditViewModelFactory>();
            var vm = factory.Create(this, departmentModel, owner, discount);
            bool isFirstTime = true;
            this.Appearing += (sender, e) => {
                Task.Run(async () => await vm.Load()).Wait();
                if (isFirstTime)
                {
                    isFirstTime = false;
                    for (var i = 0; i < pushCount; i++)
                    {
                        this.Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    }
                }
            };
            BindingContext = vm;
        }
    }

    public class DiscussionEditViewModelFactory
    {
        IAccountService accountService;
        public DiscussionEditViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public DiscussionEditViewModel Create(ContentPage page, DepartmentModel departmentModel, AccountModel owner, DiscountModel discount)
        {
            return new DiscussionEditViewModel(accountService, page, departmentModel, owner, discount);
        }
    }

    public class DiscussionEditViewModel : BaseViewModel
    {
        public DiscussionEditViewModel(IAccountService accountService, ContentPage page, DepartmentModel departmentModel, AccountModel owner, DiscountModel discount) : base(page)
        {
            this.accountService = accountService;
            this.CanNext = false;
            this.UsedDiscount = false;
            this.HasDiscount = (discount != null);
            if (this.HasDiscount)
            {
                this.discount = discount;
                this.discountPrice = discount.price + " " +
                    ResourceManagerHelper.GetValue("unit_of_mony_caption");                
            }
            this.NextCommand = new Command(async () => await nextCommand());
            this.DiscountCommand = new Command(async () => await discountCommand());
            this.RuleCommand = new Command(async () => await ruleCommand());
            this.departmentModel = departmentModel;
            this.owner = owner;
            this.price = (owner != null ? (owner.price ?? 0) : (departmentModel?.price ?? 0));
            this.priceCaption = 
                ResourceManagerHelper.GetValue("discussion_recive_price") + ":" +
                this.price + " " + 
                ResourceManagerHelper.GetValue("unit_of_mony_caption");
        }

        long price;

        IAccountService accountService;
        AccountModel me;
        public async Task Load()
        {
            this.me = await accountService.GetMe();

            this.creditCaption =string.Format("{0}: {1}", ResourceManagerHelper.GetValue("account_credit"), this.me.credit);
            if (me.credit < this.price)
            {
                this.Editable = false;
                this.HasDiscount = false;
                this.HasCreditError = true;
            }
            else
            {
                this.HasCreditError = false;
                this.Editable = true;
            }
        }

        string _creditCaption;
        public string creditCaption
        {
            get { return _creditCaption; }
            set { _creditCaption = value; OnPropertyChanged(); }
        }

        string _title;
        public string title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length <= 30)
                {
                    _title = value;
                    CanNext = !string.IsNullOrWhiteSpace(_title);
                }
                OnPropertyChanged();
            }
        }

        bool _CanNext;
        public bool CanNext
        {
            get { return _CanNext; }
            set { _CanNext = value; OnPropertyChanged(); }
        }

        bool _Editable;
        public bool Editable
        {
            get { return _Editable; }
            set { _Editable = value; OnPropertyChanged(); }
        }

        bool _HasDiscount;
        public bool HasDiscount
        {
            get { return _HasDiscount; }
            set { _HasDiscount = value; OnPropertyChanged(); }
        }
        
        bool _HasCreditError;
        public bool HasCreditError
        {
            get { return _HasCreditError; }
            set { _HasCreditError = value; OnPropertyChanged(); }
        }

        bool _UsedDiscount;
        public bool UsedDiscount
        {
            get { return _UsedDiscount; }
            set { _UsedDiscount = value; OnPropertyChanged(); }
        }

        string _priceCaption;
        public string priceCaption
        {
            get { return _priceCaption; }
            set { _priceCaption = value; OnPropertyChanged(); }
        }

        string _discountPrice;
        public string discountPrice
        {
            get { return _discountPrice; }
            set { _discountPrice = value; OnPropertyChanged(); }
        }

        private DepartmentModel departmentModel { get; set; }
        private AccountModel owner { get; set; }
        private DiscountModel discount { get;set; }

        public ICommand DiscountCommand { protected set; get; }

        public async Task discountCommand()
        {
            this.UsedDiscount = !this.UsedDiscount;
        }
        public ICommand NextCommand { protected set; get; }

        public async Task nextCommand()
        {
            /////
            DiscussionModel model = new DiscussionModel()
            {
                title = this.title,
                from = new AccountModel() { username = App.Username },
                createDate = DateTime.Now,
                state = Constants.DISCUSSION_STATE_CREATE,
                userRead = false,
                operatorRead = false,
                usedDiscount = this.discount != null ? this.discount._id  : null,
                department = this.departmentModel != null ? new DepartmentModel() { _id = this.departmentModel._id } : null,
                items = new DiscussionDetailModel[] { }
            };
            await this.Navigation.PushAsync(new ChatPage(Constants.USER_INPROGRESS_FAQ, model, 1));
            //await this.Navigation.PushAsync(new ChatPage(Constants.USER_INPROGRESS_FAQ, model));
        }

        public ICommand RuleCommand { protected set; get; }

        public async Task ruleCommand()
        {
            if (this.departmentModel != null)
            {
                var title = ResourceManagerHelper.GetValue(Constants.RULES);
                var text = this.departmentModel.userRule;
                await this.Navigation.PushAsync(new TextPage(title, text));
            }
        }
    }
}
