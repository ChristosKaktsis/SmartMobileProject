using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΕνέργειαDetailViewModel : BaseViewModel
    {
        Εργασία εργασία;
        public Εργασία Εργασία
        {
            get => εργασία;
            set => SetProperty(ref εργασία, value);
        }
        Ενέργεια ενέργεια;
        public Ενέργεια Ενέργεια
        {
            get => ενέργεια;
            set => SetProperty(ref ενέργεια, value);
        }
        private ΙδιότηταΕνέργειας τύποςενέργειας;
        public ΙδιότηταΕνέργειας ΤύποςΕνέργειας
        {
            get
            {
              return τύποςενέργειας;
            }
            set
            {
                SetProperty(ref τύποςενέργειας, value);
                Ενέργεια.Τύπος = value.Περιγραφή;
                Ενέργεια.Num = value.Τύποςιδιότητας;
                ΕπιλογέςΙδιότητας = value.ΕπιλογήΙδιότητας;
            }
        }
        private ΕπιλογήΙδιότητας επιλογήιδιότητας;
        public ΕπιλογήΙδιότητας ΕπιλογήΙδιότητας
        {
            get
            {
                return επιλογήιδιότητας;
            }
            set
            {
                SetProperty(ref επιλογήιδιότητας, value);
                Ενέργεια.Επιλογή = value.Περιγραφή;
            }
        }
        public XPCollection<ΙδιότηταΕνέργειας> ΙδιότηταΕνέργειας { get; set; }
        public XPCollection<ΕπιλογήΙδιότητας> ΕπιλογέςΙδιότητας { get; set; }
        public FileResult file;
        ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set => SetProperty(ref imageSource, value);
        }
        public ΕνέργειαDetailViewModel()
        {
            Εργασία = ΕργασίεςStaticViewModel.εργασία;
            Ενέργεια = ΕργασίεςStaticViewModel.ενέργεια;
            ΙδιότηταΕνέργειας = new XPCollection<ΙδιότηταΕνέργειας>(ΕργασίεςStaticViewModel.uow);
            if (τύποςενέργειας == null)
            {
                τύποςενέργειας = ΙδιότηταΕνέργειας.ToList().Find(x => x.Περιγραφή==Ενέργεια.Τύπος);
                Ενέργεια.Ημερ = DateTime.Now;
            }
            if (Ενέργεια.Επιλογή != null)
            {
                ΕπιλογήΙδιότητας = τύποςενέργειας.ΕπιλογήΙδιότητας.ToList().Find(x => x.Περιγραφή == Ενέργεια.Επιλογή);
            }
            SetImage(Ενέργεια.Image);
            Αποθήκευση = new Command(Save);
        }
        private async void Save(object obj)
        {
            if (string.IsNullOrEmpty(Ενέργεια.Τύπος))
                Ενέργεια.Τύπος = "(χωρίς Τύπο)";
            if (ΕργασίεςStaticViewModel.uow.InTransaction)
            {
                Εργασία.Ενέργειες.Add(Ενέργεια);
                ΕργασίεςStaticViewModel.uow.CommitChanges();
            }
            await Shell.Current.GoToAsync("..");
        }

        public  void SetImage(string fileResult)
        {
            if (fileResult != null)
            {
                var stream = fileResult;
                ImageSource =  ImageSource.FromFile(stream);
                Ενέργεια.Image = fileResult;
            }
        }
        public ICommand Αποθήκευση { get; set; }
    }
}
