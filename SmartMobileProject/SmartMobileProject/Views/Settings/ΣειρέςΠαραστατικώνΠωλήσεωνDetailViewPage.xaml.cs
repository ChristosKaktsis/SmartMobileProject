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
    public partial class ΣειρέςΠαραστατικώνΠωλήσεωνDetailViewPage : ContentPage
    {
        public ΣειρέςΠαραστατικώνΠωλήσεωνDetailViewPage(object σειρά)
        {
            InitializeComponent();
            
            if (!(BindingContext is ΣειρέςΠαραστατικώνΠωλήσεωνViewModel model))
                return;
            if (σειρά != null)
            {
                var seira = (ΣειρέςΠαραστατικώνΠωλήσεων)σειρά;
                Πρόθεμα.Text = seira.Σειρά;
                Περιγραφή.Text = seira.Περιγραφή;
                ΣκοπόςΔιακίνησης.Text = seira.ΣκοπόςΔιακίνησης;
                Μετρητής.Text = seira.Counter.ToString();
                Λιανική.IsChecked = seira.Λιανική;
                //PrintType.SelectedItem = seira.PrintType;
                model.Σειρά = seira;
            }
            
        }
    }
}