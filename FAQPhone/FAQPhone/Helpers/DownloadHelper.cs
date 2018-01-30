using FAQPhone.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FAQPhone.Helpers
{
    public class DownloadHelper<T>
    {
        IDownloader _downloader;
        Queue<string> _urlList;
        Action<T> _action;
        T _obj;
        public event EventHandler Failed;
        public DownloadHelper()
        {
            this._downloader = DependencyService.Get<IDownloadService>().GetDownloader();            
            this._downloader.Downloaded += (s, e) =>
            {
                Next();
            };
            this._downloader.Failed += (s, e) =>
            {
                this.Failed?.Invoke(this, new EventArgs());
                Next();
            };
        }

        private void Next()
        {
            if (this._urlList.Count > 0)
            {
                this._downloader.Start(this._urlList.Dequeue());
            }
            else
            {
                this._action?.Invoke(this._obj);
            }
        }

        public void Start(Queue<string> urlList, Action<T> action, T obj)
        {
            this._urlList = urlList;
            this._action = action;
            this._obj = obj;
            this._downloader.Start(this._urlList.Dequeue());
        }
    }
}
