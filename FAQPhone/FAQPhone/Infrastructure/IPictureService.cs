using System;

namespace TackPicture
{
    public interface IPictureService
    {
        void TakeAPicture(Action<byte[]> action);
    }
}
