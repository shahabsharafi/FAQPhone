using FAQPhone.Inferstructure;
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
    public class RestService<T> : IRestService<T>
    {
        private string _relativeUrl { get { return "accounts/"; } }
        HttpClient client;

        public RestService()
        {
            var authData = "";// string.Format("{0}:{1}", Constants.Username, Constants.Password);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }
        public async  Task<List<T>> GetList()
        {
            List<T> list = new List<T>();

            var uri = new Uri(string.Format(Constants.RestUrl, this._relativeUrl));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
            return list;
        }

        public async Task Save(T obj)
        {
            var uri = new Uri(string.Format(Constants.RestUrl, this._relativeUrl));

            try
            {
                var json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"				TodoItem successfully saved.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }

        public async Task Delete(string id)
        {
            var uri = new Uri(string.Format(Constants.RestUrl, string.Format("{0}{1}", this._relativeUrl, id)));

            try
            {
                var response = await client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"				TodoItem successfully deleted.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }
        
    }
}
