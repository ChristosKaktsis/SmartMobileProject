using DevExpress.XamarinForms.DataGrid;
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
    public partial class ΠαραστατικάΕισπράξεωνListViewPage : ContentPage
    {
        ΠαρασταικάΕισπράξεωνViewModel model;
        public ΠαραστατικάΕισπράξεωνListViewPage()
        {
            InitializeComponent();
            model = BindingContext as ΠαρασταικάΕισπράξεωνViewModel;
        }
        protected override void OnAppearing()
        {
            model.Reload.Execute(null);
            base.OnAppearing();
        }
        private void grid_DoubleTap(object sender, DataGridGestureEventArgs e)
        {
            if (e.Item != null)
            {
                var editForm = new EditFormPage(grid, grid.GetItem(e.RowHandle));
                editForm.ToolbarItems.Clear();
                Navigation.PushAsync(editForm);
            }

        }
    }
}