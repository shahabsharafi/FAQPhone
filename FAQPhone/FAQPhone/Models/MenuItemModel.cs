
using FAQPhone.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel;

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
        public string CommandName { get; set; }
        public string Icon { get; set; }
        public string Badge { get; set; }
        public bool ShowBadge
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Badge);
            }
        }
    }
}
