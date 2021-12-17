using DevExpress.XamarinForms.CollectionView;
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
    public partial class ΕργασίεςDetailViewPage : ContentPage
    {
        ΕργασίεςDetailViewModel model;
        public ΕργασίεςDetailViewPage()
        {
            InitializeComponent();
            model = BindingContext as ΕργασίεςDetailViewModel;
            if (model.Εργασία.Κατάσταση)
            {
                ΕνέργειαButton.IsEnabled = false;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            
            if (energeiesCollection.IsVisible)
            {
                await energeiesCollection.ScaleTo(0, 100);
                energeiesCollection.IsVisible = false;
            }
            else
            {
                energeiesCollection.IsVisible = true;
                await energeiesCollection.ScaleTo(1, 100);
            }
        }
        private void SwipeItem_Invoked_Edit(object sender, SwipeItemTapEventArgs e)
        {
            model.Επεξεργασία.Execute(e.Item);

        }
    }
}