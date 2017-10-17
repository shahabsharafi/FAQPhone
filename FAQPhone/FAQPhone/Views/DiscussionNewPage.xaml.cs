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
        public DiscussionNewPage(DepartmentModel department, int pushCount)
        {
            InitializeComponent();
            this.Appearing += (sender, e) => {
                for (var i = 0; i < pushCount; i++)
                {
                    this.Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
            };            
            var factory = App.Resolve<DiscussionEditViewModelFactory>();
            BindingContext = factory.Create(this, department);
        }
    }

    public class DiscussionEditViewModelFactory
    {
        public DiscussionEditViewModelFactory()
        {
            
        }
        public DiscussionEditViewModel Create(ContentPage page, DepartmentModel department)
        {
            return new DiscussionEditViewModel(page, department);
        }
    }

    public class DiscussionEditViewModel : BaseViewModel
    {
        public DiscussionEditViewModel(ContentPage page, DepartmentModel department) : base(page)
        {            
            this.CanNext = false;
            this.UsedDiscount = false;
            this.HasDiscount = true;
            this.discount = 1000 + " " +
                ResourceManagerHelper.GetValue("unit_of_mony_caption");
            this.NextCommand = new Command(async () => await nextCommand());
            this.DiscountCommand = new Command(async () => await discountCommand());
            this.department = department;
            this.price = 
                ResourceManagerHelper.GetValue("discussion_recive_price") + ":" + 
                department.price + " " + 
                ResourceManagerHelper.GetValue("unit_of_mony_caption");
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

        bool _HasDiscount;
        public bool HasDiscount
        {
            get { return _HasDiscount; }
            set { _HasDiscount = value; OnPropertyChanged(); }
        }

        bool _UsedDiscount;
        public bool UsedDiscount
        {
            get { return _UsedDiscount; }
            set { _UsedDiscount = value; OnPropertyChanged(); }
        }

        string _price;
        public string price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged(); }
        }

        string _discount;
        public string discount
        {
            get { return _discount; }
            set { _discount = value; OnPropertyChanged(); }
        }

        private DepartmentModel department { get; set; }

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
                department = new DepartmentModel() { _id = this.department._id },
                items = new DiscussionDetailModel[] { }
            };
            await this.Navigation.PushAsync(new ChatPage(Constants.USER_INPROGRESS_FAQ, model, 1));
            //await this.Navigation.PushAsync(new ChatPage(Constants.USER_INPROGRESS_FAQ, model));
        }
    }
}
