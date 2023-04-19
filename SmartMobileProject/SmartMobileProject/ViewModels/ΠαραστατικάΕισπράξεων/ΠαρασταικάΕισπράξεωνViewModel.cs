using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Repositories;
using SmartMobileProject.Services;
using SmartMobileProject.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΠαρασταικάΕισπράξεωνViewModel : BaseViewModel
    {
        public ObservableCollection<ΠαραστατικάΕισπράξεων> DocCollection
        { get; } = new ObservableCollection<ΠαραστατικάΕισπράξεων>();

        public ΠαρασταικάΕισπράξεωνViewModel()
        {
            Title = "Εισπράξεις";
            LoadMoreCommand = new Command(LoadMore);
            ΔημιουργίαΠαραστατικού = new Command(CreateOrder);
            ΤροποποίησηΠαρασατικού = new Command(EditOrder);
            Εκτύπωση = new Command(Print);
            ΔιαγραφήΠαρασατικού = new Command(DeleteOrder);
        }
        public void OnAppearing()
        {
            LoadDocs();
        }
        private async void LoadDocs()
        {
            IsBusy = true;
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    DocCollection.Clear();
                    var items = await uow.Query<ΠαραστατικάΕισπράξεων>()
                        .Where(d => d.Πωλητής.SmartOid == App.Πωλητής.SmartOid)
                        .Where(d => d.Πελάτης.Επωνυμία.Contains(Search_Text) || d.Πελάτης.ΑΦΜ.Contains(Search_Text))
                        .OrderByDescending(p => p.ΗμνίαΔημ)
                        .Skip(DocCollection.Count).Take(13)
                        .ToListAsync();
                    items.ForEach(item => DocCollection.Add(item));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }
        private async void LoadMore()
        {
            if (IsBusy) return;
            try
            {
                using(UnitOfWork uow = new UnitOfWork())
                {
                    var items = await uow.Query<ΠαραστατικάΕισπράξεων>()
                   .Where(d => d.Πωλητής.SmartOid == App.Πωλητής.SmartOid)
                   .Where(d => d.Πελάτης.Επωνυμία.Contains(Search_Text) || d.Πελάτης.ΑΦΜ.Contains(Search_Text))
                   .OrderByDescending(p => p.ΗμνίαΔημ)
                   .Skip(DocCollection.Count).Take(13)
                   .ToListAsync();
                    items.ForEach(item => DocCollection.Add(item));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsRefreshing = false; }
        }
        private async void CreateOrder(object obj)
        {
            DocCollectHelper.ParastatikoEispr = null;
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
            DocCollectHelper.ParastatikoEispr = editItem;
            await Shell.Current.GoToAsync(nameof(ΠαραστατικόΕισπράξεωνΒασικάΣτοιχείαPage));
        }
        public async void Print(object obj)
        {
            CreatePrintView createPrintView = new CreatePrintView();
            try
            {
                var item = obj as ΠαραστατικάΕισπράξεων;
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var doc = await uow.GetObjectByKeyAsync<ΠαραστατικάΕισπράξεων>(item.Oid);
                    await createPrintView.CreatePrint3(doc);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
        private async void DeleteOrder(object sender)
        {
            ΠαραστατικάΕισπράξεων doc = (ΠαραστατικάΕισπράξεων)sender;
            if (doc.IsUploaded)
            {
                await Application.Current.MainPage.DisplayAlert("Alert",
                    "Δεν Μπορεί να γίνει Διαγραφή σε Παραστατικό που έχει επικυρωθεί !",
                    "OK");
                return;
            }
            var answer = await Application.Current.MainPage.DisplayAlert("Ερώτηση?", "Θέλετε να γίνει η διαγραφή ", "Ναί", "Όχι");
            if (!answer) return;
            try
            {
                CollectionDocRepository repository = new CollectionDocRepository();
                var result = await repository.DeleteItemAsync(doc.SmartOid.ToString());
                if (result) DocCollection.Remove(doc);
                Debug.WriteLine($"Deleted:{result}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert("Alert", "Κάτι πήγε στραβά", "Οκ");
            }
        }
        private string _Search_Text = string.Empty;
        public string Search_Text
        {
            get { return _Search_Text; }
            set
            {
                _Search_Text = value;
                LoadDocs();
            }
        }
        public Command LoadMoreCommand { get;  }
        public ICommand ΔημιουργίαΠαραστατικού { get;  }
        public ICommand ΤροποποίησηΠαρασατικού { get;  }
        public ICommand Εκτύπωση { get; }
        public ICommand ΔιαγραφήΠαρασατικού { get; }
    }
}
