using FAQPhone.Helpers;
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
        public MainPage(string menu = null)
        {
            InitializeComponent();
            var factory = App.Resolve<MainPageViewModelFactory>();
            BindingContext = factory.Create(Navigation, menu);
        }
    }

    public class MainPageViewModelFactory
    {
        IAccountService accountService;
        IDepartmentService departmentService;
        IDiscussionService discussionService;
        public MainPageViewModelFactory(IAccountService accountService, IDepartmentService departmentService, IDiscussionService discussionService)
        {
            this.accountService = accountService;
            this.departmentService = departmentService;
            this.discussionService = discussionService;
        }
        public MainPageViewModel Create(INavigation navigation, string menu)
        {
            return new MainPageViewModel(this.accountService, this.departmentService, this.discussionService, navigation, menu);
        }
    }

    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel(IAccountService accountService, IDepartmentService departmentService, IDiscussionService discussionService, INavigation navigation, string menu) : base(navigation)
        {
            this.List = new ObservableCollection<MenuItemModel>();
            List<MenuItemModel> items = new List<MenuItemModel>();
            if (menu == "operator_faq" || (menu == null && App.Access.Contains("access_operator")))
            {
                items.AddRange(
                    new MenuItemModel[] {
                        new MenuItemModel() { CommandName = "operator_receive_faq" },
                        new MenuItemModel() { CommandName = "operator_inprogress_faq" },
                        new MenuItemModel() { CommandName = "operator_archived_faq" }
                    }
                );
                if (App.Access.Contains("access_user"))
                {
                    items.Add(new MenuItemModel() { CommandName = "user_faq" });
                }
            }
            else if (menu == "user_faq" || (menu == null && App.Access.Contains("access_user")))
            {
                items.AddRange(
                    new MenuItemModel[] {
                        new MenuItemModel() { CommandName = "user_create_faq" },
                        new MenuItemModel() { CommandName = "user_inprogress_faq" },
                        new MenuItemModel() { CommandName = "user_archived_faq" }
                    }
                );
                if (App.Access.Contains("access_operator"))
                {
                    items.Add(new MenuItemModel() { CommandName = "operator_faq" });
                }
            }
            items.Add(new MenuItemModel() { CommandName = "signout" });
            this.setList(items);
            this.accountService = accountService;
            this.departmentService = departmentService;
            this.discussionService = discussionService;
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
        IDepartmentService departmentService { get; set; }
        IDiscussionService discussionService { get; set; }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        ObservableCollection<MenuItemModel> _list;
        public ObservableCollection<MenuItemModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public ICommand SelectItemCommand { protected set; get; }

        public async Task selectItemCommand(MenuItemModel model)
        {
            if (model == null)
                return;
            if (!string.IsNullOrEmpty(model.CommandName))
            {
                List<DiscussionModel> l;
                switch (model.CommandName)
                {
                    case "user_faq":
                        await this.RootNavigate(new MainPage("user_faq"));
                        break;
                    case "user_create_faq":
                        var dl = await this.departmentService.GetByParent("");
                        if (dl != null && dl.Count() > 0)
                        {
                            await this.Navigation.PushAsync(new DepartmentPage(dl));
                        }
                        break;
                    case "user_inprogress_faq":
                        l = await this.discussionService.GetList(true, new int[] { 0, 1 });
                        if (l != null && l.Count() > 0)
                        {
                            await this.Navigation.PushAsync(new DiscussionPage("user_inprogress_faq", l));
                        }
                        break;
                    case "user_archived_faq":
                        l = await this.discussionService.GetList(true, new int[] { 2 });
                        if (l != null && l.Count() > 0)
                        {
                            await this.Navigation.PushAsync(new DiscussionPage("user_archived_faq", l));
                        }
                        break;
                    case "operator_faq":
                        await this.RootNavigate(new MainPage("operator_faq"));
                        break;
                    case "operator_receive_faq":
                        var d = await this.discussionService.Recive();
                        if (d != null)
                        {
                            await this.Navigation.PushAsync(new DiscussionRecivePage(d));
                        }
                        break;
                    case "operator_inprogress_faq":
                        l = await this.discussionService.GetList(false, new int[] { 0, 1 });
                        if (l != null && l.Count() > 0)
                        {
                            await this.Navigation.PushAsync(new DiscussionPage("operator_inprogress_faq", l));
                        }
                        break;
                    case "operator_archived_faq":
                        l = await this.discussionService.GetList(true, new int[] { 2 });
                        if (l != null && l.Count() > 0)
                        {
                            await this.Navigation.PushAsync(new DiscussionPage("operator_archived_faq", l));
                        }
                        break;
                    case "signout":
                        this.accountService.SignOut();
                        Settings.Username = string.Empty;
                        Settings.Password = string.Empty;
                        await this.RootNavigate(new SendCodePage(FlowType.Signup));
                        break;
                }
                this.SelectedItem = null;
            }            
        }
    }
}