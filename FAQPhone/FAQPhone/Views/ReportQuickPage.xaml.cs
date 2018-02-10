using FAQPhone.Helpers;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportQuickPage : ContentPage
    {
        public ReportQuickPage()
        {
            InitializeComponent();
            var factory = App.Resolve<ReportQuickViewModelFactory>();
            var vm = factory.Create(this);
            this.Appearing += (sender, e) => {
                Task.Run(() => vm.Load()).Wait();
            };
            BindingContext = vm;
        }
    }

    public class ReportQuickViewModelFactory
    {
        IReportService reportService;
        public ReportQuickViewModelFactory(IReportService reportService)
        {
            this.reportService = reportService;
        }
        public ReportQuickViewModel Create(ContentPage page)
        {
            return new ReportQuickViewModel(this.reportService, page);
        }
    }

    public class ReportQuickViewModel : BaseViewModel
    {
        public ReportQuickViewModel(IReportService reportService, ContentPage page) : base(page)
        {
            this.reportService = reportService;
            this.SelectItemCommand = new Command<KeyValueModel>((m) => selectItemCommand(m));
            this.model = model;
            this.List = new ObservableCollection<KeyValueModel>();
        }
        private IReportService reportService { get; set; }
        private AccountModel model { get; set; }

        public ICommand SelectItemCommand { protected set; get; }

        public void selectItemCommand(KeyValueModel model)
        {
            if (model == null)
                return;
            this.SelectedItem = null;
        }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        public async void Load()
        {
            var l = await this.reportService.GetQuick();
            if (l != null && l.Count() > 0)
            {
                this.setList(l);
            }
        }

        ObservableCollection<KeyValueModel> _list;
        public ObservableCollection<KeyValueModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        private void setList(List<KeyValueModel> list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                this.List.Add(new KeyValueModel {
                    key = ResourceManagerHelper.GetValue(item.key),
                    value = item.value
            });
            }
        }
    }
}