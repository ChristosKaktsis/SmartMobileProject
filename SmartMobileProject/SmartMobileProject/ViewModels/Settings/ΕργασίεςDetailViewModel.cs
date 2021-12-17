using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΕργασίεςDetailViewModel : BaseViewModel
    {
        Εργασία εργασία;
        public XPCollection<Πελάτης> Πελάτες { get; set; }
        public XPCollection<Ενέργεια> Ενέργειεs { get; set; }
        public ΕργασίεςDetailViewModel()
        {
            Εργασία = ΕργασίεςStaticViewModel.εργασία;
            Πελάτες = ΕργασίεςStaticViewModel.Πελάτες;
            Εργασία.Ενέργειες.Reload();
            Ενέργειεs = Εργασία.Ενέργειες;
            ΝέαΕνέργεια = new Command(NewΕνέργεια);
            Επεξεργασία = new Command(Edit);
            Ακύρωση = new Command(Cancel);
        }
        private async void NewΕνέργεια(object obj)
        {
            ΕργασίεςStaticViewModel.ενέργεια = new Ενέργεια(ΕργασίεςStaticViewModel.uow);
            ΕργασίεςStaticViewModel.ενέργεια.SmartOid = Guid.NewGuid();
            await Shell.Current.GoToAsync(nameof(ΕνέργειαDetailViewPage));
        }
        private async void Edit(object obj)
        {
            ΕργασίεςStaticViewModel.ενέργεια = (Ενέργεια)obj;
            if (ΕργασίεςStaticViewModel.ενέργεια.Τύπος.Contains("Παραστατικό"))
                return;
            await Shell.Current.GoToAsync(nameof(ΕνέργειαDetailViewPage));
        }
        public Εργασία Εργασία
        {
            get => εργασία;
            set => SetProperty(ref εργασία, value);
        }
        public ICommand ΝέαΕνέργεια { get; set; }
        public ICommand Επεξεργασία { get; set; }
        public ICommand Ακύρωση { set; get; }
    }
}
