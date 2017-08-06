using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomControl
{
    public class ExtendedEditor : Editor
    {
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create<ExtendedEditor, string>(view => view.Placeholder, String.Empty);

        public ExtendedEditor()
        {
        }

        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }

            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }
    }

}
