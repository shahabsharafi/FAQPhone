using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomControl
{
    public class ExtendedButton: Button
    {
        public ExtendedButton()
        {
            this.PropertyChanged += ExtendedButton_PropertyChanged;

        }

        new public IList<string> StyleClass
        {
            get
            {
                return base.StyleClass;
            }
            set
            {
                this.StyleClass = value;
            }
        }

        private void ExtendedButton_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEnabled")
            {
                if (this.IsEnabled)
                    base.StyleClass = this.StyleClass;
                else
                    base.StyleClass = null;
            }
        }
    }
}
