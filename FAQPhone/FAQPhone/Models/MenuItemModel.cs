
namespace FAQPhone.Models
{
    public class MenuItemModel
    {
        public string DisplayName { get; set; }
        public MenuItemModel[] Children { get; set; }
        public string CommandName { get; set; }
    }
}
