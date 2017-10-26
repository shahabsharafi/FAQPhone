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
    public class MessageService : BaseRestService, IMessageService
    {
        public MessageService () : base()
        {
            
        }

        public async Task<List<MessageModel>> GetMyMessages()
        {
            string url = string.Format(Constants.RestUrl, "messages/mymessages");
            var data = await this.get<List<MessageModel>>(url);
            return data;
        }

        public async Task<List<MessageModel>> GetAllMessages()
        {
            string url = string.Format(Constants.RestUrl, "messages");
            var data = await this.get<PaginationModel<MessageModel>>(url);
            return data.docs.ToList();
        }
    }
}
