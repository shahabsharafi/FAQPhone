using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
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
    public partial class DiscountUsedPage : ContentPage
    {
        public DiscountUsedPage()
        {
            InitializeComponent();
            var factory = App.Resolve<DiscountUsedViewModelFactory>();
            var vm = factory.Create(this);
            this.Appearing += (sender, e) => {
                Task.Run(() => vm.loadItems()).Wait();
            };
            BindingContext = vm;
        }
    }

    public class DiscountUsedViewModelFactory
    {
        IDiscussionService discussionService;
        public DiscountUsedViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public DiscountUsedViewModel Create(ContentPage page)
        {
            return new DiscountUsedViewModel(this.discussionService, page);
        }
    }

    public class DiscountUsedViewModel : BaseViewModel
    {

        public DiscountUsedViewModel(IDiscussionService discussionService, ContentPage page) : base(page)
        {
            this.discussionService = discussionService;
            this.List = new ObservableCollection<DiscussionModel>();
            this.AddCommand = new Command(async () => await addCommand());
        }
        private IDiscussionService discussionService { get; set; }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        ObservableCollection<DiscussionModel> _list;
        public ObservableCollection<DiscussionModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public async Task loadItems()
        {
            var list = await this.discussionService.GetDiscoussionWithDiscount();
            this.setList(list);
        }

        private void setList(List<DiscussionModel> list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                string fName = item.usedDiscount?.owner?.profile?.firstName;
                string lName = item.usedDiscount?.owner?.profile?.lastName;
                string category = item.usedDiscount?.category?.caption ?? "";
                string count = item.usedDiscount?.count.ToString();
                string price = item.usedDiscount?.price.ToString();
                item.Caption =
                    fName.FormatString("{0} ", "") +
                    lName.FormatString("{0} ", "") +
                    category.FormatString("در بخش {0} ", "") +
                    count.FormatString("{0} عدد ", "") +
                    price.FormatString("{0} ریالی", "");
                this.List.Add(item);
            }
        }

        public ICommand AddCommand { protected set; get; }

        public async Task addCommand()
        {
            await this.Navigation.PushAsync(new DiscountNewPage());
        }
    }
}