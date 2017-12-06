using FAQPhone.Infrastructure;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FAQPhone.Infarstructure
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty ("Text")]
	public class TranslateExtension : IMarkupExtension
	{
        //readonly CultureInfo ci = null;
		//const string ResourceId = "FAQPhone.Resx.Fa";

		public TranslateExtension() {
            //ci = new System.Globalization.CultureInfo("en");
        }

		public string Text { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (Text == null)
				return "";

			//ResourceManager temp = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);

            //var translation = temp.GetString (Text, ci);
            var translation = ResourceManagerHelper.GetValue(Text);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found.", Text),
                    "Text");
#else
				translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
		}
	}
}
