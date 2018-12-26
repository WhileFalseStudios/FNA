using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Settings used to write a bigfile from a directory
    /// </summary>
    public class BigfileWriteSettings
    {
        /// <summary>
        /// Allows the default split size of 4GiB to be overridden. This should never be higher than that since
        /// 4GiB is the size limit for FAT32 storage devices. Set to 0 to use the default 4GiB size.
        /// </summary>
        public long OverrideSplitSize { get; set; }
        /// <summary>
        /// Set <see cref="Extensions"/> to be a blacklist rather than a whitelist.
        /// </summary>
        /// <value><c>true</c> if blacklist; otherwise, <c>false</c>.</value>
        public bool ExcludeExt { get; set; }
        /// <summary>
        /// A whitelist of extensions to pack into the bigfile. Leave empty to pack anything.
        /// </summary>
        public List<string> Extensions { get; set; }
        /// <summary>
        /// A blacklist of directories to ignore when packing.
        /// </summary>
        public List<string> Directories { get; set; }
    }
}
