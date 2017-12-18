using FAQPhone.Helpers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarkupExtensions
{
    [ContentProperty("SourceImage")]
    public class PlatformImageExtension : IMarkupExtension<string>
    {

        public string SourceImage { get; set; }

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            if (SourceImage == null)
                return null;

            return Utility.GetImage(SourceImage);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}

