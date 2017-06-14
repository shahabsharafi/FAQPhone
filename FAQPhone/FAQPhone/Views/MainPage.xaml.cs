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
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var factory = App.Resolve<MainPageViewModelFactory>();
            BindingContext = factory.Create(Navigation);
        }
    }

    public class MainPageViewModelFactory
    {
        IAccountService accountService;
        public MainPageViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public MainPageViewModel Create(INavigation navigation)
        {
            return new MainPageViewModel(this.accountService, navigation);
        }
    }

    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel(IAccountService accountService, INavigation navigation): base (navigation)
        {
            MenuItemModel[] items =
            {
                new MenuItemModel()
                {
                    DisplayName = ResourceManagerHelper.GetValue("create_faq"),
                    CommandName = "create_faq"
                },
                new MenuItemModel()
                {
                    DisplayName = ResourceManagerHelper.GetValue("receive_faq"),
                    CommandName = "receive_faq"
                },
                new MenuItemModel()
                {
                    DisplayName = ResourceManagerHelper.GetValue("inprogress_faq"),
                    CommandName = "inprogress_faq"
                },
                new MenuItemModel()
                {
                    DisplayName = ResourceManagerHelper.GetValue("archived_faq"),
                    CommandName = "archived_faq"
                },
                new MenuItemModel()
                {
                    DisplayName = ResourceManagerHelper.GetValue("signout"),
                    CommandName = "signout"
                }
            };
            this.List = new ObservableCollection<MenuItemModel>();
            this.setList(items);
            this.accountService = accountService;
            this.SelectItemCommand = new Command<MenuItemModel>(async (model) => await selectItemCommand(model));
        }

        private void setList(MenuItemModel[] list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                this.List.Add(item);
            }
        }

        IAccountService accountService { get; set; }

        ObservableCollection<MenuItemModel> _list;
        public ObservableCollection<MenuItemModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public ICommand SelectItemCommand { protected set; get; }

        public async Task selectItemCommand(MenuItemModel model)
        {
            if (model.Children != null && model.Children.Length > 0)
            {
                this.setList(model.Children);
            }
            else if (!string.IsNullOrEmpty(model.CommandName))
            {
                switch (model.CommandName)
                {
                    case "create_faq":
                        await this.RootNavigate(new DepartmentPage());
                        break;
                    case "signout":
                        this.accountService.SignOut();
                        await this.RootNavigate<SigninPage>();
                        break;
                }
            }
        }

    }
}