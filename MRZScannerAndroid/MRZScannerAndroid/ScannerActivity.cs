using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Com.Scansolutions.Mrzscannerlib;
using System;

namespace MRZScannerAndroid
{
    [Activity(Label = "@string/title_activity_scanner", Theme = "@style/FullscreenTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class ScannerActivity : AppCompatActivity, IMRZScannerListener, IMRZLicenceResultListener
    {
        MRZScanner mrzScanner;
        private static String TYPE_EXTRA = "TYPE_EXTRA";

        public static void startScannerActivity(Context context, ScannerType type)
        {
            Intent intent = new Intent(context, typeof(ScannerActivity));
            intent.PutExtra(TYPE_EXTRA, type);
            context.StartActivity(intent);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_scanner);

            ScannerType type = (ScannerType)Intent.GetSerializableExtra(TYPE_EXTRA);

            mrzScanner = (MRZScanner)SupportFragmentManager.FindFragmentById(Resource.Id.scannerFragment);
            mrzScanner.SetScannerType(type);      // Options: [ScannerTypeMrz, ScannerTypeDocImageId, ScannerTypeDocImagePassport]. Default: MRZ.
            MRZScanner.SetIDActive(true);         // Enable/disable the ID document type. Default: true.
            MRZScanner.SetPassportActive(true);   // Enable/disable the Passport document type. Default: true.
            MRZScanner.SetVisaActive(true);       // Enable/disable the Visa document type. Default: true.
            MRZScanner.SetMaxThreads(2);          // Set the max CPU threads that the scanner can use. Default: 2.
            MRZScanner.RegisterWithLicenseKey(this, "licenseKey", this);
        }

        public void OnRegisterWithLicenceResult(int result, string errorMessage)
        {
            if (errorMessage != null)
            {
                Console.WriteLine("Result code: " + result + ". " + errorMessage);
            }
            else
            {
                Console.WriteLine("Result code: " + result + ". Registration successful.");
            }
        }

        public void SuccessfulScanWithResult(MRZResultModel mrzResultModel)
        {
            new Android.Support.V7.App.AlertDialog.Builder(this)
                    .SetMessage(mrzResultModel.ToReadableString())
                    .SetCancelable(false)
                    .SetPositiveButton("Resume", (dialogInterface, i) =>
                    {
                        mrzScanner.ResumeScanning();
                    })
                    .SetNegativeButton("Close", (dialogInterface, i) =>
                    {
                        Finish();
                    })
                    .Show();
        }

        public void SuccessfulScanWithDocumentImage(Bitmap bitmap)
        {
            ImageView imageView = new ImageView(this);
            imageView.SetImageBitmap(bitmap);

            new Android.Support.V7.App.AlertDialog.Builder(this)
                    .SetMessage("Captured image")
                    .SetPositiveButton("Scan again", (dialogInterface, i) =>
                    {
                        mrzScanner.ResumeScanning();
                    })
                    .SetNegativeButton("Close", (dialogInterface, i) =>
                    {
                        Finish();
                    })
                    .SetView(imageView)
                    .SetCancelable(false)
                    .Show();
        }

        public void ScanImageFailed()
        {
        }

        public void PermissionsWereDenied()
        {
        }

        public void SuccessfulIdFrontImageScan(Bitmap fullImage, Bitmap portrait)
        {

        }
    }
}
