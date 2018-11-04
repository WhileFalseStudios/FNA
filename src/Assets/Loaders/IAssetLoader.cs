using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    public interface IAssetLoader<T>
    {
        T LoadFromStream(Stream stream);
    }
}
