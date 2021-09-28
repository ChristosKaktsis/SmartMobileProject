﻿using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΠωλητήςViewModel : BaseViewModel
    {
        UnitOfWork uow = ((App)Application.Current).uow;
        Πωλητής πωλητής;
        public Πωλητής Πωλητής
        {
            get
            {
                return πωλητής;
            }
            set
            {
                SetProperty(ref πωλητής, value);
            }
        }
        public ΠωλητήςViewModel()
        {
            Πωλητής = ((AppShell)Application.Current.MainPage).πωλητής;
            
            Application.Current.Properties["Πωλητής"] = Πωλητής.Oid.ToString();
            Αποθήκευση = new Command(Save);
        }
        private void Save(object obj)
        {
            if (uow.InTransaction)
            {
                uow.CommitChanges();
            }
        }
        public ICommand Αποθήκευση { set; get; }
    }
}
