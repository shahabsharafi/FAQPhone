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
            BindingContext = factory.Create(Navigation, model);
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
        public DiscussionReciveViewModel Create(INavigation navigation, DiscussionModel model)
        {
            return new DiscussionReciveViewModel(this.discussionService, this.departmentService, navigation, model);
        }
    }

    public class DiscussionReciveViewModel : BaseViewModel
    {
        public DiscussionReciveViewModel(IDiscussionService discussionService, IDepartmentService departmentService, INavigation navigation, DiscussionModel model) : base(navigation)
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
            set { _title = value; OnPropertyChanged(); }
        }
        string _text;
        public string text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }
        string _replay;
        public string replay
        {
            get { return _replay; }
            set { _replay = value; OnPropertyChanged(); }
        }
        public ICommand AcceptCommand { protected set; get; }

        public async Task acceptCommand()
        {
            /////
            model.state = 1;
            model.to = new AccountModel() { username = App.Username };
            await this.discussionService.Save(model);
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
            /*
            if (!string.IsNullOrWhiteSpace(this.replay))
            {
                var l = this.model.items.ToList();
                l.Add(new DiscussionDetailModel
                {
                    createDate = DateTime.Now,
                    owner = new AccountModel() { username = App.Username },
                    text = this.replay
                });
                model.items = l.ToArray();
                model.state = 1;
                model.to = new AccountModel() { username = App.Username };
                await this.discussionService.Save(model);
                await this.Navigation.PopAsync();
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
            model.state = 9;
            model.to = new AccountModel() { username = App.Username };
            await this.discussionService.Save(model);
            await this.Navigation.PopAsync();
        }
        
    }
}
