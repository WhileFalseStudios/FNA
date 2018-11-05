using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Simple data storage format.
    /// </summary>
    public class Bigfile : IDisposable
    {
        #region Constants

        /// <summary>
        /// 4-byte file header (BIGF)
        /// </summary>
        const int FileMagic = ('F' << 24) + ('G' << 16) + ('I' << 8) + 'B';
        /// <summary>
        /// 4GB (max file size on FAT32)
        /// </summary>
        const long DefaultSplitSize = 4 * 1073741824L;

        #endregion

        #region Evil Persistent streams

        private FileStream headerStream;
        private List<FileStream> streams;

        #endregion

        #region Bigfile info

        private string filePath;

        private int splitCount;

        Dictionary<string, BigfileEntry> files = new Dictionary<string, BigfileEntry>();

        #endregion

        #region Initialisation

        public static Bigfile Open(string filePath)
        {
            return new Bigfile(filePath);
        }        

        private Bigfile(string path)
        {
            filePath = path;
            headerStream = File.OpenRead(path);
            using (BinaryReader br = new BinaryReader(headerStream))
            {
                int magic = br.ReadInt32();

                if (magic != FileMagic)
                    throw new InvalidDataException("Given file is not a valid bigfile archive");

                ushort version = br.ReadUInt16();
                switch (version)
                {
                    case 1:
                        ReadHeaderV1(br);
                        InitFileV1();
                        break;
                    default:
                        throw new InvalidDataException("Unknown bigfile version number");
                }
            }
        }

        #endregion

        #region File reading

        /// <summary>
        /// Gets whether the specified file exists in the bigfile.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FileExists(string path)
        {
            return files.ContainsKey(path);
        }

        /// <summary>
        /// Opens the specified file from the bigfile.
        /// If the file is not found, <see cref="null"/> is returned.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns></returns>
        public Stream OpenFile(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            if (files.ContainsKey(path))
            {
                BigfileEntry file = files[path];
                FileStream fs = streams[file.Split];
                SubStream ss = new SubStream(fs, file.Offset, file.Length);
                return ss;
            }
            else return null;
        }

        #endregion

        #region Bigfile Creation

        

        #endregion

        #region Bigfile V1

        private void ReadHeaderV1(BinaryReader br)
        {
            splitCount = br.ReadInt32();
            int fileCount = br.ReadInt32();
            for (int i = 0; i < fileCount; i++)
            {
                string fname = br.ReadString();
                int split = br.ReadInt32();
                long offset = br.ReadInt64();
                long length = br.ReadInt64();

                files.Add(fname, new BigfileEntry(fname, split, offset, length));
            }
        }

        private void InitFileV1()
        {
            for (int i = 0; i < splitCount; i++)
            {
                streams[i] = File.OpenRead(Path.ChangeExtension(filePath, i.ToString("###")));
            }
        }

        #endregion

        #region IDisposable Implementation
        bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
            {
                streams.ForEach(stream => ((IDisposable)stream).Dispose());
                streams.Clear();
                headerStream.Dispose();
                headerStream = null;

                files.Clear();

                disposed = true;
            }
        }
        #endregion
    }
}
