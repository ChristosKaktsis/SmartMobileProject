using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.Services
{
    public class CreatePrintView
    {
        ΣτοιχείαΕταιρίας στοιχείαΕταιρίας ;
        XPCollection<ΦΠΑ> φπά;
        private string στοιχείαΕταιρίας_ΔΟΥ_Περιγραφή = "";
        private string order_Πελάτης_ΚεντρικήΔιευθυνση_τηλ = "";

        private string order_Πελάτης_ΚεντρικήΔιευθυνση_Πόλη_ΟνομαΠόλης = "";
        private string order_Πελάτης_ΔΟΥ_Περιγραφή="";
        private string στοιχείαΕταιρίας_ΤΚ_Ονοματκ="";
        private string στοιχείαΕταιρίας_Πόλη_ΟνομαΠόλης ="";
        private string στοιχείαΕταιρίας_ΤΚ_Χώρα="";
        private string order_Πελάτης_ΚεντρικήΔιευθυνση_ΤΚ_Ονοματκ="";

        bool checkEtairia()
        {
            if (στοιχείαΕταιρίας.ΔΟΥ != null)
            {
                στοιχείαΕταιρίας_ΔΟΥ_Περιγραφή = στοιχείαΕταιρίας.ΔΟΥ.Περιγραφή;
            }
            if (στοιχείαΕταιρίας.ΤΚ != null)
            {
                στοιχείαΕταιρίας_ΤΚ_Ονοματκ = στοιχείαΕταιρίας.ΤΚ.Ονοματκ;
                στοιχείαΕταιρίας_ΤΚ_Χώρα = στοιχείαΕταιρίας.ΤΚ.Χώρα;
            }
            if (στοιχείαΕταιρίας.Πόλη != null)
            {
                στοιχείαΕταιρίας_Πόλη_ΟνομαΠόλης = στοιχείαΕταιρίας.Πόλη.ΟνομαΠόλης;
            }
            return  string.IsNullOrEmpty(στοιχείαΕταιρίας.Επωνυμία) ||
                string.IsNullOrEmpty(στοιχείαΕταιρίας.ΑΦΜ);
        }
        private bool checkPelatis(Πελάτης πελάτης)
        {
            if(πελάτης.ΚεντρικήΔιευθυνση != null)
            {
                order_Πελάτης_ΚεντρικήΔιευθυνση_τηλ = πελάτης.ΚεντρικήΔιευθυνση.Τηλέφωνο;
                if (πελάτης.ΚεντρικήΔιευθυνση.Πόλη != null)
                {
                    order_Πελάτης_ΚεντρικήΔιευθυνση_Πόλη_ΟνομαΠόλης = πελάτης.ΚεντρικήΔιευθυνση.Πόλη.ΟνομαΠόλης;
                }
                if (πελάτης.ΚεντρικήΔιευθυνση.ΤΚ != null)
                {
                    order_Πελάτης_ΚεντρικήΔιευθυνση_ΤΚ_Ονοματκ = πελάτης.ΚεντρικήΔιευθυνση.ΤΚ.Ονοματκ;
                }
            }
           
            if (πελάτης.ΔΟΥ != null)
            {
                order_Πελάτης_ΔΟΥ_Περιγραφή = πελάτης.ΔΟΥ.Περιγραφή;
            }
            return string.IsNullOrEmpty(πελάτης.Επωνυμία) ||
                string.IsNullOrEmpty(πελάτης.ΑΦΜ);
        }
        public void CreatePrint(string printpage)
        {
            if (string.IsNullOrEmpty(printpage))
                return;
            WebView browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = printpage;
            browser.Source = htmlSource;
            var printService = DependencyService.Get<IPrintService>();
            printService.Print(browser);
        }
        public async Task<string> Page1(ΠαραστατικάΠωλήσεων order)
        {
            στοιχείαΕταιρίας = await XpoHelper.GetSTOIXEIAETAIRIASData();
            if (στοιχείαΕταιρίας == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν Υπαρχουν Στοιχεια Εταιρίας", "OK");
                return null;
            }
            if (checkEtairia())
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν Ειναι σωστά συμπληρομένα τα Στοιχεια Εταιρίας.", "OK");
                return null;
            }
            if (checkPelatis(order.Πελάτης))
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν Ειναι σωστά συμπληρομένα τα του Πελάτη.", "OK");
                return string.Empty;
            }
            decimal sunoloFPA6 = 0;
            decimal sunoloFPA13 = 0;
            decimal sunoloFPA23 = 0;
            decimal kathAxia6 = 0;
            decimal kathAxia13 = 0;
            decimal kathAxia23 = 0;
            decimal sunAxia6 = 0;
            decimal sunAxia13 = 0;
            decimal sunAxia23 = 0;
            double ProhgoumenoYpoloipo = 0;
            double NeoYpoloipo = 0;
            string grammes = "";
            foreach (var i in order.ΓραμμέςΠαραστατικώνΠωλήσεων)
            {
                grammes += "<tr>" +
                            "<td>" + i.Είδος.Περιγραφή + "</td>" +
                            "<td style=\"text-align: right\">" + i.Ποσότητα + "</td>" +
                            "<td style=\"text-align: right\">" + i.Τιμή + "</td>" +
                            "<td style=\"text-align: right\">" + i.ΑξίαΓραμμής.ToString("0.##") + "</td>" +
                            "<td style=\"text-align: right\">" + i.ΠοσοστόΦπα * 100 + "%</td>" +
                       "</tr>";

                switch (i.ΠοσοστόΦπα * 100)
                {
                    case 6:
                        kathAxia6 += i.ΚαθαρήΑξία;
                        sunoloFPA6 += i.Φπα;
                        sunAxia6 += i.ΑξίαΓραμμής;
                        break;
                    case 13:
                        kathAxia13 += i.ΚαθαρήΑξία;
                        sunoloFPA13 += i.Φπα;
                        sunAxia13 += i.ΑξίαΓραμμής;
                        break;
                    case 24:
                        kathAxia23 += i.ΚαθαρήΑξία;
                        sunoloFPA23 += i.Φπα;
                        sunAxia23 += i.ΑξίαΓραμμής;
                        break;
                }
            }
            string τρόποςΑποστολής = "";
            string τρόποςΠληρωμής = "";
            if (order.ΤρόποςΑποστολής != null || order.ΤρόποςΠληρωμής != null)
            {
                τρόποςΑποστολής = order.ΤρόποςΑποστολής.Τρόποςαποστολής;
                τρόποςΠληρωμής = order.ΤρόποςΠληρωμής.Τρόποςπληρωμής;
            }
            string source = @"<html >
                                <head>
                                    <title></title>    
                                    <style>
                                        table {
                                    border-collapse:separate;
                                    border:solid black 1px;
                                    border-radius:6px;
                                    -moz-border-radius:6px;
                                }

                                td, th {
                                    border-left:solid black 1px;
                                    border-top:solid black 1px;
                                }

                                th {
                                    background-color: blue;
                                    border-top: none;
                                }

                                td:first-child, th:first-child {
                                     border-left: none;
                                }
                                    </style>
   
                                </head>" +
                                "<body style=\" width: 80mm;\">" +
                                    "<table style=\"width: 100%\">" + @"
                                        <tr>" +
                                            "<td colspan=\"4\" style=\"text-align: center\">Στοιχεια Παραστατικού</td>" + @"
                                        </tr>
                                        <tr>" +
                                            "<td colspan=\"4\" style=\"text-align: center\">" + order.Σειρά.Περιγραφή + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>ΣΕΙΡΑ:</td>
                                            <td>" + order.Σειρά.Σειρά + "</td>" + @"
                                            <td>ΑΡΙΘΜΟΣ:</td>
                                            <td>" + order.Σειρά.Counter + "</td>" + @"
                                        </tr>
                                        <tr>" +
                                            "<td colspan=\"2\">Ημερ. έκδοσης :</td>" +
                                            "<td colspan=\"2\">" + order.Ημνία + "</td>" + @"
                                        </tr>
                                        <tr>" +
                                            "<td colspan=\"2\">Ημέρ. παράδοσης :</td>" +
                                            "<td colspan=\"2\"></td>" + @"
                                        </tr>
                                    </table>
                                    <div>
                                        <p>
                                        </p>
                                    </div>" +
                                 "<table style=\"width: 80mm\">" + @"
                                        <tr>" +
                                            "<td colspan=\"2\" style=\"text-align: center\">ΣΤΟΙΧΕΙΑ ΕΚΔΟΤΗ</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>Επωνυμία :</td>
                                            <td>" + στοιχείαΕταιρίας.Επωνυμία + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>Οδός :</td>
                                            <td>" + στοιχείαΕταιρίας.Οδός + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>Α.Φ.Μ. :</td>
                                            <td>" + στοιχείαΕταιρίας.ΑΦΜ + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>ΔΟΥ :</td>
                                            <td>" + στοιχείαΕταιρίας_ΔΟΥ_Περιγραφή + "</td>" + @"
                                        </tr>
                                    </table>
                                     <div>
                                         <p>
                                        </p>
                                    </div>
                                    <table >
                                        <tr>" +
                                            "<td colspan=\"2\" style=\"text-align: center\">ΣΤΟΙΧΕΙΑ ΠΕΛΑΤΗ</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>ΕΠΩΝΥΜΙΑ</td>" +
                                            "<td style=\"width: 50%\">" + order.Πελάτης.DisplayName + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>ΕΠΑΓΓΕΛΜΑ</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>ΔΙΕΥΘΥΝΣΗ</td>
                                            <td>" + order.Πελάτης.ΚεντρικήΔιευθυνση.Addresstring + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>ΠΟΛΗ</td>
                                            <td>" + order_Πελάτης_ΚεντρικήΔιευθυνση_Πόλη_ΟνομαΠόλης + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>Α.Φ.Μ.</td>
                                            <td>" + order.Πελάτης.ΑΦΜ + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>Δ.Ο.Υ.</td>
                                            <td>" + order_Πελάτης_ΔΟΥ_Περιγραφή + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>ΤΗΛ.</td>
                                            <td>" + order_Πελάτης_ΚεντρικήΔιευθυνση_τηλ + "</td>" + @"
                                        </tr>
                                    </table>
                                     <div>
                                         <p>
                                        </p>
                                    </div>
                                    <table >
                                        <tr>" +
                                            "<td colspan=\"2\" style=\"text-align: center\">ΛΟΙΠΕΣ ΠΛΗΡΟΦΟΡΙΕΣ</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>Σχ.Παραστατικό:</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>Σκοπός Διακίνησης :</td>
                                            <td>" + order.Σειρά.Περιγραφή + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>ΔΙΕΥΘ. ΠΑΡΑΔΟΣΗΣ :</td>
                                            <td>" + order.ΔιεύθυνσηΠαράδοσης.Addresstring + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>Τρόπος Αποστ:</td>
                                            <td>" + τρόποςΑποστολής + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>Τρόπος Πληρωμης :</td>
                                            <td>" + τρόποςΠληρωμής + "</td>" + @"
                                        </tr>
                                    </table>
                                     <div>
                                         <p>
                                        </p>
                                    </div>
   
                                    <table>
                                        <tr>
                                            <td>ΠΕΡΙΓΡΑΦΗ</td>
                                            <td>ΠΟΣ.</td>
                                            <td>ΤΙΜΗ</td>
                                            <td>ΑΞΙΑ</td>
                                            <td>Φ.Π.Α.</td>
                                        </tr>" + grammes + @"
        
                                    </table>
                                     <div>
                                         <p>
                                        </p>
                                    </div>
                                    <table >
                                        <tr>
                                            <td>Τελικό σύνολο:</td>
                                            <td>" + order.ΑξίαΠαραστατικού.ToString("0.##") + " €</td>" + @"
                                        </tr>
                                    </table>
                                     <div>
                                         <p>
                                        </p>
                                    </div>" +
                                    "<table style=\"width: 80mm\">" + @"
                                        <tr>" +
                                            "<td colspan=\"4\" style=\"text-align: center\">ΑΝΑΛΥΣΗ Φ.Π.Α.</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>Φ.Π.Α.</td>
                                            <td>Καθ.αξία</td>
                                            <td>Αξία Φ.Π.Α.</td>
                                            <td>Συν.αξία</td>
                                        </tr>
                                        <tr>
                                            <td>6%</td>
                                            <td>" + kathAxia6.ToString("0.##") + "</td>" +
                                           "<td>" + sunoloFPA6.ToString("0.##") + "</td>" +
                                           "<td>" + sunAxia6.ToString("0.##") + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>13%</td>
                                            <td>" + kathAxia13.ToString("0.##") + "</td>" +
                                           "<td>" + sunoloFPA13.ToString("0.##") + "</td>" +
                                            "<td>" + sunAxia13.ToString("0.##") + "</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>24%</td>
                                            <td>" + kathAxia23.ToString("0.##") + "</td>" +
                                           "<td>" + sunoloFPA23.ToString("0.##") + "</td>" +
                                           "<td>" + sunAxia23.ToString("0.##") + "</td>" + @"
                                        </tr>
                                    </table>
                                     <div>
                                         <p>
                                        </p>
                                    </div>
                                    <table>
                                        <tr>" +
                                            "<td colspan=\"2\">Υπόλοιπα :</td>" + @"
                                        </tr>
                                        <tr>
                                            <td>Πρ. υπόλοιπο</td>
                                            <td>΄Νέο υπόλοιπο</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </body>
                                </html>";
            
            return source;
        }
        public async Task<string> page2(ΠαραστατικάΠωλήσεων order)
        {
            στοιχείαΕταιρίας = await XpoHelper.GetSTOIXEIAETAIRIASData();

            if (στοιχείαΕταιρίας == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν Υπαρχουν Στοιχεια Εταιρίας", "OK");
                return string.Empty;
            }
            if (checkEtairia())
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν Ειναι σωστά συμπληρομένα τα Στοιχεια Εταιρίας.", "OK");
                return string.Empty;
            }
            if (checkPelatis(order.Πελάτης))
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν Ειναι σωστά συμπληρομένα τα του Πελάτη.", "OK");
                return string.Empty;
            }
            decimal sunoloFPA6 = 0;
            decimal sunoloFPA13 = 0;
            decimal sunoloFPA23 = 0;
            decimal kathAxia6 = 0;
            decimal kathAxia13 = 0;
            decimal kathAxia23 = 0;
            decimal sunAxia6 = 0;
            decimal sunAxia13 = 0;
            decimal sunAxia23 = 0;
            string ypologismosFPA = "";
            string grammes = "";
            string analisifpa = "";
            foreach (var i in order.ΓραμμέςΠαραστατικώνΠωλήσεων)
            {
                grammes += "<tr>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin\">" + (i.BarCodeΕίδους == null ? i.Είδος.Κωδικός : i.BarCodeΕίδους.Κωδικός) + "</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin\">" + (i.BarCodeΕίδους == null ? i.Είδος.Περιγραφή : i.BarCodeΕίδους.Περιγραφή + " " + i.BarCodeΕίδους.Χρώμα + " " + i.BarCodeΕίδους.Μέγεθος) + "</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + i.Ποσότητα + "</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + i.Τιμή + "</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + i.ΚαθαρήΑξία.ToString("0.##") + "</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + i.ΠοσοστόΦπα * 100 + "%</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + i.ΑξίαΓραμμής.ToString("0.##") + "</td>" +
                       "</tr>";

                switch (i.ΠοσοστόΦπα * 100)
                {
                    case 6:
                        kathAxia6 += i.ΚαθαρήΑξία;
                        sunoloFPA6 += i.Φπα;
                        sunAxia6 += i.ΑξίαΓραμμής;
                        break;
                    case 13:
                        kathAxia13 += i.ΚαθαρήΑξία;
                        sunoloFPA13 += i.Φπα;
                        sunAxia13 += i.ΑξίαΓραμμής;
                        break;
                    case 24:
                        kathAxia23 += i.ΚαθαρήΑξία;
                        sunoloFPA23 += i.Φπα;
                        sunAxia23 += i.ΑξίαΓραμμής;
                        break;
                }
            }
            double ProhgoumenoYpoloipo = 0;
            if (Preferences.Get("OnlineMode", false))
                ProhgoumenoYpoloipo = await XpoHelper.GetYpoloipo(order.Πελάτης.SmartOid.ToString());

            ypologismosFPA += SetYpologismosFPAString(order.ΓραμμέςΠαραστατικώνΠωλήσεων);
            string τρόποςΑποστολής = "";
            if (order.ΤρόποςΑποστολής != null) { τρόποςΑποστολής = order.ΤρόποςΑποστολής.Τρόποςαποστολής; }
            string τρόποςΠληρωμής = "";
            if (order.ΤρόποςΠληρωμής != null)
            { τρόποςΠληρωμής = order.ΤρόποςΠληρωμής.Τρόποςπληρωμής; }
            string YpologismenoYpoloipo = YpologismosYpoloipou(τρόποςΠληρωμής, ProhgoumenoYpoloipo, (double)order.ΑξίαΠαραστατικού, order.Σειρά.ΚίνησηΣυναλασόμενου);
            string source = @"<html >
