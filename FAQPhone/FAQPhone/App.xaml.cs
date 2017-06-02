using FAQPhone.Infrastructure;
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
            MainPage = new NavigationPage(new FAQPhone.Views.SigninPage());
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
