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
    public partial class ΠόληςDetailViewPage : ContentPage
    {
        public ΠόληςDetailViewPage(object city)
        {
            InitializeComponent();
            if (!(BindingContext is ΠόληDetailViewModel model))
                return;
            if (city != null)
            {
                model.Πόλη = (Πόλη)city;
            }
        }
    }
}