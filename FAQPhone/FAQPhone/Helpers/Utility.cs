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
        public static Task Alert(string message = Constants.MESSAGE_UNKNOWN_ERROR, string title = Constants.MESSAGE_TITLE_ALERT, string cancel = Constants.COMMAND_OK)
        {
            var t = ResourceManagerHelper.GetValue(title);
            var m = ResourceManagerHelper.GetValue(message) ?? ResourceManagerHelper.GetValue(Constants.MESSAGE_UNKNOWN_ERROR);            
            var c = ResourceManagerHelper.GetValue(cancel);
            return Application.Current.MainPage.DisplayAlert(t, m, c);
        }
        public static Task<bool> Confirm(string message = Constants.MESSAGE_ARE_YOU_SURE, string title = Constants.MESSAGE_TITLE_ALERT, string accept = Constants.COMMAND_YES, string cancel = Constants.COMMAND_NO)
        {
            var t = ResourceManagerHelper.GetValue(title);
            var m = ResourceManagerHelper.GetValue(message) ?? ResourceManagerHelper.GetValue(Constants.MESSAGE_UNKNOWN_ERROR);
            var a = ResourceManagerHelper.GetValue(accept);
            var c = ResourceManagerHelper.GetValue(cancel);
            return Application.Current.MainPage.DisplayAlert(t, m, a, c);
        }
    }
}
