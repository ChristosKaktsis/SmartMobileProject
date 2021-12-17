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
    public partial class TestΠελάτηςDetailViewPage : ContentPage
    {
        public TestΠελάτηςDetailViewPage()
        {
            InitializeComponent();
             
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
    /*
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
    */
}