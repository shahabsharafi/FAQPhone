using System;

using Xamarin.Forms;

namespace CustomRenderer
{
    public class CropView : ContentPage
    {
        public byte[] Image;
        public bool DidCrop = false;
        public byte[] CroppedImage { get; set; }
        public event EventHandler Crop;

        public CropView(byte[] imageAsByte)
        {

            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Color.Black;
            Image = imageAsByte;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (DidCrop)
                Crop?.Invoke(this, new EventArgs());
        }
    }
}


