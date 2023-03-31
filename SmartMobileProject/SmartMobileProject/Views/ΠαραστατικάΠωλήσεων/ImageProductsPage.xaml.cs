using DevExpress.XamarinForms.Editors;
using SmartMobileProject.Models;
using SmartMobileProject.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageProductsPage : ContentPage
    {
        private ImageProductViewModel _viewModel;

        public ImageProductsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ImageProductViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(search_edit.Text))
            {
                products_collection.FilterString = string.Empty;
                return;
            }
            int afm = 0;
            if (int.TryParse(search_edit.Text, out afm))
                products_collection.FilterString = $"Contains(Product.Κωδικός, '{search_edit.Text}')";
            else
                products_collection.FilterString = $"Contains(Product.Περιγραφή, '{search_edit.Text}')";
        }
        
        private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
        {
            products_collection.FilterString = string.Empty;
            if (((ComboBoxEdit)sender).SelectedValue == null)
            {
                return;
            }
            this.products_collection.FilterString = "Contains(Product.Οικογένεια.Περιγραφή , '" + ((ΟικογένειαΕίδους)((ComboBoxEdit)sender).SelectedValue).Περιγραφή + "')";
        }
        private void filter_btn_Clicked(object sender, EventArgs e)
        {
            filter_popup.IsOpen = !filter_popup.IsOpen;
        }
    }
}