using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using DevExpress.Xpo;
using SmartMobileProject.Models;

namespace SmartMobileProject.ViewModels
{
    [QueryProperty(nameof(IsFromEdit), nameof(IsFromEdit))]
    class ΕίδοςDetailViewModel : BaseViewModel
    {
        
        public UnitOfWork uow = ((App)Application.Current).uow;
        // UnitOfWork uow = new UnitOfWork();
        public static Είδος editeidos;
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
            }
        }
        public XPCollection<ΦΠΑ> ΦΠΑ { get; set; }
        public XPCollection<ΟικογένειαΕίδους> ΟικογένειαΕίδους { get; set; }
        public XPCollection<ΥποοικογένειαΕίδους> ΥποοικογένειαΕίδους { get; set; }
        public XPCollection<ΟμάδαΕίδους> ΟμάδαΕίδους { get; set; }
        public XPCollection<ΚατηγορίαΕίδους> ΚατηγορίαΕίδους { get; set; }

        public bool isFromEdit = false;
        public bool IsFromEdit
        {
            get
            {
                return isFromEdit;
            }
            set
            {
                isFromEdit = value;
                SetProperty(ref isFromEdit, value);
                SetTitle();
            }
        }

        public ΕίδοςDetailViewModel()
        {
            if(editeidos == null)
            {
                Eidos = new Είδος(uow);
                Eidos.SmartOid = Guid.NewGuid();
                Eidos.ΗμνίαΔημ = DateTime.Now;
            }
            else
            {
                Eidos = editeidos;
            }
           
            ΦΠΑ = new XPCollection<ΦΠΑ>(uow);
            ΟικογένειαΕίδους = new XPCollection<ΟικογένειαΕίδους>(uow);
            ΥποοικογένειαΕίδους = new XPCollection<ΥποοικογένειαΕίδους>(uow);
            ΟμάδαΕίδους = new XPCollection<ΟμάδαΕίδους>(uow);
            ΚατηγορίαΕίδους = new XPCollection<ΚατηγορίαΕίδους>(uow);
            Αποθήκευση = new Command(Save);
            Πίσω = new Command(GoBack);
        }
        private void SetTitle()
        {
            if (IsFromEdit)
            {
                Title = "Επεξεργασια";
            }
            else
            {
                Title = "Νέο Είδος";
            }
        }
        private async void Save(object obj)
        {
            if (uow.InTransaction)
            {
                 uow.CommitChanges();
               
                //Address.Customer = appShell.customer1;
            }
            //tell the app that the save button is pressed
       
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
        public ICommand Πίσω { get; set; }
    }
}
