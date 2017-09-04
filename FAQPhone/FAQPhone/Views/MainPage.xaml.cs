using Awesome;
using FAQPhone.Helpers;
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
    public partial class MainPage : ContentPage
    {
        public MainPage(string menu = null)
        {
            InitializeComponent();
            var factory = App.Resolve<MainPageViewModelFactory>();
            BindingContext = factory.Create(this, menu);
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
        public MainPageViewModel Create(ContentPage page, string menu)
        {
            return new MainPageViewModel(this.accountService, this.departmentService, this.discussionService, page, menu);
        }
    }

    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel(IAccountService accountService, IDepartmentService departmentService, IDiscussionService discussionService, ContentPage page, string menu) : base(page)
        {
            this.accountService = accountService;
            this.departmentService = departmentService;
            this.discussionService = discussionService;
            this.List = new ObservableCollection<MenuItemModel>();
            List<MenuItemModel> items = new List<MenuItemModel>();
            if (menu == Constants.OPERATOR_FAQ || (menu == null && App.Access.Contains(Constants.ACCESS_OPERATOR)))
            {
                int count = 0;
                Task.Run(async () => count = await this.discussionService.GetCount(false, new int[] { 0, 1, 2, 3 })).Wait();
                items.AddRange(
                    new MenuItemModel[] {
                        new MenuItemModel() { CommandName = Constants.OPERATOR_RECEIVE_FAQ, Icon = FontAwesome.FADownload },
                        new MenuItemModel() { CommandName = Constants.OPERATOR_INPROGRESS_FAQ, Icon = FontAwesome.FATasks, Badge = count.ToString() }
                    }
                );
                if (App.Access.Contains(Constants.ACCESS_USER))
                {
                    items.Add(new MenuItemModel() { CommandName = Constants.USER_FAQ, Icon = FontAwesome.FAUser });
                }
                items.Add(new MenuItemModel() { CommandName = Constants.ACCOUNT, Icon = FontAwesome.FAAddressCardO });
                items.Add(new MenuItemModel() { CommandName = Constants.ABOUT_US, Icon = FontAwesome.FAInfoCircle });
                items.Add(new MenuItemModel() { CommandName = Constants.SETTING, Icon = FontAwesome.FACog });
            }
            else if (menu == Constants.USER_FAQ || (menu == null && App.Access.Contains(Constants.ACCESS_USER)))
            {
                int count = 0;
                Task.Run(async () => count = await this.discussionService.GetCount(true, new int[] { 0, 1, 2, 3 })).Wait();
                items.AddRange(
                    new MenuItemModel[] {
                        new MenuItemModel() { CommandName = Constants.USER_CREATE_FAQ, Icon = FontAwesome.FAPlus },
                        new MenuItemModel() { CommandName = Constants.USER_INPROGRESS_FAQ, Icon = FontAwesome.FATasks, Badge = count.ToString() }
                    }
                );
                if (App.Access.Contains(Constants.ACCESS_OPERATOR))
                {
                    items.Add(new MenuItemModel() { CommandName = Constants.OPERATOR_FAQ, Icon = FontAwesome.FAUserCircle });
                }
                items.Add(new MenuItemModel() { CommandName = Constants.ACCOUNT, Icon = FontAwesome.FAAddressCardO });
                items.Add(new MenuItemModel() { CommandName = Constants.ABOUT_US, Icon = FontAwesome.FAInfoCircle });
                items.Add(new MenuItemModel() { CommandName = Constants.SETTING, Icon = FontAwesome.FACog });
            }
            else if (menu == Constants.SETTING)
            {
                items.Add(new MenuItemModel() { CommandName = Constants.SIGNOUT, Icon = FontAwesome.FASignOut });
            }
                     
            this.setList(items);            
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
                        else
                        {
                            await this.DisplayAlert("message_title_alert", "message_text_not_exists", "command_ok");
                        }
                        break;
                    case Constants.USER_INPROGRESS_FAQ:
                        l = await this.discussionService.GetList(true, new int[] { 0, 1, 2, 3 });
                        if (l != null && l.Count() > 0)
                        {
                            await this.Navigation.PushAsync(new DiscussionPage(model.CommandName, l));
                        }
                        else
                        {
                            await this.DisplayAlert("message_title_alert", "message_text_not_exists", "command_ok");
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
                        else
                        {
                            await this.DisplayAlert("message_title_alert", "message_text_not_exists", "command_ok");
                        }
                        break;
                    case Constants.OPERATOR_INPROGRESS_FAQ:
                        l = await this.discussionService.GetList(false, new int[] { 0, 1, 2, 3 });
                        if (l != null && l.Count() > 0)
                        {
                            await this.Navigation.PushAsync(new DiscussionPage(model.CommandName, l));
                        }
                        else
                        {
                            await this.DisplayAlert("message_title_alert", "message_text_not_exists", "command_ok");
                        }
                        break;
                    case Constants.SETTING:
                        await this.Navigation.PushAsync(new MainPage(model.CommandName));
                        break;
                    case Constants.SIGNOUT:
                        var flag = await this.DisplayAlert("message_title_alert", "message_text_are_you_sure", "command_yes", "command_no");
                        if (flag)
                        {
                            this.accountService.SignOut();
                            Settings.Username = string.Empty;
                            Settings.Password = string.Empty;
                            await this.RootNavigate(new SendCodePage(FlowType.Signup));
                        }
                        break;
                    case Constants.ACCOUNT:
                        var account = await this.accountService.GetMe();
                        await this.Navigation.PushAsync(new AccountPage(account));
                        break;
                    case Constants.ABOUT_US:
                        await this.Navigation.PushAsync(new AboutPage());
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