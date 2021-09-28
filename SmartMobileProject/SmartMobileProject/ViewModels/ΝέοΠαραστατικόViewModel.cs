using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΝέοΠαραστατικόViewModel : BaseViewModel
    {
        public static UnitOfWork uow;
        public static Πωλητής politis;
        public static ΣτοιχείαΕταιρίας στοιχείαΕταιρίας;
        static ΠαραστατικάΠωλήσεων order;
        public static ΠαραστατικάΠωλήσεων Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }
        public ΠαραστατικάΠωλήσεων NewOrder
        {
            get
            {
                return order;
            }
            set
            {
               
                SetProperty(ref order, value);
            }
        }
        public static ΓραμμέςΠαραστατικώνΠωλήσεων editline;
        public ΝέοΠαραστατικόViewModel()
        {
            Order.Πωλητής = politis;
            NewOrder = Order;
            CalculateSums();
            SaveTask();
            Αποθήκευση = new Command(Save);
            Εκτύπωση = new Command(Print);
        }

        private void SaveTask()
        {
            if (Application.Current.Properties.ContainsKey("Εργασία"))
            {
                var oid = Application.Current.Properties["Εργασία"];
                if (oid != null)
                {
                    Εργασία task = uow.GetObjectByKey<Εργασία>(int.Parse((string)oid));
                    if (task.Πελάτης == Order.Πελάτης) { task.Ενέργειες.Add(new Ενέργεια(uow) { SmartOid = Guid.NewGuid() , Τύπος = "Παραστατικό Πώλησης  " + Order.Παραστατικό }); } 
                }
            }
        }
        private async void Save(object obj)
        {
            if (uow.InTransaction)
            {
                SaveAsTask();
                uow.CommitChanges();
                var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να γίνει Εκτύπωση ", "Ναί", "Όχι");
                if (answer)
                {
                    Εκτύπωση.Execute(null);
                }
            }
                    
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("../../..");
        }

        private void SaveAsTask()
        {
            var task = uow.Query<Εργασία>().Where
                (x => x.Χαρακτηρισμός == "Είσοδος στον πελάτη" && x.Πωλητής.Oid == Order.Πωλητής.Oid && x.Κατάσταση == false);
            if (task.Any())
            {
                return;
            }
            Εργασία Εργασία = new Εργασία(uow);
            Εργασία.SmartOid = Guid.NewGuid();
            Εργασία.Χαρακτηρισμός = "Είσοδος στον πελάτη";
            Εργασία.ΗμνίαΕναρξης = DateTime.Now;
            ΕργασίεςStaticViewModel.GetCurrentLocation(Εργασία);
            Εργασία.ΗμνίαΛηξης = DateTime.Now;
            Εργασία.Κατάσταση = true;
            Εργασία.Πελάτης = Order.Πελάτης;
            Order.Πωλητής.Εργασίες.Add(Εργασία);
            Εργασία.Ενέργειες.Add(new Ενέργεια(uow) { SmartOid = Guid.NewGuid(), Τύπος = "Παραστατικό Πώλησης  " + Order.Παραστατικό }); 
        }

        public void Print(object obj)
        {
            CreatePrintView createPrintView = new CreatePrintView();
            if(Order.Σειρά.PrintType=="80 mm")
            {
                createPrintView.CreatePrint(Order);
            }
            else
            {
                createPrintView.CreatePrint2(Order);
            }
           
        }

        public void CalculateSums()
        {
            decimal sumLine = 0;
            float sumQ = 0;
            decimal kath = 0;
            decimal ekpr = 0;
            decimal fpa = 0;
            foreach(var l in Order.ΓραμμέςΠαραστατικώνΠωλήσεων)
            {
                sumLine += l.ΑξίαΓραμμής;
                sumQ += l.Ποσότητα;
                kath += l.ΚαθαρήΑξία;
                ekpr += l.ΑξίαΕκπτωσης;
                fpa += l.Φπα;
            }
            Order.ΑξίαΠαραστατικού = sumLine;
            Order.Ποσότητα =(int) sumQ;
            Order.ΚαθαρήΑξία = kath;
            Order.ΑξίαΕκπτωσης = ekpr;
            Order.Φπα = fpa;
        }
        public ICommand Αποθήκευση { set; get; }
        public ICommand Εκτύπωση { set; get; }
    }
}
