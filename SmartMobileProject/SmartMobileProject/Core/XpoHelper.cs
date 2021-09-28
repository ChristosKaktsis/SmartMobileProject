using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System.Linq;
using SmartMobileProject.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Net;
using System.IO;
using System.Threading;


namespace SmartMobileProject.Core
{
    public static class XpoHelper
    {
        static readonly Type[] entityTypes = new Type[] { typeof(ΣτοιχείαΕταιρίας),
            typeof(Πελάτης),typeof(ΔιευθύνσειςΠελάτη),typeof(ΤαχυδρομικόςΚωδικός),typeof(Πόλη),typeof(Χώρα)
            ,typeof(ΔΟΥ),typeof(ΦΠΑ)
            ,typeof(Είδος),typeof(ΟικογένειαΕίδους),typeof(ΥποοικογένειαΕίδους),typeof(ΟμάδαΕίδους),typeof(ΚατηγορίαΕίδους)
            ,typeof(ΠαραστατικάΠωλήσεων),typeof(ΓραμμέςΠαραστατικώνΠωλήσεων),typeof(ΣειρέςΠαραστατικώνΠωλήσεων),typeof(ΤρόποςΠληρωμής),typeof(ΤρόποςΑποστολής)
            ,typeof(ΠαραστατικάΕισπράξεων),typeof(ΓραμμέςΠαραστατικώνΕισπράξεων),typeof(ΣειρέςΠαραστατικώνΕισπράξεων),typeof(ΛογαριασμοίΧρηματικώνΔιαθέσιμων),typeof(Αξιόγραφα)
            ,typeof(Τράπεζα),typeof(ΤραπεζικοίΛογαριασμοί)
            ,typeof(Appointment) ,typeof(Πωλητής), typeof(Εργασία),
            typeof(Ενέργεια),typeof(ΙδιότηταΕνέργειας),typeof(ΕπιλογήΙδιότητας)
        };
        public static async void InitXpo(string connectionString)
        {
            var dictionary = PrepareDictionary();

            if (XpoDefault.DataLayer == null)
            {
                using (var updateDataLayer = XpoDefault.GetDataLayer(connectionString, dictionary, AutoCreateOption.DatabaseAndSchema))
                {
                    updateDataLayer.UpdateSchema(false, dictionary.CollectClassInfos(entityTypes));
                }
            }
            var dataStore = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists);
            XpoDefault.DataLayer = new ThreadSafeDataLayer(dictionary, dataStore);
            XpoDefault.Session = null;


            await XpoHelper.CreatePolitisData();//

