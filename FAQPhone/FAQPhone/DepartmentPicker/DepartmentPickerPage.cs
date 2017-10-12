using FAQPhone.Models;
using FAQPhone.Services.Interfaces;
using Singleselect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FAQPhone.DepartmentPicker
{
    public class DepartmentPickerPage
    {
        SelectSingleBasePage<DepartmentModel> _selector;
        List<DepartmentModel> _list;
        public DepartmentPickerPage(INavigation navigation)
        {
            this._navigation = navigation;
            Task.Run(() => load()).Wait();            
        }

        async Task load ()
        {
            var services = DependencyService.Get<IDepartmentService>();
            this._list = await services.GetTree();
            _selector = new SelectSingleBasePage<DepartmentModel>(this._list, false, true);
            _selector.Select += Selector_Select;
        }

        INavigation _navigation;
        public async Task Open()
        {
            await _navigation.PushAsync(_selector);
        }
        public DepartmentModel selectedItem { get; private set; }
        public event EventHandler Select;
        private void Selector_Select(object sender, EventArgs e)
        {
            if (_selector.SelectedItem.children == null && _selector.SelectedItem.children.Length == 0)
            {
                _navigation.PopAsync();
                selectedItem = _selector.SelectedItem;
                Select?.Invoke(this, new EventArgs());
            }
            else
            {
                _selector.WrappedItems = _selector.SelectedItem.children;
            }
        }
    }
}
