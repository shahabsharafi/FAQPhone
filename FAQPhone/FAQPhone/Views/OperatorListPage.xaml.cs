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
    public partial class OperatorListPage : ContentPage
    {
        public OperatorListPage()
        {
            InitializeComponent();
            var factory = App.Resolve<OperatorListViewModelFactory>();
            var vm = factory.Create(this);
            this.Appearing += (sender, e) => {
                Task.Run(() => vm.loadItems()).Wait();
            };
            BindingContext = vm;
        }
    }

    public class OperatorListViewModelFactory
    {
        IAccountService accountService;
        IDepartmentService departmentService;
        IDiscountService discountService;
        public OperatorListViewModelFactory(IAccountService accountService, IDepartmentService departmentService, IDiscountService discountService)
        {
            this.accountService = accountService;
            this.departmentService = departmentService;
            this.discountService = discountService;
        }
        public OperatorListViewModel Create(ContentPage page)
        {
            return new OperatorListViewModel(this.accountService, this.departmentService, this.discountService, page);
        }
    }

    public class OperatorListViewModel : BaseViewModel
    {

        public OperatorListViewModel(IAccountService accountService, IDepartmentService departmentService, IDiscountService discountService, ContentPage page) : base(page)
        {
            this.accountService = accountService;
            this.discountService = discountService;
            this.departmentService = departmentService;
            this.List = new ObservableCollection<AccountModel>();
            this.AddCommand = new Command(async () => await addCommand());
            this.SelectItemCommand = new Command<AccountModel>((model) => selectItemCommand(model));
        }
        private IAccountService accountService { get; set; }
        private IDepartmentService departmentService { get; set; }
        private IDiscountService discountService { get; set; }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        ObservableCollection<AccountModel> _list;
        public ObservableCollection<AccountModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public async Task loadItems()
        {
            var list = await this.accountService.GetOperatoreList();
            this.setList(list);
        }

        private void setList(List<AccountModel> list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                item.FullName = (item.profile?.firstName ?? "") + " " + (item.profile?.lastName ?? "");
                string major = item.education?.major ?? "";
                string university = item.education?.university ?? "";
                var majorObj = App.AttributeList.SingleOrDefault(o => o._id == major);
                var universityObj = App.AttributeList.SingleOrDefault(o => o._id == university);
                item.Title = (majorObj?.caption ?? "") + " دانشگاه " + (universityObj?.caption ?? "");
                this.List.Add(item);
            }
        }

        public ICommand AddCommand { protected set; get; }

        public async Task addCommand()
        {
            
        }

        public ICommand SelectItemCommand { protected set; get; }

        public void selectItemCommand(AccountModel model)
        {
            if (model == null)
                return;
            this.SelectedItem = null;
        }
    }
}