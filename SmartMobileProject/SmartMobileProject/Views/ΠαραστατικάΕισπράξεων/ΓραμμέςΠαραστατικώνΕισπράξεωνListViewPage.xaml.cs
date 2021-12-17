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
    public partial class ΓραμμέςΠαραστατικώνΕισπράξεωνListViewPage : ContentPage
    {
        public ΓραμμέςΠαραστατικώνΕισπράξεωνListViewPage()
        {
            InitializeComponent();
        }
        protected async override void OnDisappearing()
        {
            base.OnAppearing();
           
            await axiografo.ScaleTo(0, 100);
            await logariasmos.ScaleTo(0, 100);
            axiografo.IsVisible = false;
            logariasmos.IsVisible = false;
            plusBtn.ImageSource = "add_white_20";
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {

            if (axiografo.IsVisible)
            {

                await axiografo.ScaleTo(0, 100);
                await logariasmos.ScaleTo(0, 100);
               
                axiografo.IsVisible = false;
                logariasmos.IsVisible = false;
                ((Button)sender).ImageSource = "add_white_20";
            }
            else
            {

                logariasmos.IsVisible = true;
                axiografo.IsVisible = true;
               
                await logariasmos.ScaleTo(1, 100);
                await axiografo.ScaleTo(1, 100);
                ((Button)sender).ImageSource = "outline_clear_white_20";
            }
        }
    }
}