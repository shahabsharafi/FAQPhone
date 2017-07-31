using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services
{
    class AttributeService : RestService<AttributeModel>, IAttributeService
    {
        public AttributeService(): base()
        {
            this._relativeUrl = "attributes/{0}";
        }

        public async Task<List<AttributeModel>> GetAll()
        {
            string url = this.getUrl("all");
            return await this.get<List<AttributeModel>>(url);
        }
    }
}
