using SmartMobileProject.Models;
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
    public partial class ΕπιλογήΠροηγΠαρΠωλPage : ContentPage
    {
        private ΕπιλογήΠροηγΠαρΠωλViewModel _viewModel;

        public ΕπιλογήΠροηγΠαρΠωλPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ΕπιλογήΠροηγΠαρΠωλViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            var sd = sender as Button;
            var par = sd.BindingContext as ΠαραστατικάΠωλήσεων;
            _viewModel.LoadSelection(par);
        }
    }
}