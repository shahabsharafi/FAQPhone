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
        public DepartmentPage(List<DepartmentModel> list = null)
        {
            InitializeComponent();
            var factory = App.Resolve<DepartmentPageViewModelFactory>();
            BindingContext = factory.Create(Navigation, list);
        }
    }

    public class DepartmentPageViewModelFactory
    {
        IDepartmentService departmentService;
        public DepartmentPageViewModelFactory(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }
        public DepartmentPageViewModel Create(INavigation navigation, List<DepartmentModel> list)
        {
            return new DepartmentPageViewModel(this.departmentService, navigation, list);
        }
    }

    public class DepartmentPageViewModel : BaseViewModel
    {

        public DepartmentPageViewModel(IDepartmentService departmentService, INavigation navigation, List<DepartmentModel> list) : base(navigation)
        {
            this.departmentService = departmentService;
            this.SelectItemCommand = new Command<DepartmentModel>(async(model) => await selectItemCommand(model));
            this.List = new ObservableCollection<DepartmentModel>();
            if (list == null)
            {
                Task.Run(async () => await loadItems(""));
            }
            else
            {
                this.setList(list);
            }
        }
        private IDepartmentService departmentService { get; set; }


        ObservableCollection<DepartmentModel> _list;
        public ObservableCollection<DepartmentModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public async Task loadItems(string parentId)
        {
            var list = await this.departmentService.GetByParent(parentId);
            if (parentId == "")
            {
                this.setList(list);
            }
            else if (list == null || list.Count == 0)
            {
                await this.Navigation.PushAsync(new DiscussionEditPage(parentId));
            }
            else
            {
                await this.Navigation.PushAsync(new DepartmentPage(list));
            }
        }

        private void setList(List<DepartmentModel> list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                this.List.Add(item);
            }
        }

        public ICommand SelectItemCommand { protected set; get; }

        public async Task selectItemCommand(DepartmentModel model)
        {
            await loadItems(model._id);
        }
    }
}