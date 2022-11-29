using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΓραμμέςΠαραστατικώνΠωλήσεωνViewModel : BaseViewModel
    {
        UnitOfWork uow = ΝέοΠαραστατικόViewModel.uow;
       
        public XPCollection<ΓραμμέςΠαραστατικώνΠωλήσεων> LineOfOrdersCollection { get; set; }

        public ΓραμμέςΠαραστατικώνΠωλήσεωνViewModel()
        {
            
            LineOfOrdersCollection = ΝέοΠαραστατικόViewModel.Order.ΓραμμέςΠαραστατικώνΠωλήσεων;
            LineOfOrdersCollection.DeleteObjectOnRemove = true;

            //titlos kai arith seiras
            SetTitle();
            //

            ΝέαΓραμμή = new Command(CreateLineOfOrder);
            ΓρήγορηΕπιλογή = new Command(QuickPick);
            Scanner = new Command(GotoScanner);
            BarCodeSelection = new Command(OnBarCodeSelectionPressed);
            ΕπεξεργασίαΓραμμής = new Command(EditLineOfOrder);
            ΔιαγραφήΓραμμής = new Command(DeleteLineOfOrder);
            Ολοκλήρωση = new Command(Submission);
        }
        private void SetTitle()
        {
            SetOrderSeiraCounter();
            
            Title = ΝέοΠαραστατικόViewModel.Order.Παραστατικό;
        }

        private void SetOrderSeiraCounter()
        {

            if (ΝέοΠαραστατικόViewModel.Order.Σειρά == null) return;
            if (!string.IsNullOrEmpty(ΝέοΠαραστατικόViewModel.Order.Παραστατικό)) return;

            var p = ΝέοΠαραστατικόViewModel.Order.Σειρά.Counter++;
            ΝέοΠαραστατικόViewModel.Order.Παραστατικό = ΝέοΠαραστατικόViewModel.Order.Σειρά.Σειρά + p.ToString().PadLeft(9, '0');
        }

        private async void QuickPick(object obj)
        {
            await Shell.Current.GoToAsync(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνQuickPickDetailViewPage));
        }
        private async void GotoScanner(object obj)
        {
            await Shell.Current.GoToAsync(nameof(ScannerViewPage));
        }
        private async void OnBarCodeSelectionPressed(object obj)
        {
            await Shell.Current.GoToAsync(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνΕπιλογήBarCodePage));
        }
        private async void CreateLineOfOrder(object obj)
        {
            ΝέοΠαραστατικόViewModel.editline = null;
            await Shell.Current.GoToAsync(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνDetailViewPage));
        }
        private async void EditLineOfOrder(object obj)
        {
            ΝέοΠαραστατικόViewModel.editline = (ΓραμμέςΠαραστατικώνΠωλήσεων)obj; 
            await Shell.Current.GoToAsync(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνDetailViewPage));
        }
        private async void DeleteLineOfOrder(object obj)
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να γίνει η διαγραφή ", "Ναί", "Όχι");

            if (answer)
            {
                ΓραμμέςΠαραστατικώνΠωλήσεων line = (ΓραμμέςΠαραστατικώνΠωλήσεων)obj;
                line.Delete();

            }
        }
        private async void Submission(object obj)
        {
            if (ΝέοΠαραστατικόViewModel.Order.ΓραμμέςΠαραστατικώνΠωλήσεων.Any())
            {
                await Shell.Current.GoToAsync(nameof(ΠαραστατικόΟλοκλήρωσηViewPage));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Δεν είναι έγκυρη", "Το παραστατικό πρέπει να έχει γραμμές","Οκ");
            }
            
        }
        public ICommand ΝέαΓραμμή { get; set; }
        public ICommand ΓρήγορηΕπιλογή { get; set; }
        public ICommand Scanner { set; get; }
        public ICommand BarCodeSelection { set; get; }
        public ICommand ΕπεξεργασίαΓραμμής { get; set; }
        public ICommand ΔιαγραφήΓραμμής { get; set; }
        public ICommand Ολοκλήρωση { get; set; }
    }
}
