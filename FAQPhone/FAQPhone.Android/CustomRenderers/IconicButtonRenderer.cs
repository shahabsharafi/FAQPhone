using System.ComponentModel;
using System.Net;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;
using CustomRenderer;
using FAQPhone.Droid.CustomRenderers;
using FAQPhone.Views;
using FAQPhone.Models;
using Android.App;
using JoanZapata.XamarinIconify.Widget;

[assembly: ExportRenderer(typeof(IconicButton), typeof(MessageRenderer))]

namespace FAQPhone.Droid.CustomRenderers
{
    public class CheckboxRenderer : ViewRenderer<IconicButton, IconButton>
    {
        private IconButton iconicButton;

        protected override void OnElementChanged(ElementChangedEventArgs<IconicButton> e)
        {
            base.OnElementChanged(e);
            var model = e.NewElement;
            iconicButton = new IconButton(Context);
            iconicButton.Tag = this;
            CheckboxPropertyChanged(model, null);
        }
        private void CheckboxPropertyChanged(IconicButton model, string propertyName)
        {
            if (propertyName == null || IconicButton.TextProperty.PropertyName == propertyName)
            {
                iconicButton.Text = model.Text;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (iconicButton != null)
            {
                base.OnElementPropertyChanged(sender, e);

                CheckboxPropertyChanged((IconicButton)sender, e.PropertyName);
            }
        }
    }


}