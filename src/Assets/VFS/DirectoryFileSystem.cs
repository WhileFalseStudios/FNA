using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    internal sealed class DirectoryFileSystem : IVirtualFileSystemMount
    {
        public string RootPath { get; set; }

        public bool DoesAssetExist(string path)
        {
            return File.Exists(Path.Combine(RootPath, path));
        }

        public Stream GetAssetStream(string path)
        {
            try
            {
                return File.OpenRead(Path.Combine(RootPath, path));
            }
            catch (Exception ex)
            {
                FNALoggerEXT.LogError.Invoke($"Failed opening stream to {path}: {ex.Message}");
                return null;
            }
        }

        public override string ToString()
        {
            return $"Directory filesystem @ {RootPath}";
        }
    }
}
