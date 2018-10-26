using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Microsoft.Xna.Framework
{
    public class AssetManager : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }

        private AssetSearchPaths searchPaths;

        internal static string GameDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
        }

        public AssetManager(IServiceProvider serviceProvider, string defaultSearchPath)
        {
            if (serviceProvider is null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            ServiceProvider = serviceProvider;

            try
            {
                using (FileStream fs = File.OpenRead("DataSearchPaths.xml"))
                {
                    searchPaths = XML.Deserialize<AssetSearchPaths>(fs);
                }                
            }
            catch (Exception ex)
            {
                FNALoggerEXT.LogWarn($"Asset Manager: failed to load DataSearchPaths.xml. Reason: {ex.Message}");
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        ~AssetManager()
        {
           Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
