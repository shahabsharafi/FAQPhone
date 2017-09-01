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
    public partial class DepartmentPage : ContentPage
    {
        public DepartmentPage(List<DepartmentModel> list = null, int pushCount = 1)
        {
            InitializeComponent();
            var factory = App.Resolve<DepartmentPageViewModelFactory>();
            BindingContext = factory.Create(this, list, pushCount);
        }
    }

    public class DepartmentPageViewModelFactory
    {
        IDepartmentService departmentService;
        public DepartmentPageViewModelFactory(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }
        public DepartmentPageViewModel Create(ContentPage page, List<DepartmentModel> list, int pushCount)
        {
            return new DepartmentPageViewModel(this.departmentService, page, list, pushCount);
        }
    }

    public class DepartmentPageViewModel : BaseViewModel
    {

        public DepartmentPageViewModel(IDepartmentService departmentService, ContentPage page, List<DepartmentModel> list, int pushCount) : base(page)
        {
            this._pushCount = pushCount;
            this.departmentService = departmentService;
            this.SelectItemCommand = new Command<DepartmentModel>(async(model) => await selectItemCommand(model));
            this.List = new ObservableCollection<DepartmentModel>();
            if (list == null)
            {
                Task.Run(async () => await loadItems(null));
            }
            else
            {
                this.setList(list);
            }
        }
        private IDepartmentService departmentService { get; set; }

        int _pushCount;

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        ObservableCollection<DepartmentModel> _list;
        public ObservableCollection<DepartmentModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public async Task loadItems(DepartmentModel model)
        {
            var list = await this.departmentService.GetByParent(model._id);
            if (model == null)
            {
                this.setList(list);
            }
            else if (list == null || list.Count == 0)
            {
                await this.Navigation.PushAsync(new DiscussionNewPage(model, this._pushCount));                
            }
            else
            {
                await this.Navigation.PushAsync(new DepartmentPage(list, (this._pushCount + 1)));                
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
            if (model == null)
                return;
            await loadItems(model);
            this.SelectedItem = null;
        }
    }
}