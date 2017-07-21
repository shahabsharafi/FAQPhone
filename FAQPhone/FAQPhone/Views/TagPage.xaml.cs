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
    public partial class TagPage : ContentPage
    {
        public TagPage(DiscussionModel model)
        {
            InitializeComponent();
            var factory = App.Resolve<TagViewModelFactory>();
            BindingContext = factory.Create(Navigation, model);
        }
    }

    public class TagViewModelFactory
    {
        IDiscussionService discussionService;
        public TagViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public TagViewModel Create(INavigation navigation, DiscussionModel model)
        {
            return new TagViewModel(this.discussionService, navigation, model);
        }
    }

    public class TagViewModel : BaseViewModel
    {

        public TagViewModel(IDiscussionService discussionService, INavigation navigation, DiscussionModel model) : base(navigation)
        {
            this.discussionService = discussionService;
            this.AddCommand = new Command(() => addCommand());
            this.SaveCommand = new Command(async () => await saveCommand());
            this.model = model;
            this.List = new ObservableCollection<TagItemModel>();
        }
        private IDiscussionService discussionService { get; set; }
        private DiscussionModel model { get; set; }
        string _tag;
        public string tag
        {
            get { return _tag; }
            set { _tag = value; OnPropertyChanged(); }
        }

        ObservableCollection<TagItemModel> _list;
        public ObservableCollection<TagItemModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { protected set; get; }

        public void addCommand()
        {
            this.List.Add(new TagItemModel()
            {
                text = this.tag
            });
        }

        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            if (this.List.Count > 2)
            {
                this.model.tags = this.List.Select(o => o.text).ToArray();
                await this.discussionService.Save(model);
                await this.RootNavigate(new ChatPage(Constants.OPERATOR_INPROGRESS_FAQ, model));
            }
        }
    }
}