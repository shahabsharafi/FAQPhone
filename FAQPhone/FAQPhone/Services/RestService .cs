using FAQPhone.Infarstructure;
using FAQPhone.Infrastructure;
using FAQPhone.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services
{
    public class RestService<T> : BaseRestService, IRestService<T> where T : new()
    {
        protected string _relativeUrl { get; set; }
        protected string getUrl(string param = "")
        {
            return string.Format(Constants.RestUrl, string.Format(this._relativeUrl, param));
        }
        public RestService(): base()
        {
            
        }
        public async  Task<List<T>> GetList()
        {
            string url = this.getUrl("");
            return await this.get<List<T>>(url);
        }

        public async Task<T> Get(string id)
        {
            string url = this.getUrl(id);
            return await this.get<T>(id);
        }

        public virtual void OnSaving(T obj)
        {

        }

        public async Task Save(T obj)
        {
            this.OnSaving(obj);
            string url = this.getUrl("");
            await this.post<T>(url, obj);
        }

        public async Task Delete(string id)
        {
            string url = this.getUrl(id);
            await this.delete(url);
        }
        
    }
}
