using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΓραμμέςΠαραστατικώνΕισπράξεωνViewModel : BaseViewModel
    {
        UnitOfWork uow = DocCollectHelper.uow;

        public XPCollection<ΓραμμέςΠαραστατικώνΕισπράξεων> LineOfOrdersCollection { get; set; }
        public ΓραμμέςΠαραστατικώνΕισπράξεωνViewModel()
        {
            LineOfOrdersCollection = DocCollectHelper.ParastatikoEispr.ΓραμμέςΠαραστατικώνΕισπράξεων;
            LineOfOrdersCollection.DeleteObjectOnRemove = true;
            //titlos
            //titlos kai arith seiras
            SetTitle();
            ΝέαΓραμμήΛογαριαμός = new Command(CreateLineOfOrder);
            ΝέαΓραμμήΑξιόγραφο = new Command(CreateAxiografo);
            ΕπεξεργασίαΓραμμής = new Command(EditLineOfOrder);
            ΔιαγραφήΓραμμής = new Command(DeleteLineOfOrder);
            Αποθήκευση = new Command(Save);
        }
        private void SetTitle()
        {
            if (DocCollectHelper.ParastatikoEispr.Σειρά == null) return;
            var p = getCounter();
            Title = DocCollectHelper.ParastatikoEispr.Σειρά.Σειρά + p.ToString().PadLeft(9, '0');
        }
        private int getCounter()
        {
            if (uow.Query<ΠαραστατικάΕισπράξεων>()
              .Where(p => p.Oid == DocCollectHelper.ParastatikoEispr.Oid).Any())
                if (!DocCollectHelper.ParastatikoEispr.IsSeiraChanged())
                    return DocCollectHelper.ParastatikoEispr.OrderSeiraCounter();
            return DocCollectHelper.ParastatikoEispr.Σειρά.Counter + 1;
        }
        private void SetOrderSeiraCounter()
        {
            if (!string.IsNullOrEmpty(DocCollectHelper.ParastatikoEispr.Παραστατικό)) return;
            if (DocCollectHelper.ParastatikoEispr.Σειρά == null) return;
           
            var p = DocCollectHelper.ParastatikoEispr.Σειρά.Counter++;
            DocCollectHelper.ParastatikoEispr.Παραστατικό = 
                DocCollectHelper
                .ParastatikoEispr.Σειρά.Σειρά + p.ToString().PadLeft(9, '0');
        }
        private async void CreateAxiografo(object obj)
        {
            DocCollectHelper.editline = null;
            await Shell.Current.GoToAsync(nameof(ΑξιόγραφοDetailViewPage));
        }

        private async void CreateLineOfOrder(object obj)
        {
            DocCollectHelper.editline = null;
            await Shell.Current.GoToAsync(nameof(ΛογαριασμόςDetailViewPage));
        }
        private async void EditLineOfOrder(object obj)
        {
            DocCollectHelper.editline = (ΓραμμέςΠαραστατικώνΕισπράξεων)obj;
            if(DocCollectHelper.editline.Αξιόγραφα == null)
            {
                await Shell.Current.GoToAsync(nameof(ΛογαριασμόςDetailViewPage));
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(ΑξιόγραφοDetailViewPage));
            }
             
        }
        private async void DeleteLineOfOrder(object obj)
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να γίνει η διαγραφή ", "Ναί", "Όχι");

            if (answer)
            {
                ΓραμμέςΠαραστατικώνΕισπράξεων line = (ΓραμμέςΠαραστατικώνΕισπράξεων)obj;
                line.Delete();

            }
        }
        private async void Save(object obj)
        {
            if(!DocCollectHelper.ParastatikoEispr.ΓραμμέςΠαραστατικώνΕισπράξεων.Any())
            {
                await Application.Current.MainPage.DisplayAlert("Δεν είναι έγκυρη", "Το παραστατικό πρέπει να έχει γραμμές", "Οκ");
                return;
            }
            try
            {
                SaveTask();
                CalculateSums();
                SetSeller();
                SaveAsTask();
                UpdateCounter();
                SetOrderTitle();
                uow.CommitChanges();
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("../..");
        }

        private void SetSeller()
        {
            var p = uow.Query<Πωλητής>().Where(x => x.Oid == App.Πωλητής.Oid);
            DocCollectHelper.ParastatikoEispr.Πωλητής = p.FirstOrDefault();
        }
        private void CalculateSums()
        {
            DocCollectHelper.ParastatikoEispr.Πίστωση = 0;
            foreach (var i in DocCollectHelper.ParastatikoEispr.ΓραμμέςΠαραστατικώνΕισπράξεων)
            {
                DocCollectHelper.ParastatikoEispr.Πίστωση += i.Ποσόν;
            }
        }
        private void SaveTask()
        {
            if (Application.Current.Properties.ContainsKey("Εργασία"))
            {
                var oid = Application.Current.Properties["Εργασία"];
                if (oid != null)
                {
                    Εργασία task = uow.GetObjectByKey<Εργασία>(int.Parse((string)oid));
                    if (task.Πελάτης == DocCollectHelper.ParastatikoEispr.Πελάτης) 
                    {
                        task.Ενέργειες.Add(new Ενέργεια(uow) {
                            SmartOid = Guid.NewGuid(),
                            Τύπος = "Παραστατικό Εισπράξεων  " + 
                            DocCollectHelper.ParastatikoEispr.Παραστατικό }); 
                    }
                }
            }
        }
        private void SaveAsTask()
        {
            var task = uow.Query<Εργασία>().Where
                (x => x.Χαρακτηρισμός == "Είσοδος στον πελάτη" && x.Πωλητής.Oid == DocCollectHelper.ParastatikoEispr.Πωλητής.Oid && x.Κατάσταση == false);
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
            Εργασία.Πελάτης = DocCollectHelper.ParastatikoEispr.Πελάτης;
            DocCollectHelper.ParastatikoEispr.Πωλητής.Εργασίες.Add(Εργασία);
            Εργασία.Ενέργειες.Add(new Ενέργεια(uow) { SmartOid = Guid.NewGuid(), Τύπος = "Παραστατικό Εισπράξεων  " + DocCollectHelper.ParastatikoEispr.Παραστατικό });
        }
        private void UpdateCounter()
        {
            var item = uow.Query<ΠαραστατικάΕισπράξεων>()
                .Where(p => p.SmartOid == DocCollectHelper.ParastatikoEispr.SmartOid).FirstOrDefault();
            if (item != null)
            {
                return;
            }
            DocCollectHelper.ParastatikoEispr.Σειρά.Counter++;
        }
        private void SetOrderTitle() => DocCollectHelper.ParastatikoEispr.Παραστατικό = Title;
        public ICommand ΝέαΓραμμήΛογαριαμός { get; set; }
        public ICommand ΝέαΓραμμήΑξιόγραφο { get; set; }
        public ICommand ΕπεξεργασίαΓραμμής { get; set; }
        public ICommand ΔιαγραφήΓραμμής { get; set; }
        public ICommand Αποθήκευση { set; get; }
    }
}
