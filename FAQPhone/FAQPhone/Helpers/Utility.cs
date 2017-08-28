using FAQPhone.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FAQPhone.Helpers
{
    public class Utility
    {
        public static Page GetCurrentPage()
        {
            Page currPage = null;
            if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            {
                //LIFO is the only game in town! - so send back the last page
                int index = Application.Current.MainPage.Navigation.NavigationStack.Count - 1;
                currPage = Application.Current.MainPage.Navigation.NavigationStack[index];
            }
            return currPage;
        }
        public static Task DisplayAlert(Page page, string title, string message, string cancel)
        {
            var t = ResourceManagerHelper.GetValue(title);
            var m = ResourceManagerHelper.GetValue(message);
            var c = ResourceManagerHelper.GetValue(cancel);
            return page.DisplayAlert(t, m, c);
        }
        public static Task<bool> DisplayAlert(Page page, string title, string message, string accept, string cancel)
        {
            var t = ResourceManagerHelper.GetValue(title);
            var m = ResourceManagerHelper.GetValue(message);
            var a = ResourceManagerHelper.GetValue(accept);
            var c = ResourceManagerHelper.GetValue(cancel);
            return page.DisplayAlert(t, m, a, c);
        }
    }
}
