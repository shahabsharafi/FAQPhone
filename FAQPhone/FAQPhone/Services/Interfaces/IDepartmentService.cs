using FAQPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentModel>> get(string parentId);
    }
}
