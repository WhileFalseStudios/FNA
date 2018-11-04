using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    public interface IAssetLoader<T>
    {
        AssetManager Context { get; set; }
        T LoadFromStream(Stream stream);
    }
}
