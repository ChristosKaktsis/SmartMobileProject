using SmartMobileProject.Views.Settings;
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
    public partial class SettingsViewPage : ContentPage
    {
        public SettingsViewPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/PrintSettingsPage");
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΣτοιχείαΕταιρίαςPage");
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΣειρέςΠαρΕισπPage");
        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΠόληPage");
        }

        private async void Button_Clicked_4(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΤΚPage");
        }

        private async void Button_Clicked_5(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΔΟΥPage");
        }

        private async void Button_Clicked_6(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΕίδοςΟικογένειαPage");
        }

        private async void Button_Clicked_7(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΕίδοςΥποοικογένειαPage");
        }

        private async void Button_Clicked_8(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΕίδοςΟμάδαPage");
        }

        private async void Button_Clicked_9(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΕίδοςΚατηγορίεςPage");
        }

        private async void Button_Clicked_10(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new PreLoginPage());
        }

        private async void Button_Clicked_11(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΛογαριασμοίΧρημΔιαθPage");
        }
        private async void Button_Clicked_12(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΦΠΑPage");
        }

        private async void Button_Clicked_13(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΚαθαρισμόςPage");
        }

        private async void Button_Clicked_14(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΤρόποςΠληρωμήςPage");
        }

        private async void Button_Clicked_15(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Settings/ΤρόποςΑποστολήςPage");
        }
    }
}