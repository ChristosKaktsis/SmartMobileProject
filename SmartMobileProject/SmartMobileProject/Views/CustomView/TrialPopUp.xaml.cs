using SmartMobileProject.Services;
using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views.CustomView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrialPopUp : ContentView
    {
        public TrialPopUp()
        {
            InitializeComponent();
            BindingContext = new TrialPopUpViewModel();
            
        }
        private async void Continue_Button_Clicked(object sender, System.EventArgs e)
        {
            await this.FadeTo(0, 200);
            this.IsVisible = false;
             
            
            if (!(BindingContext is TrialPopUpViewModel model))
                return;
            model.ContinuePressed();
        }
        private async void Frame1_Tapped(object sender, System.EventArgs e)
        {
            Frame1.BorderColor = Color.Green;
            Frame1.BackgroundColor = Color.LightGreen;
            Frame2.BorderColor = Color.Gray;
            Frame2.BackgroundColor = Color.LightGray;
            await Frame1.ScaleTo(1, 20);
            await Frame2.ScaleTo(0.9, 20);
            if (!(BindingContext is TrialPopUpViewModel model))
                return;  
            model.TrialButton();

            
        }
        private async void Frame2_Tapped(object sender, System.EventArgs e)
        {
            Frame2.BorderColor = Color.Green;
            Frame2.BackgroundColor = Color.LightGreen;
            Frame1.BorderColor = Color.Gray;
            Frame1.BackgroundColor = Color.LightGray;
            await Frame2.ScaleTo(1, 20);
            await Frame1.ScaleTo(0.9, 20);
            if (!(BindingContext is TrialPopUpViewModel model))
                return;
            model.ActivationButton();
        }
    }
}