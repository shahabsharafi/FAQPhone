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
using FAQPhone.Infrastructure;
using FAQPhone.Models;
using FAQPhone.Infarstructure;

namespace FAQPhone.Helpers
{
    public class UploadHelper
    {
        public static async Task<T> UploadFile<T>(string url, string path, string fileName, Action<bool> uploading, Dictionary<string, string> values = null)
        {
            Device.BeginInvokeOnMainThread(() => uploading.Invoke(true));
            T resultObj = default(T);
            try
            {
                //read file into upfilebytes array
                var upfilebytes = DependencyService.Get<IFileService>().ReadAllBytes(path);

                //create new HttpClient and MultipartFormDataContent and add our file, and StudentId
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("has_encode", "true");
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

                var response = await client.PostAsync(url, content);

                //read response result as a string async into json var
                var responsestr = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    resultObj = JsonConvert.DeserializeObject<T>(result);
                }

            }
            catch (Exception ex)
            {
                await Utility.Alert(ex.GetMessage());
            }
            Device.BeginInvokeOnMainThread(() => uploading.Invoke(false));
            return resultObj;
        }

        public static async Task UploadPicture(byte[] data, string fileName, Action<bool> uploading, int width, int height)
        {
            ResultModel resultObj;
            string url = Constants.UploadUrl;
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("EntityName", "none");
            Device.BeginInvokeOnMainThread(() => uploading.Invoke(true));
            try
            {
                //read file into upfilebytes array

                data = DependencyService.Get<IFileService>().LoadAndResizeBitmap(data, width, height);

                //create new HttpClient and MultipartFormDataContent and add our file, and StudentId
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent fileContent = new ByteArrayContent(data);
                content.Add(fileContent, "File", fileName);
                if (values != null)
                {
                    foreach (var item in values)
                    {
                        StringContent cnt = new StringContent(item.Value);
                        content.Add(cnt, item.Key);
                    }
                }

                var response = await client.PostAsync(url, content);

                //read response result as a string async into json var
                var responsestr = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    resultObj = JsonConvert.DeserializeObject<ResultModel>(result);
                }

            }
            catch (Exception ex)
            {
                await Utility.Alert(ex.GetMessage());
            }
            Device.BeginInvokeOnMainThread(() => uploading.Invoke(false));
        }
    }
}
