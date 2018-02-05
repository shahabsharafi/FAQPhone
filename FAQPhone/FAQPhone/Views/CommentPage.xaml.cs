using FAQPhone.Infarstructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CommentPage : ContentPage
	{
		public CommentPage (AccountModel model)
		{
			InitializeComponent ();
            EventHandler h = null;
            var factory = App.Resolve<CommentViewModelFactory>();
            BindingContext = factory.Create(this, model);
        }
    }

    public class CommentViewModelFactory
    {
        IAccountService accountService;
        public CommentViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public CommentViewModel Create(ContentPage page, AccountModel model)
        {
            return new CommentViewModel(this.accountService, page, model);
        }
    }

    public class CommentViewModel : BaseViewModel
    {
        public CommentViewModel(IAccountService accountService, ContentPage page, AccountModel model) : base(page)
        {
            this.accountService = accountService;
            this.SendCommand = new Command(async () => await sendCommand());
            this.model = model;
            this.List = new ObservableCollection<AccountCommentModel>();
            //this.setList(this.model.comments.ToList());
        }
        private IAccountService accountService { get; set; }
        private AccountModel model { get; set; }        

        string _replay;
        public string replay
        {
            get { return _replay; }
            set
            {
                _replay = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<AccountCommentModel> _list;
        public ObservableCollection<AccountCommentModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        private void setList(List<AccountCommentModel> list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                this.List.Add(item);
            }
        }

        public ICommand SendCommand { protected set; get; }

        public async Task sendCommand()
        {
            if (!string.IsNullOrWhiteSpace(this.replay))
            {
                var l = this.List.ToList();
                l.Add(new AccountCommentModel()
                {
                    //createDate = DateTime.Now,
                    caption = this.replay
                });
                //this.model.comments = l.ToArray();
                //await this.accountService.Save(model);
                this.replay = string.Empty;
                this.setList(l);
            }
        }
    }
}