using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerViewPage : ContentPage
    {
        ScannerViewModel model;
        public ScannerViewPage()
        {
            InitializeComponent();
            model = BindingContext as ScannerViewModel;
        }
        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                model.Scanresult = result.Text;
                Result.Text = result.Text + "(Το Ειδος Δεν Βρέθηκε)";
            });
        }
    }
}