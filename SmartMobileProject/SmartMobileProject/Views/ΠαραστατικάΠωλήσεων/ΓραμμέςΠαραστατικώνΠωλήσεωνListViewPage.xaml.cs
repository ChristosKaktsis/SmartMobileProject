using DevExpress.XamarinForms.Editors;
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
    public partial class ΓραμμέςΠαραστατικώνΠωλήσεωνListViewPage : ContentPage
    {
        public ΓραμμέςΠαραστατικώνΠωλήσεωνListViewPage()
        {
            InitializeComponent();
            
        }
        protected async override void OnDisappearing()
        {
            base.OnAppearing();
            await ScanSelectionBtn.ScaleTo(0, 100);
            await BarCodeSelectionBtn.ScaleTo(0, 100);
            await NewlnBtn.ScaleTo(0, 100);
            await QuickSelectionBtn.ScaleTo(0, 100);
            //await ImageSelectionBtn.ScaleTo(0, 100);
            ScanSelectionBtn.IsVisible = false;
            BarCodeSelectionBtn.IsVisible = false;
            NewlnBtn.IsVisible = false;
            QuickSelectionBtn.IsVisible = false;
            //ImageSelectionBtn.IsVisible = false;
            BackgroundDisable.IsVisible = false;
            plusBtn.ImageSource = "add_white_20";
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
           
            if (NewlnBtn.IsVisible)
            {
                await ScanSelectionBtn.ScaleTo(0, 100);
                await BarCodeSelectionBtn.ScaleTo(0, 100);
                await NewlnBtn.ScaleTo(0, 100);
                await QuickSelectionBtn.ScaleTo(0, 100);
                //await ImageSelectionBtn.ScaleTo(0, 100);
                ScanSelectionBtn.IsVisible = false;
                BarCodeSelectionBtn.IsVisible = false;
                NewlnBtn.IsVisible = false;
                QuickSelectionBtn.IsVisible = false;
                //ImageSelectionBtn.IsVisible = false;
                BackgroundDisable.IsVisible = false;
                ((Button)sender).ImageSource = "add_white_20";
            }
            else
            {
                BackgroundDisable.IsVisible = true;
                ScanSelectionBtn.IsVisible = true;
                BarCodeSelectionBtn.IsVisible = true;
                NewlnBtn.IsVisible = true;
                QuickSelectionBtn.IsVisible = true;       
                //ImageSelectionBtn.IsVisible = true;
                //await ImageSelectionBtn.ScaleTo(1, 100);
                await QuickSelectionBtn.ScaleTo(1, 100);
                await NewlnBtn.ScaleTo(1, 100);
                await BarCodeSelectionBtn.ScaleTo(1, 100);
                await ScanSelectionBtn.ScaleTo(1, 100);
                ((Button)sender).ImageSource = "outline_clear_white_20";
            }
        }
    }
}