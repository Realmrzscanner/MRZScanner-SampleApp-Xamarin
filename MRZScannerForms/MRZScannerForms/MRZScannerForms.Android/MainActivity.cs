using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Com.Scansolutions.Mrzscannerlib;
using System.IO;

namespace MRZScannerForms.Droid
{
    [Activity(Label = "@string/app_name", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.KeyboardHidden)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IMRZScannerListener, IMRZLicenceResultListener
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

        public void OnRegisterWithLicenceResult(int result, string errorMessage)
        {
            LicenseResultType licenseResult = LicenseResultType.Unknown;

            switch (result)
            {
                case 0:
                    licenseResult = LicenseResultType.Successful;
                    break;
                case -1:
                    licenseResult = LicenseResultType.Error_While_Parsing;
                    break;
                case -2:
                    licenseResult = LicenseResultType.Invalid_Licence;
                    break;
                case -3:
                    licenseResult = LicenseResultType.Invalid_Bundle_ID;
                    break;
                case -4:
                    licenseResult = LicenseResultType.Invalid_Device_Model;
                    break;
                case -5:
                    licenseResult = LicenseResultType.Licence_Expired;
                    break;
                case -6:
                    licenseResult = LicenseResultType.Invalid_Platform;
                    break;
            }

            elementScanner.OnLicenseResult(licenseResult);
        }

        public void SuccessfulScanWithResult(MRZResultModel mrzResultModel)
        {
            if (elementScanner != null)
            {
                ResultModel _resultReceived = new ResultModel(mrzResultModel.DocumentTypeRaw, mrzResultModel.IssuingCountry, mrzResultModel.MasterCheckDigit, mrzResultModel.Surnames,
                mrzResultModel.GivenNames, mrzResultModel.DocumentNumber, mrzResultModel.DocumentNumberWithCheckDigit, mrzResultModel.Nationality, mrzResultModel.DobRaw, mrzResultModel.Sex,
                mrzResultModel.EstIssuingDateRaw, mrzResultModel.ExpirationDateRaw, mrzResultModel.Optionals, mrzResultModel.DateScanned, mrzResultModel.IsExpired,
                mrzResultModel.EstIssuingDateReadable, mrzResultModel.ExpirationDateReadable, mrzResultModel.ExpirationDateWithCheckDigit, mrzResultModel.DocumentTypeReadable, mrzResultModel.DobReadable, mrzResultModel.DobWithCheckDigit,
                mrzResultModel.FullName, mrzResultModel.RawResult);

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

                if (mrzResultModel.FullImage != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        mrzResultModel.FullImage.Compress(Bitmap.CompressFormat.Png, 0, stream);
                        _resultReceived.FullImage = stream.ToArray();
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
