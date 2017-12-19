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
    public class CommonService: BaseRestService, ICommonService
    {        
        public async Task<FileStatModel> GetFileState(string filePath)
        {
            string url = string.Format(Constants.RestUrl, "common/file/stat/" + filePath);
            var data = await this.get<FileStatModel>(url);
            return data;
        }
    }
}
