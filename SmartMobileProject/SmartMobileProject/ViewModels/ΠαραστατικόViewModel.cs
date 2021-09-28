using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using SmartMobileProject.Services;
using SmartMobileProject.Views;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΠαραστατικόViewModel : BaseViewModel
    {
        UnitOfWork uow = new UnitOfWork();
        XPCollection<ΠαραστατικάΠωλήσεων> orderCollection = null;
        public XPCollection<ΠαραστατικάΠωλήσεων> OrderCollection
        {
            get { return orderCollection; }
            set { SetProperty(ref orderCollection, value); }
        }
        public ΠαραστατικόViewModel()
        {
            Title = "Πωλήσεις";

            SetPolitis();
            //OrderCollection = new XPCollection<ΠαραστατικάΠωλήσεων>(uow);
            OrderCollection = ΝέοΠαραστατικόViewModel.politis.Παραστατικόπωλήσεων;
            OrderCollection.DeleteObjectOnRemove = true;

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
            ΝέοΠαραστατικόViewModel.politis = p.FirstOrDefault();
        }

        private async void CreateOrder(object obj)
        {

            ΝέοΠαραστατικόViewModel.Order = null;
            
            ΝέοΠαραστατικόViewModel.uow = uow;
            
            await Shell.Current.GoToAsync(nameof(ΠαραστατικόΒασικάΣτοιχείαPage));
        }
        private async void EditOrder(object obj)
        {
            var editItem = (ΠαραστατικάΠωλήσεων)obj;
            if(editItem.IsUploaded)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", 
                    "Δεν Μπορεί να γίνει Επεξεργασία σε Παραστατικό που έχει επικυρωθεί !",
                    "OK");
                return;
            }
            ΝέοΠαραστατικόViewModel.uow = uow;
            ΝέοΠαραστατικόViewModel.Order = editItem;
            await Shell.Current.GoToAsync(nameof(ΠαραστατικόΒασικάΣτοιχείαPage));
        }
        public void Print(object obj)
        {
            CreatePrintView createPrintView = new CreatePrintView();
            var seira = uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>().Where(x => x.Oid == ((ΠαραστατικάΠωλήσεων)obj).Σειρά.Oid);
            if (seira.FirstOrDefault().PrintType=="80 mm")
            {
                createPrintView.CreatePrint((ΠαραστατικάΠωλήσεων)obj);
            }
            else
            {
                createPrintView.CreatePrint2((ΠαραστατικάΠωλήσεων)obj);
            }
           
        }
        private async void DeleteOrder(object sender)
        {
            ΠαραστατικάΠωλήσεων order = (ΠαραστατικάΠωλήσεων)sender;
            if (order.IsUploaded)
            {
                await Application.Current.MainPage.DisplayAlert("Alert",
                    "Δεν Μπορεί να γίνει Διαγραφή σε Παραστατικό που έχει επικυρωθεί !",
                    "OK");
                return;
            }
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να γίνει η διαγραφή ", "Ναί", "Όχι");
            if (answer)
            {
                order.Delete();
                if (uow.InTransaction)
                {
                    uow.CommitChanges();
                }
                Reload.Execute(null);
            }
        }
        private void ReloadCommand(object obj)
        {
            OrderCollection.Reload();
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
