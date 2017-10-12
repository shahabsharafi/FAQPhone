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
    public class DepartmentPickerFactory
    {
        public static DepartmentPickerPage GetPicker(INavigation navigation)
        {
            var factory = App.Resolve<DepartmentPickerFactory>();
            return factory.Create(navigation);
        }
        IDepartmentService departmentService;
        public DepartmentPickerFactory(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }
        public DepartmentPickerPage Create(INavigation navigation)
        {
            return new DepartmentPickerPage(this.departmentService, navigation);
        }
    }
    public class DepartmentPickerPage
    {
        SelectSingleBasePage<DepartmentModel> _selector;
        List<DepartmentModel> _list;
        IDepartmentService _departmentService;
        public DepartmentPickerPage(IDepartmentService departmentService, INavigation navigation)
        {
            this._navigation = navigation;
            this._departmentService = departmentService;
            Task.Run(() => load()).Wait();            
        }

        async Task load ()
        {
            this._list = await this._departmentService.GetTree();
            _selector = new SelectSingleBasePage<DepartmentModel>(this._list, false, true, "caption");
            _selector.Select += Selector_Select;
        }

        INavigation _navigation;
        public async Task Open()
        {
            await _navigation.PushAsync(_selector);
        }
        public DepartmentModel SelectedItem { get; private set; }
        public event EventHandler Select;
        private void Selector_Select(object sender, EventArgs e)
        {
            if (_selector.SelectedItem.children == null || _selector.SelectedItem.children.Length == 0)
            {
                _navigation.PopAsync();
                SelectedItem = _selector.SelectedItem;
                Select?.Invoke(this, new EventArgs());
            }
            else
            {
                _selector.WrappedItems = _selector.SelectedItem.children;
            }
        }
    }
}
