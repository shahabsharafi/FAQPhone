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
            this.SaveCommand = new Command(async () => await saveCommand());
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
        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            /////
            var l = this.model.items.ToList();
            l.Add(new DiscussionDetailModel
            {
                createDate = DateTime.Now,
                owner = new AccountModel() { username = App.Bag.username },
                text = this.replay
            });
            model.items = l.ToArray();
            model.state = 1;
            await this.discussionService.Save(model);
            await this.RootNavigate(new MainPage());
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
