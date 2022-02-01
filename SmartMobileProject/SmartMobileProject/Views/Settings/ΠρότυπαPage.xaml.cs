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
    public partial class ΠρότυπαPage : ContentPage
    {
        public ΠρότυπαPage()
        {
            InitializeComponent();
        }

        private void grid_DoubleTap(object sender, DevExpress.XamarinForms.DataGrid.DataGridGestureEventArgs e)
        {
            if (e.Item != null)
            {
                var editForm = new ΠρότυπαDetailPage(e.Item);

                Navigation.PushAsync(editForm);
            }
        }
        protected override void OnAppearing()
        {
            if (!(BindingContext is ΠρότυπαViewModel model))
                return;
            model.Save();
            base.OnAppearing();
        }
    }
}