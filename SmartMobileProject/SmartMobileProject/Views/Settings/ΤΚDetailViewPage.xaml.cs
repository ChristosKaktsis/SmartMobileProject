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
    public partial class ΤΚDetailViewPage : ContentPage
    {
        public ΤΚDetailViewPage(object tk)
        {
            InitializeComponent();
            if (!(BindingContext is ΤΚDetailViewModel model))
                return;
            if (tk != null)
            {
                model.ΤΚ = (ΤαχυδρομικόςΚωδικός)tk;
            }
        }
    }
}