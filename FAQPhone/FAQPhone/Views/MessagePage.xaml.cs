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
    public partial class MessagePage : ContentPage
    {
        public MessagePage()
        {
            InitializeComponent();
            var factory = App.Resolve<MessageViewModelFactory>();
            var vm = factory.Create(this);
            this.Appearing += (sender, e) => {
                Task.Run(() => vm.loadItems()).Wait();
            };
            BindingContext = vm;
        }
    }

    public class MessageViewModelFactory
    {
        IMessageService messageService;
        public MessageViewModelFactory(IMessageService messageService)
        {
            this.messageService = messageService;
        }
        public MessageViewModel Create(ContentPage page)
        {
            return new MessageViewModel(this.messageService, page);
        }
    }

    public class MessageViewModel : BaseViewModel
    {

        public MessageViewModel(IMessageService messageService, ContentPage page) : base(page)
        {
            this.messageService = messageService;
            this.List = new ObservableCollection<MessageModel>();
            this.SelectItemCommand = new Command<MessageModel>((model) => selectItemCommand(model));
        }
        private IMessageService messageService { get; set; }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        ObservableCollection<MessageModel> _list;
        public ObservableCollection<MessageModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }

        public async Task loadItems()
        {
            var list = await this.messageService.GetAllMessages();
            this.setList(list);
        }

        private void setList(List<MessageModel> list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                this.List.Add(item);
            }
        }

        public ICommand SelectItemCommand { protected set; get; }

        public void selectItemCommand(MessageModel model)
        {
            if (model == null)
                return;
            this.SelectedItem = null;
        }
    }
}