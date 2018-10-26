using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework.src.Assets.VFS
{
    internal sealed class DirectoryFileSystem : IVirtualFileSystemMount
    {
        public string RootDirectory { get; set; }

        public bool DoesAssetExist(string path)
        {
            return File.Exists(Path.Combine(RootDirectory, path));
        }
    }
}
