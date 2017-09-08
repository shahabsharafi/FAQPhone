using FAQPhone.Infarstructure;
using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services
{
    public class DiscussionService:  RestService<DiscussionModel>, IDiscussionService
    {
        public DiscussionService() : base()
        {
            this._relativeUrl = "discussions/{0}";
        }

        public override void OnSaving(DiscussionModel obj)
        {
            
            if (obj != null && obj.items != null && obj.items.Length > 0 && obj.to != null && obj.to.username == App.Username)
            {
                var answerList = obj.items.Where(o => o.owner.username == obj.to.username);
                if (answerList.Any())
                {
                    var item = answerList.Last();
                    obj.answerDate = item.createDate;
                }
            }
            
            base.OnSaving(obj);
        }

        public async Task<List<DiscussionModel>> GetList(bool asUser)
        {
            string url = string.Format(Constants.RestUrl, string.Format("discussions/getlist/{0}", asUser));
            var data = await this.get<List<DiscussionModel>>(url);
            return data;
        }

        public async Task<int> GetCount(bool asUser)
        {
            string url = string.Format(Constants.RestUrl, string.Format("discussions/getcount/{0}", asUser));
            var data = await this.get<int>(url);
            return data;
        }

        public async Task<DiscussionModel> Recive()
        {
            string url = string.Format(Constants.RestUrl, "discussions/recive");
            var data = await this.get<DiscussionModel>(url);
            return data;
        }
    }
}
