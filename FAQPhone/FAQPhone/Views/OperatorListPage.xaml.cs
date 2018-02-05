using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using FilePicker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
            this.SelectItemCommand = new Command<AccountModel>(async (model) => await selectItemCommand(model));
            this._downloadHelper = new DownloadHelper<List<AccountModel>>();
        }
        private IAccountService accountService { get; set; }
        private IDepartmentService departmentService { get; set; }
        private IDiscountService discountService { get; set; }

        DownloadHelper<List<AccountModel>> _downloadHelper;

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
            Action<List<AccountModel>> action = (List<AccountModel> l) =>
            {
                var fileService = DependencyService.Get<IFileService>();
                this.List.Clear();
                foreach (var item in l)
                {
                    item.FullName = (item.profile?.firstName ?? "") + " " + (item.profile?.lastName ?? "");
                    string major = item.education?.major ?? "";
                    string university = item.education?.university ?? "";
                    var majorObj = App.AttributeList.SingleOrDefault(o => o._id == major);
                    var universityObj = App.AttributeList.SingleOrDefault(o => o._id == university);
                    item.Title = (majorObj?.caption ?? "");
                    item.Description = "دانشگاه " + (universityObj?.caption ?? "");
                    item.IsOnline = (item.state == 2);                    
                    string documentsPath = fileService.GetDocumentsPath();
                    item.PictureUrl = Path.Combine(documentsPath, item.PictureName);
                    this.List.Add(item);
                }
            };
            this._downloadHelper.Start(new Queue<string>(list.Select(o => o.PictureName)), action, list);
            this._downloadHelper.Failed += _downloadHelper_Failed;    
            
        }

        private void _downloadHelper_Failed(object sender, EventArgs e)
        {
            //Utility.Alert();
        }

        public ICommand SelectItemCommand { protected set; get; }

        public async Task selectItemCommand(AccountModel model)
        {
            if (model == null)
                return;
            this.SelectedItem = null;
            //await this.Navigation.PushAsync(new DiscussionNewPage(null, model, null, 0));
            await this.Navigation.PushAsync(new ProfileInfoPage(model, Constants.ACCESS_OPERATOR));
        }
    }
}