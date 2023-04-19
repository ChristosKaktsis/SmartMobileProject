
using SmartMobileProject.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivationPage : ContentPage
    {
        private ActivationViewModel _viewModel;

        public ActivationPage()
        {
            InitializeComponent();
           BindingContext = _viewModel = new ActivationViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}