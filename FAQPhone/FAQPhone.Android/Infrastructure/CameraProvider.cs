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
using Environment = Android.OS.Environment;
using Java.IO;
using Android.Provider;
using Xamarin.Forms;

namespace FAQPhone.Droid.Infrastructure
{
    public class CameraProvider
    {
        static CameraProvider _instance;
        public static CameraProvider GetInstance()
        {
            if (_instance == null)
                _instance = new CameraProvider();
            return _instance;
        }
        private CameraProvider()
        {

        }
        public string FilePath { get; set; }
        public event EventHandler Down;
        public void TackPicture()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            var dir = new File(Environment.GetExternalStoragePublicDirectory(
                Environment.DirectoryPictures), "CameraAppDemo");

            if (!dir.Exists())
            {
                dir.Mkdirs();
            }
            var fileName = String.Format("myPhoto_{0}.jpg", Guid.NewGuid());
            var file = new File(dir, fileName);
            this.FilePath = file.Path;
            Activity activity = Forms.Context as Activity;
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));
            activity.StartActivityForResult(intent, 1);
        }

        public void TackPictureDown()
        {
            Down?.Invoke(this, new EventArgs());
        }
    }
}