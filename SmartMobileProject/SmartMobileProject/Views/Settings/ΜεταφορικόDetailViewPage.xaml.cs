using SmartMobileProject.Models;
using SmartMobileProject.ViewModels.Settings;
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
    public partial class ΜεταφορικόDetailViewPage : ContentPage
    {
        public ΜεταφορικόDetailViewPage(object transport)
        {
            InitializeComponent();
            if (!(BindingContext is ΜεταφορικόDetailViewModel model))
                return;
            if (transport != null)
            {
                model.ΜΜ = (ΜεταφορικόΜέσο)transport;
            }
        }
    }
}