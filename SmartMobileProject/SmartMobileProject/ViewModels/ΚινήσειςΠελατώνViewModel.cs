using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    [QueryProperty(nameof(CustomerID), nameof(CustomerID))]
    public class ΚινήσειςΠελατώνViewModel : BaseViewModel
    {
        private string _CustomerID;
        private string _ΤρέχωνΥπόλοιπο;
        private string _ΗμερΤελευτΧρεωσ;
        private string _ΗμερΤελευτΠιστ;

        public ObservableCollection<ΚινήσειςΠελατών> KiniseisList { get; set; }
        public Command LoadItemsCommand { get; set; }
        public ΚινήσειςΠελατώνViewModel()
        {
            KiniseisList = new ObservableCollection<ΚινήσειςΠελατών>();
            LoadItemsCommand = new Command(async ()=> await OnLoaditems());
        }
        
        private async Task OnLoaditems()
        {
            try
            {  
                IsBusy = true;
                if(string.IsNullOrEmpty(CustomerID))
                    return;
                KiniseisList.Clear();
                var completed = await XpoHelper.CreateKINISEIS(CustomerID);
                ΤρέχωνΥπόλοιπο = await XpoHelper.GetCurrentYPOLOIPO(CustomerID);
                
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var items = uow.Query<ΚινήσειςΠελατών>().Where(x=>x.Πελάτης==CustomerID).OrderBy(x=> x.Ημνία);
                    foreach (var item in items)
                    {                        
                        KiniseisList.Add(item);
                    }

                    SwapItems();
                    CalculatePrYpol();
                    SetLastDates();
                }                         
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Κάτι πήγε λάθος στο φόρτομα Κινήσεων" + e);               
            }
            finally
            {
                IsBusy = false;
            }
        }
        /// <summary>
        /// Αμα οι έχουν την ιδια μέρα και
        /// έχουν μπει : Πρώτα Πίστωση και μετά χρέωση :
        /// Γίνεται αντιστροφή ωστε να φανούν σωστα στον πίνακα
        /// </summary>
        private void SwapItems()
        {
            var limit = KiniseisList.Count;
            for (int i = 0; i < limit; i++)
            {
                var prevIndex = i - 1;
                if (prevIndex != -1)
                {
                    var previtem = KiniseisList[prevIndex];
                    if (KiniseisList[i].Ημνία == previtem.Ημνία)
                    {
                        if (previtem.Χρέωση == 0 && previtem.Πίστωση != 0)
                        {

                            KiniseisList.Insert(prevIndex, KiniseisList[i]);
                            KiniseisList.RemoveAt(i + 1);
                        }
                    }
                }
            }
        }
        private void SetLastDates()
        {
            string XreosilastDate = string.Empty;
            string PistosiLastDate = string.Empty;
            foreach (var item in KiniseisList)
            {
                XreosilastDate = item.Χρέωση != 0 ? item.Ημνία : XreosilastDate;
                PistosiLastDate = item.Πίστωση != 0 ? item.Ημνία : PistosiLastDate;
            }
            ΗμερΤελευτΠιστ = PistosiLastDate;
            ΗμερΤελευτΧρεωσ = XreosilastDate;
        }
        private void CalculatePrYpol()
        {
            foreach (var item in KiniseisList)
            {
                var index = KiniseisList.IndexOf(item) - 1;
                if (index != -1)
                {
                    var previtem = KiniseisList[index];
                    item.ΠροοδευτικόΥπόλοιπο = item.Υπόλοιπο + previtem.ΠροοδευτικόΥπόλοιπο;
                }
                else
                {
                    item.ΠροοδευτικόΥπόλοιπο = item.Υπόλοιπο;
                }
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
        public string CustomerID
        {
            get
            {
                return _CustomerID;
            }
            set
            {
                _CustomerID = value;              
            }
        }
        public string ΤρέχωνΥπόλοιπο
        {
            get
            {
                return _ΤρέχωνΥπόλοιπο;
            }
            set
            {             
                SetProperty(ref _ΤρέχωνΥπόλοιπο, value);
            }
        }
        public string ΗμερΤελευτΧρεωσ
        {
            get
            {
                return _ΗμερΤελευτΧρεωσ;
            }
            set
            {
                SetProperty(ref _ΗμερΤελευτΧρεωσ, value);
            }
        }
        public string ΗμερΤελευτΠιστ
        {
            get
            {
                return _ΗμερΤελευτΠιστ;
            }
            set
            {
                SetProperty(ref _ΗμερΤελευτΠιστ, value);
            }
        }
    }
}
