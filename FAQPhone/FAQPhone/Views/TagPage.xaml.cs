using FAQPhone.Infarstructure;
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
            BindingContext = factory.Create(this, model);
        }
    }

    public class TagViewModelFactory
    {
        IDiscussionService discussionService;
        public TagViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public TagViewModel Create(ContentPage page, DiscussionModel model)
        {
            return new TagViewModel(this.discussionService, page, model);
        }
    }

    public class TagViewModel : BaseViewModel
    {

        public TagViewModel(IDiscussionService discussionService, ContentPage page, DiscussionModel model) : base(page)
        {
            this.discussionService = discussionService;
            this.AddCommand = new Command(() => addCommand());
            this.SaveCommand = new Command(async () => await saveCommand());
            this.model = model;
            this.List = new ObservableCollection<TagItemModel>();
            this.CanSaving = false;
            this.CanAdding = false;
        }
        private IDiscussionService discussionService { get; set; }
        private DiscussionModel model { get; set; }
        string _tag;
        public string tag
        {
            get { return _tag; }
            set
            {
                _tag = value;
                OnPropertyChanged();
                CanAdding = !string.IsNullOrWhiteSpace(_tag);
            }
        }

        ObservableCollection<TagItemModel> _list;
        public ObservableCollection<TagItemModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        bool _CanAdding;
        public bool CanAdding
        {
            get { return _CanAdding; }
            set { _CanAdding = value; OnPropertyChanged(); }
        }

        bool _CanSaving;
        public bool CanSaving
        {
            get { return _CanSaving; }
            set { _CanSaving = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { protected set; get; }

        public void addCommand()
        {
            if (!string.IsNullOrWhiteSpace(this.tag))
            {
                this.List.Add(new TagItemModel()
                {
                    text = this.tag
                });
                this.tag = string.Empty;
                if (this.List.Count > 2)
                {
                    this.CanSaving = true;
                }
            }
        }

        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            if (this.List.Count > 2)
            {
                this.model.tags = this.List.Select(o => o.text).ToArray();
                //await this.discussionService.Save(model);
                await this.Navigation.PushAsync(new ChatPage(Constants.OPERATOR_INPROGRESS_FAQ, model, 2));
            }
        }
    }
}