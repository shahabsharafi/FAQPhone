using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FAQPhone.Infrastructure;
using FAQPhone.Droid.Infrastructure;
using com.xamarin.recipes.filepicker;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(FilePickerLoader))]
namespace FAQPhone.Droid.Infrastructure
{
    [Activity(Label = "FilePickerLoader")]
    public class FilePickerLoader : IFilePicker
    {
        public void Open()
        {
            var intent = new Intent(Forms.Context, typeof(FilePickerActivity));
            Forms.Context.StartActivity(intent);
        }
    }
}