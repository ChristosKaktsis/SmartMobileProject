using DevExpress.XamarinForms.DataGrid;
using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views.Settings
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ΤρόποςΑποστολήςPage : ContentPage
	{
		public ΤρόποςΑποστολήςPage ()
		{
			InitializeComponent ();
		}
        private void DataGridView_DoubleTap(object sender, DataGridGestureEventArgs e)
        {
            if (e.Item != null)
            {
                var editForm = new ΤρόποςΑποστολήςDetailViewPage(e.Item);

                Navigation.PushAsync(editForm);
            }
        }
        protected override void OnAppearing()
        {
            if (!(BindingContext is SettingsΤρόποςΑποστολήςViewModel model))
                return;
            model.Save();
            base.OnAppearing();
        }
    }
}