using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileProject.ViewModels
{
    class DocCollectHelper : BaseViewModel
    {
        public static UnitOfWork uow;
        public static Πωλητής politis;
        static ΠαραστατικάΕισπράξεων parastatikoEispr;
        public static ΓραμμέςΠαραστατικώνΕισπράξεων editline;
        public static ΠαραστατικάΕισπράξεων ParastatikoEispr
        {
            get
            {
                return parastatikoEispr;
            }
            set
            {
                parastatikoEispr = value;
            }
        }
    }
}
