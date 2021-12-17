using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΕργασίεςViewModel : BaseViewModel
    {
        UnitOfWork uow;
        public XPCollection<Εργασία> Εργασίες { get; set; }
        public List<Εργασία> ΟλοκληρομένεςΕργασίες { get; set; }
        public XPCollection<Πελάτης> Πελάτες { get; set; }
        Εργασία εργασία;
        public Εργασία Εργασία
        {
            get { return εργασία; }
            set
            {
                SetProperty(ref εργασία, value);
            }
        }
        Πωλητής πωλητής;
        public ΕργασίεςViewModel()
        {
            Title = "Εργασίες";
            uow = new UnitOfWork();
            ΕργασίεςStaticViewModel.uow = uow;
            SetPolitis();
            Εργασίες = πωλητής.Εργασίες;
            ΟλοκληρομένεςΕργασίες = new List<Εργασία>();
            Πελάτες = πωλητής.Πελάτες;
            ΕργασίεςStaticViewModel.Πελάτες = πωλητής.Πελάτες;
            ΝέαΕργασία = new Command(NewTask);
            Αποθήκευση = new Command(Save);
            Επεξεργασία = new Command(Edit);
            Διαγραφή = new Command(Delete);
            Reload = new Command(ReloadCommand);
        }

        private async void Edit(object obj)
        {
            if (!IsTrialOn)
                return;
            ΕργασίεςStaticViewModel.εργασία = (Εργασία)obj;
            await Shell.Current.GoToAsync(nameof(ΕργασίεςDetailViewPage));
        }
        private void Delete(object obj)
        {
            var task = (Εργασία)obj;
            task.Delete();
            if (uow.InTransaction)
            {
                uow.CommitChanges();
            }
        }
        private void SetPolitis()
        {
            AppShell app = (AppShell)Application.Current.MainPage;
            var p = uow.Query<Πωλητής>().Where(x => x.Oid == app.πωλητής.Oid);
            πωλητής = p.FirstOrDefault();
        }
        private void Save(object obj)
        {
            if (!IsTrialOn)
                return;
            var task = uow.Query<Εργασία>().Where
                (x => x.Χαρακτηρισμός == "Είσοδος στον πελάτη" && x.Πωλητής.Oid == πωλητής.Oid && x.Κατάσταση == false);
            if (task.Any())
            {
                Reload.Execute(null);
            }
            if (uow.InTransaction) 
            {
                Εργασίες.Add(Εργασία);
                
                uow.CommitChanges();
                if (Εργασία.Πελάτης != null)
                {
                    Application.Current.Properties["Πελάτης"] = Εργασία.Πελάτης.Oid.ToString();
                }
                Application.Current.Properties["Εργασία"] = Εργασία.Oid.ToString();
            }
        }
        private void ReloadCommand(object obj)
        {
            if (uow.InTransaction)
            {
                uow.ReloadChangedObjects();
            }
        }
        private void NewTask(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(ΕργασίεςDetailViewPage));
            Εργασία = new Εργασία(uow);
            Εργασία.SmartOid = Guid.NewGuid();
            Εργασία.Χαρακτηρισμός = "Είσοδος στον πελάτη";
            Εργασία.ΗμνίαΕναρξης = DateTime.Now;
            ΕργασίεςStaticViewModel.GetCurrentLocation(Εργασία);
        }
        public void CheckTask(object obj)
        {
            var task = (Εργασία)obj;
            if (!task.Κατάσταση)
            {
                task.Κατάσταση = true;
                Application.Current.Properties["Πελάτης"] = null;
                Application.Current.Properties["Εργασία"] = null;
                task.ΗμνίαΛηξης = DateTime.Now;
                task.DisplayEndTime = "Ολοκληρώθηκε :" + task.ΗμνίαΛηξης;
                ΟλοκληρομένεςΕργασίες.Add(task);
            }
            else
            {
               // task.Κατάσταση = false;
            }
            if (uow.InTransaction)
            {
                uow.CommitChanges();
            }
            
        }
       
        public ICommand ΝέαΕργασία { get; set; }
        public ICommand Επεξεργασία { get; set; }
        public ICommand Αποθήκευση { get; set; }
        public ICommand Διαγραφή { get; set; }
        public ICommand Reload { get; set; }

    }
}
