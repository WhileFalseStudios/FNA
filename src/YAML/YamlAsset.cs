using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// A yaml file on disk that can be deserialized into the specified class.
    /// </summary>
    /// <typeparam name="T">The object type to deserialize</typeparam>
    public class YamlAsset<T> : IDisposable
    {
        private string yamlString;
        private WeakReference cachedObjectReference;
        private bool disposed = false;

        /// <summary>
        /// The deserialized object from this file.
        /// Note: the return value is cached, so a unique object is not garuanteed each time this method is called.
        /// </summary>
        /// <returns>The deserialized object.</returns>
        public T Object
        {
            get
            {
                if (!cachedObjectReference.IsAlive && !disposed)
                {
                    cachedObjectReference.Target = YAML.Deserialize<T>(yamlString);
                }

                return (T)cachedObjectReference.Target;
            }
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