            //Ελεγχος Εαν Υπαρχουν δεδομένα στην Βαση για να μην τρέξει όλες τις μεθοδους ασκοπα   
            //using (UnitOfWork uow = CreateUnitOfWork())
            //{
            //    if (!uow.Query<Πωλητής>().Any())
            //    {
            //        await CreatePolitisData();//
            //        await Task.WhenAll(
            //            CreateDOYData(),
            //            CreateFPAData(),
            //            CreateXORAData(),
            //            CreateYPOOIKOGENEIAEIDOYSData(),
            //            CreateTROPOSPLIROMISData(),
            //            CreateTROPOSAPOSTOLISData(),
            //            CreateSEIRAPOLData(),
            //            CreateSEIRAEISData(),
            //            CreatePOLIData(),
            //            CreateTrapezaData(),
            //            CreateTRAPLOGData(),//
            //            CreateLOGXRHMDIATHData(),// 
            //            CreateIDIOTITAData(),
            //            CreateEPILOGESIDIOTITASData(),
            //            CreateOMADAEIDOUSData(),
            //            CreateOIKOGENEIAEIDOUSData(),
            //            CreateKATIGORIAEIDOUSData());
            //        await CreateDOYData();
            //        await CreateΤΚData();
            //    }
            //}
        }
        public static async Task<bool> CreateTableData()
        {
            
            var okP = await CreatePELATISData();
            if(okP)
                await CreateDIEUPELATIData();

           var ok =  await Task.WhenAll(
                CreateEIDOSData(),
                CreateSEIRAPOLData(),
                CreateSEIRAEISData(),
                CreateEvent()
                );
            return ok.All(x => x) && okP;
        }
        public static UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork();
        }
        static XPDictionary PrepareDictionary()
        {
            var dict = new ReflectionDictionary();
            dict.GetDataStoreSchema(entityTypes);
            return dict;
        }
        public static async Task CreatePolitisData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dtPolitis = await getSmartTable("select Oid, Ονοματεπώνυμο, KίνΤηλέφωνο, " +
                        "Οδός, Αριθμός, Email,  FAX, Κείμενο5 from Πωλητής where ");
                if (dtPolitis == null) { return; }
                foreach (DataRow row in dtPolitis.Rows)
                {
                    if (uow.Query<Πωλητής>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(row["Κείμενο5"].ToString()))
                    {
                        if (uow.Query<Πωλητής>().Where(x => x.SmartOid == Guid.Parse((string)row["Κείμενο5"])).Any())
                        {
                            continue;
                        }
                    }
                    Πωλητής politis = new Πωλητής(uow);
                    politis.SmartOid = Guid.Parse((string)row["Oid"]);
                    politis.Ονοματεπώνυμο = row["Ονοματεπώνυμο"].ToString();
                    politis.KίνΤηλέφωνο = row["KίνΤηλέφωνο"].ToString();
                    politis.Οδός = row["Οδός"].ToString();
                    politis.Αριθμός = row["Αριθμός"].ToString();
                    politis.Email = row["Email"].ToString();
                    politis.FAX = row["FAX"].ToString();

                    uow.Save(politis);
                }

                if (uow.InTransaction)
                {
                    try
                    {
                        uow.CommitChanges();
                    }
                    catch (Exception exc)
                    {
                        uow.RollbackTransaction();
                        Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                    }

                }
            }
        }
        public static async Task<bool> CreateDOYData()
        {
            using (var uow = CreateUnitOfWork())
            {                 
                if (!uow.Query<ΔΟΥ>().Any())
                {
                    DataTable dtDOY = await getSmartTable("Select Oid, Κωδικός, ΔΟΥ From ΔΟΥ where ");
                    if (dtDOY == null) { return false; }
                    foreach (DataRow row in dtDOY.Rows)
                    {
                        ΔΟΥ doy = new ΔΟΥ(uow);
                        doy.SmartOid = Guid.Parse((string)row["Oid"]);
                        doy.Κωδικός = row["Κωδικός"].ToString();
                        doy.Περιγραφή = row["ΔΟΥ"].ToString();

                        uow.Save(doy);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                            return true;
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                            return false;
                        }

                    }
                    
                }
                return true;
            }           
        }
        public static async Task CreateFPAData()
        {

            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΦΠΑ>().Any())
                {
                    DataTable dt = await getSmartTable("Select ΦΠΑ, ΟμάδαΦΠΑ, ΦΠΑΚανονικό, ΦΠΑΕξαίρεση, ΦΠΑΜειωμένο From ΦΠΑ where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΦΠΑ data = new ΦΠΑ(uow);
                        data.Φπαid = row["ΦΠΑ"].ToString();
                        data.Ομάδαφπα = row["ΟμάδαΦΠΑ"].ToString();
                        data.Φπακανονικό = float.Parse(row["ΦΠΑΚανονικό"].ToString());
                        data.Φπαεξαίρεση = float.Parse(row["ΦΠΑΕξαίρεση"].ToString());
                        data.Φπαμειωμένο = float.Parse(row["ΦΠΑΜειωμένο"].ToString());

                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }

                    }
                }
            }
        }
        public static async Task CreateXORAData()
        {

            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<Χώρα>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, Χώρα, Συντομογραφία  From Χώρα where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        Χώρα data = new Χώρα(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.ΌνομαΧώρας = row["Χώρα"].ToString();
                        data.Συντομογραφία = row["Συντομογραφία"].ToString();

                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }

                    }
                }
            }
        }
        public static async Task CreateYPOOIKOGENEIAEIDOYSData()
        {

            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΥποοικογένειαΕίδους>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, Περιγραφή From ΥποοικογένειαΕίδους where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΥποοικογένειαΕίδους data = new ΥποοικογένειαΕίδους(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Περιγραφή = row["Περιγραφή"].ToString();


                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }

                    }
                }
            }
        }
        public static async Task CreateTROPOSPLIROMISData()
        {

            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΤρόποςΠληρωμής>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, ΤρόποςΠληρωμής From ΤρόποςΠληρωμής where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΤρόποςΠληρωμής data = new ΤρόποςΠληρωμής(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Τρόποςπληρωμής = row["ΤρόποςΠληρωμής"].ToString();


                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }

                    }
                }
            }
        }
        public static async Task CreateTROPOSAPOSTOLISData()
        {

            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΤρόποςΑποστολής>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, ΤρόποςΑποστολής From ΤρόποςΑποστολής where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΤρόποςΑποστολής data = new ΤρόποςΑποστολής(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Τρόποςαποστολής = row["ΤρόποςΑποστολής"].ToString();


                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }

                    }
                }
            }
        }
        public static async Task<bool> CreateSEIRAPOLData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Σειρά, Περιγραφή, ΠρόθεμαΑρίθμησης From ΣειρέςΠαραστατικώνΠωλήσεων where ");
                if (dt == null) { return false; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
                    ΣειρέςΠαραστατικώνΠωλήσεων data = new ΣειρέςΠαραστατικώνΠωλήσεων(uow);
                    data.SmartOid = Guid.Parse((string)row["Oid"]);
                    data.Σειρά = row["Σειρά"].ToString();
                    data.Περιγραφή = row["Περιγραφή"].ToString();
                    data.ΠρόθεμαΑρίθμησης = Guid.Parse((string)row["ΠρόθεμαΑρίθμησης"]);
                    data.Counter = 0;
                    uow.Save(data);
                }

                if (uow.InTransaction)
                {
                    try
                    {
                        uow.CommitChanges();
                    }
                    catch (Exception exc)
                    {
                        uow.RollbackTransaction();
                        Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        return false;
                    }
                }
                return true;
            }
        }
        public static async Task<bool> CreateSEIRAEISData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Σειρά, Περιγραφή From ΣειρέςΠαραστατικώνΕισπράξεων where ");
                if (dt == null) { return false; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΣειρέςΠαραστατικώνΕισπράξεων>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
                    ΣειρέςΠαραστατικώνΕισπράξεων data = new ΣειρέςΠαραστατικώνΕισπράξεων(uow);
                    data.SmartOid = Guid.Parse((string)row["Oid"]);
                    data.Σειρά = row["Σειρά"].ToString();
                    data.Περιγραφή = row["Περιγραφή"].ToString();
                    data.Counter = 0;
                    uow.Save(data);
                }

                if (uow.InTransaction)
                {
                    try
                    {
                        uow.CommitChanges();
                    }
                    catch (Exception exc)
                    {
                        uow.RollbackTransaction();
                        Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        return false;
                    }
                }
                return true;
            }
        }
        public static async Task<int> CreateARITHMISISEIRAData(Guid Oid)
        {
            int counter=0;
            DataTable dt = await getSmartTable("Select ΤρέχουσαΤιμήΜετρητή From Μετρητές where Oid = '" + Oid + "' and ");
            if (dt == null) { return -1; }
            foreach (DataRow row in dt.Rows)
            {
                counter = int.Parse(row["ΤρέχουσαΤιμήΜετρητή"].ToString());
            }
            return counter;
        }
        public static async Task CreatePOLIData()
        {
            using (UnitOfWork uow = CreateUnitOfWork())
            {
                if (!uow.Query<Πόλη>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, Πόλη From Πόλη where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        Πόλη data = new Πόλη(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.ΟνομαΠόλης = row["Πόλη"].ToString();
                        //data.ΓεωγραφικόΜήκος = (double)row["ΓεωγραφικόΜήκος"];
                        // data.ΓεωγραφικόΠλάτος = (double)row["ΓεωγραφικόΠλάτος"];

                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }
                    }
                }
            }
        }
        public static async Task CreateΤΚData()
        {
            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΤαχυδρομικόςΚωδικός>().Any())
                {
                    DataTable dt = await getSmartTable("Select ΤαχυδρομικόςΚωδικός, Πόλη, Νομός, Περιοχή, Χώρα From ΤαχυδρομικόςΚωδικός where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΤαχυδρομικόςΚωδικός data = new ΤαχυδρομικόςΚωδικός(uow);
                        //data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Ονοματκ = row["ΤαχυδρομικόςΚωδικός"].ToString();
                        var p = uow.Query<Πόλη>().Where(x => x.SmartOid == Guid.Parse((string)row["Πόλη"]));
                        data.Πόλη = p.FirstOrDefault();
                        data.Νομός = row["Νομός"].ToString();
                        data.Περιοχή = row["Περιοχή"].ToString();
                        data.Χώρα = row["Χώρα"].ToString();

                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }

                    }
                }
            }
        }
        public static async Task CreateTrapezaData()
        {

            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<Τράπεζα>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, Τράπεζα From Τράπεζα where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        Τράπεζα data = new Τράπεζα(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.ΟνομαΤράπεζας = row["Τράπεζα"].ToString();
                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }

                    }
                }
            }
        }
        public static async Task CreateOMADAEIDOUSData()
        {

            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΟμάδαΕίδους>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, Ομάδα, ΣειράΕμφάνισης From ΟμάδεςΕίδους where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΟμάδαΕίδους data = new ΟμάδαΕίδους(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Ομάδα = row["Ομάδα"].ToString();
                        data.ΣειράΕμφάνισης = int.Parse(row["ΣειράΕμφάνισης"].ToString());

                        uow.Save(data);
                    }
                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }

                    }
                }
            }
        }
        public static async Task CreateOIKOGENEIAEIDOUSData()
        {
            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΟικογένειαΕίδους>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, Περιγραφή From ΟικογένειαΕίδους where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΟικογένειαΕίδους data = new ΟικογένειαΕίδους(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Περιγραφή = row["Περιγραφή"].ToString();
                        uow.Save(data);
                    }
                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }
                    }
                }
            }
        }
        public static async Task CreateKATIGORIAEIDOUSData()
        {

            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΚατηγορίαΕίδους>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, Κατηγορία From ΚατηγορίεςΕίδους where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΚατηγορίαΕίδους data = new ΚατηγορίαΕίδους(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Κατηγορία = row["Κατηγορία"].ToString();

                        uow.Save(data);
                    }
                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }
                    }
                }
            }
        }
        public static async Task<bool> CreateEIDOSData()
        {
            using (UnitOfWork uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select  Oid, Κωδικός, Περιγραφή, ΦΠΑ, ΤιμήΧονδρικής, Ομάδα, Κατηγορία, Οικογένεια, Υποοικογένεια From Είδος where ");
                if (dt == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Loading Είδος", "OK");
                    return false;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<Είδος>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
                    Είδος data = new Είδος(uow);
                    data.SmartOid = Guid.Parse((string)row["Oid"]);
                    data.Κωδικός = row["Κωδικός"].ToString();
                    data.Περιγραφή = row["Περιγραφή"].ToString();
                    if (!string.IsNullOrEmpty(row["ΦΠΑ"].ToString()))
                    {
                        var p = uow.Query<ΦΠΑ>().Where(x => x.Φπαid == row["ΦΠΑ"].ToString());
                        data.ΦΠΑ = p.FirstOrDefault();
                    }
                    else
                    {
                        var p = uow.Query<ΦΠΑ>().Where(x => x.Φπαid == "0" );
                        data.ΦΠΑ = p.FirstOrDefault();
                    }
                   
                    data.ΤιμήΧονδρικής = double.Parse(row["ΤιμήΧονδρικής"].ToString());
                    if (!string.IsNullOrEmpty(row["Ομάδα"].ToString()))
                    {
                        var omada = uow.Query<ΟμάδαΕίδους>().Where(x => x.SmartOid == Guid.Parse((string)row["Ομάδα"]));
                        data.Ομάδα = omada.FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(row["Κατηγορία"].ToString()))
                    {
                        var katigoria = uow.Query<ΚατηγορίαΕίδους>().Where(x => x.SmartOid == Guid.Parse((string)row["Κατηγορία"]));
                        data.Κατηγορία = katigoria.FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(row["Οικογένεια"].ToString()))
                    {
                        var oikogeneia = uow.Query<ΟικογένειαΕίδους>().Where(x => x.SmartOid == Guid.Parse((string)row["Οικογένεια"]));
                        data.Οικογένεια = oikogeneia.FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(row["Υποοικογένεια"].ToString()))
                    {
                        var ypooikogeneia = uow.Query<ΥποοικογένειαΕίδους>().Where(x => x.SmartOid == Guid.Parse((string)row["Υποοικογένεια"]));
                        data.Υποοικογένεια = ypooikogeneia.FirstOrDefault();
                    }

                    uow.Save(data);
                }

                if (uow.InTransaction)
                {
                    try
                    {
                        uow.CommitChanges();
                        return true;
                    }
                    catch (Exception exc)
                    {
                        uow.RollbackTransaction();
                        Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        return false;
                    }
                }
                //await Application.Current.MainPage.DisplayAlert("Alert", "Υπάρχουν όλα τα Είδη στην βάση", "OK");

            }
            return true;
        }
        public static async Task CreateTRAPLOGData()
        {
            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΤραπεζικοίΛογαριασμοί>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, Τράπεζα, Swift, Λογαριασμός, IBAN, Υποκατάστημα From ΤραπεζικοίΛογαριασμοί where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΤραπεζικοίΛογαριασμοί data = new ΤραπεζικοίΛογαριασμοί(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        var p = uow.Query<Τράπεζα>().Where(x => x.SmartOid == Guid.Parse((string)row["Τράπεζα"]));
                        data.Τράπεζα = p.FirstOrDefault();
                        data.Swift = row["Swift"].ToString();
                        data.Λογαριασμός = row["Λογαριασμός"].ToString();
                        data.IBAN = row["IBAN"].ToString();
                        data.Υποκατάστημα = row["Υποκατάστημα"].ToString();

                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }
                    }
                }
            }
        }
        public static async Task CreateLOGXRHMDIATHData()
        {
            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΛογαριασμοίΧρηματικώνΔιαθέσιμων>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, Τράπεζα, ΤραπεζικόςΛογαριασμός, Λογαριασμός From ΛογαριασμοίΧρηματικώνΔιαθέσιμων where ");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΛογαριασμοίΧρηματικώνΔιαθέσιμων data = new ΛογαριασμοίΧρηματικώνΔιαθέσιμων(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Λογαριασμός = row["Λογαριασμός"].ToString();
                        if (!string.IsNullOrEmpty(row["Τράπεζα"].ToString()))
                        {
                            var p = uow.Query<Τράπεζα>().Where(x => x.SmartOid == Guid.Parse((string)row["Τράπεζα"]));
                            data.Τράπεζα = p.FirstOrDefault();
                        }
                        if (!string.IsNullOrEmpty(row["ΤραπεζικόςΛογαριασμός"].ToString()))
                        {
                            var p = uow.Query<ΤραπεζικοίΛογαριασμοί>().Where(x => x.SmartOid == Guid.Parse((string)row["ΤραπεζικόςΛογαριασμός"]));
                            data.ΤραπεζικόςΛογαριασμός = p.FirstOrDefault();
                        }
                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }
                    }
                }
            }
        }
        public static async Task<bool> CreatePELATISData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Επωνυμία, ΚατηγορίαΦΠΑ, ΑΦΜ, Email, " +
                        "ΔΟΥ, ΚεντρικήΔιευθυνση, Πωλητής, Κείμενο5 , ΔιακριτικόςΤίτλος From Πελάτης where ΑΦΜ is not null and ΑΦΜ != '' and ");
                if (dt == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Loading Πελάτης", "OK");
                    return false;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<Πελάτης>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(row["Κείμενο5"].ToString()))
                    {
                        if (uow.Query<Πελάτης>().Where(x => x.SmartOid == Guid.Parse((string)row["Κείμενο5"])).Any())
                        {
                            continue;
                        }
                    }
                    Πελάτης data = new Πελάτης(uow);
                    data.SmartOid = Guid.Parse((string)row["Oid"]);
                    data.Επωνυμία = row["Επωνυμία"].ToString();
                    data.Διακριτικόςτίτλος = row["ΔιακριτικόςΤίτλος"].ToString();
                    data.ΚατηγορίαΦΠΑ = int.Parse(row["ΚατηγορίαΦΠΑ"].ToString());
                    data.ΑΦΜ = row["ΑΦΜ"].ToString();
                    data.Email = row["Email"].ToString();
                    if (!string.IsNullOrEmpty(row["Πωλητής"].ToString()))
                    {
                        var politis = uow.Query<Πωλητής>().Where(x => x.SmartOid == Guid.Parse((string)row["Πωλητής"]));
                        data.Πωλητής = politis.FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(row["ΔΟΥ"].ToString()))
                    {
                        var doy = uow.Query<ΔΟΥ>().Where(x => x.SmartOid == Guid.Parse((string)row["ΔΟΥ"]));
                        data.ΔΟΥ = doy.FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(row["ΚεντρικήΔιευθυνση"].ToString()))
                    {
                        //var dieu = uow.Query<ΔιευθύνσειςΠελάτη>().Where(x => x.SmartOid == Guid.Parse((string)row["ΚεντρικήΔιευθυνση"]));
                        //  data.ΔιευθύνσειςΠελάτη.Add(dieu.FirstOrDefault());
                        data.SmartOidΚεντρικήΔιεύθυνση = Guid.Parse((string)row["ΚεντρικήΔιευθυνση"]);
                    }
                    
                    uow.Save(data);
                }
                if (uow.InTransaction)
                {
                    try
                    {
                        uow.CommitChanges();
                    }
                    catch (Exception exc)
                    {
                        uow.RollbackTransaction();
                        Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        return false;
                    }
                }

            }
            return true;
        }
        public static async Task<bool> CreateDIEUPELATIData()
        {

            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Οδός, Αριθμός, Περιοχή, Τηλέφωνο, Τηλέφωνο1, " +
                       "KίνΤηλέφωνο, Κείμενο5, FAX, ΓεωγραφικόΠλάτος, ΓεωγραφικόΜήκος, ΤΚ, Πόλη, Πελάτης From ΔιευθύνσειςΠελάτη where ");
                if (dt == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Loading ΔιευθύνσειςΠελάτη", "OK");
                    return false;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΔιευθύνσειςΠελάτη>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(row["Κείμενο5"].ToString()))
                    {
                        if (uow.Query<ΔιευθύνσειςΠελάτη>().Where(x => x.SmartOid == Guid.Parse((string)row["Κείμενο5"])).Any())
                        {
                            continue;
                        }
                    }
                    ΔιευθύνσειςΠελάτη data = new ΔιευθύνσειςΠελάτη(uow);
                    data.SmartOid = Guid.Parse((string)row["Oid"]);
                    data.Οδός = row["Οδός"].ToString();
                    data.Αριθμός = row["Αριθμός"].ToString();
                    data.Περιοχή = row["Περιοχή"].ToString();
                    data.Τηλέφωνο = row["Τηλέφωνο"].ToString();
                    data.Τηλέφωνο1 = row["Τηλέφωνο1"].ToString();
                    data.Kίντηλέφωνο = row["KίνΤηλέφωνο"].ToString();
                    data.FAX = row["FAX"].ToString();

                    if (!string.IsNullOrEmpty(row["ΓεωγραφικόΠλάτος"].ToString()))
                    {
                        data.ΓεωγραφικόΠλάτος = double.Parse(row["ΓεωγραφικόΠλάτος"].ToString());
                    }
                    if (!string.IsNullOrEmpty(row["ΓεωγραφικόΜήκος"].ToString()))
                    {
                        data.ΓεωγραφικόΜήκος = double.Parse(row["ΓεωγραφικόΜήκος"].ToString());
                    }
                    if (!string.IsNullOrEmpty(row["ΤΚ"].ToString()))
                    {
                        var tk = uow.Query<ΤαχυδρομικόςΚωδικός>().Where(x => x.Ονοματκ == row["ΤΚ"].ToString());
                        data.ΤΚ = tk.FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(row["Πόλη"].ToString()))
                    {
                        var poli = uow.Query<Πόλη>().Where(x => x.SmartOid == Guid.Parse((string)row["Πόλη"]));
                        data.Πόλη = poli.FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(row["Πελάτης"].ToString()))
                    {
                        var pelatis = uow.Query<Πελάτης>().Where(x => x.SmartOid == Guid.Parse((string)row["Πελάτης"]));
                        data.Πελάτης = pelatis.FirstOrDefault();
                        if (data.Πελάτης != null)
                        {
                            if (data.Πελάτης.SmartOidΚεντρικήΔιεύθυνση == data.SmartOid)
                            {
                                data.Πελάτης.ΚεντρικήΔιευθυνση = data;
                            }
                        }
                    }
                    uow.Save(data);
                }

                if (uow.InTransaction)
                {
                    try
                    {
                        uow.CommitChanges();
                    }
                    catch (Exception exc)
                    {
                        uow.RollbackTransaction();
                        Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        return false;
                    }

                }
            }
            return true;
        }
        public static async Task<ΣτοιχείαΕταιρίας> CreateSTOIXEIAETAIRIASData()
        {
            ΣτοιχείαΕταιρίας στοιχείαΕταιρίας;
            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΣτοιχείαΕταιρίας>().Any())
                {
                    DataTable dt = await getSmartTable("Select Oid, Επωνυμία, ΚατηγορίαΦΠΑ, ΑΦΜ, Email," +
                        " ΔΟΥ, Οδός, Αριθμός, ΔικτυακόςΤόπος, Περιοχή, Τηλέφωνο, Τηλέφωνο1, ΤΚ, Πόλη," +
                        " UsernameΥπηρεσίαςΣτοιχείωνΜητρώου, PasswordΥπηρεσίαςΣτοιχείωνΜητρώου, Fax From ΣτοιχείαΕταιρίας where ");
                    if (dt == null) { return null; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΣτοιχείαΕταιρίας data = new ΣτοιχείαΕταιρίας(uow);
                        data.Επωνυμία = row["Επωνυμία"].ToString();
                        data.ΚατηγορίαΦΠΑ = int.Parse(row["ΚατηγορίαΦΠΑ"].ToString());
                        data.ΑΦΜ = row["ΑΦΜ"].ToString();
                        data.Οδός = row["Οδός"].ToString();
                        data.Αριθμός = row["Αριθμός"].ToString();
                        data.Περιοχή = row["Περιοχή"].ToString();
                        data.Τηλέφωνο = row["Τηλέφωνο"].ToString();
                        data.Τηλέφωνο1 = row["Τηλέφωνο1"].ToString();
                        data.FAX = row["FAX"].ToString();
                        data.ΔικτυακόςΤόπος = row["ΔικτυακόςΤόπος"].ToString();
                        data.Email = row["Email"].ToString();
                        data.UsernameΥπηρεσίαςΣτοιχείωνΜητρώου = row["UsernameΥπηρεσίαςΣτοιχείωνΜητρώου"].ToString();
                        data.PasswordΥπηρεσίαςΣτοιχείωνΜητρώου = row["PasswordΥπηρεσίαςΣτοιχείωνΜητρώου"].ToString();
                        if (!string.IsNullOrEmpty(row["ΤΚ"].ToString()))
                        {
                            var tk = uow.Query<ΤαχυδρομικόςΚωδικός>().Where(x => x.Ονοματκ == row["ΤΚ"].ToString());
                            data.ΤΚ = tk.FirstOrDefault();
                        }
                        if (!string.IsNullOrEmpty(row["Πόλη"].ToString()))
                        {
                            var poli = uow.Query<Πόλη>().Where(x => x.SmartOid == Guid.Parse((string)row["Πόλη"]));
                            data.Πόλη = poli.FirstOrDefault();
                        }
                        if (!string.IsNullOrEmpty(row["ΔΟΥ"].ToString()))
                        {
                            var doy = uow.Query<ΔΟΥ>().Where(x => x.SmartOid == Guid.Parse((string)row["ΔΟΥ"]));
                            data.ΔΟΥ = doy.FirstOrDefault();
                        }

                        uow.Save(data);

                        break;
                    }
                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }
                    }
                }

                στοιχείαΕταιρίας = uow.Query<ΣτοιχείαΕταιρίας>().First();
                return στοιχείαΕταιρίας;
            }
        }
        public static async Task CreateIDIOTITAData()
        {
            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΙδιότηταΕνέργειας>().Any())
                {
                    DataTable dt = await getSmartTable("SELECT Ιδιότητα.Oid, Ιδιότητα.Περιγραφή, ΤύποςΙδιότητας" +
                        " FROM Ιδιότητα JOIN ΟμάδαΙδιότητας on Ιδιότητα.ΟμάδαΙδιότητας = ΟμάδαΙδιότητας.Oid " +
                        "where ΟμάδαΙδιότητας.Περιγραφή = 'CRM Mobile' and Ιδιότητα.");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΙδιότηταΕνέργειας data = new ΙδιότηταΕνέργειας(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Περιγραφή = row["Περιγραφή"].ToString();
                        data.Τύποςιδιότητας = int.Parse(row["ΤύποςΙδιότητας"].ToString());

                        uow.Save(data);
                    }
                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }
                    }
                }
            }
        }
        public static async Task CreateEPILOGESIDIOTITASData()
        {
            using (var uow = CreateUnitOfWork())
            {
                if (!uow.Query<ΕπιλογήΙδιότητας>().Any())
                {
                    DataTable dt = await getSmartTable("SELECT ΕπιλογέςΙδιότητας.Oid, Ιδιότητα.Oid as ΙδιότηταOid, ΕπιλογέςΙδιότητας.Περιγραφή " +
                      "FROM Ιδιότητα JOIN ΟμάδαΙδιότητας on Ιδιότητα.ΟμάδαΙδιότητας = ΟμάδαΙδιότητας.Oid " +
                      "join ΕπιλογέςΙδιότητας on ΕπιλογέςΙδιότητας.Ιδιότητα = Ιδιότητα.Oid " +
                      "where ΟμάδαΙδιότητας.Περιγραφή = 'CRM Mobile' and Ιδιότητα.");
                    if (dt == null) { return; }
                    foreach (DataRow row in dt.Rows)
                    {
                        ΕπιλογήΙδιότητας data = new ΕπιλογήΙδιότητας(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Περιγραφή = row["Περιγραφή"].ToString();

                        if (!string.IsNullOrEmpty(row["ΙδιότηταOid"].ToString()))
                        {
                            var idiotita = uow.Query<ΙδιότηταΕνέργειας>().Where(x => x.SmartOid == Guid.Parse((string)row["ΙδιότηταOid"]));
                            data.ΙδιότηταΕνέργειας = idiotita.FirstOrDefault();
                        }
                        uow.Save(data);
                    }

                    if (uow.InTransaction)
                    {
                        try
                        {
                            uow.CommitChanges();
                        }
                        catch (Exception exc)
                        {
                            uow.RollbackTransaction();
                            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        }

                    }
                }
            }
        }
        public static async Task<bool> CreateEvent()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("SELECT event.Oid, Resource.Caption, Subject, Description, StartOn, EndOn, AllDay, Label, Location FROM Event " +
                    "join ResourceResources_EventEvents on ResourceResources_EventEvents.Events=Event.Oid " +
                    "join Resource on Resource.Oid = ResourceResources_EventEvents.Resources where " +
                    "datepart(year,StartOn) >= datepart(YEAR, GETDATE()) and DATEPART(month, StartOn)>= DATEPART(month, GETDATE()) AND Event.");
                if (dt == null) { return false; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<Appointment>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
                    Appointment data = new Appointment(uow);
                    data.SmartOid = Guid.Parse((string)row["Oid"]);
                    data.Subject = row["Subject"].ToString();
                    data.Description = row["Description"].ToString();
                    data.AllDay = bool.Parse(row["AllDay"].ToString());
                    data.StartTime = DateTime.Parse(row["StartOn"].ToString());
                    data.EndTime = DateTime.Parse(row["EndOn"].ToString());
                    data.Location = row["Location"].ToString();
                    data.LabelId = int.Parse(row["Label"].ToString());
                    data.Caption = row["Caption"].ToString();
                    uow.Save(data);
                }

                if (uow.InTransaction)
                {
                    try
                    {
                        uow.CommitChanges();
                        return true;
                    }
                    catch (Exception exc)
                    {
                        uow.RollbackTransaction();
                        Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
                        return false;
                    }

                }

            }
            return true;
        }
        /// <summary>
        /// Ανοιγει ενα HttpClient και παιρνει το response
        /// Μετατρέπει σε Json μορφή και μετα το κάνει Convert σε datatable
        /// http://192.168.3.44:80/api/Values?sql= select * from table where GCRecord is null 
        /// </summary>
        /// <param name="smartTable">Ο πινακας απο  την βαση του smart</param>
        /// <returns>Select From smartTable where GCRecord</returns>
        public static async Task<DataTable> getSmartTable(string smartTable)
        {

            //await Shell.Current.GoToAsync("//LoginPage");
            HttpClient client = new HttpClient();

            string uri = "http://192.168.3.44:80/api/Values?sql= " + smartTable + "GCRecord is null ";
            HttpResponseMessage response;
            string content;
            try
            {
                response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                content = await response.Content.ReadAsStringAsync();
                content = content.Replace("\\", "");
                content = content.Remove(0, 1);
                content = content.Remove(content.Length - 1, 1);
                DataTable dt = JsonConvert.DeserializeAnonymousType(content, new { Answare = default(DataTable) }).Answare;
                return dt;
            }
            catch (Exception exc)
            {
                Console.WriteLine("---EXEPTION IN GET SMART TABLE---" + exc);
                return null;
            }

        }
        public static async Task<bool> setSmartTable(string json, string type)
        {
            string authval = "DemoAdmin:DemoPass";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(authval);
            string conv = System.Convert.ToBase64String(plainTextBytes);

            var httpRequest = (HttpWebRequest)WebRequest.Create("http://192.168.3.44:8080/api/PutDataJson?Type="+type);
            httpRequest.ContentType = "application/json";
            httpRequest.Method = "POST";
            httpRequest.Headers.Add("Authorization", "Basic " + conv);
            
            try
            {
                var stream = await httpRequest.GetRequestStreamAsync();
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(json));
                }

                var httpResponse = await httpRequest.GetResponseAsync();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }

                return true;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Upload. " +
                    "Ελεγξτε την συνδεση σας", "OK");
                return false;
            }
            
        }
        
    }
}
