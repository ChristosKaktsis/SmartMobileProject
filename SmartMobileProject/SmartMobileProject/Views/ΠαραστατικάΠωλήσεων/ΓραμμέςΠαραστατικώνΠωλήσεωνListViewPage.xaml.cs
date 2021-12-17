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
            await NewlnBtn.ScaleTo(0, 100);
            await QuickSelectionBtn.ScaleTo(0, 100);
            ScanSelectionBtn.IsVisible = false;
            NewlnBtn.IsVisible = false;
            QuickSelectionBtn.IsVisible = false;
            plusBtn.ImageSource = "add_white_20";
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
           
            if (NewlnBtn.IsVisible)
            {
                await ScanSelectionBtn.ScaleTo(0, 100);
                await NewlnBtn.ScaleTo(0, 100);
                await QuickSelectionBtn.ScaleTo(0, 100);
                ScanSelectionBtn.IsVisible = false;
                NewlnBtn.IsVisible = false;
                QuickSelectionBtn.IsVisible = false;
                ((Button)sender).ImageSource = "add_white_20";
            }
            else
            {
                ScanSelectionBtn.IsVisible = true;
                NewlnBtn.IsVisible = true;
                QuickSelectionBtn.IsVisible = true;
                await QuickSelectionBtn.ScaleTo(1, 100);
                await NewlnBtn.ScaleTo(1, 100);
                await ScanSelectionBtn.ScaleTo(1, 100);
                ((Button)sender).ImageSource = "outline_clear_white_20";
            }
        }
    }
}