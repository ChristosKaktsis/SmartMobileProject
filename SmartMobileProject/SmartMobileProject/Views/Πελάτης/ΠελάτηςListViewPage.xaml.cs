
using DevExpress.XamarinForms.DataGrid;
using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ΠελάτηςListViewPage : ContentPage
    {
        ΠελάτηςViewModel model;
        int NavigationStack;
        public ΠελάτηςListViewPage()
        {
            InitializeComponent();
            model = BindingContext as ΠελάτηςViewModel;
            this.SearchType.ItemsSource = new List<string> { "ΑΦΜ","Επωνυμία"};
            this.SearchType.SelectedIndex = 1;
            this.Search.TextChanged += Search_TextChanged;
           // grid.FilterExpression property 
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(this.Search.Text))
            {
                this.grid.ClearFilter();
                return;
            }
            if(SearchType.SelectedIndex == 1)
            {
                this.grid.FilterString = "Contains(Επωνυμία, '" + this.Search.Text + "')";
            }

            if (SearchType.SelectedIndex == 0)
            {
                this.grid.FilterString = "Contains(ΑΦΜ, '" + this.Search.Text + "')";
            }


        }

        protected override void OnAppearing()
        {
            model.Ανανέωση.Execute(null);
            base.OnAppearing();
            NavigationStack = Navigation.NavigationStack.Count;
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