using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    class Texture2DLoader : IAssetLoader<Texture2D>
    {
        public AssetManager Context { get; set; }
        public string FilePath { get; set; }

        public Texture2D LoadFromStream(Stream stream)
        {
            return Texture2D.FromStream(Context.Game.GraphicsDevice, stream);
        }
    }
}
