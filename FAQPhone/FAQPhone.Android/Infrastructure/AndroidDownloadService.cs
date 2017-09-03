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

using Infrastructure = FAQPhone.Droid.Infrastructure;
using System.Net;
using System.IO;
using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;

[assembly: Xamarin.Forms.Dependency(typeof(Infrastructure.AndroidDownloadService))]
namespace FAQPhone.Droid.Infrastructure
{
    public class AndroidDownloadService: IDownloadService
    {
        WebClient webClient;
        Uri _url;
        public event EventHandler Downloaded;
        public event EventHandler Failed;
        public AndroidDownloadService(string fileName)
        {
            webClient = new WebClient();
            webClient.DownloadDataCompleted += (s, e) => {
                try
                {
                    var bytes = e.Result;
                    string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    string localPath = Path.Combine(documentsPath, fileName);
                    File.WriteAllBytes(localPath, bytes); // writes to local storage
                    Downloaded?.Invoke(this, new EventArgs());
                }
                catch(Exception ex)
                {
                    Failed?.Invoke(this, new EventArgs());
                }
            };
            this._url = new Uri(Constants.UploadUrl + "/" + fileName);
        }

        public void Start()
        {
            try
            {
                webClient.DownloadDataAsync(this._url);
            }
            catch (Exception ex)
            {
                Failed?.Invoke(this, new EventArgs());
            }
        }
    }
}