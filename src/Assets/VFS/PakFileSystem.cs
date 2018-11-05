using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    internal sealed class PakFileSystem : IVirtualFileSystemMount
    {
        private ZipArchive archive;

        private string INTERNAL_RootPath;
        public string RootPath
        {
            get => INTERNAL_RootPath;
            set
            {
                INTERNAL_RootPath = value;
                if (archive is null)
                {
                    try
                    {
                        archive = ZipFile.Open(RootPath, ZipArchiveMode.Read);
                    }
                    catch (Exception ex)
                    {
                        //FIXME: Some sort of error is needed here
                        FNALoggerEXT.LogError.Invoke($"PakFileSystem: Failed to load {RootPath} as zip archive!");
                    }
                }
            }
        }

        public bool DoesAssetExist(string path)
        {
            if (archive is null)
                return false;

            return !(archive.GetEntry(path) is null);
        }

        public Stream GetAssetStream(string path)
        {
            ZipArchiveEntry entry = archive.GetEntry(path);
            return entry.Open();
        }
    }
}
