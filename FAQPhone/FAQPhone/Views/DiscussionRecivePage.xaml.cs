using FAQPhone.Inferstructure;
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
    public partial class DiscussionRecivePage : ContentPage
    {
        public DiscussionRecivePage()
        {
            InitializeComponent();
            var factory = App.Resolve<DiscussionReciveViewModelFactory>();
            BindingContext = factory.Create(Navigation);
        }
    }

    public class DiscussionReciveViewModelFactory
    {
        IDiscussionService discussionService;
        public DiscussionReciveViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public DiscussionReciveViewModel Create(INavigation navigation)
        {
            return new DiscussionReciveViewModel(this.discussionService, navigation);
        }
    }

    public class DiscussionReciveViewModel : BaseViewModel
    {
        public DiscussionReciveViewModel(IDiscussionService discussionService, INavigation navigation) : base(navigation)
        {
            this.discussionService = discussionService;
            this.AcceptCommand = new Command(async () => await acceptCommand());
            this.RejectCommand = new Command(async () => await rejectCommand());
            this.ReportCommand = new Command(async () => await reportCommand());
            this.FinishCommand = new Command(async () => await finishCommand());
            Task.Run(async () => await loadItems());
        }
        private IDiscussionService discussionService { get; set; }
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

        public ICommand FinishCommand { protected set; get; }

        public async Task finishCommand()
        {
            /////     
            model.state = 2;
            await this.discussionService.Save(model);
            await this.Navigation.PopAsync();
        }
        public async Task loadItems()
        {
            this.model = await this.discussionService.Recive();
            this.title = this.model.title;
            if (this.model.items.Length > 0)
            {
                this.text = this.model.items[0].text;                
            }

        }
    }
}
