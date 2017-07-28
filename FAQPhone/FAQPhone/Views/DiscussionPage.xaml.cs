﻿using FAQPhone.Inferstructure;
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
        IDiscussionService discussionService;
        public DiscussionPageViewModelFactory(IDiscussionService discussionService)
        {
            this.discussionService = discussionService;
        }
        public DiscussionPageViewModel Create(INavigation navigation, string state, List<DiscussionModel> list)
        {
            return new DiscussionPageViewModel(this.discussionService, navigation, state, list);
        }
    }

    public class DiscussionPageViewModel : BaseViewModel
    {

        public DiscussionPageViewModel(IDiscussionService discussionService, INavigation navigation, string state, List<DiscussionModel> list) : base(navigation)
        {
            this.discussionService = discussionService;
            this.State = state;
            this.IsOperator = state == Constants.OPERATOR_INPROGRESS_FAQ;
            this.Title = ResourceManagerHelper.GetValue(state);
            this.SelectItemCommand = new Command<DiscussionModel>(async (model) => await selectItemCommand(model));
            this.List = new ObservableCollection<DiscussionModel>();
            this.setList(list);
        }
        IDiscussionService discussionService;
        public string Title { get; set; }
        public bool IsOperator { get; set; }
        string State { get; set; }

        public ICommand SelectItemCommand { protected set; get; }

        public async Task selectItemCommand(DiscussionModel model)
        {
            if (model == null)
                return;
            if (this.IsOperator && !model.operatorRead)
            {
                model.operatorRead = true;
                await this.discussionService.Save(model);
            }
            else if (!this.IsOperator && !model.userRead)
            {
                model.userRead = true;
                await this.discussionService.Save(model);
            }
            
            await this.Navigation.PushAsync(new ChatPage(this.State, model, 0));
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
                item.Mode = this.IsOperator
                    ? (item.operatorRead ? "read" : "unread")
                    : (item.userRead ? "read" : "unread");
                item.Caption = this.IsOperator ? item.display : item.title;
                this.List.Add(item);
            }
        }
    }
}
