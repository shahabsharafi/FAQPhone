using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAQPhone.Models;

namespace FAQPhone.Services.Interfaces
{
    public interface IEmailService
    {
        Task Send(EmailModel model);
    }
}
