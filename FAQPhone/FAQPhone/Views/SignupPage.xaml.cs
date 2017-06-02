﻿using FAQPhone.Inferstructure;
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
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
            BindingContext = new SignupViewModel(Navigation);
        }
    }

    public class SignupViewModel : BaseViewModel
    {
        public SignupViewModel(INavigation navigation) : base(navigation)
        {
            this.SignupCommand = new Command(async () => await signupCommand());
        }
        string _username;
        public string username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        string _password;
        public string password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        string _confirm;
        public string confirm
        {
            get { return _confirm; }
            set { _confirm = value; OnPropertyChanged(); }
        }
        string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        string _mobile;
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; OnPropertyChanged(); }
        }
        public ICommand SignupCommand { protected set; get; }

        public async Task signupCommand()
        {
            /////
            /*
            SignupModel model = new SignupModel()
            {
                username = this.username,
                password = this.password,
                email = this.email,
                mobile = this.mobile
            };
            await this.Navigation.PushAsync(new ActivatePage(model));
            */
        }
    }
}
