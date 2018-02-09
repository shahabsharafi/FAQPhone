using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using Multiselect;
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
    public partial class DiscussionRecivePage : ContentPage
    {
        public DiscussionRecivePage(DiscussionModel model)
        {
            InitializeComponent();
            var factory = App.Resolve<DiscussionReciveViewModelFactory>();
            BindingContext = factory.Create(this, model);
        }
    }

    public class DiscussionReciveViewModelFactory
    {
        IDiscussionService discussionService;
        IDepartmentService departmentService;
        IAccountService accountService;
        public DiscussionReciveViewModelFactory(IAccountService accountService, IDiscussionService discussionService, IDepartmentService departmentService)
        {
            this.accountService = accountService;
            this.discussionService = discussionService;
            this.departmentService = departmentService;
        }
        public DiscussionReciveViewModel Create(ContentPage page, DiscussionModel model)
        {
            return new DiscussionReciveViewModel(this.accountService, this.discussionService, this.departmentService, page, model);
        }
    }

    public class DiscussionReciveViewModel : BaseViewModel
    {
        public DiscussionReciveViewModel(IAccountService accountService, IDiscussionService discussionService, IDepartmentService departmentService, ContentPage page, DiscussionModel model) : base(page)
        {
            this.accountService = accountService;
            this.discussionService = discussionService;
            this.departmentService = departmentService;
            this.AcceptCommand = new Command(async () => await acceptCommand());
            this.RejectCommand = new Command(async () => await rejectCommand());
            this.ReportCommand = new Command(async () => await reportCommand());
            this.ProfileCommand = new Command(async () => await profileCommand());
            this.RuleCommand = new Command(async () => await ruleCommand());
            this.model = model;
            this.title = this.model.title;
            if (this.model.items.Length > 0)
            {
                this.text = this.model.items[0].text;
            }
        }
        private IAccountService accountService { get; set; }
        private IDiscussionService discussionService { get; set; }
        private IDepartmentService departmentService { get; set; }
        DiscussionModel model { get; set; } 
        string _title;
        public string title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length <= 30)
                {
                    _title = value;
                }
                OnPropertyChanged();
            }
        }

        string _text;
        public string text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }
        string _display;
        public string display
        {
            get { return _display; }
            set {
                _display = value;
                OnPropertyChanged();
                CanAccepting = !string.IsNullOrWhiteSpace(_display);
            }
        }

        bool _CanAccepting;
        public bool CanAccepting
        {
            get { return _CanAccepting; }
            set { _CanAccepting = value; OnPropertyChanged(); }
        }

        public ICommand AcceptCommand { protected set; get; }

        public async Task acceptCommand()
        {
            if (!string.IsNullOrWhiteSpace(this.display))
            {
                model.state = Constants.DISCUSSION_STATE_RECIVED;
                model.display = this.display;
                model.to = new AccountModel() { username = App.Username };
                //await this.discussionService.Save(model);
                await Navigation.PushAsync(new TagPage(model));
            }
        }

        private void MultiPage_Select(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public ICommand RejectCommand { protected set; get; }

        public async Task rejectCommand()
        {
            /////       
            await this.Navigation.PopAsync();
        }

        public ICommand ReportCommand { protected set; get; }

        public async Task reportCommand()
        {
            /////     
            model.state = Constants.DISCUSSION_STATE_REPORT;
            model.to = new AccountModel() { username = App.Username };
            await this.discussionService.Save(model);
            await this.Navigation.PopAsync();
        }

        public ICommand RuleCommand { protected set; get; }

        public async Task ruleCommand()
        {
            if (this.model.department != null)
            {
                var department = await this.departmentService.Get(model.department._id);
                var title = ResourceManagerHelper.GetValue(Constants.RULES);
                var text = department.operatorRule;
                await this.Navigation.PushAsync(new TextPage(title, text));
            }
        }

        public ICommand ProfileCommand { protected set; get; }
        public async Task profileCommand()
        {
            string username = model.from.username;
            var role = Constants.ACCESS_OPERATOR;
            var user = await this.accountService.GetByUsername(username);
            if (user != null)
            {
                await this.Navigation.PushAsync(new ProfileInfoPage(user, role, false));
            }
        }

    }
}
