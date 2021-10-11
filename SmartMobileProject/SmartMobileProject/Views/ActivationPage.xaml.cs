
using SmartMobileProject.ViewModels;
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
            PopUp.Opacity = 0;
            ActivationCodeEntry.Opacity = 0;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            PopUp.IsVisible = true;
            await PopUp.FadeTo(1, 200);
        }

        private async void Activate_Button_Clicked(object sender, System.EventArgs e)
        {
            BackGreyColor.IsVisible = true;
            ActivationCodeEntry.IsVisible = true;
            await ActivationCodeEntry.FadeTo(1, 200);
        }

        private async void Code_Button_Clicked(object sender, System.EventArgs e)
        {
            BackGreyColor.IsVisible = false;
            ActivationCodeEntry.IsVisible = false;
            await ActivationCodeEntry.FadeTo(0, 200);
            if (!(BindingContext is ActivationViewModel model))
                return;
             model.CheckActivationCode();
        }
    }
}