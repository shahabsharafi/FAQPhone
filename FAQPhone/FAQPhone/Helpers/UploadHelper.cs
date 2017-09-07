using FilePicker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FAQPhone.Helpers
{
    public class UploadHelper
    {
        public static async Task<T> UploadFile<T>(string url, string path, string fileName, Dictionary<string, string> values = null)
        {
            T resultObj = default(T);
            try
            {
                //read file into upfilebytes array
                var upfilebytes = DependencyService.Get<IFileService>().ReadAllBytes(path);

                //create new HttpClient and MultipartFormDataContent and add our file, and StudentId
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent fileContent = new ByteArrayContent(upfilebytes);
                content.Add(fileContent, "File", fileName);
                if (values != null)
                {
                    foreach (var item in values)
                    {
                        StringContent cnt = new StringContent(item.Value);
                        content.Add(cnt, item.Key);
                    }
                }
                //upload MultipartFormDataContent content async and store response in response var
                var response = await client.PostAsync(url, content);

                //read response result as a string async into json var
                var responsestr = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    resultObj = JsonConvert.DeserializeObject<T>(result);
                }

            }
            catch (Exception e)
            {
                throw;
            }

            return resultObj;
        }
    }
}
