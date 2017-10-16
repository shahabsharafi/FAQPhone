using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAQPhone.Models;
using FAQPhone.Infarstructure;

namespace FAQPhone.Services
{
    public class DiscountService : RestService<DiscountModel>, IDiscountService
    {
        public DiscountService() : base()
        {
            this._relativeUrl = "discounts?{0}";
        }

        public async Task<List<DiscountModel>> GetList()
        {
            string prm = "$expand=type,owner,category";
            string url = this.getUrl(prm);
            var data = await this.get<PaginationModel<DiscountModel>>(url);
            List<DiscountModel> list = new List<DiscountModel>();
            if (data.docs != null && data.docs.Length > 0)
                list = data.docs.ToList();
            return list;
        }
    }
}
