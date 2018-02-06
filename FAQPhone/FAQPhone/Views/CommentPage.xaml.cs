using FAQPhone.Helpers;
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
	public partial class CommentPage : ContentPage
	{
		public CommentPage (AccountModel model)
		{
			InitializeComponent ();
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
            this.SelectItemCommand = new Command<AccountComment>((m) => selectItemCommand(m));
            this.SendCommand = new Command(async () => await sendCommand());
            this.model = model;
            this.List = new ObservableCollection<AccountComment>();
            setList(model.comments.ToList());
        }
        private IAccountService accountService { get; set; }
        private AccountModel model { get; set; }

        public ICommand SelectItemCommand { protected set; get; }

        public void selectItemCommand(AccountComment model)
        {
            if (model == null)
                return;
            this.SelectedItem = null;
        }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

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

        ObservableCollection<AccountComment> _list;
        public ObservableCollection<AccountComment> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        private void setList(List<AccountComment> list)
        {
            var l = list.OrderByDescending(o => o.createDate);
            this.List.Clear();
            foreach (var item in l)
            {
                item.CreateDateCaption = Utility.MiladiToShamsiString(item.createDate);
                this.List.Add(item);
            }
        }

        public ICommand SendCommand { protected set; get; }

        public async Task sendCommand()
        {
            if (!string.IsNullOrWhiteSpace(this.replay))
            {
                var l = this.List.ToList();
                l.Add(new AccountComment()
                {
                    createDate = DateTime.Now,
                    text = this.replay
                });
                this.model.comments = l.ToArray();
                await this.accountService.Save(model);
                this.replay = string.Empty;
                this.setList(l);
            }
        }
    }
}