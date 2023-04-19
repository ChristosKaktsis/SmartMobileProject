using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileProject.Repositories
{
    public class CompanyRepository
    {
        public async Task<string> GetVAT()
        {
            using(UnitOfWork uow = new UnitOfWork())
            {
                var item= await uow.Query<ΣτοιχείαΕταιρίας>().FirstOrDefaultAsync();
                return item.ΑΦΜ;
            }
        }
        public async Task RefreshCompany()
        {
            var company = await XpoHelper.CreateSTOIXEIAETAIRIASData();
        }
    }
}
