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
    public partial class ΠρότυπαDetailPage : ContentPage
    {
        public ΠρότυπαDetailPage(object city)
        {
            InitializeComponent();
            if (!(BindingContext is ΠρότυπαDetailViewModel model))
                return;
            if (city != null)
            {
                model.Oid = (city as Πρότυπα).Oid;
            }
        }
    }
}