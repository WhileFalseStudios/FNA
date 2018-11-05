using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework
{
    class BigfileFileSystem : IVirtualFileSystemMount
    {
        private Bigfile bigfile;

        private string INTERNAL_RootPath;
        public string RootPath
        {
            get => INTERNAL_RootPath;
            set
            {
                INTERNAL_RootPath = value;
                bigfile = Bigfile.Open(RootPath);
            }
        }

        public bool DoesAssetExist(string path)
        {
            return bigfile.FileExists(path);
        }

        public Stream GetAssetStream(string path)
        {
            return bigfile.OpenFile(path);
        }
    }
}
