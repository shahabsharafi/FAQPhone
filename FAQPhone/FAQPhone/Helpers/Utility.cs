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
        public static Task RegulareAlert(string message)
        {
            string title = Constants.MESSAGE_TITLE_ALERT;
            string cancel = Constants.COMMAND_OK;
            var t = ResourceManagerHelper.GetValue(title);
            var m = message ?? ResourceManagerHelper.GetValue(Constants.MESSAGE_UNKNOWN_ERROR);
            var c = ResourceManagerHelper.GetValue(cancel);
            return Application.Current.MainPage.DisplayAlert(t, m, c);
        }
        public static Task<bool> RegulareConfirm(string message)
        {
            string title = Constants.MESSAGE_TITLE_ALERT;
            string accept = Constants.COMMAND_YES;
            string cancel = Constants.COMMAND_NO;
            var t = ResourceManagerHelper.GetValue(title);
            var m = message ?? ResourceManagerHelper.GetValue(Constants.MESSAGE_UNKNOWN_ERROR);
            var a = ResourceManagerHelper.GetValue(accept);
            var c = ResourceManagerHelper.GetValue(cancel);
            return Application.Current.MainPage.DisplayAlert(t, m, a, c);
        }
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

        public static int[] GetVersionInfo(string version)
        {
            var arr = version.Split('.');
            var info = new int[] { 0, 0, 0 };
            for (var i = 0; i < 3; i++)
            {
                if (arr.Length > i)
                {
                    var v = arr[i];
                    int vr;
                    if (int.TryParse(v, out vr))
                    {
                        info[i] = vr;
                    }
                }
            }
            return info;
        }

        public static int CompareVersion()
        {
            var app_version = ResourceManagerHelper.GetValue(Constants.APP_VERSION);
            var arr1 = GetVersionInfo(app_version);
            var arr2 = GetVersionInfo(App.SuportVersion);
            var result = 0;
            for (int i = 0; i < 3; i++)
            {                
                if (arr1[i] < arr2[i])
                {
                    result = -1;
                }
                else if (arr1[i] > arr2[i])
                {
                    result = 1;
                }
                if (result != 0)
                    break;
            }
            return result;
        }
    }
}
