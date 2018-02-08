using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class BalanceModel
    {
        public string sourceId { get; set; }
        public string source { get; set; }
        public DateTime date { get; set; }
        public decimal? price { get; set; }
        public decimal? count { get; set; }
        public decimal? discount { get; set; }
        public decimal? total { get; set; }
        public decimal debit { get; set; }
        public decimal credit { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public bool IsDebit { get; set; }
        public bool IsCredit { get; set; }
        public decimal Amount { get; set; }
        public string CreateDateCaption { get; set; }
    }
}
