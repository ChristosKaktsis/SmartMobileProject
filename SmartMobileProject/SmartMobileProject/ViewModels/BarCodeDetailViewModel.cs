using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class BarCodeDetailViewModel : BaseViewModel
    {
        private BarCodeΕίδους _είδοςBarCode;
        private string _κωδικός;
        private double _τιμήΧονδρικής;
        private string _περιγραφή;

        public ICommand SaveBarCodeCommand { get; set; }
        public BarCodeDetailViewModel()
        {
            SaveBarCodeCommand = new Command(OnSaveBarCode);
        }
        public BarCodeΕίδους ΕίδοςBarCode
        {
            get
            {
                return _είδοςBarCode;
            }
            set
            {
                SetProperty(ref _είδοςBarCode, value);   
            }
        }
        
        public string Κωδικός
        {
            get { return _κωδικός; }
            set { SetProperty(ref _κωδικός, value); }
        }
        public string Περιγραφή
        {
            get { return _περιγραφή; }
            set { SetProperty(ref _περιγραφή, value); }
        }
        public double ΤιμήΧονδρικής
        {
            get { return _τιμήΧονδρικής; }
            set { SetProperty(ref _τιμήΧονδρικής, value); }
        }
        private async void OnSaveBarCode(object obj)
        {
            try
            {
                using(UnitOfWork uow = new UnitOfWork())
                {
                    ΕίδοςBarCode = new BarCodeΕίδους(uow);
                    ΕίδοςBarCode.Κωδικός = Κωδικός;
                    ΕίδοςBarCode.Περιγραφή = Περιγραφή;
                    ΕίδοςBarCode.ΤιμήΧονδρικής = ΤιμήΧονδρικής;
                    uow.Save(ΕίδοςBarCode);
                    await uow.CommitTransactionAsync();                  
                }
                await AppShell.Current.GoToAsync("..");
            }
            catch(Exception e)
            {
                Debug.WriteLine("BarCodeDetail Save method error :" + e);
            }
        }
    }
}
