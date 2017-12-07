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
    public class EmailService : BaseRestService, IEmailService
    {
        public async Task Send(EmailModel model)
        {
            string url = string.Format(Constants.RestUrl, "email/send");
            await this.post<EmailModel>(url, model);
        }
    }
}
