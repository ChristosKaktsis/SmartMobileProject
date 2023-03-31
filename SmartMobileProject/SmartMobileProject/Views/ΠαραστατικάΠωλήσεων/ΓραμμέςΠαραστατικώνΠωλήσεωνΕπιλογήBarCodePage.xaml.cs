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
    public partial class ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodePage : ContentPage
    {
        private ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodeViewModel _viewModel;

        public ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodeViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            ChangeFocus();
        }

        private async void ChangeFocus()
        {
            await Task.Delay(100);//delay because you have to wait for the element to render!!!
            Search.Focus();
        }

        private void Search_Unfocused(object sender, FocusEventArgs e)
        {
            if (!_viewModel.OneOne) return;
            ChangeFocus();
        }
    }
}