<head>
    
    <title></title>" +
    "<style type=\"text/css\">" + @"
        table {
            border-collapse:separate;
            border:solid black 1px;
            border-radius:6px;
            -moz-border-radius:6px;
        }
    </style>
</head>
<body>
    <div>" +
        $@"<h2>{στοιχείαΕταιρίας.Επωνυμία}</h2>
            ΑΦΜ: { στοιχείαΕταιρίας.ΑΦΜ} <br/>
            {στοιχείαΕταιρίας.Οδός} <br/>
            { στοιχείαΕταιρίας_ΤΚ_Ονοματκ} , { στοιχείαΕταιρίας_Πόλη_ΟνομαΠόλης} ,
            {στοιχείαΕταιρίας_ΤΚ_Χώρα} <br/>
            Τηλ:  { στοιχείαΕταιρίας.Τηλέφωνο} <br/>
            FAX:  { στοιχείαΕταιρίας.FAX} <br/><br/>

    </div>
    <div>" +
        "<table style=\"border-style: groove; border-width: thin; width: 100%; text-align: center;\">" + @"
            <tr>" +
                "<td style=\"border-style: groove; border-width: thin; font-weight: bold; background-color: #CCFFFF\">ΕΙΔΟΣ ΠΑΡΑΣΤΑΤΙΚΟΥ</td>" +
                "<td style=\"border-style: groove; border-width: thin; font-weight: bold; background-color: #CCFFFF\">ΑΡΙΘΜΟΣ</td>" +
                "<td style=\"border-style: groove; border-width: thin; font-weight: bold; background-color: #CCFFFF\">ΗΜΕΡΟΜΗΝΙΑ</td>" +
            "</tr>" +
            $@"<tr>
                <td> {order.Σειρά.Περιγραφή} </td>
                <td>{order.Παραστατικό}</td>
                <td>{order.Ημνία} </td>
            </tr>       
        </table><br/><br/>
    </div>
    <div>" +
        "<div style=\"float:left;\">" +
            "<table style=\"border-style: groove; border-width: thin; width: 100%; text-align: center;\">" + @"
                <tr>" +
                    "<td colspan=\"5\" style=\"font-weight: bold; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΣΤΟΙΧΕΙΑ ΠΕΛΑΤΗ</td>" +
                $@"</tr>
                <tr>
                    <td>
                        ΕΠΩΝΥΜΙΑ :</td>
                    <td> {order.Πελάτης.DisplayName}</td>                   
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>                        
                        ΕΠΑΓΓΕΛΜΑ :</td>                    
                    <td>                        
                        &nbsp;</td>
                    <td>                        
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>                        
                        ΔΙΕΥΘΥΝΣΗ :</td>
                    <td>{ order.Πελάτης.Addresstring}</td>                   
                    <td>
                        Τ.Κ. :</td>
                    <td>{ order_Πελάτης_ΚεντρικήΔιευθυνση_ΤΚ_Ονοματκ} </td>
                </tr>
                <tr>
                    <td>
                        ΠΟΛΗ :</td>
                    <td>{order_Πελάτης_ΚεντρικήΔιευθυνση_Πόλη_ΟνομαΠόλης}  </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        ΤΗΛΕΦΩΝΟ :</td>
                    <td>{ order_Πελάτης_ΚεντρικήΔιευθυνση_τηλ}</td>
                    <td>
                        FAX :</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        Α.Φ.Μ. :</td>
                    <td>{ order.Πελάτης.ΑΦΜ} </td>
                    <td>
                        Δ.Ο.Υ:</td>
                    <td>{ order_Πελάτης_ΔΟΥ_Περιγραφή} </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td >
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                </table>
        </div>"+
        "<div style=\"float:left;\">"+
            "<br />"+
            "<table style=\"border-style: groove; border-width: thin; width: 100%;\">"+
                $@"<tr>
                    <td >ΜΕΤΑΦΟΡΙΚΟ ΜΕΣΟ</td>
                    <td ></td>
                </tr>
                <tr>
                    <td>ΤΡΟΠΟΣ ΦΟΡΤΗΣΗΣ</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>ΤΡΟΠΟΣ ΑΠΟΣΤΟΛΗΣ</td>
                    <td>{ τρόποςΑποστολής}</td>
                </tr>
                <tr>
                    <td>ΣΚΟΠΟΣ ΔΙΑΚΙΝΗΣΗΣ</td>
                    <td></td>
                </tr>
                <tr>
                    <td>ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ</td>
                    <td>{ τρόποςΠληρωμής}</td>
                </tr>
            </table>
            <br/><br/>
        </div>
    </div>"+
    "<div><table style=\"width:100% ;border-style: groove; border-width: thin\">"+@"
            <tr>"+
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Κωδικός</td>"+
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Περιγραφή είδους</td>"+
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Ποσότητα</td>"+
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Τιμή μονάδας</td>"+
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Αξία</td>"+
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΦΠΑ</td>"+
                 "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Ποσό</td>" + @"
            </tr>" +grammes+ @"
        </table>
        <br/><br/>
    </div>"+
   "<div style =\"float:center;\">" +
              "<table style=\"text-align: center; border-style: groove; border-width: thin\">" + @"
                <tr>" +
                   "<td colspan=\"4\" style=\"text-align: center\">Σχόλια</td>" + 
             $@"</tr>
                <tr>
                    <td>{ order.Σχολια} </td>
                </tr>
            </table>
        </div>
      <div>"+YpologismenoYpoloipo+
        " <div style=\"float:left;\">" +
            "<table style=\"text-align: center; border-style: groove; border-width: thin\">"+@"
                <tr>" +
                   "<td colspan=\"4\" style=\"text-align: center\">ΑΝΑΛΥΣΗ Φ.Π.Α.</td>"+
             $@"</tr>
                <tr>
                          <td>Φ.Π.Α.</td>
                          <td>Καθ.αξία</td>
                          <td>Αξία Φ.Π.Α.</td>
                          <td>Συν.αξία</td>
                </tr>
                      {ypologismosFPA}
            </table>
        </div>"+
        "<div style=\"float:right;\">" +
            "<table style=\"border-style: groove; border-width: thin; width: 100%;\">"+@"
                <tr>"+
                    "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΣΥΝΟΛΟ ΚΑΘΑΡΗΣ ΑΞΙΑΣ</td>"+@"
                    <td>"+order.ΚαθαρήΑξία.ToString("0.##") + @" €</td>
                </tr>
                <tr>"+
                    "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΑΞΙΑ ΕΚΠΤΩΣΗΣ</td>"+@"
                    <td>"+order.ΑξίαΕκπτωσης.ToString("0.##") + @" €</td>
                </tr>
                <tr>"+
                    "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΣΥΝΟΛΟ Φ.Π.Α.</td>"+@"
                    <td>"+order.Φπα.ToString("0.##") + @" €</td>
                </tr>
                <tr>"+
                    "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΤΕΛΙΚΟ ΣΥΝΟΛΟ</td>"+@"
                    <td>"+order.ΑξίαΠαραστατικού.ToString("0.##") + @" €</td>
                </tr>
            </table>
        </div>
    </div>
