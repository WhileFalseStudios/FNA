using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    class YamlAssetLoader<T> : IAssetLoader<YamlAsset<T>>
    {
        public YamlAsset<T> LoadFromStream(Stream stream)
        {
            string yaml = null;

            using (StreamReader sr = new StreamReader(stream))
            {
                yaml = sr.ReadToEnd();
            }

            return new YamlAsset<T>(yaml);
        }
    }
}
