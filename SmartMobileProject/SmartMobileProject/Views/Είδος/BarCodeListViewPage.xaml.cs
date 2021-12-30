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
    public partial class BarCodeListViewPage : ContentPage
    {
        private BarCodeListViewModel _viewModel;

        public BarCodeListViewPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new BarCodeListViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}