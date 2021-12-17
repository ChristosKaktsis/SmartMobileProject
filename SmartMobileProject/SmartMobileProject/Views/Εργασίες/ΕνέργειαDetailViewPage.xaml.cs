using SmartMobileProject.Models;
using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ΕνέργειαDetailViewPage : ContentPage
    {
        ΕνέργειαDetailViewModel model;
        public ΕνέργειαDetailViewPage()
        {
            InitializeComponent();
            model = BindingContext as ΕνέργειαDetailViewModel;
            if (model.Εργασία.Κατάσταση)
            {
                addimgbtn.IsEnabled = false;
                ΤύποςEdit.IsEnabled = false;
                ΠεριγραφήEdit.IsEnabled = false;
                ΑριθμόςEdit.IsEnabled = false;
                CheckEdit.IsEnabled = false;
                ΕπιλογήEdit.IsEnabled = false;
                ΗμEdit.IsEnabled = false;
            }
        }
        private async void TakeImage(object sender, EventArgs e)
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync();
                model.SetImage(result.FullPath);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        private void ΤύποςEdit_SelectionChanged(object sender, EventArgs e)
        {
            var val = (ΙδιότηταΕνέργειας)ΤύποςEdit.SelectedValue;
            switch (val.Τύποςιδιότητας)
            {
                case 0 :
                    ΠεριγραφήEdit.IsVisible = true;
                    CheckEdit.IsVisible = false;
                    ΕπιλογήEdit.IsVisible = false;
                    ΗμEdit.IsVisible = false;
                    ΑριθμόςEdit.IsVisible = false;
                    break;
                case 1 :
                    ΑριθμόςEdit.IsVisible = true;
                    ΠεριγραφήEdit.IsVisible = false;
                    CheckEdit.IsVisible = false;
                    ΕπιλογήEdit.IsVisible = false;
                    ΗμEdit.IsVisible = false;
                    break;
                case 2:
                    ΗμEdit.IsVisible = true;
                    ΑριθμόςEdit.IsVisible = false;
                    ΠεριγραφήEdit.IsVisible = false;
                    CheckEdit.IsVisible = false;
                    ΕπιλογήEdit.IsVisible = false;
                    break;
                case 3:
                    ΕπιλογήEdit.IsVisible = true;
                    ΗμEdit.IsVisible = false;
                    ΑριθμόςEdit.IsVisible = false;
                    ΠεριγραφήEdit.IsVisible = false;
                    CheckEdit.IsVisible = false;
                    break;
                case 4:
                    CheckEdit.IsVisible = true;
                    ΕπιλογήEdit.IsVisible = false;
                    ΗμEdit.IsVisible = false;
                    ΑριθμόςEdit.IsVisible = false;
                    ΠεριγραφήEdit.IsVisible = false;
                    break;
            }
        }
    }
}