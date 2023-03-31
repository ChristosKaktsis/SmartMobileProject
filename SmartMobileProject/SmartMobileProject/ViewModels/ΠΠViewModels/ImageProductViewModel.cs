using DevExpress.Data.Linq.Helpers;
using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.ViewModels.ΠΠViewModels.TemplatesViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    public class ImageProductViewModel : BaseViewModel
    {
        private UnitOfWork uow;
        public ObservableCollection<ImageItemViewModel> ProductCollection 
        { get; } = new ObservableCollection<ImageItemViewModel>();
        public ObservableCollection<ΟικογένειαΕίδους> FamilyCollection 
        { get; } = new ObservableCollection<ΟικογένειαΕίδους>();
        public List<ImageItemViewModel> Cart { get; } = new List<ImageItemViewModel>();
        public Command LoadCommand { get;}
        public Command LoadMoreCommand { get; }
        public Command SaveCommand { get; }

        public ImageProductViewModel()
        {
            uow = DocHelperViewModel.uow;
            LoadCommand = new Command( async() => await LoadProducts() );
            LoadMoreCommand = new Command(async () => await LoadMoreProducts());
            SaveCommand = new Command(SaveProducts);
        }

        private string _Search_Text = string.Empty;

        public string Search_Text
        {
            get { return _Search_Text; }
            set 
            { 
                _Search_Text = value;
                OnAppearing();
            }
        }

        private ΟικογένειαΕίδους _SelectedFamily;

        public ΟικογένειαΕίδους SelectedFamily
        {
            get { return _SelectedFamily; }
            set 
            {
                _SelectedFamily = value;
                OnAppearing();
            }
        }

        public async void OnAppearing()
        {
            AddToCart();
            await LoadProducts();
            await LoadProductFamily();
        }

        private void AddToCart()
        {
            try
            {
                foreach (var item in ProductCollection)
                {
                    if (item.Ποσότητα > 0)
                        if(Cart.All(i => i.Product != item.Product))
                            Cart.Add(item);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task LoadProducts()
        {
            try
            {
                IsBusy = true;
                ProductCollection.Clear();
                if (SelectedFamily == null) await LoadAndFill();
                else await LoadAndFill(SelectedFamily);

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }
        private async Task LoadMoreProducts()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(1000);
                if (SelectedFamily == null) await LoadAndFill();
                else await LoadAndFill(SelectedFamily);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }
        private async Task LoadAndFill()
        {
            var items = await uow.Query<Είδος>()
                    .Where(i => i.Περιγραφή.Contains(Search_Text))
                    .OrderBy(i => i.Περιγραφή).Skip(ProductCollection.Count).Take(10).ToListAsync();
            items.ForEach(i => ProductCollection.Add(new ImageItemViewModel { Product = i }));
        }
        private async Task LoadAndFill(ΟικογένειαΕίδους family)
        {
            var items = await uow.Query<Είδος>()
                    .Where(i=>i.Οικογένεια==family)
                    .Where(i => i.Περιγραφή.Contains(Search_Text))
                    .OrderBy(i => i.Περιγραφή).Skip(ProductCollection.Count).Take(10).ToListAsync();
            items.ForEach(i => ProductCollection.Add(new ImageItemViewModel { Product = i }));
        }
        private async Task LoadProductFamily()
        {
            try
            {
                FamilyCollection.Clear();
                var items = await uow.Query<ΟικογένειαΕίδους>().OrderBy(i => i.Περιγραφή).ToListAsync();
                items.ForEach(i => FamilyCollection.Add(i));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }
        private async void SaveProducts()
        {
            try
            {
                AddToCart();
                foreach(var item in Cart)
                {
                    CreateNewLine(item);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            await Shell.Current.GoToAsync("..");
        }

        private void CreateNewLine(ImageItemViewModel item)
        {
            var newLine = new ΓραμμέςΠαραστατικώνΠωλήσεων(uow);
            newLine.SmartOid = Guid.NewGuid();
            newLine.Είδος = item.Product;
            newLine.ΑξίαΕκπτωσης = item.ΑξίαΕκπτωσης;
            newLine.ΑξίαΓραμμής = item.ΑξίαΓραμμής;
            newLine.ΚαθαρήΑξία = item.ΚαθαρήΑξία;
            newLine.Εκπτωση = item.Εκπτωση;
            newLine.Φπα = item.Φπα;
            newLine.ΠοσοστόΦπα = item.ΠοσοστόΦπα;
            newLine.Τιμή = item.Τιμή;
            newLine.Ποσότητα = item.Ποσότητα;
            newLine.ΠαραστατικάΠωλήσεων = DocHelperViewModel.Order;
        }
    }
}
