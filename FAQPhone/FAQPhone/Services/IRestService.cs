using System.Collections.Generic;
using System.Threading.Tasks;

namespace FAQPhone.Services
{
	public interface IRestService<T>
	{
		Task<List<T>> GetList ();

		Task Save (T obj);

		Task Delete (string id);
	}
}
