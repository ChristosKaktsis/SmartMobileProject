using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΤΚDetailViewModel : BaseViewModel
    {
        UnitOfWork uow = new UnitOfWork();
        public ΤΚDetailViewModel()
        {
            Πόλης = new XPCollection<Πόλη>(uow);
            Αποθήκευση = new Command(Save);
        }
        ΤαχυδρομικόςΚωδικός τκ;
        public ΤαχυδρομικόςΚωδικός ΤΚ
        {
            get
            {
                return τκ;
            }
            set
            {
                SetProperty(ref τκ, value);
                Όνομα = value.Ονοματκ;
                Πόλη = uow.GetObjectByKey<Πόλη>(value.Πόλη.Oid);
                Περιοχή = value.Περιοχή;
                Νομός = value.Νομός;
                Χώρα = value.Χώρα;
            }
        }
        string όνομα;
        public string Όνομα
        {
            get
            {
                return όνομα;
            }
            set
            {
                SetProperty(ref όνομα, value);
            }
        }
        string περιοχή;
        public string Περιοχή
        {
            get
            {
                return περιοχή;
            }
            set
            {
                SetProperty(ref περιοχή, value);
            }
        }
        string νομός;
        public string Νομός
        {
            get
            {
                return νομός;
            }
            set
            {
                SetProperty(ref νομός, value);
            }
        }
        string χώρα;
        public string Χώρα
        {
            get
            {
                return χώρα;
            }
            set
            {
                SetProperty(ref χώρα, value);
            }
        }
        Πόλη πόλη;
        public Πόλη Πόλη
        {
            get
            {
                return πόλη;
            }
            set
            {
                SetProperty(ref πόλη, value);      
            }
        }
        public XPCollection<Πόλη> Πόλης { get; set; }
        private async void Save(object obj)
        {
            if (string.IsNullOrEmpty(Όνομα))
                return;

            ΤαχυδρομικόςΚωδικός ταχ;
            if (ΤΚ == null)
            {
                ταχ = new ΤαχυδρομικόςΚωδικός(uow);
                ταχ.SmartOid = Guid.NewGuid();
            }
            else
            {
                ταχ = uow.GetObjectByKey<ΤαχυδρομικόςΚωδικός>(ΤΚ.Oid);
            }

            ταχ.Ονοματκ = Όνομα;
            ταχ.Πόλη = Πόλη;
            ταχ.Νομός = Νομός;
            ταχ.Περιοχή = Περιοχή;
            ταχ.Χώρα = Χώρα;
            await uow.CommitTransactionAsync();
            
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}
