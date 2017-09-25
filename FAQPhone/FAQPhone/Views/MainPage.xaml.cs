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
            var vm = factory.Create(this, menu);
            this.Appearing += (sender, e) => {
                vm.ClearCompleteProfile();
            };
            BindingContext = vm;
        }
    }

    public class MainPageViewModelFactory
    {
        IAccountService accountService;
        IDepartmentService departmentService;
        IDiscussionService discussionService;
        IAttributeService attributeService;
        public MainPageViewModelFactory(IAccountService accountService, IAttributeService attributeService, IDepartmentService departmentService, IDiscussionService discussionService)
        {
            this.accountService = accountService;
            this.departmentService = departmentService;
            this.discussionService = discussionService;
            this.attributeService = attributeService;
        }
        public MainPageViewModel Create(ContentPage page, string menu)
        {
            return new MainPageViewModel(this.accountService, this.attributeService, this.departmentService, this.discussionService, page, menu);
        }
    }

    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel(IAccountService accountService, IAttributeService attributeService, IDepartmentService departmentService, IDiscussionService discussionService, ContentPage page, string menu) : base(page)
        {
            this.accountService = accountService;
            this.attributeService = attributeService;
            this.departmentService = departmentService;
            this.discussionService = discussionService;
            Task.Run(async () => await loadAttribute());
            this.List = new ObservableCollection<MenuItemModel>();
            List<MenuItemModel> items = new List<MenuItemModel>();
            
            if (menu == Constants.OPERATOR_FAQ || (menu == null && App.Access.Contains(Constants.ACCESS_OPERATOR)))
            {
                int count = 0;
                Task.Run(async () => count = await this.discussionService.GetCount(false)).Wait();
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
                items.Add(new MenuItemModel() { CommandName = Constants.ACCOUNT, Icon = FontAwesome.FAAddressCardO, Parms = new string[] { Constants.ACCESS_OPERATOR } });
                items.Add(new MenuItemModel() { CommandName = Constants.ABOUT_US, Icon = FontAwesome.FAInfoCircle });
                items.Add(new MenuItemModel() { CommandName = Constants.SETTING, Icon = FontAwesome.FACog });
            }
            else if (menu == Constants.USER_FAQ || (menu == null && App.Access.Contains(Constants.ACCESS_USER)))
            {
                int count = 0;
                Task.Run(async () => count = await this.discussionService.GetCount(true)).Wait();
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
                items.Add(new MenuItemModel() { CommandName = Constants.ACCOUNT, Icon = FontAwesome.FAAddressCardO, Parms = new string[] { Constants.ACCESS_USER } });
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

        bool? _completeProfile = null;
        async Task<bool> CompleteProfile()
        {
            if (!this._completeProfile.HasValue)
            {
                AccountModel me = await this.accountService.GetMe();
                this._completeProfile = (!string.IsNullOrWhiteSpace(me?.profile?.firstName)) || (!string.IsNullOrWhiteSpace(me?.email));
            }
            return this._completeProfile.Value;
        }
        public void ClearCompleteProfile()
        {
            this._completeProfile = null;
        }

        private async Task loadAttribute()
        {
            App.AttributeList = await this.attributeService.GetAll();
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
        IAttributeService attributeService { get; set; }

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
                
                switch (model.CommandName)
                {
                    case Constants.USER_FAQ:
                        await this.RootNavigate(new MainPage(model.CommandName));
                        break;
                    case Constants.USER_CREATE_FAQ:
                        await CreateFAQByUser();
                        break;
                    case Constants.USER_INPROGRESS_FAQ:
                        await ReadInprogressFAQByUser(model);
                        break;
                    case Constants.OPERATOR_FAQ:
                        await this.RootNavigate(new MainPage(model.CommandName));
                        break;
                    case Constants.OPERATOR_RECEIVE_FAQ:
                        await ReciveFAQByOperator();
                        break;
                    case Constants.OPERATOR_INPROGRESS_FAQ:
                        await ReadInprogressFAQByOperator(model);
                        break;
                    case Constants.SETTING:
                        await this.Navigation.PushAsync(new MainPage(model.CommandName));
                        break;
                    case Constants.SIGNOUT:
                        await Signout();
                        break;
                    case Constants.ACCOUNT:
                        await ViewAndChangeAccountProfile(model);
                        break;
                    case Constants.ABOUT_US:
                        await this.Navigation.PushAsync(new AboutPage());
                        break;
                }
                this.SelectedItem = null;
            }            
        }

        private async Task ViewAndChangeAccountProfile(MenuItemModel model)
        {
            var parm = model.Parms[0];
            var account = await this.accountService.GetMe();
            await this.Navigation.PushAsync(new AccountPage(account, parm));
        }

        private async Task Signout()
        {
            var flag = await Utility.Confirm();
            if (flag)
            {
                this.accountService.SignOut();
                Settings.Username = string.Empty;
                Settings.Password = string.Empty;
                await this.RootNavigate(new SendCodePage());
            }
        }

        private async Task ReadInprogressFAQByOperator(MenuItemModel model)
        {
            if (await this.CompleteProfile())
            {
                List<DiscussionModel> l = await this.discussionService.GetList(false);
                if (l != null && l.Count() > 0)
                {
                    await this.Navigation.PushAsync(new DiscussionPage(model.CommandName, l));
                }
                else
                {
                    await Utility.Alert("message_not_exists");
                }
            }
            else
            {
                await Utility.Alert("message_profile_not_completed");
            }
        }

        private async Task ReciveFAQByOperator()
        {
            if (await this.CompleteProfile())
            {
                var d = await this.discussionService.Recive();
                if (d != null)
                {
                    await this.Navigation.PushAsync(new DiscussionRecivePage(d));
                }
                else
                {
                    await Utility.Alert("message_not_exists");
                }
            }
            else
            {
                await Utility.Alert("message_profile_not_completed");
            }
        }

        private async Task ReadInprogressFAQByUser(MenuItemModel model)
        {
            if (await this.CompleteProfile())
            {
                List<DiscussionModel> l = await this.discussionService.GetList(true);
                if (l != null && l.Count() > 0)
                {
                    await this.Navigation.PushAsync(new DiscussionPage(model.CommandName, l));
                }
                else
                {
                    await Utility.Alert("message_not_exists");
                }
            }
            else
            {
                await Utility.Alert("message_profile_not_completed");
            }
        }

        private async Task CreateFAQByUser()
        {
            if (await this.CompleteProfile())
            {
                var dl = await this.departmentService.GetByParent("");
                if (dl != null && dl.Count() > 0)
                {
                    await this.Navigation.PushAsync(new DepartmentPage(dl));
                }
                else
                {
                    await Utility.Alert("message_not_exists");
                }
            }
            else
            {
                await Utility.Alert("message_profile_not_completed");
            }
        }
    }
}