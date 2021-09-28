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
    public partial class ΕίδοςΟικογένειαDetailViewPage : ContentPage
    {
        public ΕίδοςΟικογένειαDetailViewPage(object οικογενεια)
        {
            InitializeComponent();
            if (!(BindingContext is ΕίδοςΟικογένειαDetailViewModel model))
                return;
            if (οικογενεια != null)
            {
                model.ΟικογένειαΕίδους = (ΟικογένειαΕίδους)οικογενεια;
            }
        }
    }
}