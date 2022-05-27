using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    public class ΕπιλογήΠροηγΠαρΠωλViewModel
    {
        UnitOfWork uow;
        public XPCollection<ΠαραστατικάΠωλήσεων> OrderCollection { get; }
        public ObservableCollection<ΠαραστατικάΠωλήσεων> PrevSaleOrders { get; set; }
        public ΕπιλογήΠροηγΠαρΠωλViewModel()
        {
            OrderCollection = ΝέοΠαραστατικόViewModel.politis.Παραστατικόπωλήσεων;
            uow = ΝέοΠαραστατικόViewModel.uow;
            PrevSaleOrders = new ObservableCollection<ΠαραστατικάΠωλήσεων>();        
        }
        public async void OnAppearing()
        {
            await LoadPrevSales();
        }
        async Task LoadPrevSales()
        {
            try
            {
                var items = await uow.Query<ΠαραστατικάΠωλήσεων>().Where(
                    x => x.Σειρά.Σειρά == ΝέοΠαραστατικόViewModel.Order.Σειρά.Σειρά
                    && x.Πελάτης.Oid == ΝέοΠαραστατικόViewModel.Order.Πελάτης.Oid).OrderByDescending(
                    d => d.ΗμνίαΔημ).ToListAsync();
                foreach (var item in items)
                    PrevSaleOrders.Add(item);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async void LoadSelection(ΠαραστατικάΠωλήσεων selected)
        {
            var items = await uow.Query<ΓραμμέςΠαραστατικώνΠωλήσεων>().Where(x=>x.ΠαραστατικάΠωλήσεων.Oid == selected.Oid).ToListAsync();
            foreach (var item in items)
            {
                var newLine = new ΓραμμέςΠαραστατικώνΠωλήσεων(uow);
                
                if (uow.InTransaction)
                {
                    newLine.ΑξίαΓραμμής = item.ΑξίαΓραμμής;
                    newLine.ΑξίαΓραμμής = item.ΑξίαΓραμμής;
                    newLine.Είδος = item.Είδος;
                    newLine.Εκπτωση = item.Εκπτωση;
                    newLine.ΚαθαρήΑξία = item.ΚαθαρήΑξία;
                    newLine.Ποσότητα = item.Ποσότητα;
                    newLine.ΠοσοστόΦπα = item.ΠοσοστόΦπα;
                    newLine.Σχολια = item.Σχολια;
                    newLine.Τιμή = item.Τιμή;
                    newLine.Φπα = item.Φπα;
                    newLine.ΠαραστατικάΠωλήσεων = ΝέοΠαραστατικόViewModel.Order;
                }
            }
            await Shell.Current.GoToAsync(nameof(ΓραμμέςΠαραστατικώνΠωλήσεωνListViewPage));
        }
    }
}
