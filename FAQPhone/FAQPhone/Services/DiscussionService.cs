using FAQPhone.Inferstructure;
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

        public async Task<List<DiscussionModel>> GetList(bool isUser, int state)
        {
            string param = isUser
                ? "$filter=state eq " + state + " and from_username eq '" + App.Bag.username + "'"
                : "$filter=state eq " + state + " and to_username eq '" + App.Bag.username + "'";
            string url = string.Format(Constants.RestUrl, string.Format("discussions?{0}", param));
            var data = await this.get<PaginationModel<DiscussionModel>>(url);
            return data.docs.ToList();
        }

        public async Task<DiscussionModel> Recive()
        {
            return null;
        }
    }
}
