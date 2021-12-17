using SmartMobileProject.Services;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using DevExpress.Xpo.DB;
using System.IO;
using SmartMobileProject.Core;
using DevExpress.Xpo;
using System.Reflection;
using Xamarin.Essentials;


namespace SmartMobileProject
{
    public partial class App : Application
    {

        public UnitOfWork uow;
        public App()
        {
            DevExpress.XamarinForms.DataGrid.Initializer.Init();
            DevExpress.XamarinForms.Scheduler.Initializer.Init();
            DevExpress.XamarinForms.Editors.Initializer.Init();
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Connect to SQLite
            var filePath = Path.Combine(documentsPath, "xpoXamarinDemo.db");
            string connectionString = SQLiteConnectionProvider.GetConnectionString(filePath) + ";Cache=Shared;";
            //get data from local db
            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            Stream embeddedDatabaseStream = assembly.GetManifestResourceStream("SmartMobileProject.xpoXamarinDemo.db");
            if (!File.Exists(filePath))
            {
                FileStream fileStreamToWrite = File.Create(filePath);
                embeddedDatabaseStream.Seek(0, SeekOrigin.Begin);
                embeddedDatabaseStream.CopyTo(fileStreamToWrite);
                fileStreamToWrite.Close();
            }
            //

            XpoHelper.InitXpo(connectionString);
            uow = XpoHelper.CreateUnitOfWork();

            MainPage = new AppShell();
            //Application.Current.Properties.Clear();
            //string deviceIdentifier = DependencyService.Get<IDevice>().GetIdentifier();

        }

        protected override async void OnStart()
        {                      
            if (!Preferences.Get("Remember", false))
            {
                await Application.Current.MainPage.DisplayAlert("Online",
                "Για να μπορείτε να ανεβάζετε και να κατεβάζετε δεδομένα απο το Smart θα πρέπει να γίνουν κάποιες ρυθμήσεις στο Server. " +
                "Μπορείτε να αλλάξετε τις ρυθμήσεις από το Application Settings", "Εντάξει");
            }
            else
            {
                await AppShell.Current.GoToAsync("///LoginPage");       
            }
            //await ActivationCheck.CheckActivationCode();           
            TrialCheck.Check();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
           
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.ExceptionObject);
        }

    }
}
