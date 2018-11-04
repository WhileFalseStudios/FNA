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

        public Game Game { get; }
        private AssetSearchPaths searchPaths;
        private List<IVirtualFileSystemMount> fileSystems = new List<IVirtualFileSystemMount>();
        private Dictionary<Type, object> assetLoaders = new Dictionary<Type, object>();
        private Dictionary<string, object> assets = new Dictionary<string, object>();

        internal static string GameDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
        }

        internal AssetManager(Game game, IServiceProvider serviceProvider, string defaultSearchPath)
        {
            if (serviceProvider is null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            Game = game;

            ServiceProvider = serviceProvider;

            try
            {
                using (FileStream fs = File.OpenRead(Path.Combine(GameDirectory, "DataSearchPaths.yaml")))
                {
                    searchPaths = YAML.Deserialize<AssetSearchPaths>(fs);
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

        public void LogSearchPaths()
        {
            searchPaths.LogSearchPaths();
        }

        #region Asset Loading

        private IAssetLoader<T> GetLoader<T>()
        {
            if (assetLoaders.ContainsKey(typeof(T)))
            {
                return (IAssetLoader<T>)assetLoaders[typeof(T)];
            }

            var type = typeof(IAssetLoader<T>);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

            var loader = types.FirstOrDefault();
            if (loader is null)
                return null;

            var inst = (IAssetLoader<T>)Activator.CreateInstance(loader);
            inst.Context = this;
            assetLoaders.Add(typeof(T), inst);

            return inst;
        }

        /// <summary>
        /// Loads an asset of the given type from the specified path.
        /// </summary>
        /// <typeparam name="T">The asset type to load.</typeparam>
        /// <param name="path">The path, with extension, to load the asset from.</param>
        /// <returns>Loaded asset, or null if it could not be loaded for any reason. If a load fails an error will be printed to the console.</returns>
        public T Load<T>(string path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));

            // See if an asset at that path and of this type already exists (lookup strings maybe should also track type). Return that if it exists.
            // Check if the file exists from the AssetSearchPaths. If not, throw or return default(T)
            // Look through all loaders to see if there's one that matches IAssetLoader<T>. If so instantiate it.
            // Get the stream to the desired file from the VFS system. Pass it into the asset loader.
            // Store the resulting asset along with its reference count.
            // Return the asset

            if (assets.ContainsKey(path))
                return (T)assets[path];

            IVirtualFileSystemMount mount = null;

            foreach (var fs in fileSystems)
            {
                if (fs.DoesAssetExist(path))
                {
                    mount = fs;
                    break;
                }
            }

            if (mount is null)
            {
                FNALoggerEXT.LogError.Invoke($"Asset Manager: could not find {typeof(T).Name} at {path}");
                return default(T);
            }

            IAssetLoader<T> loader = GetLoader<T>();
            if (loader is null)
            {
                FNALoggerEXT.LogError.Invoke($"Asset Manager: could not instantiate an IAssetLoader for asset type {typeof(T).Name}");
                return default(T);
            }

            try
            {
                using (Stream stream = mount.GetAssetStream(path))
                {
                    var asset = loader.LoadFromStream(stream);
                    assets.Add(path, asset);
                    return asset;
                }
            }
            catch (Exception ex)
            {
                FNALoggerEXT.LogError.Invoke($"Asset Manager: asset load failed for {path} Reason: {ex.Message}");
                return default(T);
            }
        }

        /// <summary>
        /// Releases a reference to an asset. When the reference count reaches 0, the asset is unloaded.
        /// </summary>
        /// <param name="path">The asset path to release a reference to. If the path is not a loaded asset nothing happens.</param>
        public void Release(string path)
        {

        }

        #endregion

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
