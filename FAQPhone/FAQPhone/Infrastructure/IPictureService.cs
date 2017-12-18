using System;

namespace TackPicture
{
    public interface IPictureService
    {
        void TakeAPicture(Action<string> action);
    }
}
