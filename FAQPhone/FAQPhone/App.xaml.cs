using FAQPhone.Helpers;
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
using System.Threading.Tasks;
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

            if (!String.IsNullOrEmpty(Settings.Username) && !String.IsNullOrEmpty(Settings.Password))
            {
                Task.Run(async () => await this.Login()).Wait();
            }
            else
            {
                this.Go(false);
            }
        }

        public Task DisplayAlert(string title, string message, string cancel)
        {
            Page page = Utility.GetCurrentPage();
            return Utility.DisplayAlert(page, title, message, cancel);
        }
        public Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            Page page = Utility.GetCurrentPage();
            return Utility.DisplayAlert(page, title, message, accept, cancel);
        }

        async Task Login()
        {
            SigninModel model = new SigninModel()
            {
                username = Settings.Username,
                password = Settings.Password
            };
            var accountService = App.Resolve<AccountService>();
            var flag = await accountService.SignIn(model);
            var attributeService = App.Resolve<AttributeService>();
            AttributeList = await attributeService.GetAll();
            Go(flag);
        }

        void Go(bool flag)
        {
            if (flag)
            {
                var page = new MainPage();
                MainPage = new NavigationPage(page);
            }
            else
            {
                var page = new SendCodePage(FlowType.Signup);
                MainPage = new NavigationPage(page);
            }
        }
        public static UnityContainer Container { get; set; }
        public static void Initialize()
        {
            App.Container = new UnityContainer();
            App.Container.RegisterType<IAccountService, AccountService>();
            App.Container.RegisterType<IDepartmentService, DepartmentService>();
            App.Container.RegisterType<IDiscussionService, DiscussionService>();
        }

        public static string Username { get; set; }

        public static string Token { get; set; }

        public static string[] Access { get; set; }

        public static List<AttributeModel> AttributeList { get; set; }

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
        public LayoutOptions Layout { get { return ResourceManagerHelper.Layout; } }
    }
}
