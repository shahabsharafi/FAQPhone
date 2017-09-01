﻿using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
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
    public partial class SendCodePage : ContentPage
    {
        public SendCodePage(FlowType flow)
        {
            InitializeComponent();
            var factory = App.Resolve<SendCodeViewModelFactory>();
            BindingContext = factory.Create(this, flow);
        }
    }
    
    public class SendCodeViewModelFactory
    {
        IAccountService accountService;
        public SendCodeViewModelFactory(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        public SendCodeViewModel Create(ContentPage page, FlowType flow)
        {
            return new SendCodeViewModel(this.accountService, page, flow);
        }
    }
    
    public class SendCodeViewModel : BaseViewModel
    {
        public SendCodeViewModel(IAccountService accountService, ContentPage page, FlowType flow) : base(page)
        {
            this.accountService = accountService;
            this.flow = flow;
            this.SendCodeCommand = new Command(async () => await sendCodeCommand());
        }
        private IAccountService accountService { get; set; }
        private FlowType flow { get; set; }
        string _mobile;
        public string mobile
        {
            get { return _mobile; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length <= 11)
                {
                    _mobile = value;                    
                }
                OnPropertyChanged();
            }
        }
        public ICommand SendCodeCommand { protected set; get; }

        public async Task sendCodeCommand()
        {
            /////
            var mobile = this.mobile.ToEnglishNumber();
            var codeResult = await this.accountService.SendCode(mobile);            
            await this.Navigation.PushAsync(new SecurityCodePage(this.flow, mobile, codeResult));
        }
    }
}
