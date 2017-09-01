using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace FAQPhone.Droid.Infrastructure
{
    public static class FileExtentions
    {
        /// <summary>
        ///   This method will tell us if the given FileSystemInfo instance is a directory.
        /// </summary>
        /// <param name="fsi"> </param>
        /// <returns> </returns>
        public static bool IsDirectory(this FileSystemInfo fsi)
        {
            if (fsi == null || !fsi.Exists)
            {
                return false;
            }

            return (fsi.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        /// <summary>
        ///   This method will tell us if the the given FileSystemInfo instance is a file.
        /// </summary>
        /// <param name="fsi"> </param>
        /// <returns> </returns>
        public static bool IsFile(this FileSystemInfo fsi)
        {
            if (fsi == null || !fsi.Exists)
            {
                return false;
            }
            return !IsDirectory(fsi);
        }

        public static bool IsVisible(this FileSystemInfo fsi)
        {
            if (fsi == null || !fsi.Exists)
            {
                return false;
            }

            var isHidden = (fsi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
            return !isHidden;
        }
    }
}