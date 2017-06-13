using FAQPhone.Inferstructure;
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
    public partial class DepartmentPage : ContentPage
    {
        public DepartmentPage()
        {
            InitializeComponent();
            var factory = App.Resolve<DepartmentPageViewModelFactory>();
            BindingContext = factory.Create(Navigation);
        }
    }

    public class DepartmentPageViewModelFactory
    {
        IDepartmentService departmentService;
        public DepartmentPageViewModelFactory(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }
        public DepartmentPageViewModel Create(INavigation navigation)
        {
            return new DepartmentPageViewModel(this.departmentService, navigation);
        }
    }

    public class DepartmentPageViewModel : BaseViewModel
    {

        public DepartmentPageViewModel(IDepartmentService departmentService, INavigation navigation) : base(navigation)
        {
            this.departmentService = departmentService;
            this.SelectItemCommand = new Command<string>(async (parentId) => await selectItemCommand(parentId));
            this.List = new ObservableCollection<DepartmentModel>();
            Task.Run(async () => await loadItems(""));
        }
        private IDepartmentService departmentService { get; set; }


        ObservableCollection<DepartmentModel> _list;
        public ObservableCollection<DepartmentModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public ICommand SelectItemCommand { get; }

        public async Task selectItemCommand(string parentId)
        {
            ///// 
            await loadItems(parentId);
        }
        public async Task loadItems(string parentId)
        {
            this.List.Clear();
            var list = await this.departmentService.get(parentId);
            foreach (var item in list)
            {
                this.List.Add(item);
            }
        }
    }
}