using FAQPhone.Inferstructure;
using FAQPhone.Infrastructure;
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
    public partial class DiscussionNewPage : ContentPage
    {
        public DiscussionNewPage(DepartmentModel department, int pushCount)
        {
            InitializeComponent();
            this.Appearing += (sender, e) => {
                for (var i = 0; i < pushCount; i++)
                {
                    this.Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
            };            
            var factory = App.Resolve<DiscussionEditViewModelFactory>();
            BindingContext = factory.Create(Navigation, department);
        }
    }

    public class DiscussionEditViewModelFactory
    {
        public DiscussionEditViewModelFactory()
        {
            
        }
        public DiscussionEditViewModel Create(INavigation navigation, DepartmentModel department)
        {
            return new DiscussionEditViewModel(navigation, department);
        }
    }

    public class DiscussionEditViewModel : BaseViewModel
    {
        public DiscussionEditViewModel(INavigation navigation, DepartmentModel department) : base(navigation)
        {            
            this.CanNext = false;
            this.NextCommand = new Command(async () => await nextCommand());
            this.department = department;
            this.price = ResourceManagerHelper.GetValue("discussion_recive_price") + ":" + department.price;
        }

        string _title;
        public string title
        {
            get { return _title; }
            set {
                _title = value;
                OnPropertyChanged();
                CanNext = !string.IsNullOrWhiteSpace(_title);
            }
        }

        bool _CanNext;
        public bool CanNext
        {
            get { return _CanNext; }
            set { _CanNext = value; OnPropertyChanged(); }
        }

        string _price;
        public string price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged(); }
        }

        private DepartmentModel department { get; set; }
        public ICommand NextCommand { protected set; get; }

        public async Task nextCommand()
        {
            /////
            DiscussionModel model = new DiscussionModel()
            {
                title = this.title,
                from = new AccountModel() { username = App.Username },
                createDate = DateTime.Now,
                state = Constants.DISCUSSION_STATE_CREATE,
                userRead = false,
                operatorRead = false,
                department = new DepartmentModel() { _id = this.department._id },
                items = new DiscussionDetailModel[] { }
            };
            await this.Navigation.PushAsync(new ChatPage(Constants.USER_INPROGRESS_FAQ, model, 1));
            //await this.Navigation.PushAsync(new ChatPage(Constants.USER_INPROGRESS_FAQ, model));
        }
    }
}
