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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        { 
            InitializeComponent();
            TrialPopUp.Opacity = 0;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //await AppShell.Current.Navigation.PushAsync(new Views.ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodePage());
            if (!(BindingContext is LoginViewModel model))
                return;
            if(!model.Remember)
            {
               // TrialPopUp.IsVisible = true;
                await Task.Delay(1000);
                await TrialPopUp.FadeTo(1, 200);
            }
        }
    }
}