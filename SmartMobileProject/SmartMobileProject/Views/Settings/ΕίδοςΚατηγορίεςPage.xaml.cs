using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XamarinForms.DataGrid;
using SmartMobileProject.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ΕίδοςΚατηγορίεςPage : ContentPage
    {
        public ΕίδοςΚατηγορίεςPage()
        {
            DevExpress.XamarinForms.DataGrid.Initializer.Init();
            InitializeComponent();
        }
        private void DataGridView_DoubleTap(object sender, DataGridGestureEventArgs e)
        {
            if (e.Item != null)
            {
                var editForm = new ΕίδοςΚατηγορίεςDetailViewPage(e.Item);

                Navigation.PushAsync(editForm);
            }
        }
        protected override void OnAppearing()
        {
            if (!(BindingContext is SettingsΕίδοςΚατηγορίεςViewModel model))
                return;
            model.Save();
            base.OnAppearing();
        }
    }
}