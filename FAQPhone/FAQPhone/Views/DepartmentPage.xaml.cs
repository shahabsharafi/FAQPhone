using FAQPhone.Inferstructure;
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
            this.SelectCommand = new Command(async () => await selectCommand());
            this.ParentId = "";
            this.list = new ObservableCollection<DepartmentModel>();
            Task.Run(async () => await loadItems());
        }
        private IDepartmentService departmentService { get; set; }
        public string ParentId { get; set; }

        public ObservableCollection<DepartmentModel> list { get; set; }

        public ICommand SelectCommand { get; }

        public async Task selectCommand()
        {
            ///// 
            await loadItems();
        }
        public async Task loadItems()
        {
            this.list.Clear();
            var list = await this.departmentService.get(this.ParentId);
            foreach (var item in list)
            {
                this.list.Add(item);
            }
        }
    }
}