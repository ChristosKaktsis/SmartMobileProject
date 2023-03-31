using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels.ΠΠViewModels.TemplatesViewModel
{
    public class ImageItemViewModel : BaseViewModel
    {
        public Command AddQuantCommand { get; }
        public Command RemoveQuantCommand { get; }
        public ImageItemViewModel() 
        {
            AddQuantCommand = new Command(() => Ποσότητα++);
            RemoveQuantCommand = new Command(() => Ποσότητα--);
        }
        private int _Ποσότητα;
        public int Ποσότητα
        {
            get => _Ποσότητα;
            set
            {
                SetProperty(ref _Ποσότητα, value);
                ChangeValue();
            }
        }
        private double τιμή;
        public double Τιμή
        {
            get { return τιμή; }
            set { SetProperty(ref τιμή, value); }
        }
        private Είδος _Product ;
        public Είδος Product 
        {
            get { return _Product; }
            set 
            { 
                _Product = value;
                if (value == null) return;
                Τιμή = setPriceOfLine(value);
                ΠοσοστόΦπα = value.ΦΠΑ != null ? (value.ΦΠΑ.Φπακανονικό / 100) : 0;
                Ποσότητα =(int)value.Ποσότητα;
                Εκπτωση = 0;
                ChangeValue();
            }
        }
        /// <summary>
        /// Επιστρέφει τιμή λιανικής ή χονδρικής του είδους ανάλογα την σειρά
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double setPriceOfLine(Είδος value)
        {
            if (value == null)
                return 0;
            var currentparastatiko = DocHelperViewModel.Order;
            if (currentparastatiko == null)
                return 0;
            if (currentparastatiko.Σειρά == null)
                return 0;
            return value.getPrice(currentparastatiko.Σειρά.Λιανική);
        }
        public decimal ΑξίαΕκπτωσης { get; set; }
        public float ΠοσοστόΦπα { get; set; }
        public float Εκπτωση { get; set; }
        public decimal ΚαθαρήΑξία { get; set; }
        public decimal Φπα { get; set; }
        public decimal ΑξίαΓραμμής { get; set; }
        private void ChangeValue()
        {
            double clearvalue = CalculateClearValue(Τιμή, ΠοσοστόΦπα);
            ΑξίαΕκπτωσης = (decimal)(Ποσότητα * clearvalue * Εκπτωση);
            ΚαθαρήΑξία = (decimal)(Ποσότητα * clearvalue) - ΑξίαΕκπτωσης;
            Φπα = ΚαθαρήΑξία * (decimal)ΠοσοστόΦπα;
            ΑξίαΓραμμής = ΚαθαρήΑξία + Φπα;
        }
        /// <summary>
        /// Υπολογισμός καθαρής αξίας χωρίς καμια εκπτωση
        /// </summary>
        /// <param name="τιμή"></param>
        /// <param name="ποσοστόΦπα"></param>
        /// <returns></returns>
        private double CalculateClearValue(double τιμή, float ποσοστόΦπα)
        {
            var currentparastatiko = DocHelperViewModel.Order;
            if (currentparastatiko == null)
                return 0;
            if (currentparastatiko.Σειρά == null)
                return 0;
            if (currentparastatiko.Σειρά.Λιανική)
                return τιμή / (1 + ποσοστόΦπα);
            return τιμή;
        }
    }
}