</body>
</html>";
            return source;
        }

        private string SetYpologismosFPAString(XPCollection<ΓραμμέςΠαραστατικώνΠωλήσεων> γραμμέςΠαραστατικώνΠωλήσεων)
        {
            string htmlstring = "";
           
            using (UnitOfWork uow = new UnitOfWork())
            {
                var listFPA = uow.Query<ΦΠΑ>();
                foreach (var item in listFPA)
                {
                    decimal kathAxia = 0;
                    decimal sunoloFPA = 0;
                    decimal sunAxia = 0;

                    foreach (var gram in γραμμέςΠαραστατικώνΠωλήσεων)
                    {
                        var posostofpa = gram.ΠοσοστόΦπα * 100;
                        if (item.Φπακανονικό == posostofpa)
                        {
                            kathAxia += gram.ΚαθαρήΑξία;
                            sunoloFPA += gram.Φπα;
                            sunAxia += gram.ΑξίαΓραμμής;
                        }
                    }
                    //mhn kaneis print ama exei timi 0 sto fpa
                    if(kathAxia != 0 || sunoloFPA != 0 || sunAxia !=0)
                        htmlstring += $@"<tr>
                          <td> {item.Φπακανονικό} %</td >
                          <td>{kathAxia.ToString("0.##")} </td>
                          <td>{sunoloFPA.ToString("0.##")} </td>
                          <td>{sunAxia.ToString("0.##")} </td>
                     </tr>";
                }
            }
            return htmlstring;
        }
        private string YpologismosYpoloipou(string τρόποςΠληρωμής,double ProhgoumenoYpoloipo, double αξίαΠαραστατικού ,int κίνηση)
        {
            string htmlpeace = ""; 
            double NeoYpoloipo = 0;
            if (ProhgoumenoYpoloipo == 0)
                return htmlpeace;
            if (κίνηση == 0)
            {
                NeoYpoloipo = ProhgoumenoYpoloipo + αξίαΠαραστατικού;
            }
            else if(κίνηση == 1)
            {
                NeoYpoloipo = ProhgoumenoYpoloipo - αξίαΠαραστατικού;
            }
            else
            {
                return htmlpeace;
            }
            if (τρόποςΠληρωμής == "ΤΟΙΣ ΜΕΤΡΗΤΟΙΣ")
                NeoYpoloipo = ProhgoumenoYpoloipo;
            htmlpeace = "<div style =\"float:right;\">" +
               "<table style=\"border-style: groove; border-width: thin; width: 100%;\">" + @"
                <tr>" +
                       "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΠΡΟΗΓ.ΥΠΟΛΟΙΠΟ</td>" + @"
                    <td>" + ProhgoumenoYpoloipo.ToString("0.##") + @" €</td>
                </tr>
                <tr>" +
                       "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΝΕΟ ΥΠΟΛΟΙΠΟ</td>" + @"
                    <td>" + NeoYpoloipo.ToString("0.##") + @" €</td>
                </tr>
  
            </table>
        </div>";

            return htmlpeace;
        }

        //        public  async void CreatePrint2(ΠαραστατικάΠωλήσεων order)
        //        {
        //            στοιχείαΕταιρίας = await XpoHelper.CreateSTOIXEIAETAIRIASData();
        //            if (στοιχείαΕταιρίας == null)
        //            {
        //                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν Υπαρχουν Στοιχεια Εταιρίας", "OK");
        //                return;
        //            }
        //            if (checkEtairia())
        //            {
        //                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν Ειναι σωστά συμπληρομένα τα Στοιχεια Εταιρίας.", "OK");
        //                return;
        //            }       
        //            WebView browser = new WebView();
        //            var htmlSource = new HtmlWebViewSource();
        //            decimal sunoloFPA6 = 0;
        //            decimal sunoloFPA13 = 0;
        //            decimal sunoloFPA23 = 0;
        //            decimal kathAxia6 = 0;
        //            decimal kathAxia13 = 0;
        //            decimal kathAxia23 = 0;
        //            decimal sunAxia6 = 0;
        //            decimal sunAxia13 = 0;
        //            decimal sunAxia23 = 0;
        //            string grammes = "";
        //            foreach (var i in order.ΓραμμέςΠαραστατικώνΠωλήσεων)
        //            {
        //                grammes += "<tr>" +
        //                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin\">" + i.Είδος.Κωδικός + "</td>" +
        //                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin\">" + i.Είδος.Περιγραφή + "</td>" +
        //                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + i.Ποσότητα + "</td>" +
        //                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + i.Τιμή + "</td>" +
        //                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + i.ΑξίαΓραμμής.ToString("0.##") + "</td>" +
        //                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + i.ΠοσοστόΦπα * 100 + "%</td>" +
        //                       "</tr>";

        //                switch (i.ΠοσοστόΦπα * 100)
        //                {
        //                    case 6:
        //                        kathAxia6 += i.ΚαθαρήΑξία;
        //                        sunoloFPA6 += i.Φπα;
        //                        sunAxia6 += i.ΑξίαΓραμμής;
        //                        break;
        //                    case 13:
        //                        kathAxia13 += i.ΚαθαρήΑξία;
        //                        sunoloFPA13 += i.Φπα;
        //                        sunAxia13 += i.ΑξίαΓραμμής;
        //                        break;
        //                    case 23:
        //                        kathAxia23 += i.ΚαθαρήΑξία;
        //                        sunoloFPA23 += i.Φπα;
        //                        sunAxia23 += i.ΑξίαΓραμμής;
        //                        break;
        //                }
        //            }
        //            string τρόποςΑποστολής = "";
        //            if (order.ΤρόποςΑποστολής != null) { τρόποςΑποστολής = order.ΤρόποςΑποστολής.Τρόποςαποστολής; }
        //            string τρόποςΠληρωμής = "";
        //            if ( order.ΤρόποςΠληρωμής != null)
        //            { τρόποςΠληρωμής = order.ΤρόποςΠληρωμής.Τρόποςπληρωμής; }
        //            string source = @"<html >
        //<head>

        //    <title></title>"+
        //    "<style type=\"text/css\">"+ @"
        //        table {
        //            border-collapse:separate;
        //            border:solid black 1px;
        //            border-radius:6px;
        //            -moz-border-radius:6px;
        //        }
        //    </style>
        //</head>
        //<body>
        //    <div>
        //        <h2>" + στοιχείαΕταιρίας.Επωνυμία + "</h2>"+ @"
        //            ΑΦΜ: " + στοιχείαΕταιρίας.ΑΦΜ + "<br/>"+
        //            στοιχείαΕταιρίας.Οδός +"<br/>"+
        //            στοιχείαΕταιρίας.ΤΚ.Ονοματκ +", "+ στοιχείαΕταιρίας.Πόλη.ΟνομαΠόλης +","+
        //            στοιχείαΕταιρίας.ΤΚ.Χώρα +"<br/>"+
        //            "Τηλ: "+ στοιχείαΕταιρίας.Τηλέφωνο +"<br/>"+
        //            "FAX: "+ στοιχείαΕταιρίας.FAX +"<br/><br/>" +@"

        //    </div>
        //    <div>"+
        //        "<table style=\"border-style: groove; border-width: thin; width: 100%; text-align: center;\">"+@"
        //            <tr>"+
        //                "<td style=\"border-style: groove; border-width: thin; font-weight: bold; background-color: #CCFFFF\">ΕΙΔΟΣ ΠΑΡΑΣΤΑΤΙΚΟΥ</td>"+
        //                "<td style=\"border-style: groove; border-width: thin; font-weight: bold; background-color: #CCFFFF\">ΑΡΙΘΜΟΣ</td>"+
        //                "<td style=\"border-style: groove; border-width: thin; font-weight: bold; background-color: #CCFFFF\">ΗΜΕΡΟΜΗΝΙΑ</td>"+@"
        //            </tr>
        //            <tr>
        //                <td>"+ order.Σειρά.Περιγραφή +"</td>"+
        //                "<td>"+order.Παραστατικό+"</td>"+
        //                "<td>"+order.Ημνία+"</td>"+@"
        //            </tr>       
        //        </table><br/><br/>
        //    </div>
        //    <div>"+
        //        "<div style=\"float:left;\">"+
        //            "<table style=\"border-style: groove; border-width: thin; width: 100%; text-align: center;\">"+@"
        //                <tr>"+
        //                    "<td colspan=\"5\" style=\"font-weight: bold; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΣΤΟΙΧΕΙΑ ΠΕΛΑΤΗ</td>"+@"
        //                </tr>
        //                <tr>
        //                    <td>
        //                        ΕΠΩΝΥΜΙΑ :</td>
        //                    <td> "+order.Πελάτης.DisplayName+"</td>"+ @"                    
        //                    <td>&nbsp;</td>
        //                </tr>
        //                <tr>
        //                    <td>                        
        //                        ΕΠΑΓΓΕΛΜΑ :</td>                    
        //                    <td>                        
        //                        &nbsp;</td>
        //                    <td>                        
        //                        &nbsp;</td>
        //                </tr>
        //                <tr>
        //                    <td>                        
        //                        ΔΙΕΥΘΥΝΣΗ :</td>
        //                    <td>" + order.Πελάτης.Addresstring+"</td>"+ @"                   
        //                    <td>
        //                        Τ.Κ. :</td>
        //                    <td>" + order.Πελάτης.ΚεντρικήΔιευθυνση.ΤΚ.Ονοματκ +"</td>"+ @"
        //                </tr>
        //                <tr>
        //                    <td>
        //                        ΠΟΛΗ :</td>
        //                    <td>" + order.Πελάτης.ΚεντρικήΔιευθυνση.Πόλη.ΟνομαΠόλης+" </td>"+ @"
        //                    <td>
        //                        &nbsp;</td>
        //                    <td>
        //                        &nbsp;</td>
        //                </tr>
        //                <tr>
        //                    <td>
        //                        ΤΗΛΕΦΩΝΟ :</td>
        //                    <td>" + order.Πελάτης.ΚεντρικήΔιευθυνση.Τηλέφωνο +"</td>"+ @"
        //                    <td>
        //                        FAX :</td>
        //                    <td>
        //                        &nbsp;</td>
        //                </tr>
        //                <tr>
        //                    <td>
        //                        Α.Φ.Μ. :</td>
        //                    <td>" + order.Πελάτης.ΑΦΜ +"</td>"+ @"
        //                    <td>
        //                        Δ.Ο.Υ:</td>
        //                    <td>"+ order.Πελάτης.ΔΟΥ.Περιγραφή+"</td>"+@"
        //                </tr>
        //                <tr>
        //                    <td>
        //                        &nbsp;</td>
        //                    <td>
        //                        &nbsp;</td>
        //                    <td >
        //                        &nbsp;</td>
        //                    <td>
        //                        &nbsp;</td>
        //                    <td>
        //                        &nbsp;</td>
        //                </tr>
        //                </table>
        //        </div>"+
        //        "<div style=\"float:left;\">"+
        //            "<table style=\"border-style: groove; border-width: thin; width: 100%; text-align: center;\">"+@"
        //                <tr>"+
        //                    "<td style=\"font-weight: bold; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΣΧΕΤΙΚΑ ΠΑΡΑΣΤΑΤΙΚΑ</td>"+@"
        //                </tr>
        //                <tr>
        //                    <td>&nbsp;</td>
        //                </tr>
        //                </table><br />"+
        //            "<table style=\"border-style: groove; border-width: thin; width: 100%;\">"+@"
        //                <tr>
        //                    <td >ΜΕΤΑΦΟΡΙΚΟ ΜΕΣΟ</td>
        //                    <td ></td>
        //                </tr>
        //                <tr>
        //                    <td>ΤΡΟΠΟΣ ΦΟΡΤΗΣΗΣ</td>
        //                    <td>&nbsp;</td>
        //                </tr>
        //                <tr>
        //                    <td>ΤΡΟΠΟΣ ΑΠΟΣΤΟΛΗΣ</td>
        //                    <td>"+ τρόποςΑποστολής +"</td>"+@"
        //                </tr>
        //                <tr>
        //                    <td>ΣΚΟΠΟΣ ΔΙΑΚΙΝΗΣΗΣ</td>
        //                    <td></td>
        //                </tr>
        //                <tr>
        //                    <td>ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ</td>
        //                    <td>"+τρόποςΠληρωμής+"</td>"+@"
        //                </tr>
        //            </table>
        //            <br/><br/>
        //        </div>
        //    </div>"+
        //    "<div><table style=\"width:100% ;border-style: groove; border-width: thin\">"+@"
        //            <tr>"+
        //                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Κωδικός</td>"+
        //                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Περιγραφή είδους</td>"+
        //                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Ποσότητα</td>"+
        //                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Τιμή μονάδας</td>"+
        //                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">Αξία</td>"+
        //                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΦΠΑ</td>"+@"
        //            </tr>"+grammes+@"
        //        </table>
        //        <br/><br/>
        //    </div>
        //    <div>"+
        //        "<div style=\"float:left;\">"+
        //            "<table style=\"text-align: center; border-style: groove; border-width: thin\">"+@"
        //                <tr>" +
        //                         "<td colspan=\"4\" style=\"text-align: center\">ΑΝΑΛΥΣΗ Φ.Π.Α.</td>" + @"
        //                     </tr>
        //                     <tr>
        //                          <td>Φ.Π.Α.</td>
        //                          <td>Καθ.αξία</td>
        //                          <td>Αξία Φ.Π.Α.</td>
        //                          <td>Συν.αξία</td>
        //                     </tr>
        //                     <tr>
        //                          <td>6%</td>
        //                          <td>"+ kathAxia6.ToString("0.##") + "</td>"+
        //                          "<td>"+ sunoloFPA6.ToString("0.##") + "</td>"+
        //                          "<td>"+ sunAxia6.ToString("0.##") + "</td>"+ @"
        //                     </tr>
        //                     <tr>
        //                          <td>13%</td>
        //                          <td>" + kathAxia13.ToString("0.##") + "</td>" +
        //                          "<td>" + sunoloFPA13.ToString("0.##") + "</td>" +
        //                          "<td>" + sunAxia13.ToString("0.##") + "</td>" + @"
        //                     </tr>
        //                     <tr>
        //                        <td>23%</td>
        //                       <td>" + kathAxia23.ToString("0.##") + "</td>" +
        //                       "<td>" + sunoloFPA23.ToString("0.##") + "</td>" +
        //                       "<td>" + sunAxia23.ToString("0.##") + "</td>" + @"
        //                     </tr>
        //            </table>
        //        </div>"+
        //        "<div style=\"float:right;\">"+
        //            "<table style=\"border-style: groove; border-width: thin; width: 100%;\">"+@"
        //                <tr>"+
        //                    "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΣΥΝΟΛΟ ΚΑΘΑΡΗΣ ΑΞΙΑΣ</td>"+@"
        //                    <td>"+order.ΚαθαρήΑξία.ToString("0.##") + @" €</td>
        //                </tr>
        //                <tr>"+
        //                    "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΑΞΙΑ ΕΚΠΤΩΣΗΣ</td>"+@"
        //                    <td>"+order.ΑξίαΕκπτωσης.ToString("0.##") + @" €</td>
        //                </tr>
        //                <tr>"+
        //                    "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΣΥΝΟΛΟ Φ.Π.Α.</td>"+@"
        //                    <td>"+order.Φπα.ToString("0.##") + @" €</td>
        //                </tr>
        //                <tr>"+
        //                    "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΤΕΛΙΚΟ ΣΥΝΟΛΟ</td>"+@"
        //                    <td>"+order.ΑξίαΠαραστατικού.ToString("0.##") + @" €</td>
        //                </tr>
        //            </table>
        //        </div>
        //    </div>
        //</body>
        //</html>";
        //            htmlSource.Html = source;
        //            browser.Source = htmlSource;
        //            var printService = DependencyService.Get<IPrintService>();
        //            printService.Print(browser);
        //        }
        public  async void CreatePrint3(ΠαραστατικάΕισπράξεων order)
        {
            στοιχείαΕταιρίας = await XpoHelper.GetSTOIXEIAETAIRIASData();
            if (στοιχείαΕταιρίας == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν Υπαρχουν Στοιχεια Εταιρίας", "OK");
                return;
            }
            if (checkEtairia())
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Δεν Ειναι σωστά συμπληρομένα τα Στοιχεια Εταιρίας.", "OK");
                return;
            }
            WebView browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            string grammes = "";
            foreach (var i in order.ΓραμμέςΠαραστατικώνΕισπράξεων)
            {
                var λογαριασμος = " ";
                var αξιογραφο = " ";
                var τραπεζα = " ";
                var εκδοτης = "";
                var ημερ = " ";
                if (i.Λογαριασμός != null)
                {
                    λογαριασμος = i.Λογαριασμός.Λογαριασμός;
                }
                else
                {
                    αξιογραφο = i.Αξιόγραφα.ΑριθμόςΑξιογράφου;
                    τραπεζα = i.Αξιόγραφα.ΤράπεζαΕκδοσης.ΟνομαΤράπεζας;
                    εκδοτης = i.Αξιόγραφα.Εκδότης;
                    ημερ = i.Αξιόγραφα.ΗμνίαΛήξης.ToString();
                }
                
                grammes += "<tr>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin\">" + λογαριασμος + "</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin\">" + αξιογραφο + "</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + τραπεζα + "</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + εκδοτης + "</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + ημερ + "</td>" +
                            "<td style=\"border-right-style: dashed; border-bottom-style: dashed; border-width: thin; text-align: right\">" + i.Ποσόν.ToString("0.##") + " €</td>" +
                       "</tr>";
            }   
            string source = @"<html >
