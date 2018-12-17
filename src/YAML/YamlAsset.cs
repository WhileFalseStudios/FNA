using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// A yaml file on disk that can be deserialized into a specific class.
    /// </summary>
    public class YamlAsset : IDisposable
    {
        private string yamlString;
        private WeakReference cachedObjectReference;
        private bool disposed = false;

        /// <summary>
        /// The deserialized object from this file.
        /// Note: the return value is cached, so a unique object is not garuanteed each time this method is called.
        /// </summary>
        /// <returns>The deserialized object.</returns>
        public T GetObject<T>()
        {
            if (!cachedObjectReference.IsAlive && !disposed)
            {
                cachedObjectReference = new WeakReference(YAML.Deserialize<T>(yamlString));
            }

            return (T)cachedObjectReference.Target;
        }

        internal YamlAsset(string yaml)
        {
            yamlString = yaml;
        }

        public void Dispose()
        {
            disposed = true;
            yamlString = null;
            cachedObjectReference.Target = null;
        }        
    }
}
