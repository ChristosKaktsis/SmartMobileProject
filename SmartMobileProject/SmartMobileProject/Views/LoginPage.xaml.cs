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
            
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //await AppShell.Current.Navigation.PushAsync(new Views.ΚινήσειςΠελατώνViewPage());
            if (!(BindingContext is LoginViewModel model))
                return;
            if(!model.Remember)
            {
               // TrialPopUp.IsVisible = true;
                await Task.Delay(1000);
                
            }
        }
        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Search.Text))
            {
                SalerCollection.FilterString = string.Empty;
                return;
            }
            SalerCollection.FilterString = $"Contains(Ονοματεπώνυμο, '{Search.Text}')";
        }
    }
}