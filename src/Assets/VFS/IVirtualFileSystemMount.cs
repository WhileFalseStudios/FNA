using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    internal interface IVirtualFileSystemMount
    {
        bool DoesAssetExist(string path);

        Stream GetAssetStream(string path);

        string RootPath { get; set; }
    }
}
