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
    public partial class ΔΟΥDetailViewPage : ContentPage
    {
        public ΔΟΥDetailViewPage(object doy)
        {
            InitializeComponent();
            if (!(BindingContext is ΔΟΥDetailViewModel model))
                return;
            if (doy != null)
            {
                model.ΔΟΥ = (ΔΟΥ)doy;
            }
        }
    }
}