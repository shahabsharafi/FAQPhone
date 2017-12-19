using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Models
{
    public class FileStatModel
    {
        public long dev { get; set; }
        public long ino { get; set; }
        public long mode { get; set; }
        public long nlink { get; set; }
        public long uid { get; set; }
        public long gid { get; set; }
        public long rdev { get; set; }
        public long size { get; set; }
        public long blksize { get; set; }
        public long blocks { get; set; }
        public DateTime atime { get; set; }
        public DateTime mtime { get; set; }
        public DateTime ctime { get; set; }
    }
}
