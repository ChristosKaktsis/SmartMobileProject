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
    public partial class ΠαραστατικόΕισπράξεωνΒασικάΣτοιχείαPage : ContentPage
    {
        private ΠαραστατικόDetailViewModel _viewModel;

        public ΠαραστατικόΕισπράξεωνΒασικάΣτοιχείαPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ΠαραστατικάΕισπράξεωνDetailViewModel viewModel)
                viewModel.OnAppearing();
        }
        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Search.Text))
            {
                CustomersCollection.FilterString = string.Empty;
                return;
            }
            int afm = 0;
            if (int.TryParse(Search.Text, out afm))
                CustomersCollection.FilterString = $"Contains(ΑΦΜ, '{Search.Text}')";
            else
                CustomersCollection.FilterString = $"Contains(DisplayName, '{Search.Text}')";
        }
    }
}