using System;

using Xamarin.Forms;

namespace MRZScannerForms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnStartScanning_Clicked(object sender, EventArgs e)
        {
            imgResult.Source = "";
            await Navigation.PushAsync(new ScanPage(ScanType.Mrz, this), true);
        }

        private async void btnCaptureImage_Clicked(object sender, EventArgs e)
        {
            imgResult.Source = "";
            await Navigation.PushAsync(new ScanPage(ScanType.DocImagePassport, this), true);
        }
    }
}
