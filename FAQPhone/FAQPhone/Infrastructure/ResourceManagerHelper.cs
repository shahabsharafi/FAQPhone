using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reflection;

namespace FAQPhone.Infrastructure
{
    public class ResourceManagerHelper
    {
        public static string GetValue(string key)
        {
            string ResourceId = "FAQPhone.Resx.AppResources";
            CultureInfo ci = new System.Globalization.CultureInfo("en");
            ResourceManager temp = new ResourceManager(ResourceId, typeof(ResourceManagerHelper).GetTypeInfo().Assembly);
            var val = temp.GetString(key, ci);
            return val;
        }

        public static TextAlignment Direction
        {
            get
            {
                var dr = GetValue("direction");
                return dr == "RTL" ? TextAlignment.End : TextAlignment.Start;
            }
        }
    }
}
