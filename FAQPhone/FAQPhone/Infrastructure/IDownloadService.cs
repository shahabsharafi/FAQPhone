using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Infrastructure
{
    public interface IDownloader
    {
        event EventHandler Downloaded;

        event EventHandler Failed;
        void Start(string fileName);
    }
    public interface IDownloadService
    {
        IDownloader GetDownloader();
    }
}
