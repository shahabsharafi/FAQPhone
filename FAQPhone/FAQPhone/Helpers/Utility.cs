using FAQPhone.Infarstructure;
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
        public static Task Alert(string message = Constants.MESSAGE_UNKNOWN_ERROR, string title = Constants.MESSAGE_TITLE_ALERT, string cancel = Constants.COMMAND_OK)
        {
            var page = GetCurrentPage();
            var t = ResourceManagerHelper.GetValue(title);
            var m = ResourceManagerHelper.GetValue(message) ?? ResourceManagerHelper.GetValue(Constants.MESSAGE_UNKNOWN_ERROR);            
            var c = ResourceManagerHelper.GetValue(cancel);
            return page.DisplayAlert(t, m, c);
        }
        public static Task<bool> Confirm(string message = Constants.MESSAGE_ARE_YOU_SURE, string title = Constants.MESSAGE_TITLE_ALERT, string accept = Constants.COMMAND_YES, string cancel = Constants.COMMAND_NO)
        {
            var page = GetCurrentPage();
            var t = ResourceManagerHelper.GetValue(title);
            var m = ResourceManagerHelper.GetValue(message) ?? ResourceManagerHelper.GetValue(Constants.MESSAGE_UNKNOWN_ERROR);
            var a = ResourceManagerHelper.GetValue(accept);
            var c = ResourceManagerHelper.GetValue(cancel);
            return page.DisplayAlert(t, m, a, c);
        }
    }
}
