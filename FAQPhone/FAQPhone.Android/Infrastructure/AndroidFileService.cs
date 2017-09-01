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
    }
}