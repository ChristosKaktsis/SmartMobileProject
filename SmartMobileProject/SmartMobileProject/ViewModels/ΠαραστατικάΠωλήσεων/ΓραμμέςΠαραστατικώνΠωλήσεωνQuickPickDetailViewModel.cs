using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<ΟικογένειαΕίδους> OikogeneiaCollection { get; set; }
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
                //LineOfOrders.Τιμή = value.ΤιμήΧονδρικής;   
                LineOfOrders.Τιμή = setPriceOfLine(value);
                LineOfOrders.ΠοσοστόΦπα =value.ΦΠΑ!=null ? (value.ΦΠΑ.Φπακανονικό / 100) : 0;
                LineOfOrders.Ποσότητα = value.Ποσότητα;
                LineOfOrders.Εκπτωση = 0;
                OnPropertyChanged("Eidos");
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
            var currentparastatiko = ΝέοΠαραστατικόViewModel.Order;
            if (currentparastatiko == null)
                return 0;
            if (currentparastatiko.Σειρά == null)
                return 0;
            return value.getPrice(currentparastatiko.Σειρά.Λιανική);
        }
        private void ChangeValue()
        {
            double clearvalue = CalculateClearValue(LineOfOrders.Τιμή, LineOfOrders.ΠοσοστόΦπα);
            LineOfOrders.ΑξίαΕκπτωσης = (decimal)(LineOfOrders.Ποσότητα * clearvalue * LineOfOrders.Εκπτωση);
            LineOfOrders.ΚαθαρήΑξία = (decimal)(LineOfOrders.Ποσότητα * clearvalue) - LineOfOrders.ΑξίαΕκπτωσης;
            LineOfOrders.Φπα = LineOfOrders.ΚαθαρήΑξία * (decimal)LineOfOrders.ΠοσοστόΦπα;
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
            var currentparastatiko = ΝέοΠαραστατικόViewModel.Order;
            if (currentparastatiko == null)
                return 0;
            if (currentparastatiko.Σειρά == null)
                return 0;
            if (currentparastatiko.Σειρά.Λιανική)
                return τιμή / (1 + ποσοστόΦπα);
            return τιμή;
        }
        public ΓραμμέςΠαραστατικώνΠωλήσεωνQuickPickDetailViewModel()
        {
            uow = ΝέοΠαραστατικόViewModel.uow;
            OikogeneiaCollection = new XPCollection<ΟικογένειαΕίδους>(uow).OrderBy(x => x.Περιγραφή).ToList();
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
