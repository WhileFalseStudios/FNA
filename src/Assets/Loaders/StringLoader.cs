using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    class StringLoader : IAssetLoader<string>
    {
        public AssetManager Context { get; set; }

        public string LoadFromStream(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
