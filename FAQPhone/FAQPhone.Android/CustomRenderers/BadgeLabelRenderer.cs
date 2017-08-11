using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using FAQPhone.Droid.CustomRenderers;
using CustomRenderer;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Util;
using Android.Widget;
using Android.Views;

[assembly: ExportRenderer(typeof(BadgeLabel), typeof(BadgeLabelRenderer))]

namespace FAQPhone.Droid.CustomRenderers
{
    public class BadgeLabelRenderer : LabelRenderer
    {
        private const int DefaultLrPaddingDip = 10;
        private const int DefaultCornerRadiusDip = 14;
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (!string.IsNullOrEmpty(e.NewElement?.FontFamily))
            {
                var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, e.NewElement.FontFamily + ".ttf");

                Control.Typeface = font;
            }
            //var lp = new FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            //lp.Gravity = GravityFlags.Center;
            //Control.LayoutParameters = lp;            
            Control.SetTextColor(Android.Graphics.Color.White);
            var sh = CreateBackgroundShape();
            Control.SetBackground(sh);
        }

        private ShapeDrawable CreateBackgroundShape()
        {
            var radius = DipToPixels(DefaultCornerRadiusDip);
            var outerR = new float[] { radius, radius, radius, radius, radius, radius, radius, radius };

            var sh = new ShapeDrawable(new RoundRectShape(outerR, null, null));
            sh.SetPadding(DefaultLrPaddingDip, DefaultLrPaddingDip, DefaultLrPaddingDip, DefaultLrPaddingDip);
            sh.Paint.Color = Android.Graphics.Color.ParseColor("#39abf7");
            return sh;
        }

        private int DipToPixels(int dip)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, Resources.DisplayMetrics);
        }
    }
}