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
        IDownloadService _downloadService;
        Queue<string> _urlList;
        Action<T> _action;
        T _obj;
        public event EventHandler Failed;
        public DownloadHelper()
        {
            this._downloadService = DependencyService.Get<IDownloadService>();            
            this._downloadService.Downloaded += (s, e) =>
            {
                if (this._urlList.Count > 0)
                {
                    this._downloadService.Start(this._urlList.Dequeue());
                }
                else
                {
                    this._action?.Invoke(this._obj);
                }
            };
            this._downloadService.Failed += (s, e) =>
            {
                this.Failed?.Invoke(this, new EventArgs());
            };
        }

        public void Start(Queue<string> urlList, Action<T> action, T obj)
        {
            this._urlList = urlList;
            this._action = action;
            this._obj = obj;
            this._downloadService.Start(this._urlList.Dequeue());
        }
    }
}
