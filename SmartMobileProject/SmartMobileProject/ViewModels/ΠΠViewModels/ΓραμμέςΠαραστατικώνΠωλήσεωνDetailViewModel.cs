using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΓραμμέςΠαραστατικώνΠωλήσεωνDetailViewModel : BaseViewModel
    {
        UnitOfWork uow;

        ΓραμμέςΠαραστατικώνΠωλήσεων lineOfOrders;
        Είδος eidos;
        Single ποσότητα;
        double τιμή;
        public Είδος Eidos
        {
            get => eidos;          
            set
            {
                if (value == null)
                    return;
                value = ΕίναιBarCode ? LineOfOrders.Είδος : value;
                SetProperty(ref eidos, value);              
                if (LineOfOrders == null)
                    return;
                LineOfOrders.Είδος = value;
                //LineOfOrders.Τιμή = ΕίναιBarCode? LineOfOrders.Τιμή : value.ΤιμήΧονδρικής;
                LineOfOrders.Τιμή = Τιμή = ΕίναιBarCode ? LineOfOrders.Τιμή : setPriceOfLine(value);
                //Τιμή = ΕίναιBarCode ? LineOfOrders.Τιμή : value.ΤιμήΧονδρικής;
                ΠοσοστόΦπα = value.ΦΠΑ != null ? (value.ΦΠΑ.Φπακανονικό / 100) : 0;               
                ChangeValue();
            }
        }
        //
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
        public Single Ποσότητα
        {
            get
            {
                return ποσότητα; 
            }
            set
            {
                SetProperty(ref ποσότητα, value);
                LineOfOrders.Ποσότητα = value;
                ChangeValue();
            }
        }
        public double Τιμή
        {
            get { return τιμή; }
            set 
            {
                SetProperty(ref τιμή, value);
                LineOfOrders.Τιμή = value;
                ChangeValue();
            }
        }
        public Single Εκπτωση
        {
            get { return εκπτωση; }
            set
            {
                SetProperty(ref εκπτωση, value);
                LineOfOrders.Εκπτωση = value;
                ChangeValue();
            }
        }
        Single εκπτωση;
        public Single ΠοσοστόΦπα
        {
            get { return ποσοστόφπα; }
            set 
            {
                SetProperty(ref ποσοστόφπα, value);
                LineOfOrders.ΠοσοστόΦπα = value;
                ChangeValue();
            }
        }
        Single ποσοστόφπα;
        public bool ΕίναιBarCode
        {
            get { return _ΕίναιBarCode; }
            set
            {
                SetProperty(ref _ΕίναιBarCode, value);               
            }
        }
        bool _ΕίναιBarCode;
        private void ChangeValue()
        {
            double clearvalue = CalculateClearValue(LineOfOrders.Τιμή, LineOfOrders.ΠοσοστόΦπα);
            LineOfOrders.ΑξίαΕκπτωσης = (decimal)(LineOfOrders.Ποσότητα * clearvalue * LineOfOrders.Εκπτωση);
            LineOfOrders.ΚαθαρήΑξία = (decimal)(LineOfOrders.Ποσότητα * clearvalue) - LineOfOrders.ΑξίαΕκπτωσης;

            LineOfOrders.Φπα = LineOfOrders.ΚαθαρήΑξία *(decimal)LineOfOrders.ΠοσοστόΦπα;
            LineOfOrders.ΑξίαΓραμμής = LineOfOrders.ΚαθαρήΑξία + LineOfOrders.Φπα;
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

        public ΓραμμέςΠαραστατικώνΠωλήσεων LineOfOrders
        {
            get
            {
                return lineOfOrders;
            }
            set
            {
               
                SetProperty(ref lineOfOrders, value);
                OnPropertyChanged("LineOfOrders");
            }
        }
        public XPCollection<Είδος> EidosCollection {get; set;}

        public ΓραμμέςΠαραστατικώνΠωλήσεωνDetailViewModel()
        {
            Title ="Γραμμή "+ DocHelperViewModel.Order.Παραστατικό;
            uow = DocHelperViewModel.uow;
            if (DocHelperViewModel.editline == null)
            {
                LineOfOrders = new ΓραμμέςΠαραστατικώνΠωλήσεων(uow);
                LineOfOrders.SmartOid = Guid.NewGuid();
            }
            else
            {
                LineOfOrders = DocHelperViewModel.editline;
                ΕίναιBarCode = LineOfOrders.BarCodeΕίδους != null;
                Eidos = LineOfOrders.Είδος;
                Ποσότητα = LineOfOrders.Ποσότητα;
                Εκπτωση = LineOfOrders.Εκπτωση;
                ΠοσοστόΦπα = LineOfOrders.ΠοσοστόΦπα;              
            }
            
            EidosCollection = new XPCollection<Είδος>(uow);
           //LineOfOrders.Τιμή = LineOfOrders.Είδος.ΤιμήΧονδρικής;
            Αποθήκευση = new Command(Save);
            Ακύρωση = new Command(Cancel);
        }

        private async void Save(object obj)
        {
            if (uow.InTransaction)
            {
                // app.uow.CommitChanges();
                LineOfOrders.ΠαραστατικάΠωλήσεων = DocHelperViewModel.Order;
                //Address.Customer = appShell.customer1;
            }
        
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public ICommand Αποθήκευση { set; get; }
        public ICommand Ακύρωση { set; get; }

    }
}
