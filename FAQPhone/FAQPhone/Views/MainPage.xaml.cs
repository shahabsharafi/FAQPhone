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
            if (menu == Constants.OPERATOR_FAQ || (menu == null && App.Access.Contains(Constants.ACCESS_OPERATOR)))
            {
                items.AddRange(
                    new MenuItemModel[] {
                        new MenuItemModel() { CommandName = Constants.OPERATOR_RECEIVE_FAQ },
                        new MenuItemModel() { CommandName = Constants.OPERATOR_INPROGRESS_FAQ }
                    }
                );
                if (App.Access.Contains(Constants.ACCESS_USER))
                {
                    items.Add(new MenuItemModel() { CommandName = Constants.USER_FAQ });
                }
            }
            else if (menu == Constants.USER_FAQ || (menu == null && App.Access.Contains(Constants.ACCESS_USER)))
            {
                items.AddRange(
                    new MenuItemModel[] {
                        new MenuItemModel() { CommandName = Constants.USER_CREATE_FAQ },
                        new MenuItemModel() { CommandName = Constants.USER_INPROGRESS_FAQ }
                    }
                );
                if (App.Access.Contains(Constants.ACCESS_OPERATOR))
                {
                    items.Add(new MenuItemModel() { CommandName = Constants.OPERATOR_FAQ });
                }
            }
            items.Add(new MenuItemModel() { CommandName = Constants.ACCOUNT });
            items.Add(new MenuItemModel() { CommandName = Constants.SIGNOUT });
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
                    case Constants.USER_FAQ:
                        await this.RootNavigate(new MainPage(model.CommandName));
                        break;
                    case Constants.USER_CREATE_FAQ:
                        var dl = await this.departmentService.GetByParent("");
                        if (dl != null && dl.Count() > 0)
                        {
                            await this.Navigation.PushAsync(new DepartmentPage(dl));
                        }
                        break;
                    case Constants.USER_INPROGRESS_FAQ:
                        l = await this.discussionService.GetList(true, new int[] { 0, 1, 2 });
                        if (l != null && l.Count() > 0)
                        {
                            await this.Navigation.PushAsync(new DiscussionPage(model.CommandName, l));
                        }
                        break;
                    case Constants.OPERATOR_FAQ:
                        await this.RootNavigate(new MainPage(model.CommandName));
                        break;
                    case Constants.OPERATOR_RECEIVE_FAQ:
                        var d = await this.discussionService.Recive();
                        if (d != null)
                        {
                            await this.Navigation.PushAsync(new DiscussionRecivePage(d));
                        }
                        break;
                    case Constants.OPERATOR_INPROGRESS_FAQ:
                        l = await this.discussionService.GetList(false, new int[] { 0, 1, 2 });
                        if (l != null && l.Count() > 0)
                        {
                            await this.Navigation.PushAsync(new DiscussionPage(model.CommandName, l));
                        }
                        break;
                    case Constants.SIGNOUT:
                        this.accountService.SignOut();
                        Settings.Username = string.Empty;
                        Settings.Password = string.Empty;
                        await this.RootNavigate(new SendCodePage(FlowType.Signup));
                        break;
                    case Constants.ACCOUNT:
                        var account = await this.accountService.GetMe();
                        await this.Navigation.PushAsync(new ProfilePage(account));
                        break;
                    //case "FILE_PICKER":
                    //    FilePickerHelper.Open();
                    //    break;
                }
                this.SelectedItem = null;
            }            
        }
    }
}