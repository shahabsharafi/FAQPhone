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
        public MainPage(List<MenuItemModel> list = null)
        {
            InitializeComponent();
            var factory = App.Resolve<MainPageViewModelFactory>();
            BindingContext = factory.Create(Navigation, list);
        }
    }

    public class MainPageViewModelFactory
    {
        IAccountService accountService;
        public MainPageViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public MainPageViewModel Create(INavigation navigation, List<MenuItemModel> list)
        {
            return new MainPageViewModel(this.accountService, navigation, list);
        }
    }

    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel(IAccountService accountService, INavigation navigation, List<MenuItemModel> list) : base(navigation)
        {
            this.List = new ObservableCollection<MenuItemModel>();
            if (list == null)
            {
                List<MenuItemModel> items = new List<MenuItemModel>();
                if (App.Bag.access.Contains("access_user"))
                {
                    items.Add(new MenuItemModel()
                    {
                        CommandName = "user_faq",
                        Children = new List<MenuItemModel>
                        {
                            new MenuItemModel()
                            {
                                CommandName = "user_create_faq",
                            },
                            new MenuItemModel()
                            {
                                CommandName = "user_inprogress_faq"
                            },
                            new MenuItemModel()
                            {
                                CommandName = "user_archived_faq"
                            }
                        }
                    });
                }
                if (App.Bag.access.Contains("access_operator"))
                {
                    items.Add(new MenuItemModel()
                    {
                        CommandName = "operator_faq",
                        Children = new List<MenuItemModel>
                        {
                            new MenuItemModel()
                            {
                                CommandName = "operator_receive_faq",
                            },
                            new MenuItemModel()
                            {
                                CommandName = "operator_inprogress_faq"
                            },
                            new MenuItemModel()
                            {
                                CommandName = "operator_archived_faq"
                            }
                        }
                    });
                }
                items.Add(new MenuItemModel()
                {
                    CommandName = "signout"
                });

                this.setList(items);
            }
            else
            {
                this.setList(list);
            }
            this.accountService = accountService;
            this.SelectItemCommand = new Command<MenuItemModel>(async (model) => await selectItemCommand(model));
        }

        private void setList(List<MenuItemModel> list)
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
            if (model.Children != null && model.Children.Count > 0)
            {
                await this.Navigation.PushAsync(new MainPage(model.Children));
            }
            else if (!string.IsNullOrEmpty(model.CommandName))
            {
                switch (model.CommandName)
                {
                    case "user_create_faq":
                        await this.Navigation.PushAsync(new DepartmentPage());
                        break;
                    case "user_inprogress_faq":                        
                        await this.Navigation.PushAsync(new DiscussionPage(true, 0));
                        break;
                    case "user_archived_faq":
                        await this.Navigation.PushAsync(new DiscussionPage(true, 1));
                        break;
                    case "operator_receive_faq":
                        break;
                    case "operator_inprogress_faq":
                        await this.Navigation.PushAsync(new DiscussionPage(false, 0));
                        break;
                    case "operator_archived_faq":
                        await this.Navigation.PushAsync(new DiscussionPage(false, 1));
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