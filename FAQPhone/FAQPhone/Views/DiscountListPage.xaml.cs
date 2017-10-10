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
using FAQPhone.Infrastructure;

namespace FAQPhone.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiscountListPage : ContentPage
    {
        public DiscountListPage()
        {
            InitializeComponent();
            var factory = App.Resolve<DiscountListViewModelFactory>();
            var vm = factory.Create(this);
            this.Appearing += (sender, e) => {
                Task.Run(() => vm.loadItems()).Wait();
            };
            BindingContext = vm;
        }
    }

    public class DiscountListViewModelFactory
    {
        IDiscountService discountService;
        public DiscountListViewModelFactory(IDiscountService discountService)
        {
            this.discountService = discountService;
        }
        public DiscountListViewModel Create(ContentPage page)
        {
            return new DiscountListViewModel(this.discountService, page);
        }
    }

    public class DiscountListViewModel : BaseViewModel
    {

        public DiscountListViewModel(IDiscountService discountService, ContentPage page) : base(page)
        {
            this.discountService = discountService;
            this.List = new ObservableCollection<DiscountModel>();
        }
        private IDiscountService discountService { get; set; }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        ObservableCollection<DiscountModel> _list;
        public ObservableCollection<DiscountModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public async Task loadItems()
        {
            var list = await this.discountService.GetList();
            this.setList(list);
        }

        private void setList(List<DiscountModel> list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                string fName = item.owner?.profile?.firstName;
                string lName = item.owner?.profile?.lastName;
                string category = item.category?.caption ?? "";
                item.Caption = 
                    fName.FormatString("{0} ", "") +
                    lName.FormatString("{0} ", "") + 
                    category.FormatString("در بخش {0} ", "") +
                    item.count.ToString().FormatString("{0} عدد ", "") +
                    item.price.ToString().FormatString("{0} ریالی", "");
                this.List.Add(item);
            }
        }
    }
}