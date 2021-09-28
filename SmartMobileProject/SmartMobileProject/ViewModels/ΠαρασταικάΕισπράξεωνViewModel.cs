using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Services;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΠαρασταικάΕισπράξεωνViewModel : BaseViewModel
    {
        UnitOfWork uow = new UnitOfWork();
        XPCollection<ΠαραστατικάΕισπράξεων> parastatika = null;
        public XPCollection<ΠαραστατικάΕισπράξεων> Parastatika
        {
            get { return parastatika; }
            set { SetProperty(ref parastatika, value); }
        }

        public ΠαρασταικάΕισπράξεωνViewModel()
        {
            Title = "Εισπράξεις";

            SetPolitis();

            Parastatika = ΠαραστατικάΕισπράξεωνStaticViewModel.politis.ΠαραστατικάΕισπράξεων;
            Parastatika.DeleteObjectOnRemove = true;

            Reload = new Command(ReloadCommand);
            ΔημιουργίαΠαραστατικού = new Command(CreateOrder);
            ΤροποποίησηΠαρασατικού = new Command(EditOrder);
            Εκτύπωση = new Command(Print);
            ΔιαγραφήΠαρασατικού = new Command(DeleteOrder);
        }
        private void SetPolitis()
        {
            AppShell app = (AppShell)Application.Current.MainPage;
            var p = uow.Query<Πωλητής>().Where(x => x.Oid == app.πωλητής.Oid);
            ΠαραστατικάΕισπράξεωνStaticViewModel.politis = p.FirstOrDefault();
        }
        private async void CreateOrder(object obj)
        {
            ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr = null;
            ΠαραστατικάΕισπράξεωνStaticViewModel.uow = uow;
           //await Shell.Current.GoToAsync(nameof(ΝέοΠαραστατικόPage));
           await Shell.Current.GoToAsync(nameof(ΠαραστατικόΕισπράξεωνΒασικάΣτοιχείαPage));
        }
        private async void EditOrder(object obj)
        {
            var editItem = (ΠαραστατικάΕισπράξεων)obj;
            if (editItem.IsUploaded)
            {
                await Application.Current.MainPage.DisplayAlert("Alert",
                    "Δεν Μπορεί να γίνει Επεξεργασία σε Παραστατικό που έχει επικυρωθεί !",
                    "OK");
                return;
            }
            ΠαραστατικάΕισπράξεωνStaticViewModel.uow = uow;
            ΠαραστατικάΕισπράξεωνStaticViewModel.ParastatikoEispr = editItem;
            await Shell.Current.GoToAsync(nameof(ΠαραστατικόΕισπράξεωνΒασικάΣτοιχείαPage));
        }
        public void Print(object obj)
        {
            CreatePrintView createPrintView = new CreatePrintView();
            createPrintView.CreatePrint3((ΠαραστατικάΕισπράξεων)obj);
        }
        private async void DeleteOrder(object sender)
        {
            ΠαραστατικάΕισπράξεων par = (ΠαραστατικάΕισπράξεων)sender;
            if (par.IsUploaded)
            {
                await Application.Current.MainPage.DisplayAlert("Alert",
                    "Δεν Μπορεί να γίνει Διαγραφή σε Παραστατικό που έχει επικυρωθεί !",
                    "OK");
                return;
            }
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να γίνει η διαγραφή ", "Ναί", "Όχι");
            if (answer)
            {
                par.Delete();
                if (uow.InTransaction)
                {
                    uow.CommitChanges();
                }
                Reload.Execute(null);
            }
        }

        private void ReloadCommand(object obj)
        {
            Parastatika.Reload();
            uow.ReloadChangedObjects();
        }
        public ICommand OpenWebCommand { get; }
        public ICommand Reload { get; set; }
        public ICommand ΔημιουργίαΠαραστατικού { get; set; }
        public ICommand ΤροποποίησηΠαρασατικού { get; set; }
        public ICommand Εκτύπωση { set; get; }
        public ICommand ΔιαγραφήΠαρασατικού { get; set; }
    }
}
