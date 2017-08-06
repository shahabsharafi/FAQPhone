using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using CustomControl;
using System.ComponentModel;
using FAQPhone.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]

namespace FAQPhone.Droid.CustomRenderers
{
    public class ExtendedEditorRenderer : EditorRenderer
    {
        public ExtendedEditorRenderer()
        {
        }

        protected override void OnElementChanged(
            ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var element = e.NewElement as ExtendedEditor;
                this.Control.Hint = element.Placeholder;
            }
        }

        protected override void OnElementPropertyChanged(
            object sender,
            PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ExtendedEditor.PlaceholderProperty.PropertyName)
            {
                var element = this.Element as ExtendedEditor;
                this.Control.Hint = element.Placeholder;
            }
        }
    }
}