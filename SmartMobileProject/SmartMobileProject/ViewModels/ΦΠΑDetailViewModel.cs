using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΦΠΑDetailViewModel : BaseViewModel
    {
        public ΦΠΑDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        ΦΠΑ φπα;
        public ΦΠΑ ΦΠΑ
        {
            get
            {
                return φπα;
            }
            set
            {
                SetProperty(ref φπα, value);
               // Όνομα = value.ΟνομαΠόλης;
            }
        }
        float fpakanoniko;
        float fpameiomeno;
        float fpaeksairesi;
        public float FpaKanoniko
        {
            get
            {
                return fpakanoniko;
            }
            set
            {
                SetProperty(ref fpakanoniko, value);
            }
        }
        public float FpaMeiomeno
        {
            get
            {
                return fpameiomeno;
            }
            set
            {
                SetProperty(ref fpameiomeno, value);
            }
        }
        public float FpaEksairesi
        {
            get
            {
                return fpaeksairesi;
            }
            set
            {
                SetProperty(ref fpaeksairesi, value);
            }
        }
        private async void Save(object obj)
        {           
            using (UnitOfWork uow = new UnitOfWork())
            {
                ΦΠΑ φπας;
                if (ΦΠΑ == null)
                {
                    φπας = new ΦΠΑ(uow);
                    φπας.Φπαid = FpaKanoniko.ToString();
                }
                else
                {
                    φπας = uow.GetObjectByKey<ΦΠΑ>(ΦΠΑ.Oid);
                }

                φπας.Φπακανονικό = FpaKanoniko;
                φπας.Φπαμειωμένο = FpaMeiomeno;
                φπας.Φπαεξαίρεση = fpaeksairesi;
                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}
