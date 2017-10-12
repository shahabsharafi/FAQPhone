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
        Task<List<DepartmentModel>> GetByParent(string parentId);
        Task<List<DepartmentModel>> GetById(string id);
        Task<List<DepartmentModel>> GetTree();
    }
}
