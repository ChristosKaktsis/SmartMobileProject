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
    public partial class ΚινήσειςΠελατώνViewPage : ContentPage
    {
        private ΚινήσειςΠελατώνViewModel _viewModel;

        public ΚινήσειςΠελατώνViewPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ΚινήσειςΠελατώνViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}