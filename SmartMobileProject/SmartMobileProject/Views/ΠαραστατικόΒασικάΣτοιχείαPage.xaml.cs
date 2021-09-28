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

        private void ComboBoxDieuthParadosis_SelectionChanged(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
          
        }
    }
}