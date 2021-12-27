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
using Xamarin.Essentials;
using System.Net.NetworkInformation;

namespace SmartMobileProject.Core
{
    public static class XpoHelper
    {
        static readonly Type[] entityTypes = new Type[] { typeof(ΣτοιχείαΕταιρίας),
            typeof(Πελάτης),typeof(ΔιευθύνσειςΠελάτη),typeof(ΤαχυδρομικόςΚωδικός),typeof(Πόλη),typeof(Χώρα)
            ,typeof(ΔΟΥ),typeof(ΦΠΑ)
            ,typeof(Είδος),typeof(BarCodeΕίδους),typeof(ΟικογένειαΕίδους),typeof(ΥποοικογένειαΕίδους),typeof(ΟμάδαΕίδους),typeof(ΚατηγορίαΕίδους)
            ,typeof(ΠαραστατικάΠωλήσεων),typeof(ΓραμμέςΠαραστατικώνΠωλήσεων),typeof(ΣειρέςΠαραστατικώνΠωλήσεων),typeof(ΤρόποςΠληρωμής),typeof(ΤρόποςΑποστολής)
            ,typeof(ΠαραστατικάΕισπράξεων),typeof(ΓραμμέςΠαραστατικώνΕισπράξεων),typeof(ΣειρέςΠαραστατικώνΕισπράξεων),typeof(ΛογαριασμοίΧρηματικώνΔιαθέσιμων),typeof(Αξιόγραφα)
            ,typeof(Τράπεζα),typeof(ΤραπεζικοίΛογαριασμοί)
            ,typeof(Appointment) ,typeof(Πωλητής), typeof(Εργασία),
            typeof(Ενέργεια),typeof(ΙδιότηταΕνέργειας),typeof(ΕπιλογήΙδιότητας)
        };
        public static void InitXpo(string connectionString)
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
                     
        }
        public static async Task<bool> CreateTableData()
        {
            
            await Task.WhenAll(
                 CreateDOYData(),
                 CreateFPAData(),
                 CreateXORAData(),
                 CreateTROPOSPLIROMISData(),
                 CreateTROPOSAPOSTOLISData(),
                 CreateOIKOGENEIAEIDOUSData(),
                 CreateOMADAEIDOUSData(),
                 CreateKATIGORIAEIDOUSData(),
                 CreateYPOOIKOGENEIAEIDOYSData(),
                 CreatePOLIData(),
                 CreateTRAPLOGData()
                );

            var ok = await Task.WhenAll(
                 CreateEIDOSData(),
                 CreateSEIRAPOLData(),
                 CreateSEIRAEISData(),
                 CreateEvent(),
                 CreatePolitisData(),
                 CreateTrapezaData(),
                 CreateΤΚData(),
                 CreateBarCodeEIDOSData()
                 );
            var okP = await CreatePELATISData();
            if (okP)
                await CreateDIEUPELATIData();
            await CreateIDIOTITAData();
            await CreateEPILOGESIDIOTITASData();
            await CreateLOGXRHMDIATHData();
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
        public static async Task<bool> CreatePolitisData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dtPolitis = await getSmartTable("select Oid, Ονοματεπώνυμο, KίνΤηλέφωνο, " +
                        "Οδός, Αριθμός, Email,  FAX, Κείμενο5 from Πωλητής where ");
                if (dtPolitis == null) 
                { 
                    await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Loading Πωλητής", "OK"); 
                    return false; 
                }
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
                        return false;
                    }

                }
                return true;
            }
        }
        public static async Task<bool> CreateDOYData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dtDOY = await getSmartTable("Select Oid, Κωδικός, ΔΟΥ From ΔΟΥ where ");
                if (dtDOY == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Loading ΔΟΥ", "OK");
                    return false;
                }
                foreach (DataRow row in dtDOY.Rows)
                {
                    if (uow.Query<ΔΟΥ>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
                    var doylist = uow.Query<ΔΟΥ>().Where(x => x.Κωδικός == row["Κωδικός"].ToString());
                    if (doylist.Any())
                    {
                        doylist.FirstOrDefault().SmartOid = Guid.Parse((string)row["Oid"]);
                        continue;
                    }
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
                return true;
            }           
        }
        public static async Task CreateFPAData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select ΦΠΑ, ΟμάδαΦΠΑ, ΦΠΑΚανονικό, ΦΠΑΕξαίρεση, ΦΠΑΜειωμένο From ΦΠΑ where ");
                if (dt == null) 
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Loading ΦΠΑ", "OK");
                    return; 
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΦΠΑ>().Where(x => x.Φπαid == (string)row["ΦΠΑ"]).Any())
                    {
                        continue;
                    }
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
        public static async Task CreateXORAData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Χώρα, Συντομογραφία  From Χώρα where ");
                if (dt == null) 
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Loading Χώρα", "OK");
                    return; 
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<Χώρα>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
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
        public static async Task CreateYPOOIKOGENEIAEIDOYSData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Περιγραφή From ΥποοικογένειαΕίδους where ");
                if (dt == null) { return; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΥποοικογένειαΕίδους>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
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
        public static async Task CreateTROPOSPLIROMISData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, ΤρόποςΠληρωμής From ΤρόποςΠληρωμής where ");
                if (dt == null) { return; }
                foreach (DataRow row in dt.Rows)
                {
                    var troposlist = uow.Query<ΤρόποςΠληρωμής>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"]));
                    var troposmeIdioOnoma = uow.Query<ΤρόποςΠληρωμής>().Where(x => x.Τρόποςπληρωμής == row["ΤρόποςΠληρωμής"].ToString());
                    if (troposlist.Any() || troposmeIdioOnoma.Any())
                    {
                        troposmeIdioOnoma.FirstOrDefault().SmartOid = Guid.Parse((string)row["Oid"]);
                        continue;
                    }
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
        public static async Task CreateTROPOSAPOSTOLISData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, ΤρόποςΑποστολής From ΤρόποςΑποστολής where ");
                if (dt == null) { return; }
                foreach (DataRow row in dt.Rows)
                {
                    var troposlist = uow.Query<ΤρόποςΑποστολής>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"]));
                    var troposmeIdioOnoma = uow.Query<ΤρόποςΑποστολής>().Where(x => x.Τρόποςαποστολής == row["ΤρόποςΑποστολής"].ToString());
                    if (troposlist.Any() || troposmeIdioOnoma.Any())
                    {
                        troposmeIdioOnoma.FirstOrDefault().SmartOid = Guid.Parse((string)row["Oid"]);
                        continue;
                    }
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
        public static async Task<bool> CreateSEIRAPOLData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Σειρά, Περιγραφή, ΚίνησηΣυναλασόμενου, ΠρόθεμαΑρίθμησης From ΣειρέςΠαραστατικώνΠωλήσεων where ");
                if (dt == null) { return false; }
                foreach (DataRow row in dt.Rows)
                {
                    var seireslist = uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>().Where(x => x.SmartOid.ToString() == row["Oid"].ToString());
                    var seiresmeIdioOnoma = uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>().Where(x => x.Σειρά == row["Σειρά"].ToString());
                    if (seireslist.Any() || seiresmeIdioOnoma.Any())
                    {
                        seireslist.FirstOrDefault().ΚίνησηΣυναλασόμενου = row["ΚίνησηΣυναλασόμενου"] == DBNull.Value ? 2 : int.Parse(row["ΚίνησηΣυναλασόμενου"].ToString());
                        continue;
                    }
                    ΣειρέςΠαραστατικώνΠωλήσεων data = new ΣειρέςΠαραστατικώνΠωλήσεων(uow);
                    data.SmartOid = Guid.Parse((string)row["Oid"]);
                    data.Σειρά = row["Σειρά"].ToString();
                    data.Περιγραφή = row["Περιγραφή"].ToString();
                    data.ΠρόθεμαΑρίθμησης = Guid.Parse((string)row["ΠρόθεμαΑρίθμησης"]);
                    data.ΚίνησηΣυναλασόμενου = row["ΚίνησηΣυναλασόμενου"] == DBNull.Value ? 2:int.Parse(row["ΚίνησηΣυναλασόμενου"].ToString());
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
                DataTable dt = await getSmartTable("Select Oid, Πόλη From Πόλη where ");
                if (dt == null) { return; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<Πόλη>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
                    var doylist = uow.Query<Πόλη>().Where(x => x.ΟνομαΠόλης == row["Πόλη"].ToString());
                    if (doylist.Any())
                    {
                        doylist.FirstOrDefault().SmartOid = Guid.Parse((string)row["Oid"]);
                        continue;
                    }
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
        public static async Task<bool> CreateΤΚData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select ΤαχυδρομικόςΚωδικός, Πόλη, Νομός, Περιοχή, Χώρα From ΤαχυδρομικόςΚωδικός where ");
                if (dt == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Loading ΤΚ", "OK");
                    return false;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΤαχυδρομικόςΚωδικός>().Where(x => x.Ονοματκ == row["ΤαχυδρομικόςΚωδικός"].ToString()).Any())
                    {
                        continue;
                    }
                    ΤαχυδρομικόςΚωδικός data = new ΤαχυδρομικόςΚωδικός(uow);
                    //data.SmartOid = Guid.Parse((string)row["Oid"]);
                    data.Ονοματκ = row["ΤαχυδρομικόςΚωδικός"].ToString();
                    var pd = row["Πόλη"].ToString();
                    var p = uow.Query<Πόλη>().Where(x => x.SmartOid.ToString() == pd);
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
                        return true;
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
        public static async Task<bool> CreateTrapezaData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Τράπεζα From Τράπεζα where ");
                if (dt == null) { return false; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<Τράπεζα>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
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
                        return true;
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
        public static async Task CreateOMADAEIDOUSData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Ομάδα, ΣειράΕμφάνισης From ΟμάδεςΕίδους where ");
                if (dt == null) { return; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΟμάδαΕίδους>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
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
        public static async Task CreateOIKOGENEIAEIDOUSData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Περιγραφή From ΟικογένειαΕίδους where ");
                if (dt == null) { return; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΟικογένειαΕίδους>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
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
        public static async Task CreateKATIGORIAEIDOUSData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Κατηγορία From ΚατηγορίεςΕίδους where ");
                if (dt == null) { return; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΚατηγορίαΕίδους>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
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
        public static async Task<bool> CreateEIDOSData()
        {
            using (UnitOfWork uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select  Oid, Κωδικός, REPLACE(Περιγραφή,'\"','') as Περιγραφή, ΦΠΑ, ΤιμήΧονδρικής, Ομάδα, Κατηγορία, Οικογένεια, Υποοικογένεια From Είδος where ");
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
        public static async Task<bool> CreateBarCodeEIDOSData()
        {
            using (UnitOfWork uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("SELECT BarCode , ΑκολουθείΤήνΤιμήΕίδους, REPLACE(Περιγραφή, '\"', '') as Περιγραφή, " +
                                                  "ΤιμήΧονδρικής, Είδος, Χρώματα.Χρώματα, Μεγέθη.Μεγέθη, ΦΠΑ"+
                                             " FROM BarCodeΕίδους left join Χρώματα on BarCodeΕίδους.Χρώμα=Χρώματα.Oid left join Μεγέθη on BarCodeΕίδους.Μέγεθος=Μεγέθη.Oid  where BarCodeΕίδους.");
                if (dt == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Loading BarCode", "OK");
                    return false;
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<BarCodeΕίδους>().Where(x => x.Κωδικός == (string)row["BarCode"]).Any())
                    {
                        continue;
                    }
                    BarCodeΕίδους data = new BarCodeΕίδους(uow);                    
                    data.Κωδικός = row["BarCode"].ToString();
                    data.Περιγραφή = row["Περιγραφή"].ToString();
                    data.ΤιμήΧονδρικής = row["ΤιμήΧονδρικής"] != DBNull.Value ? double.Parse(row["ΤιμήΧονδρικής"].ToString()) : 0.0;
                    data.Μέγεθος = row["Μεγέθη"] != DBNull.Value ? row["Μεγέθη"].ToString() : "";
                    data.Χρώμα = row["Χρώματα"] != DBNull.Value ? row["Χρώματα"].ToString() : "";
                    data.ΑκολουθείΤήνΤιμήΕίδους = row["ΑκολουθείΤήνΤιμήΕίδους"].ToString() == "0"? false : true;
                    data.ΕίδοςOid = row["Είδος"] != DBNull.Value ? Guid.Parse(row["Είδος"].ToString()) : Guid.Empty;
                    if (!string.IsNullOrEmpty(row["ΦΠΑ"].ToString()))
                    {
                        var p = uow.Query<ΦΠΑ>().Where(x => x.Φπαid == row["ΦΠΑ"].ToString());
                        data.ΦΠΑ = p.FirstOrDefault();
                    }
                    else
                    {
                        var p = uow.Query<ΦΠΑ>().Where(x => x.Φπαid == "0");
                        data.ΦΠΑ = p.FirstOrDefault();
                    }

                    data.ΤιμήΧονδρικής = double.Parse(row["ΤιμήΧονδρικής"].ToString()); 

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
                DataTable dt = await getSmartTable("Select Oid, Τράπεζα, Swift, Λογαριασμός, IBAN, Υποκατάστημα From ΤραπεζικοίΛογαριασμοί where ");
                if (dt == null) { return; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΤραπεζικοίΛογαριασμοί>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
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
        public static async Task CreateLOGXRHMDIATHData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, Τράπεζα, ΤραπεζικόςΛογαριασμός, Λογαριασμός From ΛογαριασμοίΧρηματικώνΔιαθέσιμων where ");
                if (dt == null) { return; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΛογαριασμοίΧρηματικώνΔιαθέσιμων>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
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
        public static async Task<bool> CreatePELATISData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("Select Oid, REPLACE(Επωνυμία,'\"','') as Επωνυμία, ΚατηγορίαΦΠΑ, ΑΦΜ, Email, " +
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
                DataTable dt = await getSmartTable("Select Oid, REPLACE(Οδός,'\"','') as Οδός, Αριθμός, REPLACE(Περιοχή,'\"','') as Περιοχή, Τηλέφωνο, Τηλέφωνο1, " +
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
                    var eterialist =  uow.Query<ΣτοιχείαΕταιρίας>();
                    if (eterialist.Any())
                    {
                        //var eteria= eterialist.FirstOrDefault();
                        // eteria = data;
                        uow.Delete(eterialist.FirstOrDefault());
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

                στοιχείαΕταιρίας = uow.Query<ΣτοιχείαΕταιρίας>().First();
                return στοιχείαΕταιρίας;
            }
        }
        public static async Task<bool> CreateIDIOTITAData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("SELECT Ιδιότητα.Oid, Ιδιότητα.Περιγραφή, ΤύποςΙδιότητας" +
                        " FROM Ιδιότητα JOIN ΟμάδαΙδιότητας on Ιδιότητα.ΟμάδαΙδιότητας = ΟμάδαΙδιότητας.Oid " +
                        "where ΟμάδαΙδιότητας.Περιγραφή = 'CRM Mobile' and Ιδιότητα.");
                if (dt == null) { return false; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΙδιότηταΕνέργειας>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
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
                        return true;
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
        public static async Task<bool> CreateEPILOGESIDIOTITASData()
        {
            using (var uow = CreateUnitOfWork())
            {
                DataTable dt = await getSmartTable("SELECT ΕπιλογέςΙδιότητας.Oid, Ιδιότητα.Oid as ΙδιότηταOid, ΕπιλογέςΙδιότητας.Περιγραφή " +
                      "FROM Ιδιότητα JOIN ΟμάδαΙδιότητας on Ιδιότητα.ΟμάδαΙδιότητας = ΟμάδαΙδιότητας.Oid " +
                      "join ΕπιλογέςΙδιότητας on ΕπιλογέςΙδιότητας.Ιδιότητα = Ιδιότητα.Oid " +
                      "where ΟμάδαΙδιότητας.Περιγραφή = 'CRM Mobile' and Ιδιότητα.");
                if (dt == null) { return false; }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΕπιλογήΙδιότητας>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
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
                        return true;
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
            HttpClient client = new HttpClient();
            string ip = Preferences.Get("IP", "79.129.5.42");
            string port = Preferences.Get("Port1", "8881");
            string uri = "http://"+ip+":"+port+"/mobile/Values?sql= " + smartTable + "GCRecord is null ";
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
            string ip = Preferences.Get("IP", "79.129.5.42");
            string port = Preferences.Get("Port2", "8882");
            var httpRequest = (HttpWebRequest)WebRequest.Create("http://"+ip+":"+port+"/api/PutDataJson?Type=" + type);
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
        public static void CheckForConnection()
        {
            string who = Preferences.Get("IP", "79.129.5.42");
            AutoResetEvent waiter = new AutoResetEvent(false);

            Ping pingSender = new Ping();

            // When the PingCompleted event is raised,
            // the PingCompletedCallback method is called.
            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "a";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            // Wait 12 seconds for a reply.
            int timeout = 12000;

            // Set options for transmission:
            // The data can go through 64 gateways or routers
            // before it is destroyed, and the data packet
            // cannot be fragmented.
            PingOptions options = new PingOptions(128, true);

            //Console.WriteLine("Time to live: {0}", options.Ttl);
            //Console.WriteLine("Don't fragment: {0}", options.DontFragment);

            // Send the ping asynchronously.
            // Use the waiter as the user token.
            // When the callback completes, it can wake up this thread.
            pingSender.SendAsync(who, timeout, buffer, options, waiter);

            // Prevent this example application from ending.
            // A real application should do something useful
            // when possible.
            //waiter.WaitOne();
            Console.WriteLine("Ping example completed.");
        }
        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            // If the operation was canceled, display a message to the user.
            if (e.Cancelled)
            {
                Console.WriteLine("Ping canceled.");

                // Let the main thread resume.
                // UserToken is the AutoResetEvent object that the main thread
                // is waiting for.
                ((AutoResetEvent)e.UserState).Set();
            }

            // If an error occurred, display the exception to the user.
            if (e.Error != null)
            {
                Console.WriteLine("Ping failed:");
                Console.WriteLine(e.Error.ToString());

                // Let the main thread resume.
                ((AutoResetEvent)e.UserState).Set();
            }

            PingReply reply = e.Reply;

            DisplayReply(reply);

            // Let the main thread resume.
            ((AutoResetEvent)e.UserState).Set();
        }
        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            Console.WriteLine("ping status: {0}", reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }
        }
        public static bool ClearAllData()
        {
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    uow.Delete(new XPCollection<ΣτοιχείαΕταιρίας>(uow));
                    uow.Delete(new XPCollection<Πελάτης>(uow));
                    uow.Delete(new XPCollection<Είδος>(uow));
                    uow.Delete(new XPCollection<BarCodeΕίδους>(uow));
                    uow.Delete(new XPCollection<ΔιευθύνσειςΠελάτη>(uow));
                    uow.Delete(new XPCollection<ΟμάδαΕίδους>(uow));
                    uow.Delete(new XPCollection<ΟικογένειαΕίδους>(uow));
                    uow.Delete(new XPCollection<ΚατηγορίαΕίδους>(uow));
                    uow.Delete(new XPCollection<ΥποοικογένειαΕίδους>(uow));
                    uow.Delete(new XPCollection<ΦΠΑ>(uow));
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }
        public static async Task<double> GetYpoloipo(string customerOid)
        {
            double trexousaxreosh=0;
            double trexousaPistosh=0;
            DataTable dt = await getSmartTable($@"Select _ΤρέχουσαΠίστωση, _ΤρέχουσαΧρέωση
                        From Πελάτης where Oid ='{customerOid}' and ΑΦΜ is not null and ΑΦΜ != '' and ");
            if (dt == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στον υπολογισμό υπολ. Πελάτη", "OK");
                return 0;
            }
            foreach (DataRow row in dt.Rows)
            {
                 trexousaxreosh = row["_ΤρέχουσαΧρέωση"] == DBNull.Value ? 0 : double.Parse(row["_ΤρέχουσαΧρέωση"].ToString());
                 trexousaPistosh = row["_ΤρέχουσαΠίστωση"] == DBNull.Value ? 0 : double.Parse(row["_ΤρέχουσαΠίστωση"].ToString());
            }
            return trexousaxreosh - trexousaPistosh;
        }
        public static async Task<ΣτοιχείαΕταιρίας> GetSTOIXEIAETAIRIASData()
        {
            return await Task<ΣτοιχείαΕταιρίας>.Run(() =>
            {
                ΣτοιχείαΕταιρίας στοιχείαΕταιρίας = null;
                using (var uow = CreateUnitOfWork())
                {
                    if(uow.Query<ΣτοιχείαΕταιρίας>().Any())
                    {
                        στοιχείαΕταιρίας = uow.Query<ΣτοιχείαΕταιρίας>().First();
                    }
                    
                    return στοιχείαΕταιρίας;
                }
            });
        }
    }
}
