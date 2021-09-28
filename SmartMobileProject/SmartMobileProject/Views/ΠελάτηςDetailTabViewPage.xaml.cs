using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ΠελάτηςDetailTabViewPage : TabbedPage
    {
        //ΠελάτηςDetailViewModel model;
        public ΠελάτηςDetailTabViewPage()
        {
            InitializeComponent();
           // model = BindingContext as ΠελάτηςDetailViewModel;
        }
        protected override void OnDisappearing()
        {
            //αμα το IsFromEdit =  true δεν θα τρεξει 
            //Ειναι απαραιτητη στο Create new customer γιατι σε περιπτωση που δεν θελει 
            //ο χρηστης να γινει αποθηκευση θα πρεπει να γινει διαγραφη του new Πελατης(UOW)
            //Διαφορετικα θα υπαρχει στο uow και οταν γινει commit θα γραψει εναν κενο Πελατη
           
            base.OnDisappearing();
        }

    }
    class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                return null;
            }
            if (value == null)
            {
                return false;
            }
            if (!(value is string stringValue))
            {
                return null;
            }
            return !string.IsNullOrEmpty(stringValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}