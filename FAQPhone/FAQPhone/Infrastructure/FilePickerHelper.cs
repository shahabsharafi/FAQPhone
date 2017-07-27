using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FAQPhone.Infrastructure
{
    public class FilePickerHelper
    {
        public static void Open()
        {
            DependencyService.Register<IFilePicker>();
            DependencyService.Get<IFilePicker>().Open();
        }
    }
}
