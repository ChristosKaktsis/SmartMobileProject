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
            get
            {
                return eidos;
            }
            set
            {
               
                SetProperty(ref eidos, value);
                LineOfOrders.Είδος = value;
                LineOfOrders.Τιμή = value.ΤιμήΧονδρικής;
                Τιμή = value.ΤιμήΧονδρικής;
                ΠοσοστόΦπα = value.ΦΠΑ != null ? (value.ΦΠΑ.Φπακανονικό / 100) : 0;
                OnPropertyChanged("Eidos");
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
