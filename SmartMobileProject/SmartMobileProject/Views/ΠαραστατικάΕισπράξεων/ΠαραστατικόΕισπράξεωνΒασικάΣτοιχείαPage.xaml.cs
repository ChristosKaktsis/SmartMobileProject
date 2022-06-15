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
        public ΠαραστατικόΕισπράξεωνΒασικάΣτοιχείαPage()
        {
            InitializeComponent();
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