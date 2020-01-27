using CoreGraphics;
using Foundation;
using MRZScanneriOSBinding;
using System;
using UIKit;

namespace MRZScanneriOS
{
    public partial class ViewController : UIViewController, IMRZScannerDelegate
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        MRZScannerController mrzScannerController;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void startScanner(UIButton sender)
        {
            scannedImage.Image = null;

            mrzScannerController = new MRZScannerController();

            // Options: [Mrz, DocumentImage, DocumentImagePassport]. Default: Mrz.
            mrzScannerController.SetScannerType(MRZScannerType.Mrz);
            // Enable/disable the ID document type. Default: true.
            MRZScannerController.SetIDActive(true);
            // Enable/disable the Passport document type. Default: true.
            MRZScannerController.SetPassportActive(true);
            // Enable/disable the Visa document type. Default: true.
            MRZScannerController.SetVisaActive(true);
            // Set the max CPU threads that the scanner can use. Default: 2.
            mrzScannerController.SetMaxCPUCores(2);
            // Set license key
            MRZScannerController.RegisterLicenseWithKey("License_key");

            mrzScannerController.WeakDelegate = this;

            UIViewController currentViewController = NavigationController;
            if (currentViewController == null)
                currentViewController = this;

            currentViewController.AddChildViewController(mrzScannerController);
            mrzScannerController.InitUI(currentViewController);
        }

        partial void startPassportImageCapture(UIButton sender)
        {
            scannedImage.Image = null;

            mrzScannerController = new MRZScannerController();

            // Options: [Mrz, DocumentImage, DocumentImagePassport]. Default: Mrz.
            mrzScannerController.SetScannerType(MRZScannerType.DocumentImagePassport);
            // Enable/disable the ID document type. Default: true.
            MRZScannerController.SetIDActive(true);
            // Enable/disable the Passport document type. Default: true.
            MRZScannerController.SetPassportActive(true);
            // Enable/disable the Visa document type. Default: true.
            MRZScannerController.SetVisaActive(true);
            // Set the max CPU threads that the scanner can use. Default: 2.
            mrzScannerController.SetMaxCPUCores(2);
            // Set license key
            MRZScannerController.RegisterLicenseWithKey("License_key");

            mrzScannerController.WeakDelegate = this;

            UIViewController currentViewController = NavigationController;
            if (currentViewController == null)
                currentViewController = this;

            currentViewController.AddChildViewController(mrzScannerController);
            mrzScannerController.InitUI(currentViewController);
        }

        [Export("successfulScanWithResult:")]
        public void SuccessfulScanWithResult(MRZResultDataModel mrzResultDataModel)
        {
            UIAlertController alertController = UIAlertController.Create(mrzResultDataModel.FullName, mrzResultDataModel.Document_number, UIAlertControllerStyle.Alert);

            alertController.AddAction(UIAlertAction.Create("Close", UIAlertActionStyle.Cancel, (obj) =>
            {
                if (mrzScannerController != null)
                {
                    mrzScannerController.CloseScanner();
                    mrzScannerController = null;
                }
            }));

            if (mrzScannerController != null)
            {
                alertController.AddAction(UIAlertAction.Create("Resume", UIAlertActionStyle.Default, (obj) =>
                {
                    mrzScannerController.ResumeScanner();
                }));
            }

            PresentViewController(alertController, true, null);
        }

        [Export("successfulDocumentScanWithImageResult:")]
        public void SuccessfulDocumentScanWithImageResult(UIImage resultImage)
        {
            scannedImage.Image = resultImage;
        }
    }
}
