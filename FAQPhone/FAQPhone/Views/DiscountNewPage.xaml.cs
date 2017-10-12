using FAQPhone.DepartmentPicker;
using FAQPhone.Infarstructure;
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
    public partial class DiscountNewPage : ContentPage
    {
        public DiscountNewPage()
        {
            InitializeComponent();
            var factory = App.Resolve<DiscountNewFactory>();
            BindingContext = factory.Create(this);
        }
    }

    public class DiscountNewFactory
    {
        IDiscountService discountService;
        public DiscountNewFactory(IDiscountService discountService)
        {
            this.discountService = discountService;
        }
        public DiscountNewViewModel Create(ContentPage page)
        {
            return new DiscountNewViewModel(this.discountService, page);
        }
    }

    public class DiscountNewViewModel : BaseViewModel
    {

        public DiscountNewViewModel(IDiscountService discountService, ContentPage page) : base(page)
        {
            this.discountService = discountService;
            this.category = null;
            this.SaveCommand = new Command(async () => await saveCommand());
            this.SelectCommand = new Command(async () => await selectCommand());
            this.ClearCommand = new Command(async () => await clearCommand());
        }
        private IDiscountService discountService { get; set; }

        string _price;
        public string price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged();
                setTotal();
            }
        }

        private void setTotal()
        {
            int price, count;
            if (int.TryParse(this.price, out price) && int.TryParse(this.count, out count))
            {
                this.total = (price * count).ToString();
            }
        }

        string _count;
        public string count
        {
            get { return _count; }
            set {
                _count = value;
                OnPropertyChanged();
                setTotal();
            }
        }

        string _total;
        public string total
        {
            get { return _total; }
            set { _total = value; OnPropertyChanged(); }
        }

        DepartmentModel _category;
        public DepartmentModel category
        {
            get { return _category; }
            set {
                _category = value;
                OnPropertyChanged();
                HasCategory = (value != null);
            }
        }

        bool _HasCategory;
        public bool HasCategory
        {
            get { return _HasCategory; }
            set { _HasCategory = value; OnPropertyChanged(); }
        }

        public ICommand SelectCommand { protected set; get; }

        public async Task selectCommand()
        {            
            DepartmentPicker.DepartmentPickerPage departmentPicker = DepartmentPickerFactory.GetPicker(this.Navigation);
            await departmentPicker.Open();
            departmentPicker.Select += (sender, e) =>
            {
                this.category = departmentPicker.SelectedItem;
            };
        }
        public ICommand ClearCommand { protected set; get; }

        public async Task clearCommand()
        {
            this.category = null;
        }
        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            int price, count, total;
            if (int.TryParse(this.price, out price) && 
                int.TryParse(this.count, out count) &&
                int.TryParse(this.total, out total))
            {
                DiscountModel model = new DiscountModel()
                {
                    owner = new AccountModel() { username = App.Username },
                    price = price,
                    count = count,
                    total = total,
                    category = this.category
                };
                await this.discountService.Save(model);
                await this.Navigation.PopAsync();
            }            
        }
    }
}