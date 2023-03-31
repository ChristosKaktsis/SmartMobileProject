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
        UnitOfWork uow = DocHelperViewModel.uow;
       
        public XPCollection<ΓραμμέςΠαραστατικώνΠωλήσεων> LineOfOrdersCollection { get; set; }

        public ΓραμμέςΠαραστατικώνΠωλήσεωνViewModel()
        {
            
            LineOfOrdersCollection = DocHelperViewModel.Order.ΓραμμέςΠαραστατικώνΠωλήσεων;
            LineOfOrdersCollection.DeleteObjectOnRemove = true;
            //titlos kai arith seiras
            SetTitle();
            //
            ΝέαΓραμμή = new Command(CreateLineOfOrder);
            ΓρήγορηΕπιλογή = new Command(QuickPick);
            ImageSelection = new Command(async () => await Shell.Current.GoToAsync(nameof(ImageProductsPage)));
            Scanner = new Command(GotoScanner);
            BarCodeSelection = new Command(OnBarCodeSelectionPressed);
            ΕπεξεργασίαΓραμμής = new Command(EditLineOfOrder);
            ΔιαγραφήΓραμμής = new Command(DeleteLineOfOrder);
            Ολοκλήρωση = new Command(Submission);
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
                if(!DocHelperViewModel.Order.IsSeiraChanged())
                    return DocHelperViewModel.Order.OrderSeiraCounter();

            return DocHelperViewModel.Order.Σειρά.Counter +1;
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
            DocHelperViewModel.editline = null;
            await Shell.Current.GoToAsync(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνDetailViewPage));
        }
        private async void EditLineOfOrder(object obj)
        {
            DocHelperViewModel.editline = (ΓραμμέςΠαραστατικώνΠωλήσεων)obj; 
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
            if (DocHelperViewModel.Order.ΓραμμέςΠαραστατικώνΠωλήσεων.Any())
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
        public ICommand ImageSelection { get; set; }
        public ICommand Scanner { set; get; }
        public ICommand BarCodeSelection { set; get; }
        public ICommand ΕπεξεργασίαΓραμμής { get; set; }
        public ICommand ΔιαγραφήΓραμμής { get; set; }
        public ICommand Ολοκλήρωση { get; set; }
    }
}
