using SmartMobileProject.Services;
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
        }
        private async void Continue_Button_Clicked(object sender, System.EventArgs e)
        {
            await this.FadeTo(0, 200);
            this.IsVisible = false;
            //crypt test 
           //string crypto = AesOperation.EncryptString("b14ca5898a4e4133bbce2ea2315a1916", "Hello Crypto");
           // Console.WriteLine("Before Encrypt : Hello Crypto \n After Encrypt :" + crypto);
           // string decrypto = AesOperation.DecryptString("b14ca5898a4e4133bbce2ea2315a1916", crypto);
           // Console.WriteLine("After Decrypt :" + decrypto);
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