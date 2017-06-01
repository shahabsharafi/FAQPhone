using FAQPhone.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FAQPhone.ViewModels
{
    public class BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public BaseViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
        }

        public TextAlignment Direction { get { return ResourceManagerHelper.Direction; } }
    }
}
