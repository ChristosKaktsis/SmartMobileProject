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
    public partial class ΦΠΑDetailViewPage : ContentPage
    {
        public ΦΠΑDetailViewPage(object φπα)
        {
            InitializeComponent();
            if (!(BindingContext is ΦΠΑDetailViewModel model))
                return;
            if (φπα != null)
            {
                model.ΦΠΑ = (ΦΠΑ)φπα;
            }
        }
    }
}