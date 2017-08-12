using FAQPhone.Inferstructure;
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
        public DiscussionReciveViewModelFactory(IDiscussionService discussionService, IDepartmentService departmentService)
        {
            this.discussionService = discussionService;
            this.departmentService = departmentService;
        }
        public DiscussionReciveViewModel Create(ContentPage page, DiscussionModel model)
        {
            return new DiscussionReciveViewModel(this.discussionService, this.departmentService, page, model);
        }
    }

    public class DiscussionReciveViewModel : BaseViewModel
    {
        public DiscussionReciveViewModel(IDiscussionService discussionService, IDepartmentService departmentService, ContentPage page, DiscussionModel model) : base(page)
        {
            this.discussionService = discussionService;
            this.departmentService = departmentService;
            this.AcceptCommand = new Command(async () => await acceptCommand());
            this.RejectCommand = new Command(async () => await rejectCommand());
            this.ReportCommand = new Command(async () => await reportCommand());
            this.model = model;
            this.title = this.model.title;
            if (this.model.items.Length > 0)
            {
                this.text = this.model.items[0].text;
            }
        }
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
            /////
            if (!string.IsNullOrWhiteSpace(this.display))
            {
                model.state = Constants.DISCUSSION_STATE_RECIVED;
                model.display = this.display;
                model.to = new AccountModel() { username = App.Username };
                await this.discussionService.Save(model);
                await Navigation.PushAsync(new TagPage(model));
            }
            /*
            if (model.department != null && model.department.tags != null && model.department.tags.Length > 0)
            {
                var items = new List<CheckItem>();
                foreach (var item in model.department.tags)
                {
                    items.Add(new CheckItem { Name = item });
                }

                var multiPage = new SelectMultipleBasePage<CheckItem>(items) { Title = ResourceManagerHelper.GetValue("discussion_tags") };
                multiPage.Select += (sender, e) =>
                {
                    var results = multiPage.GetSelection();
                    if (results != null && results.Count > 0)
                    {
                        model.tags = results.Select(o => o.Name).ToArray();
                    }
                    else
                    {
                        model.tags = new string[] { };                        
                    }
                    this.Navigation.PopAsync();
                    this.Navigation.PopAsync();
                    this.Navigation.PushAsync(new ChatPage(model));
                };
                await Navigation.PushAsync(multiPage);
            }
            else
            {
                await Navigation.PushAsync(new ChatPage(model));
            }
            */
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
        
    }
}
