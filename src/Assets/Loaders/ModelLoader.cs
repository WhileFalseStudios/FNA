using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    class ModelLoader : IAssetLoader<Model>
    {
        public AssetManager Context { get; set; }
        public string FilePath { get; set; }

        public Model LoadFromStream(Stream stream)
        {
            switch (Path.GetExtension(FilePath))
            {
                case "gltf":
                    return LoadGLTF2(stream);
                case "glb":
                    return LoadGLB(stream);
                case "fmdl":
                    return LoadFMDL(stream);
            }

            FNALoggerEXT.LogError.Invoke($"Could not load model with extension {Path.GetExtension(FilePath)}");
            return null;
        }

        private Model LoadGLTF2(Stream stream)
        {

        }

        private Model LoadGLB(Stream stream)
        {

        }

        private Model LoadFMDL(Stream stream)
        {

        }
    }
}
