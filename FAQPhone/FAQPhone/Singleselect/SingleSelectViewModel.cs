using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleselect
{
    public class SingleSelectViewModel<T>
    {
        public SingleSelectViewModel()
        {
            WrappedItems = new ObservableCollection<T>();
        }
        public ObservableCollection<T> WrappedItems { get; set; }
    }
}
