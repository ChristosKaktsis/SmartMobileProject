using System;
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
using System.Collections.Generic;

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
            ,typeof(Appointment) ,typeof(Πωλητής), typeof(Εργασία),typeof(ΚινήσειςΠελατών),
            typeof(Ενέργεια),typeof(ΙδιότηταΕνέργειας),typeof(ΕπιλογήΙδιότητας),typeof(Πρότυπα)
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
                //DataTable dtPolitis = await getSmartTable("select Oid, Ονοματεπώνυμο, KίνΤηλέφωνο, " +
                //        "Οδός, Αριθμός, Email,  FAX, Κείμενο5 from Πωλητής where ");
                DataTable dtPolitis = await GetData("getPolites");
                if (dtPolitis.Rows.Count == 0)
                { 
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο Πωλητής", "OK"); 
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
                    politis.Ονοματεπώνυμο = row["Ονοματεπώνυμο"] == DBNull.Value ?"" : row["Ονοματεπώνυμο"].ToString();
                    politis.KίνΤηλέφωνο = row["KίνΤηλέφωνο"] == DBNull.Value ? "" : row["KίνΤηλέφωνο"].ToString();
                    politis.Οδός = row["Οδός"] == DBNull.Value ? "" : row["Οδός"].ToString();
                    politis.Αριθμός = row["Αριθμός"] == DBNull.Value ? "" : row["Αριθμός"].ToString();
                    politis.Email = row["Email"] == DBNull.Value ? "" : row["Email"].ToString();
                    politis.FAX = row["FAX"] == DBNull.Value ? "" : row["FAX"].ToString();

                    uow.Save(politis);
                }

                if (uow.InTransaction)
                {
                    uow.CommitChanges();
                }
                return true;
            }
        }
        public static async Task<bool> CreateDOYData()
        {
            using (var uow = CreateUnitOfWork())
            {
                //DataTable dtDOY = await getSmartTable("Select Oid, Κωδικός, ΔΟΥ From ΔΟΥ where ");
                DataTable dtDOY = await GetData("getDOY");
                if (dtDOY.Rows.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΔΟΥ", "OK");
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
                    doy.Κωδικός = row["Κωδικός"] == DBNull.Value ?"" : row["Κωδικός"].ToString();
                    doy.Περιγραφή = row["ΔΟΥ"] == DBNull.Value ? "" : row["ΔΟΥ"].ToString();

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
                //DataTable dt = await getSmartTable("Select ΦΠΑ, ΟμάδαΦΠΑ, ΦΠΑΚανονικό, ΦΠΑΕξαίρεση, ΦΠΑΜειωμένο From ΦΠΑ where ");
                DataTable dt = await GetData("getFPA");
                if (dt.Rows.Count == 0) 
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΦΠΑ", "OK");
                    return; 
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (uow.Query<ΦΠΑ>().Where(x => x.Φπαid == (string)row["ΦΠΑ"]).Any())
                    {
                        continue;
                    }
                    ΦΠΑ data = new ΦΠΑ(uow);
                    data.Φπαid = row["ΦΠΑ"] == DBNull.Value ? "" : row["ΦΠΑ"].ToString();
                    data.Ομάδαφπα = row["ΟμάδαΦΠΑ"] == DBNull.Value ? "" : row["ΟμάδαΦΠΑ"].ToString();
                    data.Φπακανονικό = row["ΦΠΑΚανονικό"] == DBNull.Value ? 0 : float.Parse(row["ΦΠΑΚανονικό"].ToString());
                    data.Φπαεξαίρεση = row["ΦΠΑΕξαίρεση"] == DBNull.Value ? 0 : float.Parse(row["ΦΠΑΕξαίρεση"].ToString());
                    data.Φπαμειωμένο = row["ΦΠΑΜειωμένο"] == DBNull.Value ? 0 : float.Parse(row["ΦΠΑΜειωμένο"].ToString());

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
                //DataTable dt = await getSmartTable("Select Oid, Χώρα, Συντομογραφία  From Χώρα where ");
                DataTable dt = await GetData("getCountry");
                if (dt.Rows.Count == 0) 
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο Χώρα", "OK");
                    return; 
                }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                   
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
                //DataTable dt = await getSmartTable("Select Oid, Περιγραφή From ΥποοικογένειαΕίδους where ");
                DataTable dt = await GetData("getYpoOikogeneiaEidous");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΥποοικογένειαΕίδους", "OK");
                    return; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select Oid, ΤρόποςΠληρωμής From ΤρόποςΠληρωμής where ");
                DataTable dt = await GetData("getTroposPliromis");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΤρόποςΠληρωμής", "OK");

                    return; }

                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select Oid, ΤρόποςΑποστολής From ΤρόποςΑποστολής where ");
                DataTable dt = await GetData("getTroposApostolis");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΤρόποςΑποστολής", "OK");
                    return; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
            var currentP = ((AppShell)Application.Current.MainPage).πωλητής;
            if(currentP == null)
            {
                Console.WriteLine("Create SeiraPol Politis Is Null !!!!");
                return false;
            }
            using (var uow = CreateUnitOfWork())
            {
                //DataTable dt = await getSmartTable("Select Oid, Σειρά, Περιγραφή, ΚίνησηΣυναλασόμενου,Πωλητής, ΠρόθεμαΑρίθμησης " +
                //    $"From ΣειρέςΠαραστατικώνΠωλήσεων where Ακυρωτικό = 0 and Ενεργή=1 and SmartMobile = 1 and Πωλητής='{currentP.SmartOid}' and ");
                DataTable dt = await GetData("getSeiraPoliseon", $"getSeiraPoliseon,@politisId,{currentP.SmartOid}");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΣειρέςΠαραστατικώνΠωλήσεων", "OK");

                    return false; }
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        ΣειρέςΠαραστατικώνΠωλήσεων data;
                        var seireslist = uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>().Where(x => x.SmartOid.ToString() == row["Oid"].ToString());
                        var seiresmeIdioOnoma = uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>().Where(x => x.Σειρά == row["Σειρά"].ToString());
                        if (seireslist.Any())
                        {
                            //seireslist.FirstOrDefault().ΚίνησηΣυναλασόμενου = row["ΚίνησηΣυναλασόμενου"] == DBNull.Value ? 2 : int.Parse(row["ΚίνησηΣυναλασόμενου"].ToString());
                            //continue;
                            data = seireslist.FirstOrDefault();
                        }
                        else if (seiresmeIdioOnoma.Any())
                        {
                            data = seiresmeIdioOnoma.FirstOrDefault();
                        }
                        else
                        {
                            data = new ΣειρέςΠαραστατικώνΠωλήσεων(uow);
                        }

                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Σειρά = row["Σειρά"].ToString();
                        data.Περιγραφή = row["Περιγραφή"].ToString();
                        data.ΠρόθεμαΑρίθμησης = Guid.Parse((string)row["ΠρόθεμαΑρίθμησης"]);
                        data.ΚίνησηΣυναλασόμενου = row["ΚίνησηΣυναλασόμενου"] == DBNull.Value ? 2 : int.Parse(row["ΚίνησηΣυναλασόμενου"].ToString());
                        data.IDΠωλητή = Guid.Parse((string)row["Πωλητής"]);
                        data.Counter = 0;
                        data.Λιανική = bool.Parse(row["Λιανική"].ToString());
                        uow.Save(data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select Oid, Σειρά, Περιγραφή From ΣειρέςΠαραστατικώνΕισπράξεων where Ενεργή=1 and ");
                DataTable dt = await GetData("getSeiraEispraxeon");

                if (dt.Rows.Count==0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΣειρέςΠαραστατικώνΕισπράξεων", "OK");

                    return false; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
            //DataTable dt = await getSmartTable("Select ΤρέχουσαΤιμήΜετρητή From Μετρητές where Oid = '" + Oid + "' and ");
            DataTable dt = await GetData("getMetritisSeiras", $"getMetritisSeiras,@oid,{Oid}");

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
                //DataTable dt = await getSmartTable("Select Oid, Πόλη From Πόλη where ");
                DataTable dt = await GetData("getCity");

                if (dt.Rows.Count==0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο Πόλη", "OK");

                    return; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select ΤαχυδρομικόςΚωδικός, Πόλη, Νομός, Περιοχή, Χώρα From ΤαχυδρομικόςΚωδικός where ");
                DataTable dt = await GetData("getTK");

                if (dt.Rows.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΤΚ", "OK");
                    return false;
                }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select Oid, Τράπεζα From Τράπεζα where ");
                DataTable dt = await GetData("getTrapeza");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο Τράπεζα", "OK");

                    return false; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select Oid, Ομάδα, ΣειράΕμφάνισης From ΟμάδεςΕίδους where ");
                DataTable dt = await GetData("getOmadaEidous");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΟμάδεςΕίδους", "OK");

                    return; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select Oid, Περιγραφή From ΟικογένειαΕίδους where ");
                DataTable dt = await GetData("getOikogeneiaEidous");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΟικογένειαΕίδους", "OK");

                    return; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select Oid, Κατηγορία From ΚατηγορίεςΕίδους where ");
                DataTable dt = await GetData("getKatigoriaEidous");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΚατηγορίεςΕίδους", "OK");

                    return; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select  Oid, Κωδικός, REPLACE(Περιγραφή,'\"','') as Περιγραφή, ΦΠΑ, ΤιμήΧονδρικής, Ομάδα, Κατηγορία, Οικογένεια, Υποοικογένεια From Είδος where Ενεργό=1 and ");
                DataTable dt = await GetData("getEidos");

                if (dt.Rows.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο Είδος", "OK");
                    return false;
                }
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        Είδος data;
                        if (uow.Query<Είδος>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                        {
                            data = uow.Query<Είδος>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).FirstOrDefault();
                        }
                        else
                        {
                            data = new Είδος(uow);
                        }

                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Κωδικός = row["Κωδικός"].ToString();
                        data.Περιγραφή = row["Περιγραφή"].ToString();
                        data.ΤιμήΧονδρικής = double.Parse(row["ΤιμήΧονδρικής"].ToString());
                        data.ΤιμήΛιανικής = double.Parse(row["ΤιμήΛιανικής"].ToString());

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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                   
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
                //DataTable dt = await getSmartTable("SELECT BarCode , ΑκολουθείΤήνΤιμήΕίδους, REPLACE(Περιγραφή, '\"', '') as Περιγραφή, " +
                //                                  "ΤιμήΧονδρικής, Είδος, REPLACE(Χρώματα.Χρώματα, '\"', '') as Χρώματα,  Μεγέθη.Μεγέθη, ΦΠΑ" +
                //                             " FROM BarCodeΕίδους left join Χρώματα on BarCodeΕίδους.Χρώμα=Χρώματα.Oid left join Μεγέθη on BarCodeΕίδους.Μέγεθος=Μεγέθη.Oid  where BarCodeΕίδους.Ενεργό = '1' and BarCodeΕίδους.");
                DataTable dt = await GetData("getBarcodeEidos");

                if (dt.Rows.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο BarCode", "OK");
                    return false;
                }
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        BarCodeΕίδους data;
                        if (uow.Query<BarCodeΕίδους>().Where(x => x.Κωδικός == (string)row["BarCode"]).Any())
                        {
                            data = uow.Query<BarCodeΕίδους>().Where(x => x.Κωδικός == (string)row["BarCode"]).FirstOrDefault();
                        }
                        else
                        {
                            data = new BarCodeΕίδους();
                        }
                        data.Κωδικός = row["BarCode"].ToString();
                        data.Περιγραφή = row["Περιγραφή"].ToString();
                        data.ΤιμήΧονδρικής = row["ΤιμήΧονδρικής"] != DBNull.Value ? double.Parse(row["ΤιμήΧονδρικής"].ToString()) : 0.0;
                        data.ΤιμήΛιανικής = row["ΤιμήΛιανικής"] != DBNull.Value ? double.Parse(row["ΤιμήΧονδρικής"].ToString()) : 0.0;
                        data.Μέγεθος = row["Μεγέθη"] != DBNull.Value ? row["Μεγέθη"].ToString() : "";
                        data.Χρώμα = row["Χρώματα"] != DBNull.Value ? row["Χρώματα"].ToString() : "";
                        data.ΑκολουθείΤήνΤιμήΕίδους = bool.Parse(row["ΑκολουθείΤήνΤιμήΕίδους"].ToString());
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select Oid, Τράπεζα, Swift, Λογαριασμός, IBAN, Υποκατάστημα From ΤραπεζικοίΛογαριασμοί where ");
                DataTable dt = await GetData("getTrapLogarismos");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΤραπεζικοίΛογαριασμοί", "OK");

                    return; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("Select Oid, Τράπεζα, ΤραπεζικόςΛογαριασμός, Λογαριασμός From ΛογαριασμοίΧρηματικώνΔιαθέσιμων where ");
                DataTable dt = await GetData("getLogXrhmDiath");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΛογαριασμοίΧρηματικώνΔιαθέσιμων", "OK");

                    return; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
            var currentP = ((AppShell)Application.Current.MainPage).πωλητής;
            if (currentP == null)
            {
                Console.WriteLine("Create CreatePELATISData Politis Is Null !!!!");
                return false;
            }
            using (var uow = CreateUnitOfWork())
            {
                bool asktoloadcustomers;
                if (Preferences.Get("LACFlag", false))
                {
                    asktoloadcustomers = Preferences.Get("LoadAllCustomers", false);
                }
                else
                {
                    asktoloadcustomers = await Application.Current.MainPage.DisplayAlert(
                    "Πελάτες", "Θέλετε να κατέβουν όλοι οι πελάτες ή του συγκεκριμένου πωλητή", "Όλοι", "Του πωλητή");
                    Preferences.Set("LoadAllCustomers", asktoloadcustomers);//set it glob ???????
                    Preferences.Set("LACFlag", true);
                }
                //string query = string.Empty;
                DataTable dt;
                if (asktoloadcustomers)
                {
                    //query = "Select Oid, REPLACE(Επωνυμία,'\"','') as Επωνυμία, ΚατηγορίαΦΠΑ, ΑΦΜ, Email, " +
                    //    "ΔΟΥ, ΚεντρικήΔιευθυνση, Πωλητής, Κείμενο5 , ΔιακριτικόςΤίτλος From Πελάτης " +
                    //    $"where ΑΦΜ is not null and ΑΦΜ != '' and Ενεργός=1 and ";
                     dt = await GetData("getPelates");
                }
                else
                {
                    //query = "Select Oid, REPLACE(Επωνυμία,'\"','') as Επωνυμία, ΚατηγορίαΦΠΑ, ΑΦΜ, Email, " +
                    //    "ΔΟΥ, ΚεντρικήΔιευθυνση, Πωλητής, Κείμενο5 , ΔιακριτικόςΤίτλος From Πελάτης " +
                    //    $"where ΑΦΜ is not null and ΑΦΜ != '' and Ενεργός=1 and Πωλητής='{currentP.SmartOid}' and ";
                     dt = await GetData("getPelatesFromPoliti", $"getPelatesFromPoliti,@politisId,{currentP.SmartOid}");
                }
                //DataTable dt = await getSmartTable(query);
                if (dt.Rows.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο Πελάτης", "OK");
                    return false;
                }
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        Πελάτης data = new Πελάτης(uow); 
                        var pelatiGuid = uow.Query<Πελάτης>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"]));
                        if (pelatiGuid.Any())
                        {
                            //continue;
                            data = pelatiGuid.FirstOrDefault();
                        }
                        if (row["Κείμενο5"] != DBNull.Value)
                        {
                            var pelatisKeim5 = uow.Query<Πελάτης>().Where(x => x.SmartOid == Guid.Parse((string)row["Κείμενο5"]));
                            if (pelatisKeim5.Any())
                            {
                                //continue;
                                data = pelatisKeim5.FirstOrDefault();
                            }
                        }
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Επωνυμία = row["Επωνυμία"].ToString();
                        data.Διακριτικόςτίτλος = row["ΔιακριτικόςΤίτλος"].ToString();
                        data.ΚατηγορίαΦΠΑ = int.Parse(row["ΚατηγορίαΦΠΑ"].ToString());
                        data.ΑΦΜ = row["ΑΦΜ"].ToString();
                        data.Email = row["Email"].ToString();
                        if (row["Πωλητής"] != DBNull.Value)
                        {
                            var politis = uow.Query<Πωλητής>().Where(x => x.SmartOid == Guid.Parse((string)row["Πωλητής"]));
                            data.Πωλητής = politis.FirstOrDefault();
                        }
                        if (row["ΔΟΥ"] != DBNull.Value)
                        {
                            var doy = uow.Query<ΔΟΥ>().Where(x => x.SmartOid == Guid.Parse((string)row["ΔΟΥ"]));
                            data.ΔΟΥ = doy.FirstOrDefault();
                        }
                        if (row["ΚεντρικήΔιευθυνση"] != DBNull.Value)
                        {
                            //var dieu = uow.Query<ΔιευθύνσειςΠελάτη>().Where(x => x.SmartOid == Guid.Parse((string)row["ΚεντρικήΔιευθυνση"]));
                            //  data.ΔιευθύνσειςΠελάτη.Add(dieu.FirstOrDefault());

                            data.SmartOidΚεντρικήΔιεύθυνση = Guid.Parse(row["ΚεντρικήΔιευθυνση"].ToString());
                        }
                        //add adress to customer
                        await AddAddressToCustomer(uow, data);
                        await uow.SaveAsync(data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
        private static async Task AddAddressToCustomer(UnitOfWork uow, Πελάτης πελάτης)
        {
            if (πελάτης == null)
                return;

            DataTable dt;
            //DataTable dt = await getSmartTable("Select Oid, REPLACE(Οδός,'\"','') as Οδός, Αριθμός, REPLACE(Περιοχή,'\"','') as Περιοχή, Τηλέφωνο, Τηλέφωνο1, " +
            //       "KίνΤηλέφωνο, Κείμενο5, FAX, ΓεωγραφικόΠλάτος, ΓεωγραφικόΜήκος, ΤΚ, Πόλη, Πελάτης From ΔιευθύνσειςΠελάτη where ");
            dt = await GetData("getDieuthinseisPelatiWithID", $"getDieuthinseisPelatiWithID,@pelatisId,{πελάτης.SmartOid}");

            if (dt.Rows.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΔιευθύνσειςΠελάτη", "OK");
                return ;
            }
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ΔιευθύνσειςΠελάτη data = null;
                    var idaddress = uow.Query<ΔιευθύνσειςΠελάτη>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"]));
                    if (idaddress.Any())
                    {
                        data = idaddress.FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(row["Κείμενο5"].ToString()))
                    {
                        var k5address = uow.Query<ΔιευθύνσειςΠελάτη>().Where(x => x.SmartOid == Guid.Parse((string)row["Κείμενο5"]));
                        if (k5address.Any())
                        {
                            data = k5address.FirstOrDefault();
                        }
                    }
                    if(data == null)
                        data = new ΔιευθύνσειςΠελάτη(uow);
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

                    data.Πελάτης = πελάτης;
                    if (data.Πελάτης.SmartOidΚεντρικήΔιεύθυνση == data.SmartOid)
                        data.Πελάτης.ΚεντρικήΔιευθυνση = data;

                    uow.Save(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    await Application.Current.MainPage.DisplayAlert("Εισαγωγή Διεύθυνση στον Πελάτη",
                         $"Κάτι πήγε στραβά στην εισαγωγή Διεύθυνσης Πελάτη \n {ex}", 
                        "OK");
                }
            }
        }
        public static async Task<bool> CreateDIEUPELATIData()
        {
            //using (var uow = CreateUnitOfWork())
            //{
            //    DataTable dt;
            //    //DataTable dt = await getSmartTable("Select Oid, REPLACE(Οδός,'\"','') as Οδός, Αριθμός, REPLACE(Περιοχή,'\"','') as Περιοχή, Τηλέφωνο, Τηλέφωνο1, " +
            //    //       "KίνΤηλέφωνο, Κείμενο5, FAX, ΓεωγραφικόΠλάτος, ΓεωγραφικόΜήκος, ΤΚ, Πόλη, Πελάτης From ΔιευθύνσειςΠελάτη where ");
            //    dt = await GetData("getDieuthinseisPelati");

            //    if (dt.Rows.Count == 0)
            //    {
            //        await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΔιευθύνσειςΠελάτη", "OK");
            //        return false;
            //    }
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        try
            //        {
            //            if (uow.Query<ΔιευθύνσειςΠελάτη>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
            //            {
            //                continue;
            //            }
            //            if (!string.IsNullOrEmpty(row["Κείμενο5"].ToString()))
            //            {
            //                if (uow.Query<ΔιευθύνσειςΠελάτη>().Where(x => x.SmartOid == Guid.Parse((string)row["Κείμενο5"])).Any())
            //                {
            //                    continue;
            //                }
            //            }
            //            ΔιευθύνσειςΠελάτη data = new ΔιευθύνσειςΠελάτη(uow);
            //            data.SmartOid = Guid.Parse((string)row["Oid"]);
            //            data.Οδός = row["Οδός"].ToString();
            //            data.Αριθμός = row["Αριθμός"].ToString();
            //            data.Περιοχή = row["Περιοχή"].ToString();
            //            data.Τηλέφωνο = row["Τηλέφωνο"].ToString();
            //            data.Τηλέφωνο1 = row["Τηλέφωνο1"].ToString();
            //            data.Kίντηλέφωνο = row["KίνΤηλέφωνο"].ToString();
            //            data.FAX = row["FAX"].ToString();

            //            if (!string.IsNullOrEmpty(row["ΓεωγραφικόΠλάτος"].ToString()))
            //            {
            //                data.ΓεωγραφικόΠλάτος = double.Parse(row["ΓεωγραφικόΠλάτος"].ToString());
            //            }
            //            if (!string.IsNullOrEmpty(row["ΓεωγραφικόΜήκος"].ToString()))
            //            {
            //                data.ΓεωγραφικόΜήκος = double.Parse(row["ΓεωγραφικόΜήκος"].ToString());
            //            }
            //            if (!string.IsNullOrEmpty(row["ΤΚ"].ToString()))
            //            {
            //                var tk = uow.Query<ΤαχυδρομικόςΚωδικός>().Where(x => x.Ονοματκ == row["ΤΚ"].ToString());
            //                data.ΤΚ = tk.FirstOrDefault();
            //            }
            //            if (!string.IsNullOrEmpty(row["Πόλη"].ToString()))
            //            {
            //                var poli = uow.Query<Πόλη>().Where(x => x.SmartOid == Guid.Parse((string)row["Πόλη"]));
            //                data.Πόλη = poli.FirstOrDefault();
            //            }
            //            if (!string.IsNullOrEmpty(row["Πελάτης"].ToString()))
            //            {
            //                var pelatis = uow.Query<Πελάτης>().Where(x => x.SmartOid == Guid.Parse((string)row["Πελάτης"]));
            //                data.Πελάτης = pelatis.FirstOrDefault();
            //                if (data.Πελάτης != null)
            //                {
            //                    if (data.Πελάτης.SmartOidΚεντρικήΔιεύθυνση == data.SmartOid)
            //                    {
            //                        data.Πελάτης.ΚεντρικήΔιευθυνση = data;
            //                    }
            //                }
            //            }
            //            uow.Save(data);
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex);
            //        }
                    
            //    }

            //    if (uow.InTransaction)
            //    {
            //        try
            //        {
            //            uow.CommitChanges();
            //        }
            //        catch (Exception exc)
            //        {
            //            uow.RollbackTransaction();
            //            Console.WriteLine("{0} Exeption In XPoHelper inTransaction Caught!!!", exc);
            //            return false;
            //        }

            //    }
            //}
            return true;
        }
        public static async Task<ΣτοιχείαΕταιρίας> CreateSTOIXEIAETAIRIASData()
        {
            ΣτοιχείαΕταιρίας στοιχείαΕταιρίας;
            using (var uow = CreateUnitOfWork())
            {
                //DataTable dt = await getSmartTable("Select Oid, Επωνυμία, ΚατηγορίαΦΠΑ, ΑΦΜ, Email," +
                //       " ΔΟΥ, Οδός, Αριθμός, ΔικτυακόςΤόπος, Περιοχή, Τηλέφωνο, Τηλέφωνο1, ΤΚ, Πόλη," +
                //       " UsernameΥπηρεσίαςΣτοιχείωνΜητρώου, PasswordΥπηρεσίαςΣτοιχείωνΜητρώου, Fax From ΣτοιχείαΕταιρίας where ");
                DataTable dt = await GetData("getStoixeiaEtaireias");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΣτοιχείαΕταιρίας", "OK");

                    return null; }
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        ΣτοιχείαΕταιρίας data;
                        if (uow.Query<ΣτοιχείαΕταιρίας>().Any())
                        {
                            data = uow.Query<ΣτοιχείαΕταιρίας>().FirstOrDefault();
                        }
                        else
                        {
                            data = new ΣτοιχείαΕταιρίας(uow);
                        }
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
                        //var eterialist =  uow.Query<ΣτοιχείαΕταιρίας>();
                        //if (eterialist.Any())
                        //{
                        //    //var eteria= eterialist.FirstOrDefault();
                        //    // eteria = data;
                        //    uow.Delete(eterialist.FirstOrDefault());
                        //}


                        uow.Save(data);

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
                //DataTable dt = await getSmartTable("SELECT Ιδιότητα.Oid, Ιδιότητα.Περιγραφή, ΤύποςΙδιότητας" +
                //        " FROM Ιδιότητα JOIN ΟμάδαΙδιότητας on Ιδιότητα.ΟμάδαΙδιότητας = ΟμάδαΙδιότητας.Oid " +
                //        "where ΟμάδαΙδιότητας.Περιγραφή = 'CRM Mobile' and Ιδιότητα.");
                DataTable dt = await GetData("getIdiotita");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο Ιδιότητα", "OK");

                    return false; }
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        ΙδιότηταΕνέργειας data = new ΙδιότηταΕνέργειας(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Περιγραφή = row["Περιγραφή"].ToString();
                        data.Τύποςιδιότητας = int.Parse(row["ΤύποςΙδιότητας"].ToString());

                        uow.Save(data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    if (uow.Query<ΙδιότηταΕνέργειας>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                    {
                        continue;
                    }
                    
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
                //DataTable dt = await getSmartTable("SELECT ΕπιλογέςΙδιότητας.Oid, Ιδιότητα.Oid as ΙδιότηταOid, ΕπιλογέςΙδιότητας.Περιγραφή " +
                //      "FROM Ιδιότητα JOIN ΟμάδαΙδιότητας on Ιδιότητα.ΟμάδαΙδιότητας = ΟμάδαΙδιότητας.Oid " +
                //      "join ΕπιλογέςΙδιότητας on ΕπιλογέςΙδιότητας.Ιδιότητα = Ιδιότητα.Oid " +
                //      "where ΟμάδαΙδιότητας.Περιγραφή = 'CRM Mobile' and Ιδιότητα.");
                DataTable dt = await GetData("getEpilogesIdiotitas");

                if (dt.Rows.Count == 0) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΕπιλογέςΙδιότητας", "OK");

                    return false; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                   
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
                //DataTable dt = await getSmartTable("SELECT event.Oid, Resource.Caption, Subject, Description, StartOn, EndOn, AllDay, Label, Location FROM Event " +
                //    "join ResourceResources_EventEvents on ResourceResources_EventEvents.Events=Event.Oid " +
                //    "join Resource on Resource.Oid = ResourceResources_EventEvents.Resources where " +
                //    "datepart(year,StartOn) >= datepart(YEAR, GETDATE()) and DATEPART(month, StartOn)>= DATEPART(month, GETDATE()) AND Event.");
                DataTable dt = await GetData("getEvents");

                //if (dt.Rows.Count == 0) {
                //    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο Event", "OK");

                //    return false; }
                foreach (DataRow row in dt.Rows)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
        public static async Task<bool> CreateKINISEIS(string customerid)
        {
            using (var uow = CreateUnitOfWork())
            {
                if (string.IsNullOrEmpty(customerid))
                    return false;
                //DataTable dt = await getSmartTable2($@"select top(10) Oid ,Ημνία,Πελάτης,Παραστατικό,Χρέωση,Πίστωση FROM ΚινήσειςΠελατών where Πελάτης='{customerid}' and GCRecord is null order by (Ημνία) desc");
                DataTable dt = await GetData("getKiniseisPelati", $"getKiniseisPelati,@pelatisId,{customerid}");

                if (dt == null) {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο ΚινήσειςΠελατών", "OK");

                    return false; }
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        if (uow.Query<ΚινήσειςΠελατών>().Where(x => x.SmartOid == Guid.Parse((string)row["Oid"])).Any())
                        {
                            continue;
                        }
                        ΚινήσειςΠελατών data = new ΚινήσειςΠελατών(uow);
                        data.SmartOid = Guid.Parse((string)row["Oid"]);
                        data.Ημνία = row["Ημνία"] != DBNull.Value ? DateTime.Parse(row["Ημνία"].ToString()) : DateTime.MinValue;
                        data.Παραστατικό = row["Παραστατικό"] != DBNull.Value ? row["Παραστατικό"].ToString() : string.Empty;
                        data.Πελάτης = row["Πελάτης"] != DBNull.Value ? row["Πελάτης"].ToString() : string.Empty;
                        data.Χρέωση = row["Χρέωση"] != DBNull.Value ? decimal.Parse(row["Χρέωση"].ToString()) : 0;
                        data.Πίστωση = row["Πίστωση"] != DBNull.Value ? decimal.Parse(row["Πίστωση"].ToString()) : 0;
                        data.Υπόλοιπο = data.Χρέωση - data.Πίστωση;
                        uow.Save(data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
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
        public static async Task<string> GetCurrentYPOLOIPO(string customerid)
        {
            if (string.IsNullOrEmpty(customerid))
                return string.Empty;
            //DataTable dt = await getSmartTable($@"select _ΤρέχουσαΧρέωση-_ΤρέχουσαΠίστωση as Υπόλοιπο FROM Πελάτης where Oid='{customerid}' and ");
            DataTable dt = await GetData("getTrexonYpoloipoPelati", $"getTrexonYpoloipoPelati,@pelatisId,{customerid}");

            string currentypoloipo = string.Empty;
            if (dt == null) {
                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν φορτώθηκε κανένα αντικείμενο Υπόλοιπο", "OK");

                return string.Empty; }
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    currentypoloipo = row["Υπόλοιπο"] != DBNull.Value ? row["Υπόλοιπο"].ToString() : string.Empty;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return currentypoloipo;
        }
        public static async Task<bool> setSmartTable(string json, string type)
        {
            //string authval = "DemoAdmin:DemoPass";
            string authval = $"{Preferences.Get("uname", "DemoAdmin")}:{ Preferences.Get("passwrd", "DemoPass")}";
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
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Alert", "Κάτι πήγε στραβά στο Upload. " +
                    "Ελεγξτε την συνδεση σας", "OK");
                return false;
            }
            
        }
        public static async Task<DataTable> GetData(string method, string param = "Nothing")
        {
            DataTable dt = new DataTable();
            try
            {
                string JSONString = await GetJSONFromhttpResponse(method,param);
                dt = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));
            }
            catch(JsonSerializationException js)
            {
                Console.WriteLine(js);
                await Application.Current.MainPage.DisplayAlert(
                    "Alert", $"Κάτι πήγε στραβά στο method='{method}' Get JSON Deserialize Object. ", "OK");
            }
            catch(WebException web)
            {
                Console.WriteLine(web);
                await Application.Current.MainPage.DisplayAlert(
                    "Alert", $"Κάτι πήγε στραβά στο GetJSON method='{method}' From httpResponse. ", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Alert", $"Κάτι πήγε στραβά στο method='{method}' Get.", "OK");
            }
            return dt;
        }
        private static async Task<string> GetJSONFromhttpResponse(string method,string param = "Nothing")
        {
            string authval = $"{Preferences.Get("uname", "DemoAdmin")}:{ Preferences.Get("passwrd", "DemoPass")}";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(authval);
            string conv = System.Convert.ToBase64String(plainTextBytes);
            string ip = Preferences.Get("IP", "79.129.5.42");
            string port = Preferences.Get("Port2", "8882");
            var httpRequest = (HttpWebRequest)WebRequest.Create("http://" + ip + ":" + port +
                $"/api/GetDataJson?SelectMethod={method}&RelationParameter=Nothing&Parameters={param}");
            httpRequest.ContentType = "application/json";
            httpRequest.Method = "GET";
            httpRequest.Headers.Add("Authorization", "Basic " + conv);
            var httpResponse = await httpRequest.GetResponseAsync();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                result = result.Replace("\\", "");
                result = result.Remove(0, 1);
                result = result.Remove(result.Length - 1, 1);
                return result;
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
        public static async Task<double> GetYpoloipo(string customerOid)
        {
            double trexousaxreosh=0;
            double trexousaPistosh=0;
            //DataTable dt = await getSmartTable($@"Select _ΤρέχουσαΠίστωση, _ΤρέχουσαΧρέωση
            //            From Πελάτης where Oid ='{customerOid}' and ΑΦΜ is not null and ΑΦΜ != '' and ");
            DataTable dt = await GetData("getYpoloipoPelati", $"getYpoloipoPelati,@pelatisId,{customerOid}");

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
        public static async Task DeleteAllΕίδοςData()
        {
            using (UnitOfWork uow = CreateUnitOfWork())
            {
                var objtodelete = uow.Query<Είδος>();
                foreach(var item in objtodelete)
                    await uow.DeleteAsync(item);

                uow.CommitChanges();
            }
        }
        public static async Task DeleteAllΠελάτηςData()
        {
            using (UnitOfWork uow = CreateUnitOfWork())
            {
                var objtodelete = uow.Query<Πελάτης>();
                foreach (var item in objtodelete)
                    await uow.DeleteAsync(item);

                uow.CommitChanges();
            }
        }
        public static async Task DeleteAllΣεράData()
        {
            using (UnitOfWork uow = CreateUnitOfWork())
            {
                var objtodelete = uow.Query<ΣειρέςΠαραστατικώνΕισπράξεων>();
                foreach (var item in objtodelete)
                    await uow.DeleteAsync(item);

                var objtodelete2 = uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>();
                foreach (var item in objtodelete2)
                    await uow.DeleteAsync(item);
                uow.CommitChanges();
            }
        }
        public static async Task DeleteAllKinisisData()
        {
            using (UnitOfWork uow = CreateUnitOfWork())
            {
                var objtodelete = uow.Query<ΚινήσειςΠελατών>();
                foreach (var item in objtodelete)
                    await uow.DeleteAsync(item);

                uow.CommitChanges();
            }
        }
        public static async Task DeleteAllData()
        {
            await Task.WhenAll(
                DeleteAllΕίδοςData(),
                DeleteAllΠελάτηςData(),
                DeleteAllΣεράData(),
                DeleteAllKinisisData()
                );
        }
        /*
        public static async Task<DataTable> getSmartTable(string smartTable)
        {
            HttpClient client = new HttpClient();
            string ip = Preferences.Get("IP", "79.129.5.42");
            string port = Preferences.Get("Port1", "8881");
            string uri = "http://" + ip + ":" + port + "/mobile/Values?sql= " + smartTable + "GCRecord is null ";
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
        //delete this afternew way
        public static async Task<DataTable> getSmartTable2(string smartTable)
        {
            HttpClient client = new HttpClient();
            string ip = Preferences.Get("IP", "79.129.5.42");
            string port = Preferences.Get("Port1", "8881");
            string uri = "http://" + ip + ":" + port + "/mobile/Values?sql= " + smartTable;
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
        */
    }
}
