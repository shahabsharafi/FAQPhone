using FAQPhone.Inferstructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
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
    public partial class DiscussionEditPage : ContentPage
    {
        public DiscussionEditPage(string departmentId)
        {
            InitializeComponent();
            var factory = App.Resolve<DiscussionEditViewModelFactory>();
            BindingContext = factory.Create(Navigation, departmentId);
        }
    }

    public class DiscussionEditViewModelFactory
    {
        IDiscussionService discussionService;
        public DiscussionEditViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public DiscussionEditViewModel Create(INavigation navigation, string departmentId)
        {
            return new DiscussionEditViewModel(this.discussionService, navigation, departmentId);
        }
    }

    public class DiscussionEditViewModel : BaseViewModel
    {
        public DiscussionEditViewModel(IDiscussionService discussionService, INavigation navigation, string departmentId) : base(navigation)
        {
            this.discussionService = discussionService;            
            this.SaveCommand = new Command(async () => await saveCommand());
            this.departmentId = departmentId;
        }
        private IDiscussionService discussionService { get; set; }
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
        private string departmentId { get; set; }
        public ICommand SaveCommand { protected set; get; }

        public async Task saveCommand()
        {
            /////
            DiscussionModel model = new DiscussionModel()
            {
                title = this.title,
                from = new AccountModel() { username = App.Username },
                createDate = DateTime.Now,
                state = 0,
                department = new DepartmentModel() { _id = this.departmentId },
                items = new DiscussionDetailModel[]
                {
                    new DiscussionDetailModel()
                    {
                        createDate = DateTime.Now,
                        owner = new AccountModel() { username = App.Username },
                        text = this.text
                    }                    
                }
            };
            await this.discussionService.Save(model);
            await this.RootNavigate(new MainPage());
        }
    }
}
