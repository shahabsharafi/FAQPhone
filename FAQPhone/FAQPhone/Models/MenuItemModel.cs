
using FAQPhone.Infrastructure;
using System.Collections.Generic;

namespace FAQPhone.Models
{
    public class MenuItemModel
    {
        public string DisplayName
        {
            get
            {
                return ResourceManagerHelper.GetValue(this.CommandName);
            }
        }
        public List<MenuItemModel> Children { get; set; }
        public string CommandName { get; set; }
    }
}
