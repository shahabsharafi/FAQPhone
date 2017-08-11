using FAQPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services.Interfaces
{
    public interface IDiscussionService : IRestService<DiscussionModel>
    {
        Task<List<DiscussionModel>> GetList(bool isUser, int[] states);
        Task<int> GetCount(bool isUser, int[] states);
        Task<DiscussionModel> Recive();
    }
}
