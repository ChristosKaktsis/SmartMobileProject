using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class Appointment : XPObject
    {
        public Appointment() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Appointment(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
        public Guid SmartOid
        {
            get { return smartOid; }
            set { SetPropertyValue(nameof(SmartOid), ref smartOid, value); }
        }
        Guid smartOid;
        public int Id
        {
            get { return id; }
            set { SetPropertyValue(nameof(Id), ref id, value); }
        }
        int id;
       
        public DateTime StartTime
        {
            get { return starttime; }
            set { SetPropertyValue(nameof(StartTime), ref starttime, value); }
        }
        DateTime starttime;
        public DateTime EndTime
        {
            get { return endtime; }
            set { SetPropertyValue(nameof(EndTime), ref endtime, value); }
        }
        DateTime endtime;
        public string Subject
        {
            get { return subject; }
            set { SetPropertyValue(nameof(Subject), ref subject, value); }
        }
        string subject;
        public string Description
        {
            get { return description; }
            set { SetPropertyValue(nameof(Description), ref description, value); }
        }
        string description;
        public int LabelId
        {
            get { return labelid; }
            set { SetPropertyValue(nameof(LabelId), ref labelid, value); }
        }
        int labelid;
        public int Status
        {
            get { return status; }
            set { SetPropertyValue(nameof(Status), ref status, value); }
        }
        int status;
        public string RecurrenceInfo
        {
            get { return recurrenceInfo; }
            set { SetPropertyValue(nameof(RecurrenceInfo), ref recurrenceInfo, value); }
        }
        string recurrenceInfo;
        public string ReminderInfo
        {
            get { return reminderInfo; }
            set { SetPropertyValue(nameof(ReminderInfo), ref reminderInfo, value); }
        }
        string reminderInfo;
        public string Location
        {
            get { return location; }
            set { SetPropertyValue(nameof(Location), ref location, value); }
        }
        string location;
        public bool AllDay
        {
            get { return allDay; }
            set { SetPropertyValue(nameof(AllDay), ref allDay, value); }
        }
        bool allDay;
        public int Type
        {
            get { return type; }
            set { SetPropertyValue(nameof(Type), ref type, value); }
        }
        int type;
        public string Caption
        {
            get { return caption; }
            set { SetPropertyValue(nameof(Caption), ref caption, value); }
        }
        string caption;
    }

}