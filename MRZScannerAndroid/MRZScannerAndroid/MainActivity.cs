using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Com.Scansolutions.Mrzscannerlib;

namespace MRZScannerAndroid
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnStartScanner;
        Button btnCaptureImage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            btnStartScanner = (Button)FindViewById(Resource.Id.btnStartMrzScanner);
            btnStartScanner.Click += (sender, e) =>
            {
                ScannerActivity.startScannerActivity(this, ScannerType.ScannerTypeMrz);
            };

            btnCaptureImage = (Button)FindViewById(Resource.Id.btnCapturePassportImage);
            btnCaptureImage.Click += (sender, e) =>
            {
                ScannerActivity.startScannerActivity(this, ScannerType.ScannerTypeDocImagePassport);
            };
        }
    }
}
