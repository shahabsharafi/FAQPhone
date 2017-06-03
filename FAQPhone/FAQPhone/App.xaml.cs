using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services;
using FAQPhone.Services.Interfaces;
using FAQPhone.Views;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace FAQPhone
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            BindingContext = new AppViewModel();

            App.Initialize();

            var signinPage = App.Resolve<SigninPage>();// App.Container.Resolve(typeof(SigninPage), "SigninPage") as SigninPage;
            MainPage = new NavigationPage(signinPage);
        }
        public static UnityContainer Container { get; set; }
        public static void Initialize()
        {
            App.Bag = new BagModel();
            App.Container = new UnityContainer();
            App.Container.RegisterType<IAccountService, AccountService>();
            App.Container.RegisterType<IAuthenticationService, AuthenticationService>();
        }

        public static BagModel Bag { get; set; }

        public static T Resolve<T>() where T: class
        {
            return App.Container.Resolve(typeof(T), typeof(T).Name) as T;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    public class AppViewModel
    {
        public TextAlignment Direction { get { return ResourceManagerHelper.Direction; } }
    }
}
