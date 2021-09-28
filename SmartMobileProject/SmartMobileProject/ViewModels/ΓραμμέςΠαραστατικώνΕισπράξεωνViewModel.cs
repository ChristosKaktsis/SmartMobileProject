using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΓραμμέςΠαραστατικώνΕισπράξεωνViewModel : BaseViewModel
    {
        UnitOfWork uow = ΠαραστατικάΕισπράξεωνStaticViewModel.uow;

        public XPCollection<ΓραμμέςΠαραστατικώνΕισπράξεων> LineOfOrdersCollection { get; set; }
        public ΓραμμέςΠαραστατικώνΕισπράξεωνViewModel()
        {
            LineOfOrdersCollection = ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.ΓραμμέςΠαραστατικώνΕισπράξεων;
            LineOfOrdersCollection.DeleteObjectOnRemove = true;

            //titlos
            if (string.IsNullOrEmpty(ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Παραστατικό))
            {
                if (ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Σειρά != null)
                {
                    var p = ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Σειρά.Counter += 1;
                    ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Παραστατικό = ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Σειρά.Σειρά + p.ToString().PadLeft(8, '0');
                }
            }
            Title = ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Παραστατικό;
            //
            ΝέαΓραμμήΛογαριαμός = new Command(CreateLineOfOrder);
            ΝέαΓραμμήΑξιόγραφο = new Command(CreateAxiografo);
            ΕπεξεργασίαΓραμμής = new Command(EditLineOfOrder);
            ΔιαγραφήΓραμμής = new Command(DeleteLineOfOrder);
            Αποθήκευση = new Command(Save);
        }

        private async void CreateAxiografo(object obj)
        {
            ΠαραστατικάΕισπράξεωνStaticViewModel.editline = null;
            await Shell.Current.GoToAsync(nameof(ΑξιόγραφοDetailViewPage));
        }

        private async void CreateLineOfOrder(object obj)
        {
            ΠαραστατικάΕισπράξεωνStaticViewModel.editline = null;
            await Shell.Current.GoToAsync(nameof(ΛογαριασμόςDetailViewPage));
        }
        private async void EditLineOfOrder(object obj)
        {
            ΠαραστατικάΕισπράξεωνStaticViewModel.editline = (ΓραμμέςΠαραστατικώνΕισπράξεων)obj;
            if(ΠαραστατικάΕισπράξεωνStaticViewModel.editline.Αξιόγραφα == null)
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
            SaveTask();
            if (uow.InTransaction)
            {
                ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Πίστωση = 0;
                foreach (var i in ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.ΓραμμέςΠαραστατικώνΕισπράξεων)
                {
                    ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Πίστωση += i.Ποσόν;
                }
                ΠαραστατικάΕισπράξεωνStaticViewModel.politis.ΠαραστατικάΕισπράξεων.Add(ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr);
                SaveAsTask();
                uow.CommitChanges();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("../..");
        }
        private void SaveTask()
        {
            if (Application.Current.Properties.ContainsKey("Εργασία"))
            {
                var oid = Application.Current.Properties["Εργασία"];
                if (oid != null)
                {
                    Εργασία task = uow.GetObjectByKey<Εργασία>(int.Parse((string)oid));
                    if (task.Πελάτης == ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Πελάτης) 
                    {
                        task.Ενέργειες.Add(new Ενέργεια(uow) {
                            SmartOid = Guid.NewGuid(),
                            Τύπος = "Παραστατικό Εισπράξεων  " + 
                            ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Παραστατικό }); 
                    }
                }
            }
        }
        private void SaveAsTask()
        {
            var task = uow.Query<Εργασία>().Where
                (x => x.Χαρακτηρισμός == "Είσοδος στον πελάτη" && x.Πωλητής.Oid == ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Πωλητής.Oid && x.Κατάσταση == false);
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
            Εργασία.Πελάτης = ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Πελάτης;
            ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Πωλητής.Εργασίες.Add(Εργασία);
            Εργασία.Ενέργειες.Add(new Ενέργεια(uow) { SmartOid = Guid.NewGuid(), Τύπος = "Παραστατικό Εισπράξεων  " + ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr.Παραστατικό });
        }
        public ICommand ΝέαΓραμμήΛογαριαμός { get; set; }
        public ICommand ΝέαΓραμμήΑξιόγραφο { get; set; }
        public ICommand ΕπεξεργασίαΓραμμής { get; set; }
        public ICommand ΔιαγραφήΓραμμής { get; set; }
        public ICommand Αποθήκευση { set; get; }
    }
}
