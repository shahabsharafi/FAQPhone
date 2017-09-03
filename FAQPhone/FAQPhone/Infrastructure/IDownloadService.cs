using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Infrastructure
{
    public interface IDownloadService
    {
        event EventHandler Downloaded;

        event EventHandler Failed;
        void Start();
    }
}
