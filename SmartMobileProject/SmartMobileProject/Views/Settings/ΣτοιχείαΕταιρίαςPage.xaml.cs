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
    public partial class ΣτοιχείαΕταιρίαςPage : ContentPage
    {
        private SettingsStaticSourceViewModel _viewModel;

        public ΣτοιχείαΕταιρίαςPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SettingsStaticSourceViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadCompanyCommand.Execute(null);
        }
    }
}