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
                LineOfOrders.Τιμή = ΕίναιBarCode? LineOfOrders.Τιμή : value.ΤιμήΧονδρικής;
                Τιμή = ΕίναιBarCode ? LineOfOrders.Τιμή : value.ΤιμήΧονδρικής;
                ΠοσοστόΦπα = value.ΦΠΑ != null ? (value.ΦΠΑ.Φπακανονικό / 100) : 0;               
                ChangeValue();
            }
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
            LineOfOrders.ΑξίαΕκπτωσης = (decimal)(LineOfOrders.Ποσότητα * LineOfOrders.Τιμή * LineOfOrders.Εκπτωση);
            LineOfOrders.ΚαθαρήΑξία = (decimal)(LineOfOrders.Ποσότητα * LineOfOrders.Τιμή ) - LineOfOrders.ΑξίαΕκπτωσης;
            LineOfOrders.Φπα = LineOfOrders.ΚαθαρήΑξία *(decimal)LineOfOrders.ΠοσοστόΦπα;
            LineOfOrders.ΑξίαΓραμμής = LineOfOrders.ΚαθαρήΑξία + LineOfOrders.Φπα;
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
            Title ="Γραμμή "+ ΝέοΠαραστατικόViewModel.Order.Παραστατικό;
            uow = ΝέοΠαραστατικόViewModel.uow;
            if (ΝέοΠαραστατικόViewModel.editline == null)
            {
                LineOfOrders = new ΓραμμέςΠαραστατικώνΠωλήσεων(uow);
                LineOfOrders.SmartOid = Guid.NewGuid();
            }
            else
            {
                LineOfOrders = ΝέοΠαραστατικόViewModel.editline;
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
                LineOfOrders.ΠαραστατικάΠωλήσεων = ΝέοΠαραστατικόViewModel.Order;
                //Address.Customer = appShell.customer1;
            }
        
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public ICommand Αποθήκευση { set; get; }
        public ICommand Ακύρωση { set; get; }

    }
}
