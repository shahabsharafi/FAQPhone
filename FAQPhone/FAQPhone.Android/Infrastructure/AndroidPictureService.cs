using System;
using AndroidPictureService = FAQPhone.Droid.Infrastructure.AndroidPictureService;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidPictureService))]
namespace FAQPhone.Droid.Infrastructure
{
    public class AndroidPictureService : TackPicture.IPictureService
    {
        CameraProvider camera;
        public void TakeAPicture(Action<string> action)
        {
            camera = CameraProvider.GetInstance();
            camera.TackPicture();
            this._action = action;
            camera.Down += Camera_Down;
        }

        Action<string> _action;

        private void Camera_Down(object sender, System.EventArgs e)
        {
            this._action.Invoke(camera.FilePath);
        }
    }
}