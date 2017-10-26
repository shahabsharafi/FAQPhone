using FAQPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services.Interfaces
{
    public interface IMessageService
    {
        Task<List<MessageModel>> GetNewMessages();
        Task<List<MessageModel>> GetAllMessages();
    }
}
