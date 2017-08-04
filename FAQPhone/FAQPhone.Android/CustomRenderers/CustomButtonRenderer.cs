using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using FAQPhone.Droid.CustomRenderers;
using CustomRenderer;

[assembly: ExportRenderer(typeof(Button), typeof(CustomButtonRenderer))]

namespace FAQPhone.Droid.CustomRenderers
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
            
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
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