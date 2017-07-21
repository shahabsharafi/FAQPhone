using FAQPhone.Inferstructure;
using FAQPhone.Infrastructure;
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
    public partial class DiscussionPage : ContentPage
    {
        public DiscussionPage(string state, List<DiscussionModel> list)
        {
            InitializeComponent();
            var factory = App.Resolve<DiscussionPageViewModelFactory>();
            BindingContext = factory.Create(Navigation, state, list);
        }
    }

    public class DiscussionPageViewModelFactory
    {
        public DiscussionPageViewModelFactory()
        {
            
        }
        public DiscussionPageViewModel Create(INavigation navigation, string state, List<DiscussionModel> list)
        {
            return new DiscussionPageViewModel(navigation, state, list);
        }
    }

    public class DiscussionPageViewModel : BaseViewModel
    {

        public DiscussionPageViewModel(INavigation navigation, string state, List<DiscussionModel> list) : base(navigation)
        {
            this.State = state;
            this.Title = ResourceManagerHelper.GetValue(state);
            this.SelectItemCommand = new Command<DiscussionModel>(async (model) => await selectItemCommand(model));
            this.List = new ObservableCollection<DiscussionModel>();
            this.setList(list);
        }
        public string Title { get; set; }
        string State { get; set; }

        public ICommand SelectItemCommand { protected set; get; }

        public async Task selectItemCommand(DiscussionModel model)
        {
            if (model == null)
                return;            
            await this.Navigation.PushAsync(new ChatPage(this.State, model));
            this.SelectedItem = null;
        }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        ObservableCollection<DiscussionModel> _list;
        public ObservableCollection<DiscussionModel> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged(); }
        }
        private void setList(List<DiscussionModel> list)
        {
            this.List.Clear();
            foreach (var item in list)
            {
                this.List.Add(item);
            }
        }
    }
}
