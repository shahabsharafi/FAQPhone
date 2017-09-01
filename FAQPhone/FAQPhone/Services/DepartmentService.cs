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
    public class DepartmentService : BaseRestService, IDepartmentService
    {
        protected string _relativeUrl { get; set; }
        protected string getUrl(string param = "")
        {
            return string.Format(Constants.RestUrl, string.Format(this._relativeUrl, param));
        }
        public DepartmentService() : base()
        {
            this._relativeUrl = "departments?{0}";
        }
        public async Task<List<DepartmentModel>> GetByParent(string parentId)
        {
            string prm = parentId == ""
                ? "$filter=type eq 'department'"
                : "$filter=parentId eq '" + parentId + "'";
            string url = this.getUrl(prm);
            var data = await this.get<PaginationModel<DepartmentModel>>(url);
            return data.docs.ToList();
        }

        public async Task<List<DepartmentModel>> GetById(string id)
        {
            string prm = "$filter=_id eq '" + id + "'";
            string url = this.getUrl(prm);
            var data = await this.get<PaginationModel<DepartmentModel>>(url);
            return data.docs.ToList();
        }
    }
}
