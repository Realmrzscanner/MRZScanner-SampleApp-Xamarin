// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MRZScanneriOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView scannedImage { get; set; }

        [Action ("startPassportImageCapture:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void startPassportImageCapture (UIKit.UIButton sender);

        [Action ("startScanner:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void startScanner (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (scannedImage != null) {
                scannedImage.Dispose ();
                scannedImage = null;
            }
        }
    }
}