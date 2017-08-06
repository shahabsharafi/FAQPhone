using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using FAQPhone.Droid.CustomRenderers;
using CustomRenderer;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]

namespace FAQPhone.Droid.CustomRenderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (!string.IsNullOrEmpty(e.NewElement?.FontFamily))
            {
                var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, e.NewElement.FontFamily + ".ttf");

                Control.Typeface = font;
            }
        }
    }
}