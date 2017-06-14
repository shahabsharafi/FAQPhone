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

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    string parentId = (_selectedItem as DepartmentModel)._id;
                    Task.Run(async () => await loadItems(parentId));
                }
                OnPropertyChanged();
            }
        }

        public async Task loadItems(string parentId)
        {
            var list = await this.departmentService.get(parentId);
            if (parentId != "" && (list == null || list.Count == 0))
            {
                await this.Navigation.PushAsync(new DiscussionPage());
            }
            else
            {
                this.List.Clear();
                foreach (var item in list)
                {
                    this.List.Add(item);
                }
            }
        }
    }
}