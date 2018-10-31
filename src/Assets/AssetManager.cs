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
        private List<IVirtualFileSystemMount> fileSystems = new List<IVirtualFileSystemMount>();

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
                using (FileStream fs = File.OpenRead(Path.Combine(GameDirectory, "DataSearchPaths.xml")))
                {
                    searchPaths = XML.Deserialize<AssetSearchPaths>(fs);
                    if (searchPaths is null)
                    {
                        throw new Exception("XML deserialisation failed");
                    }
                }                
            }
            catch (Exception ex)
            {
                FNALoggerEXT.LogWarn($"Asset Manager: failed to load DataSearchPaths.xml. Reason: {ex.Message}");
                searchPaths = new AssetSearchPaths();
            }
        }

        #region VFS Mounting

        /// <summary>
        /// Mounts a given path in the filesystem. This should be given without an extension as we automatically look for all of the relevant
        /// file types.
        /// The load order is:
        /// For each search path...
        ///     Look for [path]/ (a directory)
        ///     Look for [path].pak (a renamed zip file)
        ///     Look for [path].dat (a bigfile)
        /// Multiple paths may be mounted by default. Set onlyFirst to true to only mount the first valid path and ignore any others.
        /// </summary>
        /// <param name="path">The path to try and find a mountable path in. Relative to any data path from DataSearchPaths.xml and with no extension.</param>
        /// <param name="onlyFirst">Only mount the first valid path. Any successive valid paths are discarded.</param>
        /// <returns>The number of paths mounted. Will be 0 if nothing is found.</returns>
        public int MountDataPath(string path, bool onlyFirst = false)
        {
            if (path is null) throw new ArgumentNullException("path");

            string pathAsBigFile = Path.ChangeExtension(path, ".dat");
            string pathAsArchive = Path.ChangeExtension(path, ".pak");
            int foundCount = 0;
            
            foreach (var dataDir in searchPaths)
            {
                if (Directory.Exists(Path.Combine(dataDir, path))) // We found a directory with the given name, load it
                {
                    DirectoryFileSystem dfs = new DirectoryFileSystem();
                    dfs.RootPath = Path.Combine(dataDir, path);
                    fileSystems.Add(dfs);
                    foundCount++;
                    if (onlyFirst) break;
                }
                if (File.Exists(Path.Combine(dataDir, pathAsArchive))) // We found an archive file with the given name
                {
                    throw new NotImplementedException("Archive filesystem not yet implemented");
                }
                if (File.Exists(Path.Combine(dataDir, pathAsBigFile))) // We found a bigfile with the given name
                {
                    throw new NotImplementedException("Bigfile filesystem not yet implemented");
                }                
            }

            return foundCount;
        }

        #endregion

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
