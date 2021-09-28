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
    public partial class ΕίδοςListViewPage : ContentPage
    {
        ΕίδοςViewModel model;
        public ΕίδοςListViewPage()
        {
            InitializeComponent();
            model = BindingContext as ΕίδοςViewModel;
            this.SearchType.ItemsSource = new List<string> { "Κωδικός", "Περιγραφή" };
            this.SearchType.SelectedIndex = 1;
            this.Search.TextChanged += Search_TextChanged;
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            this.grid.GroupsInitiallyExpanded = true;
            if (string.IsNullOrEmpty(this.Search.Text))
            {
                this.grid.ClearFilter();
                this.grid.GroupsInitiallyExpanded = false;
                this.grid.ClearGrouping();
                this.grid.GroupBy("Οικογένεια.Περιγραφή");
                return;
            }
            if (SearchType.SelectedIndex == 1)
            {
                this.grid.FilterString = "Contains(Περιγραφή, '" + this.Search.Text + "')";
            }

            if (SearchType.SelectedIndex == 0)
            {
                this.grid.FilterString = "Contains(Κωδικός, '" + this.Search.Text + "')";
            }
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