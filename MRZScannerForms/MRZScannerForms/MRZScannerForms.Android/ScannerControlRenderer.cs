using Android.Content;
using Android.Support.V4.App;
using Com.Scansolutions.Mrzscannerlib;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MRZScannerForms.ScannerControl), typeof(MRZScannerForms.Droid.ScannerControlRenderer))]
namespace MRZScannerForms.Droid
{
    public class ScannerControlRenderer : ViewRenderer
    {
        public ScannerControlRenderer(Context context) : base(context) { }

        MRZScanner mrzScanner;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            Android.Views.View _scannerLayout = ((FormsAppCompatActivity)Context).LayoutInflater.Inflate(Resource.Layout.activity_scanner, this, false);

            mrzScanner = (MRZScanner)((FormsAppCompatActivity)Context).SupportFragmentManager.FindFragmentById(Resource.Id.scannerFragment);

            // Options: [ScannerTypeMrz, ScannerTypeDocImageId, ScannerTypeDocImagePassport]. Default: MRZ.
            switch (((ScannerControl)Element).ScannerType)
            {
                case ScanType.Mrz:
                    mrzScanner.SetScannerType(ScannerType.ScannerTypeMrz);
                    break;
                case ScanType.DocImageId:
                    mrzScanner.SetScannerType(ScannerType.ScannerTypeDocImageId);
                    break;
                case ScanType.DocImagePassport:
                    mrzScanner.SetScannerType(ScannerType.ScannerTypeDocImagePassport);
                    break;
                case ScanType.DocImageIdFront:
                    mrzScanner.SetScannerType(ScannerType.ScannerTypeDocImageIdFront);
                    break;
                case ScanType.IdSession:
                    mrzScanner.SetScannerType(ScannerType.ScannerTypeIdSession);
                    break;
                default:
                    mrzScanner.SetScannerType(ScannerType.ScannerTypeMrz);
                    break;
            }
            // Enable/disable the ID document type. Default: true.
            MRZScanner.SetIDActive(((ScannerControl)Element).IDActive);
            // Enable/disable the Passport document type. Default: true.
            MRZScanner.SetPassportActive(((ScannerControl)Element).PassportActive);
            // Enable/disable the Visa document type. Default: true.
            MRZScanner.SetVisaActive(((ScannerControl)Element).VisaActive);
            // Set the max CPU threads that the scanner can use. Default: 2.
            MRZScanner.SetMaxThreads(((ScannerControl)Element).MaxThreads);
            MRZScanner.SetExtractFullPassportImageEnabled(((ScannerControl)Element).ExtractFullPassportImageEnabled);
            MRZScanner.SetExtractIdBackImageEnabled(((ScannerControl)Element).ExtractIdBackImageEnabled);
            MRZScanner.SetExtractPortraitEnabled(((ScannerControl)Element).ExtractPortraitEnabled);
            MRZScanner.SetExtractSignatureEnabled(((ScannerControl)Element).ExtractSignatureEnabled);

            // Set license key
            MRZScanner.RegisterWithLicenseKey(Context, ((ScannerControl)Element).LicenseKey, (IMRZLicenceResultListener)Context);

            if (Control == null)
                SetNativeControl(_scannerLayout);

            MainActivity.elementScanner = (ScannerControl)Element;

            ((ScannerControl)Element).OnResumeScanning += ScannerControlRenderer_OnResumeScanning;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Element != null)
                    ((ScannerControl)Element).OnResumeScanning -= ScannerControlRenderer_OnResumeScanning;

                try
                {
                    Fragment fragment = ((FormsAppCompatActivity)Context).SupportFragmentManager.FindFragmentById(Resource.Id.scannerFragment);
                    if (fragment != null)
                        ((FormsAppCompatActivity)Context).SupportFragmentManager.BeginTransaction().Remove(fragment).Commit();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

                MainActivity.elementScanner = null;
                mrzScanner = null;
            }

            base.Dispose(disposing);
        }

        private void ScannerControlRenderer_OnResumeScanning(object sender, EventArgs e)
        {
            mrzScanner.ResumeScanning();
        }
    }
}
