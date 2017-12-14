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
                var page = new ConnectingPage();
                MainPage = new NavigationPage(page);
            }
            else
            {
                var page = new SendCodePage();
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
            App.Container.RegisterType<IAttributeService, AttributeService>();
            App.Container.RegisterType<IDiscountService, DiscountService>();
            App.Container.RegisterType<IMessageService, MessageService>();
            App.Container.RegisterType<IEmailService, EmailService>();
        }

        public static bool EnterAsOperator { get; set; }
        public static string SuportVersion { get; set; }
        public static bool Blocked { get; set; }

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
