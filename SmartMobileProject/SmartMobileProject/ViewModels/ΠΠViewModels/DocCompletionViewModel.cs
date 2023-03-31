using SmartMobileProject.Models;
using SmartMobileProject.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static DevExpress.Data.Helpers.ExpressiveSortInfo;
using System.Windows.Input;
using Xamarin.Forms;
using DevExpress.Xpo;
using System.Linq;

namespace SmartMobileProject.ViewModels
{
    public class DocCompletionViewModel : BaseViewModel
    {
        private UnitOfWork uow;
        public DocCompletionViewModel()
        {
            uow = DocHelperViewModel.uow;
            Order = DocHelperViewModel.Order;
            SetPolitis();
            SetTitle();
            CalculateSums();
            SaveTask();
            Αποθήκευση = new Command(Save);
        }
        public ΠαραστατικάΠωλήσεων Order{get; set;}
        private void SetPolitis()
        {
            var p = uow.Query<Πωλητής>().Where(x => x.Oid == App.Πωλητής.Oid);
            Order.Πωλητής = p.FirstOrDefault();
        }
        private void SetTitle()
        {
            if (DocHelperViewModel.Order.Σειρά == null) return;
            var p = getCounter();
            Title = DocHelperViewModel
                .Order.Σειρά.Σειρά + p.ToString().PadLeft(9, '0');
        }
        private int getCounter()
        {
            if (uow.Query<ΠαραστατικάΠωλήσεων>()
              .Where(p => p.Oid == DocHelperViewModel.Order.Oid).Any())
                if (!DocHelperViewModel.Order.IsSeiraChanged())
                    return DocHelperViewModel.Order.OrderSeiraCounter();
            return DocHelperViewModel.Order.Σειρά.Counter + 1;
        }
        private void SaveTask()
        {
            if (Application.Current.Properties.ContainsKey("Εργασία"))
            {
                var oid = Application.Current.Properties["Εργασία"];
                if (oid != null)
                {
                    Εργασία task = uow.GetObjectByKey<Εργασία>(int.Parse((string)oid));
                    if (task.Πελάτης == Order.Πελάτης) { task.Ενέργειες.Add(new Ενέργεια(uow) { SmartOid = Guid.NewGuid(), Τύπος = "Παραστατικό Πώλησης  " + Order.Παραστατικό }); }
                }
            }
        }
        private async void Save()
        {
            try
            {
                UpdateCounter();
                SetOrderTitle();
                SaveAsTask();
                await uow.CommitChangesAsync();
                var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να γίνει Εκτύπωση ", "Ναί", "Όχι");
                if (answer) Print(Order);
                await Shell.Current.Navigation.PopToRootAsync();
            }
            catch (Exception ex) 
            { 
                Debug.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert(
                    "Σφάλμα", "Το παραστατικό Δεν αποθηκεύτηκε", "Όχι");
            }
        }
        private void UpdateCounter()
        {
            var item = uow.Query<ΠαραστατικάΠωλήσεων>()
                .Where(p => p.SmartOid == Order.SmartOid).FirstOrDefault();
            if (item != null)
            {
                return;
            }
            Order.Σειρά.Counter++;
        }
        private void SetOrderTitle() => Order.Παραστατικό = Title;
        private void SaveAsTask()
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public async void Print(ΠαραστατικάΠωλήσεων item)
        {
            CreatePrintView createPrintView = new CreatePrintView();
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var order = await uow.GetObjectByKeyAsync<ΠαραστατικάΠωλήσεων>(item.Oid);
                    string print = await createPrintView.page2(order);
                    createPrintView.CreatePrint(print);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
        public void CalculateSums()
        {
            decimal sumLine = 0;
            float sumQ = 0;
            decimal kath = 0;
            decimal ekpr = 0;
            decimal fpa = 0;
            try
            {
                foreach (var l in Order.ΓραμμέςΠαραστατικώνΠωλήσεων)
                {
                    sumLine += l.ΑξίαΓραμμής;
                    sumQ += l.Ποσότητα;
                    kath += l.ΚαθαρήΑξία;
                    ekpr += l.ΑξίαΕκπτωσης;
                    fpa += l.Φπα;
                }
                Order.ΑξίαΠαραστατικού = sumLine;
                Order.Ποσότητα = (int)sumQ;
                Order.ΚαθαρήΑξία = kath;
                Order.ΑξίαΕκπτωσης = ekpr;
                Order.Φπα = fpa;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        public ICommand Αποθήκευση { set; get; }
    }
}
