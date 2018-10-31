using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    internal interface IVirtualFileSystemMount
    {
        bool DoesAssetExist(string path);

        string RootPath { get; set; }
    }
}
