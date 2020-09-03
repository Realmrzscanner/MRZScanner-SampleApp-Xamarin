using System;
using Foundation;
using MRZScanneriOSBinding;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MRZScannerForms.ScannerControl), typeof(MRZScannerForms.iOS.ScannerControlRenderer))]
namespace MRZScannerForms.iOS
{
    public class ScannerControlRenderer : ViewRenderer, IMRZScannerDelegate
    {
        MRZScannerController mrzSannerController;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            mrzSannerController = new MRZScannerController();

            // Options: [ScannerTypeMrz, ScannerTypeDocImageId, ScannerTypeDocImagePassport]. Default: MRZ.
            switch (((ScannerControl)Element).ScannerType)
            {
                case ScanType.Mrz:
                    mrzSannerController.SetScannerType(MRZScannerType.Mrz);
                    break;
                case ScanType.DocImageId:
                    mrzSannerController.SetScannerType(MRZScannerType.DocumentImageId);
                    break;
                case ScanType.DocImagePassport:
                    mrzSannerController.SetScannerType(MRZScannerType.DocumentImagePassport);
                    break;
                default:
                    mrzSannerController.SetScannerType(MRZScannerType.Mrz);
                    break;
            }
            // Enable/disable the ID document type. Default: true.
            MRZScannerController.SetIDActive(((ScannerControl)Element).IDActive);
            // Enable/disable the Passport document type. Default: true.
            MRZScannerController.SetPassportActive(((ScannerControl)Element).PassportActive);
            // Enable/disable the Visa document type. Default: true.
            MRZScannerController.SetVisaActive(((ScannerControl)Element).VisaActive);
            // Set the max CPU threads that the scanner can use. Default: 2.
            mrzSannerController.SetMaxCPUCores(((ScannerControl)Element).MaxThreads);
            // Set license key
            MRZScannerController.RegisterLicenseWithKey(((ScannerControl)Element).LicenseKey, (result, error) =>
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

                ((ScannerControl)Element).OnLicenseResult(licenseResult);
            });

            mrzSannerController.WeakDelegate = this;

            UIViewController currentViewController = UIApplication.SharedApplication.KeyWindow.RootViewController.NavigationController != null ? UIApplication.SharedApplication.KeyWindow.RootViewController.NavigationController : UIApplication.SharedApplication.KeyWindow.RootViewController;
            currentViewController.AddChildViewController(mrzSannerController);
            mrzSannerController.InitUI(currentViewController);

            if (Control == null)
                SetNativeControl(mrzSannerController.View);

            ((ScannerControl)Element).OnResumeScanning += ScannerControlRenderer_OnResumeScanning;
        }

        private void ScannerControlRenderer_OnResumeScanning(object sender, EventArgs e)
        {
            if (mrzSannerController != null)
                mrzSannerController.ResumeScanner();
        }

        [Export("successfulScanWithResult:")]
        public void SuccessfulScanWithResult(MRZResultDataModel mrzResultDataModel)
        {
            if (Element != null)
            {
                ResultModel _resultReceived = new ResultModel(mrzResultDataModel.Document_type_raw, mrzResultDataModel.Issuing_country, mrzResultDataModel.Master_check_digit, mrzResultDataModel.Surnames,
                mrzResultDataModel.Given_names, mrzResultDataModel.Document_number, mrzResultDataModel.Document_number_with_check_digit, mrzResultDataModel.Nationality, mrzResultDataModel.Dob_raw, mrzResultDataModel.Sex,
                mrzResultDataModel.Est_issuing_date_raw, mrzResultDataModel.Expiration_date_raw, mrzResultDataModel.Optionals, mrzResultDataModel.DateScanned, mrzResultDataModel.IsExpired,
                mrzResultDataModel.Est_issuing_date_readable, mrzResultDataModel.Expiration_date_readable, mrzResultDataModel.Expiration_date_with_check_digit, mrzResultDataModel.Document_type_readable,
                mrzResultDataModel.Dob_readable, mrzResultDataModel.Dob_with_check_digit, mrzResultDataModel.FullName, mrzResultDataModel.Raw_result);

                if (mrzResultDataModel.Portrait != null)
                {
                    using (NSData imageData = mrzResultDataModel.Portrait.AsPNG())
                    {
                        Byte[] myByteArray = new Byte[imageData.Length];
                        System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                        _resultReceived.ResultImage = myByteArray;
                    }
                }

                if (mrzResultDataModel.Signature != null)
                {
                    using (NSData imageData = mrzResultDataModel.Signature.AsPNG())
                    {
                        Byte[] myByteArray = new Byte[imageData.Length];
                        System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                        _resultReceived.ResultImage = myByteArray;
                    }
                }

                ((ScannerControl)Element).OnScanningFinished(_resultReceived);
            }
        }
        [Export("successfulDocumentScanWithImageResult:")]
        public void SuccessfulDocumentScanWithImageResult(UIImage resultImage)
        {
            if (Element != null)
            {
                ResultModel resultWithImage = new ResultModel();

                if (resultImage != null)
                {
                    using (NSData imageData = resultImage.AsPNG())
                    {
                        Byte[] myByteArray = new Byte[imageData.Length];
                        System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                        resultWithImage.ResultImage = myByteArray;
                    }
                }

                ((ScannerControl)Element).OnScanningFinished(resultWithImage);
            }
        }

        [Export("scannerWasDismissed")]
        public void ScannerWasDismissed()
        {
            if (Element != null)
                ((ScannerControl)Element).OnScannerDismissed();
        }

        [Export("permissionsWereDenied")]
        public void PermissionsWereDenied()
        {
            if (Element != null)
                ((ScannerControl)Element).OnScanningFinished(new ResultModel("Permissions were denied"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Element != null)
                    ((ScannerControl)Element).OnResumeScanning -= ScannerControlRenderer_OnResumeScanning;

                if (mrzSannerController != null)
                {
                    mrzSannerController.CloseScanner();
                    mrzSannerController.Dispose();
                    mrzSannerController = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}