using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker
{
    public interface IFileService
    {
        IList<PathModel> GetFileInfos(string path = null);
        string GetBaseDirectory();
        string GetDocumentsPath();
        byte[] ReadAllBytes(string path);
        byte[] LoadAndResizeBitmap(byte[] data, int width, int height);
        bool Exists(string path);
        void OpenFile(string filePath);
        void DeleteFile(string filePath);
        DateTime GetCreationDate(string filePath);
    }
}
