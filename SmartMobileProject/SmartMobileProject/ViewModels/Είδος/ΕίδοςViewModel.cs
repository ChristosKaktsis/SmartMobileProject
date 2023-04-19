using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using SmartMobileProject.Views;
using DevExpress.Xpo;
using SmartMobileProject.Models;

namespace SmartMobileProject.ViewModels
{
    class ΕίδοςViewModel : BaseViewModel
    {
        public UnitOfWork uow = ((App)Application.Current).uow;
        //public UnitOfWork uow = new UnitOfWork();
        XPCollection<Είδος> eidosCollection = null;
        public XPCollection<Είδος> EidosCollection
        {
            get { return eidosCollection; }
            set { SetProperty(ref eidosCollection, value); }
        }
        
        public ΕίδοςViewModel()
        {
            Title = "Είδος";
            
            EidosCollection = new XPCollection<Είδος>(uow);
            EidosCollection.DeleteObjectOnRemove = true;           

            ΝέοΕίδος = new Command(CreateEidos);
            ΕπεξεργασίαΕίδος = new Command(EditEidos);
            ΔιαγραφήΕίδους = new Command(DeleteEidos);
            Reload = new Command(ReloadCommand);
           
            
        }

        private async void CreateEidos(object obj)
        {
            if (uow.InTransaction)
             {
                 uow.ReloadChangedObjects();
             }
            ΕίδοςDetailViewModel.editeidos = null;
            await Shell.Current.GoToAsync($"{nameof(ΕίδοςDetailViewPage)}?" +
                $"{nameof(ΔιευθύνσειςΠελάτηDetailViewModel.IsFromEdit)}={false}");
          
        }
        private async void EditEidos(object obj)
        {
            if (uow.InTransaction)
             {
                 uow.ReloadChangedObjects();
             }
             ΕίδοςDetailViewModel.editeidos = (Είδος)obj;
            await Shell.Current.GoToAsync($"{nameof(ΕίδοςDetailViewPage)}?" +
                $"{nameof(ΔιευθύνσειςΠελάτηDetailViewModel.IsFromEdit)}={true}");

        }

        private async void DeleteEidos(object obj)
        {
            if (uow.InTransaction)
            {
                uow.ReloadChangedObjects();
            }
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να γίνει η διαγραφή ", "Ναί", "Όχι");

            if (answer)
            {
                Είδος eidos = (Είδος)obj;
                eidos.Delete();
                if (uow.InTransaction)
                {
                    uow.CommitChanges();
                }
                Reload.Execute(null);
            }          
        }
        private  void ReloadCommand(object obj)
        {
             EidosCollection.Reload();
        }

        public ICommand ΝέοΕίδος { get; set; }
        public ICommand ΕπεξεργασίαΕίδος { get; set; }
        public ICommand ΔιαγραφήΕίδους { get; set; }
        public ICommand Reload { get; set; }
    }
}
