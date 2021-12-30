using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        public ObservableCollection<ΚινήσειςΠελατών> KiniseisList { get; set; }
        public Command LoadItemsCommand { get; set; }
        public ΚινήσειςΠελατώνViewModel()
        {
            KiniseisList = new ObservableCollection<ΚινήσειςΠελατών>();
            LoadItemsCommand = new Command(async () => await OnLoaditems());
        }

        private async Task<bool> OnLoaditems()
        {
            try
            {
                IsBusy = true;
                if(string.IsNullOrEmpty(CustomerID))
                    return await Task.FromResult(false);

                KiniseisList.Clear();
                var completed = await XpoHelper.CreateKINISEIS(CustomerID);
                ΤρέχωνΥπόλοιπο = await XpoHelper.GetCurrentYPOLOIPO(CustomerID);
                if (!completed)
                    return await Task.FromResult(false);

                using (UnitOfWork uow = new UnitOfWork())
                {
                    var items = uow.Query<ΚινήσειςΠελατών>().Where(x=>x.Πελάτης==CustomerID).OrderBy(x=> x.Ημνία);
                    foreach (var item in items)
                    {                        
                        KiniseisList.Add(item);
                    }
                    CalculatePrYpol();
                        
                }               
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Κάτι πήγε λάθος στο φόρτομα barcode " + e);
                return await Task.FromResult(false);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CalculatePrYpol()
        {         
            
            foreach (var item in KiniseisList)
            {
                var index = KiniseisList.IndexOf(item) - 1;
                if(index != -1)
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

    }
}
