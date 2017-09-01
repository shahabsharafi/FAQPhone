using Singleselect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker
{
    public class PathModel: ISingleSelectItem
    {
        public string Name { get; set; }
        public bool IsFile { get; set; }
        public string Icon { get; set; }
    }
}