<head>
    <title></title>" +
    "<style type=\"text/css\">" + @"
        table {
            border-collapse:separate;
            border:solid black 1px;
            border-radius:6px;
            -moz-border-radius:6px;
        }
    </style>
</head>
<body>
    <div>
        <h2>" + στοιχείαΕταιρίας.Επωνυμία + "</h2>" + @"
            ΑΦΜ: " + στοιχείαΕταιρίας.ΑΦΜ + "<br/>" +
            στοιχείαΕταιρίας.Οδός + "<br/>" +
            στοιχείαΕταιρίας_ΤΚ_Ονοματκ + ", " + στοιχείαΕταιρίας_Πόλη_ΟνομαΠόλης + "," +
            στοιχείαΕταιρίας_ΤΚ_Χώρα + "<br/>" +
            "Τηλ: " + στοιχείαΕταιρίας.Τηλέφωνο + "<br/>" +
            "FAX: " + στοιχείαΕταιρίας.FAX + "<br/><br/>" + @"
    </div>
    <div>" +
        "<table style=\"border-style: groove; border-width: thin; width: 100%; text-align: center;\">" + @"
            <tr>" +
                "<td style=\"border-style: groove; border-width: thin; font-weight: bold; background-color: #CCFFFF\">ΕΙΔΟΣ ΠΑΡΑΣΤΑΤΙΚΟΥ</td>" +
                "<td style=\"border-style: groove; border-width: thin; font-weight: bold; background-color: #CCFFFF\">ΑΡΙΘΜΟΣ</td>" +
                "<td style=\"border-style: groove; border-width: thin; font-weight: bold; background-color: #CCFFFF\">ΗΜΕΡΟΜΗΝΙΑ</td>" + @"
            </tr>
            <tr>
                <td>" + order.Σειρά.Περιγραφή + "</td>" +
                "<td>" + order.Παραστατικό + "</td>" +
                "<td>" + order.Ημνία + "</td>" + @"
            </tr>       
        </table><br/><br/>
    </div>
    <div>" +
        "<div style=\"float:left;\">" +
            "<table style=\"border-style: groove; border-width: thin; width: 100%; text-align: center;\">" + @"
                <tr>" +
                    "<td colspan=\"5\" style=\"font-weight: bold; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΣΤΟΙΧΕΙΑ ΠΕΛΑΤΗ</td>" + @"
                </tr>
                <tr>
                    <td>
                        ΕΠΩΝΥΜΙΑ :</td>
                    <td> " + order.Πελάτης.DisplayName + "</td>" + @"                    
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>                        
                        ΕΠΑΓΓΕΛΜΑ :</td>                    
                    <td>                        
                        &nbsp;</td>
                    <td>                        
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>                        
                        ΔΙΕΥΘΥΝΣΗ :</td>
                    <td>" + order.Πελάτης.Addresstring + "</td>" + @"                   
                    <td>
                        Τ.Κ. :</td>
                    <td>" + order_Πελάτης_ΚεντρικήΔιευθυνση_ΤΚ_Ονοματκ + "</td>" + @"
                </tr>
                <tr>
                    <td>
                        ΠΟΛΗ :</td>
                    <td>" + order_Πελάτης_ΚεντρικήΔιευθυνση_Πόλη_ΟνομαΠόλης + " </td>" + @"
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        ΤΗΛΕΦΩΝΟ :</td>
                    <td>" + order.Πελάτης.ΚεντρικήΔιευθυνση.Τηλέφωνο + "</td>" + @"
                    <td>
                        FAX :</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        Α.Φ.Μ. :</td>
                    <td>" + order.Πελάτης.ΑΦΜ + "</td>" + @"
                    <td>
                        Δ.Ο.Υ:</td>
                    <td>" + order_Πελάτης_ΔΟΥ_Περιγραφή + "</td>" + @"
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td >
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
              </table>
                <br/><br/>
        </div>" +
        "<div style=\"float:left;\">" +
            "<table style=\"border-style: groove; border-width: thin; width: 50%; text-align: center;\">" + @"
                <tr>" +
                    "<td style=\"font-weight: bold; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΑΙΤΙΟΛΟΓΙΑ</td>" + @"
                </tr>
                <tr>
                    <td>"+order.Αιτιολογία+ @"</td>
                </tr>
             </table>
            <br/><br/>
        </div>
        
    </div>" +
    "<div><table style=\"width:100% ;border-style: groove; border-width: thin\">" + @"
            <tr>" +
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΛΟΓΑΡΙΑΣΜΟΣ</td>" +
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΑΡ.ΑΞΙΟΓΡΑΦΟΥ</td>" +
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΤΡΑΠΕΖΑ</td>" +
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΕΚΔΟΤΗΣ</td>" +
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΗΜΝΙΑ ΛΗΞΗΣ</td>" +
                "<td style=\"text-align: center; background-color: #CCFFFF; border-style: groove; border-width: thin\">ΠΟΣΟ</td>" + @"
            </tr>" + grammes + @"
        </table>
        <br/><br/>
    </div>
    <div>" +
        "<div style=\"float:right;\">" +
            "<table style=\"border-style: groove; border-width: thin; width: 100%;\">" + @"
                <tr>" +
                    "<td style=\"font-weight: bold; text-align: center; background-color: #CCFFFF\">ΤΕΛΙΚΟ ΣΥΝΟΛΟ</td>" + @"
                    <td>" + order.Πίστωση.ToString("0.##") + @" €</td>
                </tr>
            </table>
        </div>
    </div>
</body>
</html>";
            htmlSource.Html = source;
            browser.Source = htmlSource;
            var printService = DependencyService.Get<IPrintService>();
            printService.Print(browser);
        }
    }
}
