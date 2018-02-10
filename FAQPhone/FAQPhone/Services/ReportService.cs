using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services
{
    public class ReportService : BaseRestService, IReportService
    {
        public async Task<List<BalanceModel>> GetBalance()
        {
            string url = string.Format(Constants.RestUrl, "report/balance");
            var data = await this.get<List<BalanceModel>>(url);
            return data;
        }

        public async Task<List<KeyValueModel>> GetQuick()
        {
            string url = string.Format(Constants.RestUrl, "report/quick");
            var data = await this.get<List<KeyValueModel>>(url);
            return data;
        }
    }
}
