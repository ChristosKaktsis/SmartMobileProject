using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ScannerViewModel : BaseViewModel
    {
        string scanresult;
        UnitOfWork uow;
        ΓραμμέςΠαραστατικώνΠωλήσεων lineOfOrders;
        public XPCollection<Είδος> EidosCollection { get; set; }

        public ScannerViewModel()
        {
            uow = ΝέοΠαραστατικόViewModel.uow;         
            EidosCollection = new XPCollection<Είδος>(uow);
        }

        public string Scanresult
        {
            get
            {
                return scanresult;
            }
            set
            {
                SetProperty(ref scanresult, value);
                GoBack();
            }
        }

        private async void GoBack()
        {
            lineOfOrders = new ΓραμμέςΠαραστατικώνΠωλήσεων(uow);
            lineOfOrders.SmartOid = Guid.NewGuid();
            lineOfOrders.Είδος = uow.FindObject<Είδος>(CriteriaOperator.Parse(
            "Κωδικός = ? ", Scanresult));
            if (lineOfOrders.Είδος == null)
            {
                return;
            }
            ΝέοΠαραστατικόViewModel.editline = lineOfOrders;
            await Shell.Current.GoToAsync("..");
            await Shell.Current.GoToAsync(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνDetailViewPage));
        }
    }
}
