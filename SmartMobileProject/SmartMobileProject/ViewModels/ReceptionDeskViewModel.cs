using System;
using System.Collections.Generic;
using System.ComponentModel;
using SmartMobileProject.Models;
using DevExpress.Xpo;
using Xamarin.Forms;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;

namespace SmartMobileProject.ViewModels
{
    class ReceptionDeskViewModel : BaseViewModel
    {
        UnitOfWork uow = new UnitOfWork();
        public XPCollection<Appointment> Appointments { get; protected set; }
        public List<Appointment> Appointments2 { get; set; }
        public DateTime StartDate { get { return DateTime.Now; } }
       
        public ReceptionDeskViewModel()
        {
            Title ="Προγραμματισμός";
            Appointments = new XPCollection<Appointment>(uow, getAppointments());
            //Appointments.CollectionChanged += Appointments_CollectionChanged;
            //foreach (Appointment appointment in Appointments)
            //    SubscribeAppointmentEvent(appointment);

            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);
            CommitChanges = new Command(Commit);
        }
        private List<Appointment> getAppointments()
        {
            var a = uow.Query<Appointment>().Where(x => x.Caption == ((AppShell)Application.Current.MainPage).πωλητής.Ονοματεπώνυμο);
            return a.ToList();
        }
        private void Appointments_CollectionChanged(object sender, XPCollectionChangedEventArgs e)
        {
            var aw = e.ChangedObject as Appointment;
            if(e.CollectionChangedType == XPCollectionChangedType.AfterRemove)
            {
                UnSubscribeAppointmentEvent(aw);
            }
            else
            {
                SubscribeAppointmentEvent(aw);
            }
                Commit(aw);
        }

        public void SubscribeAppointmentEvent(Appointment apt)
        {
            apt.Changed += Apt_Changed;
        }

        public void UnSubscribeAppointmentEvent(Appointment apt)
        {
            apt.Changed -= Apt_Changed;
        }
        private void Apt_Changed(object sender, ObjectChangeEventArgs e)
        {
            Commit(null);
        }

        public void Save(Object obj)
        {
            var app = (Appointment)obj;
            app.Caption = ((AppShell)Application.Current.MainPage).πωλητής.Ονοματεπώνυμο;
            uow.Save((Appointment)obj);
            Commit(null);
            
        }
        public void Reload()
        {
            Appointments.AddRange(getAppointments());
        }
        public void Delete(Object obj)
        {
            uow.Delete((Appointment)obj);
            Commit(null);
            
        }
        public void Commit(Object obj)
        {
            if (uow.InTransaction)
                uow.CommitTransaction();
        }
        
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CommitChanges { get; set; }
    }
}
