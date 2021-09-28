using DevExpress.XamarinForms.Scheduler;
using SmartMobileProject.Services;
using SmartMobileProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMobileProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ΠρογραμματισμόςViewPage : ContentPage
    {
        readonly RemindersNotificationCenter remindersNotificationCenter =
                                                        new RemindersNotificationCenter();
        ReceptionDeskViewModel model;
        bool inNavigation = false;
        public ΠρογραμματισμόςViewPage()
        {
            InitializeComponent();
            model = BindingContext as ReceptionDeskViewModel;
            schedule.DataStorage.AppointmentItems.CollectionChanged += AppointmentItems_CollectionChanged;
        }

        private void AppointmentItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (AppointmentItem obj in e.NewItems)
                {
                    model.SaveCommand.Execute(obj.SourceObject);
                }
            if(e.OldItems != null)
                foreach (AppointmentItem obj in e.OldItems)
                {
                    model.DeleteCommand.Execute(obj.SourceObject);
                }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.inNavigation = false;
            base.OnBindingContextChanged();
            if (!(BindingContext is ReceptionDeskViewModel model))
                return;
            model.Reload();
        }
        private void schedule_LongPress(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo == null)
            {
                return;
            }
            AppointmentItem appointment = e.AppointmentInfo.Appointment;
            
            //storage.RemoveAppointment(appointment);          
            //model.DeleteCommand.Execute(appointment.SourceObject);
        }
        void OnRemindersChanged(object sender, EventArgs e)
        {
            remindersNotificationCenter.UpdateNotifications(storage);
        }

        async void WeekView_OnTap(object sender, SchedulerGestureEventArgs e)
        {
            if (this.inNavigation)
                return;
            Page appointmentPage = this.storage.CreateAppointmentPageOnTap(e, true);
            if (appointmentPage != null)
            {
                this.inNavigation = true;
                await Navigation.PushAsync(appointmentPage);
            }
        }
      

    }
}