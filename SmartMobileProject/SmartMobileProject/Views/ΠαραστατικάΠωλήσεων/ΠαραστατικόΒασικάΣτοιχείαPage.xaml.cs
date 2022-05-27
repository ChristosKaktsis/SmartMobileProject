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
    public partial class ΠαραστατικόΒασικάΣτοιχείαPage : ContentPage
    {
        public ΠαραστατικόΒασικάΣτοιχείαPage()
        {
            InitializeComponent();
            ComboBoxDieuthParadosis.SelectionChanged += ComboBoxDieuthParadosis_SelectionChanged;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!(BindingContext is ΠαραστατικόDetailViewModel model))
                return;
            model.OnAppearing();
        }
        private void ComboBoxDieuthParadosis_SelectionChanged(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
          
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Search.Text))
            {
                CustomersCollection.FilterString = string.Empty;
                return;
            }
            int afm = 0;
            if(int.TryParse(Search.Text,out afm))
                CustomersCollection.FilterString = $"Contains(ΑΦΜ, '{Search.Text}')";
            else
                CustomersCollection.FilterString = $"Contains(DisplayName, '{Search.Text}')";
        }
    }
}