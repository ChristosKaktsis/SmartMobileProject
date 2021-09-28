using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΓραμμέςΠαραστατικώνΠωλήσεωνQuickPickDetailViewModel : BaseViewModel
    {
        UnitOfWork uow;
        List<Είδος> selectedEidosCollection;
        ΓραμμέςΠαραστατικώνΠωλήσεων lineOfOrders;
        public XPCollection<ΟικογένειαΕίδους> OikogeneiaCollection { get; set; }
        public XPCollection<Είδος> EidosCollection { get; set; }
        public List<Είδος> SelectedEidosCollection
        {
            get
            {
                return selectedEidosCollection;
            }
            set
            {
                SetProperty(ref selectedEidosCollection, value);
                OnPropertyChanged("SelectedEidosCollection");
            }
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
        Είδος eidos;
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
                LineOfOrders.ΠοσοστόΦπα = value.ΦΠΑ.Φπακανονικό / 100;
                LineOfOrders.Ποσότητα = value.Ποσότητα;
                LineOfOrders.Εκπτωση = 0;
                OnPropertyChanged("Eidos");
                ChangeValue();
            }
        }
       
       
        private void ChangeValue()
        {
            LineOfOrders.ΑξίαΕκπτωσης = (decimal)(LineOfOrders.Ποσότητα * LineOfOrders.Τιμή * LineOfOrders.Εκπτωση);
            LineOfOrders.ΚαθαρήΑξία = (decimal)(LineOfOrders.Ποσότητα * LineOfOrders.Τιμή) - LineOfOrders.ΑξίαΕκπτωσης;
            LineOfOrders.Φπα = LineOfOrders.ΚαθαρήΑξία * (decimal)LineOfOrders.ΠοσοστόΦπα;
            LineOfOrders.ΑξίαΓραμμής = LineOfOrders.ΚαθαρήΑξία + LineOfOrders.Φπα;
        }
        public ΓραμμέςΠαραστατικώνΠωλήσεωνQuickPickDetailViewModel()
        {
            uow = ΝέοΠαραστατικόViewModel.uow;
            OikogeneiaCollection = new XPCollection<ΟικογένειαΕίδους>(uow);
            EidosCollection = new XPCollection<Είδος>(uow);
            SelectedEidosCollection = new List<Είδος>();

            Αποθήκευση = new Command(Save);
        }

        private async void Save(object obj)
        {
            foreach(Είδος i in SelectedEidosCollection)
            {
                LineOfOrders = new ΓραμμέςΠαραστατικώνΠωλήσεων(uow);
                LineOfOrders.SmartOid = Guid.NewGuid();
                Eidos = i;
                if (uow.InTransaction)
                {
                    // app.uow.CommitChanges();
                    LineOfOrders.ΠαραστατικάΠωλήσεων = ΝέοΠαραστατικόViewModel.Order;
                    //Address.Customer = appShell.customer1;
                }
            }
           

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }


        public ICommand Αποθήκευση { set; get; }

    }
}
