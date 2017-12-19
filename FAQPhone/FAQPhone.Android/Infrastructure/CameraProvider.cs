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
using Android.Graphics;

namespace FAQPhone.Droid.Infrastructure
{
    public class CameraProvider
    {
        public static readonly int CAMERA_CAPTURE = 1;
        public static readonly int CROP_PICTURE = 2;
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
        public byte[] Data;
        private string filePath { get; set; }
        public event EventHandler Down;
        public event EventHandler Faild;
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
            this.filePath = file.Path;
            Activity activity = Forms.Context as Activity;
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));
            activity.StartActivityForResult(intent, CAMERA_CAPTURE);
        }

        public void PerformCrop()
        {
            try
            {
                Activity activity = Forms.Context as Activity;
                //call the standard crop action intent (the user device may not support it)
                Intent cropIntent = new Intent("com.android.camera.action.CROP");
                //indicate image type and Uri
                var file = new File(this.filePath);
                cropIntent.SetDataAndType(Android.Net.Uri.FromFile(file), "image/*");
                //set crop properties
                cropIntent.PutExtra("crop", "true");
                //indicate aspect of desired crop
                cropIntent.PutExtra("aspectX", 1);
                cropIntent.PutExtra("aspectY", 1);
                //indicate output X and Y
                cropIntent.PutExtra("outputX", 256);
                cropIntent.PutExtra("outputY", 256);
                //retrieve data on return
                cropIntent.PutExtra("return-data", true);
                //start the activity - we handle returning in onActivityResult
                activity.StartActivityForResult(cropIntent, CROP_PICTURE);
            }
            catch (ActivityNotFoundException anfe)
            {
                Faild?.Invoke(this, new EventArgs());
            }
        }

        public void TackPictureDown(Intent data)
        {
            //get the returned data
            Bundle extras = data.Extras;
            //get the cropped bitmap
            Bitmap thePic = (Bitmap)extras.GetParcelable("data");

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            thePic.Compress(Bitmap.CompressFormat.Png, 0, stream);
            this.Data = stream.ToArray();

            Down?.Invoke(this, new EventArgs());
        }
    }
}