
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivationPage : ContentPage
    {
        public ActivationPage()
        {
            InitializeComponent();
            PopUp.Scale = 0;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            BackGreyColor.IsVisible = true;
            PopUp.IsVisible = true;
            
            await PopUp.ScaleTo(1, 200);
        }

        private async void Continue_Button_Clicked(object sender, System.EventArgs e)
        {
            await PopUp.ScaleTo(0, 200);
            
            BackGreyColor.IsVisible = false;
            PopUp.IsVisible = false;
        }

        private async void Frame1_Tapped(object sender, System.EventArgs e)
        {
            Frame1.BorderColor = Color.Green;
            Frame1.BackgroundColor = Color.LightGreen;
            Frame2.BorderColor = Color.Gray;
            Frame2.BackgroundColor = Color.LightGray;
            await Frame1.ScaleTo(1, 20);
            await Frame2.ScaleTo(0.9, 20);
        }
        private async void Frame2_Tapped(object sender, System.EventArgs e)
        {
            Frame2.BorderColor = Color.Green;
            Frame2.BackgroundColor = Color.LightGreen;
            Frame1.BorderColor = Color.Gray;
            Frame1.BackgroundColor = Color.LightGray;
            await Frame2.ScaleTo(1, 20);
            await Frame1.ScaleTo(0.9, 20);
        }
    }
}