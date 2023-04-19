using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Repositories;
using SmartMobileProject.Services;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΠαραστατικόViewModel : BaseViewModel
    {
        public ObservableCollection<ΠαραστατικάΠωλήσεων> OrderCollection 
        { get; } = new ObservableCollection<ΠαραστατικάΠωλήσεων>();
       
        public ΠαραστατικόViewModel()
        {
            Title = "Πωλήσεις";
            ΔημιουργίαΠαραστατικού = new Command(CreateOrder);
            ΤροποποίησηΠαρασατικού = new Command(EditOrder);
            Εκτύπωση = new Command(Print);
            ΔιαγραφήΠαρασατικού = new Command(DeleteOrder);
            Αποστολή_Email = new Command<ΠαραστατικάΠωλήσεων>(SendEmailClicked);
            LoadMoreDocs = new Command(LoadMore);
        }
        public void OnAppearing()
        {
            //SetPolitis();
            LoadDocs();
        }
        //private void SetPolitis()
        //{
        //    AppShell app = (AppShell)Application.Current.MainPage;
        //    var p = uow.Query<Πωλητής>().Where(x => x.Oid == app.πωλητής.Oid);
        //    ΝέοΠαραστατικόViewModel.politis = p.FirstOrDefault();
        //}
        private async void LoadDocs()
        {
            IsBusy = true;
            try
            {
                using(UnitOfWork uow = new UnitOfWork())
                {
                    OrderCollection.Clear();
                    var items = await uow.Query<ΠαραστατικάΠωλήσεων>()
                        .Where(d => d.Πωλητής.SmartOid == App.Πωλητής.SmartOid)
                        .Where(d => d.Πελάτης.Επωνυμία.Contains(Search_Text) || d.Πελάτης.ΑΦΜ.Contains(Search_Text))
                        .OrderByDescending(p => p.ΗμνίαΔημ)
                        .Skip(OrderCollection.Count).Take(13)
                        .ToListAsync();
                    items.ForEach(item => OrderCollection.Add(item));
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
                    var items = await uow.Query<ΠαραστατικάΠωλήσεων>()
                    .Where(d => d.Πωλητής.SmartOid == App.Πωλητής.SmartOid)
                    .Where(d => d.Πελάτης.Επωνυμία.Contains(Search_Text) || d.Πελάτης.ΑΦΜ.Contains(Search_Text))
                    .OrderByDescending(p => p.ΗμνίαΔημ)
                    .Skip(OrderCollection.Count).Take(13)
                    .ToListAsync();
                    items.ForEach(item => OrderCollection.Add(item));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsRefreshing = false; }
        }
        private async void SendEmailClicked(ΠαραστατικάΠωλήσεων obj)
        {
            if (obj == null)
                return;
            var subject = $"Αποστολή παραστατικού {obj.Παραστατικό}";
            var body = "";
            var reccipients = new List<string> { obj.Πελάτης.Email};
            await SendEmail(subject,body,reccipients);
        }
        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                Console.WriteLine(fbsEx);
            }
            catch (Exception ex)
            {
                // Some other exception occurred
                Console.WriteLine(ex);
            }
        }
        private async void CreateOrder(object obj)
        {
            DocHelperViewModel.Order = null;
            //ΝέοΠαραστατικόViewModel.uow = uow;
            DocHelperViewModel.πελατης = null;

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
            //ΝέοΠαραστατικόViewModel.uow = uow;
            DocHelperViewModel.Order = editItem;
            await Shell.Current.GoToAsync(nameof(ΠαραστατικόΒασικάΣτοιχείαPage));
        }
        public async void Print(object obj)
        {
            CreatePrintView createPrintView = new CreatePrintView();
            try
            {
                var item = obj as ΠαραστατικάΠωλήσεων;
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var order = await uow.GetObjectByKeyAsync<ΠαραστατικάΠωλήσεων>(item.Oid);
                    string print = await createPrintView.page2(order);
                    createPrintView.CreatePrint(print);
                }
            }
            catch(Exception ex) { Debug.WriteLine(ex); }
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
            var answer = await Application.Current.MainPage
                .DisplayAlert("Ερώτηση?", "Θέλετε να γίνει η διαγραφή ", "Ναί", "Όχι");
            if (!answer) return;
            try
            {
                SalesDocRepository repository = new SalesDocRepository();
                var result = await repository.DeleteItemAsync(order.SmartOid.ToString());
                if(result) OrderCollection.Remove(order);
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
        public ICommand OpenWebCommand { get; }
        public ICommand ΔημιουργίαΠαραστατικού { get; set; }
        public ICommand ΤροποποίησηΠαρασατικού { get; set; }
        public ICommand Εκτύπωση { set; get; }
        public ICommand ΔιαγραφήΠαρασατικού { get; set; }
        public Command<ΠαραστατικάΠωλήσεων> Αποστολή_Email { get; set; }
        public Command LoadMoreDocs { get; }
    }
}
