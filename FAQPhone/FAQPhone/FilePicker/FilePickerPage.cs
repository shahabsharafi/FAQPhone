using Singleselect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FilePicker
{
    public class FilePickerPage
    {
        SelectSingleBasePage<PathModel> _selector;
        public FilePickerPage(INavigation navigation)
        {
            this._navigation = navigation;
            this._stack = new Stack<string>();
            string path = DependencyService.Get<IFileService>().GetBaseDirectory();
            this._stack.Push(path);
            IList<PathModel> list = DependencyService.Get<IFileService>().GetFileInfos(path);
            CorrectIcon(list);
            _selector = new SelectSingleBasePage<PathModel>(list);
            _selector.Select += Selector_Select;
        }

        private static void CorrectIcon(IList<PathModel> list)
        {
            foreach (var item in list)
            {
                item.Icon = item.IsFile ? Awesome.FontAwesome.FAFile : Awesome.FontAwesome.FAFolder;
            }
        }

        INavigation _navigation;
        public string Dir { get; private set; }
        public string Path { get; private set; }
        public string FileName { get; private set; }
        public async Task Open()
        {
            await _navigation.PushAsync(_selector);
        }
        Stack<string> _stack;
        public event EventHandler Select;
        private void Selector_Select(object sender, EventArgs e)
        {
            if (_selector.SelectedItem.IsFile)
            {
                _navigation.PopAsync();
                Dir = _stack.First();
                FileName = _selector.SelectedItem.Name;
                Path = Dir + "/" + FileName;
                Select?.Invoke(this, new EventArgs());                
            }
            else
            {
                string path = string.Empty;
                if (_selector.SelectedItem.Name == "...")
                {
                    _stack.Pop();
                    path = _stack.First();
                }
                else
                {
                    path = _stack.First() + "/" + _selector.SelectedItem.Name;
                    this._stack.Push(path);
                }
                IList<PathModel> list = DependencyService.Get<IFileService>().GetFileInfos(path);
                if (_stack.Count > 1)
                    list.Insert(0, new PathModel() { Name = "...", IsFile = false });
                CorrectIcon(list);
                _selector.WrappedItems = list;
            }
        }
    }
}
