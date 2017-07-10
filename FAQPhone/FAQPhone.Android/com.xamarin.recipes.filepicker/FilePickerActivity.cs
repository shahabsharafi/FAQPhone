namespace com.xamarin.recipes.filepicker
{
    using Android.App;
    using Android.OS;
    using Android.Support.V4.App;
    using FAQPhone.Droid;

    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/appicon")]
    public class FilePickerActivity : FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
        }
    }
}
