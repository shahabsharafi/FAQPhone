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
using System.IO;
using Infrastructure = FAQPhone.Droid.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Infrastructure;
using FilePicker;

[assembly: Xamarin.Forms.Dependency(typeof(Infrastructure.AndroidFileService))]
namespace FAQPhone.Droid.Infrastructure
{
    [Activity(Label = "AndroidFileService")]
    public class AndroidFileService : FilePicker.IFileService
    {
        public string GetBaseDirectory()
        {
            return (string)Android.OS.Environment.ExternalStorageDirectory;
        }

        public IList<PathModel> GetFileInfos(string path)
        {
            string directory = path;
            IList<FileSystemInfo> visibleThings = new List<FileSystemInfo>();
            var dir = new DirectoryInfo(directory);

            try
            {
                foreach (var item in dir.GetFileSystemInfos().Where(item => item.IsVisible()))
                {
                    visibleThings.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("message_file_path_access_erorr");
            }
            return visibleThings.Select(o => new PathModel { Name = o.Name, IsFile = o.IsFile() }).ToList();
        }

        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }
        
        public void OpenFile(string filePath)
        {

            var bytes = File.ReadAllBytes(filePath);

            //Copy the private file's data to the EXTERNAL PUBLIC location
            string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
            string application = "";

            string extension = System.IO.Path.GetExtension(filePath);

            switch (extension.ToLower())
            {
                case ".doc":
                case ".docx":
                    application = "application/msword";
                    break;
                case ".pdf":
                    application = "application/pdf";
                    break;
                case ".xls":
                case ".xlsx":
                    application = "application/vnd.ms-excel";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    application = "image/jpeg";
                    break;
                default:
                    application = "*/*";
                    break;
            }
            var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/report" + extension;
            File.WriteAllBytes(externalPath, bytes);

            Java.IO.File file = new Java.IO.File(externalPath);
            file.SetReadable(true);
            //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
            Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, application);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            try
            {
                Xamarin.Forms.Forms.Context.StartActivity(intent);
            }
            catch (Exception)
            {
                Toast.MakeText(Xamarin.Forms.Forms.Context, "Nessuna applicazione disponibile per questo tipo di documento.", ToastLength.Short).Show();
            }
        }
    }
}