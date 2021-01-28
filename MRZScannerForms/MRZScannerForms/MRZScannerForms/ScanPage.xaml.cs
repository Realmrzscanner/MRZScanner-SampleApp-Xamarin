using System;
using System.Diagnostics;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MRZScannerForms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        Page mainPage;

        public ScanPage(ScanType type, Page mainPage)
        {
            InitializeComponent();

            scannerControl.ScannerType = type;
            this.mainPage = mainPage;

            if (Device.RuntimePlatform == Device.Android)
                scannerControl.LicenseKey = "AndroidLicenseKey";
            else if (Device.RuntimePlatform == Device.iOS)
                scannerControl.LicenseKey = "iOSLicenseKey";
        }

        private async void scannerControl_ScanningFinished(object sender, ResultModel resultModel)
        {
            if (scannerControl.ScannerType == ScanType.Mrz)
            {
                if (resultModel.Error == null)
                {
                    if (await DisplayAlert(resultModel.FullName, resultModel.DocumentNumber, "Resume", "Close"))
                        scannerControl.ResumeScanning();
                    else
                    {
                        if (resultModel.IdBack != null)
                            ((MainPage)mainPage).imgResult.Source = ImageSource.FromStream(() => new MemoryStream(resultModel.IdBack));

                        await Navigation.PopAsync(true);
                    }
                }
                else
                {
                    await Navigation.PopAsync(true);
                }
            }
            else
            {
                if (resultModel.ResultImage != null)
                    ((MainPage)mainPage).imgResult.Source = ImageSource.FromStream(() => new MemoryStream(resultModel.ResultImage));

                await Navigation.PopAsync(true);
            }
        }

        private void scannerControl_LicenseResult(object sender, LicenseResultType result)
        {
            Debug.WriteLine("License result: " + result.ToString());
        }

        //Executed only on iOS platform when embedded close button is pressed
        private async void scannerControl_ScannerDismissed(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }
    }
}