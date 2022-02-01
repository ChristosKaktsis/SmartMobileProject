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
    public partial class ΚαθαρισμόςPage : ContentPage
    {
        private ΚαθαρισμόςViewModel _viewModel;

        public ΚαθαρισμόςPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ΚαθαρισμόςViewModel();
        }
    }
}