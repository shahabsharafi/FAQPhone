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

            e.NewElement.PropertyChanged += NewElement_PropertyChanged;            

            if (!string.IsNullOrEmpty(e.NewElement?.FontFamily))
            {
                var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, e.NewElement.FontFamily + ".ttf");

                Control.Typeface = font;
            }
        }

        private void NewElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender != null && this.Control != null)
            {
                Button btn = (Button)sender;
                if (!btn.IsEnabled)
                {
                    this.Control.SetBackgroundColor(Android.Graphics.Color.Gray);
                    this.Control.SetTextColor(Android.Graphics.Color.White);
                }
                else
                {
                    this.Control.SetBackgroundColor(btn.BackgroundColor.ToAndroid());
                    this.Control.SetTextColor(btn.TextColor.ToAndroid());
                }
            }
        }
    }
}