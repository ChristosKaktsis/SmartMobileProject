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
    public partial class ΣειρέςΠαραστατικώνΕισπράξεωνDetailViewPage : ContentPage
    {
        public ΣειρέςΠαραστατικώνΕισπράξεωνDetailViewPage(object σειρά)
        {
            InitializeComponent();
            if (!(BindingContext is ΣειρέςΠαραστατικώνΕισπράξεωνViewModel model))
                return;
            if (σειρά != null)
            {
                var seira = (ΣειρέςΠαραστατικώνΕισπράξεων)σειρά;
                Πρόθεμα.Text = seira.Σειρά;
                Περιγραφή.Text = seira.Περιγραφή;
                model.Σειρά = seira;
            }
        }
    }
}