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
    public partial class ReportBalancePage : ContentPage
    {
        public ReportBalancePage()
        {
            InitializeComponent();
            var factory = App.Resolve<ReportBalanceViewModelFactory>();
            var vm = factory.Create(this);
            this.Appearing += (sender, e) => {
                Task.Run(() => vm.Load()).Wait();
            };
            BindingContext = vm;
        }
    }

    public class ReportBalanceViewModelFactory
    {
        IReportService reportService;
        public ReportBalanceViewModelFactory(IReportService reportService)
        {
            this.reportService = reportService;
        }
        public ReportBalanceViewModel Create(ContentPage page)
        {
            return new ReportBalanceViewModel(this.reportService, page);
        }
    }

    public class ReportBalanceViewModel : BaseViewModel
    {
        public ReportBalanceViewModel(IReportService reportService, ContentPage page) : base(page)
        {
            this.reportService = reportService;
            this.SelectItemCommand = new Command<BalanceModel>((m) => selectItemCommand(m));
            this.model = model;
            this.List = new ObservableCollection<BalanceModel>();
        }
        private IReportService reportService { get; set; }
        private AccountModel model { get; set; }

        public ICommand SelectItemCommand { protected set; get; }

        public void selectItemCommand(BalanceModel model)
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

        string _replay;
        public string replay
        {
            get { return _replay; }
            set
            {
                _replay = value;
                OnPropertyChanged();
            }
        }

        public async void Load()
        {
            var l = await this.reportService.GetBalance();
            if (l != null && l.Count() > 0)
            {
                this.setList(l);
            }
        }

        ObservableCollection<BalanceModel> _list;
        public ObservableCollection<BalanceModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        private void setList(List<BalanceModel> list)
        {
            var l = list.OrderByDescending(o => o.date);
            this.List.Clear();
            foreach (var item in l)
            {
                var amount = (item.type == "credit") ? item.credit : item.debit;
                item.Amount = string.Format("{0:n0}", amount);
                item.CreateDateCaption = Utility.MiladiToShamsiString(item.date);
                item.IsCredit = item.type == "credit";
                item.IsDebit = item.type == "debit";
                item.SourceCaption = ResourceManagerHelper.GetValue(item.source);
                this.List.Add(item);                
            }
        }
    }
}