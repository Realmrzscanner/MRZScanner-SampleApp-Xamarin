using Android.App;
using Android.Content.PM;
using Android.OS;
using Com.Scansolutions.Mrzscannerlib;
using Android.Graphics;
using System.IO;

namespace MRZScannerForms.Droid
{
    [Activity(Label = "@string/app_name", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.KeyboardHidden)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IMRZScannerListener
    {
        public static ScannerControl elementScanner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        public void SuccessfulScanWithResult(MRZResultModel mrzResultModel)
        {
            if (elementScanner != null)
            {
                ResultModel _resultReceived = new ResultModel(mrzResultModel.DocumentTypeRaw, mrzResultModel.IssuingCountry, mrzResultModel.Surnames,
                mrzResultModel.GivenNames, mrzResultModel.DocumentNumber, mrzResultModel.Nationality, mrzResultModel.DobRaw, mrzResultModel.Sex,
                mrzResultModel.EstIssuingDateRaw, mrzResultModel.ExpirationDateRaw, mrzResultModel.Optionals, mrzResultModel.DateScanned,
                mrzResultModel.EstIssuingDateReadable, mrzResultModel.ExpirationDateReadable, mrzResultModel.DocumentTypeReadable, mrzResultModel.DobReadable, mrzResultModel.FullName, mrzResultModel.RawResult);

                if (mrzResultModel.Portrait != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        mrzResultModel.Portrait.Compress(Bitmap.CompressFormat.Png, 0, stream);
                        _resultReceived.Portrait = stream.ToArray();
                    }
                }

                if (mrzResultModel.Signature != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        mrzResultModel.Signature.Compress(Bitmap.CompressFormat.Png, 0, stream);
                        _resultReceived.Signature = stream.ToArray();
                    }
                }

                elementScanner.OnScanningFinished(_resultReceived);
            }
        }

        public void SuccessfulScanWithDocumentImage(Bitmap bitmap)
        {
            if (elementScanner != null)
            {
                ResultModel resultWithImage = new ResultModel();

                if (bitmap != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                        resultWithImage.ResultImage = stream.ToArray();
                    }
                }

                elementScanner.OnScanningFinished(resultWithImage);
            }
        }

        public void ScanImageFailed()
        {
        }

        public void PermissionsWereDenied()
        {
            if (elementScanner != null)
                elementScanner.OnScanningFinished(new ResultModel("Permissions were denied"));
        }
    }
}
