using System;

namespace Singleselect
{
	public class ISingleSelectItem
    {
        string Name { get; set; }
        string Icon { get; set; }
    }

    public class SingleSelectItem: ISingleSelectItem
    {
		public SingleSelectItem()
		{
		}

		public string Name {get;set;}
        public string Icon { get; set; }
	}
}

