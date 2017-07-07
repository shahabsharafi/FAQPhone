using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using FAQPhone.Droid.CustomRenderers;
using CustomRenderer;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRenderer))]

namespace FAQPhone.Droid.CustomRenderers
{
    public class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
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