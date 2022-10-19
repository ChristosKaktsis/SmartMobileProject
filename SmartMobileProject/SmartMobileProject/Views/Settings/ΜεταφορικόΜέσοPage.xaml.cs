using DevExpress.XamarinForms.DataGrid;
using SmartMobileProject.ViewModels;
using SmartMobileProject.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ΜεταφορικόΜέσοPage : ContentPage
    {
        public ΜεταφορικόΜέσοPage()
        {
            InitializeComponent();
            BindingContext = new SettingsΜΜViewModel();
        }
        private async void DataGridView_DoubleTap(object sender, DataGridGestureEventArgs e)
        {
            if (e.Item != null)
            {
                var editForm = new ΜεταφορικόDetailViewPage(e.Item);
                await Navigation.PushAsync(editForm);
            }
        }
        protected override void OnAppearing()
        {
            if (!(BindingContext is  SettingsΜΜViewModel model))
                return;
            model.Save();
            base.OnAppearing();
        }
    }
}