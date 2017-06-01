using System.Collections.Generic;
using System.Threading.Tasks;

namespace FAQPhone.Services.Interfaces
{
	public interface IRestService<T> where T : new()
    {
		Task<List<T>> GetList ();

        Task<T> Get(string id);

        Task Save (T obj);

		Task Delete (string id);
	}
}
