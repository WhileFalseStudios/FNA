using MonoGame.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Microsoft.Xna.Framework
{
    [XmlRoot("SearchPaths")]
    public sealed class AssetSearchPaths : IEnumerable<string>
    {
        [XmlElement(ElementName = "Path")]
        public List<string> searchPaths;

        public AssetSearchPaths()
        {
            searchPaths = new List<string>();
        }

        internal void AddPath(string path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            searchPaths.Add(path);
        }

        internal void MakeAllPathsRelative(string relativeTo)
        {
            if (relativeTo is null) throw new ArgumentNullException(nameof(relativeTo));

            for (int i = searchPaths.Count; i >= 0; i--)
            {
                searchPaths[i] = FileHelpers.ResolveRelativePath(relativeTo, searchPaths[i]);
            }
        }

        internal void LogSearchPaths()
        {
            FNALoggerEXT.LogInfo.Invoke("Asset Manager: search paths");
            searchPaths.ForEach(path => FNALoggerEXT.LogInfo.Invoke(path));
            FNALoggerEXT.LogInfo.Invoke("----------");
        }

        #region IEnumerator Support
        public IEnumerator<string> GetEnumerator()
        {
            return searchPaths.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return searchPaths.GetEnumerator();
        }
        #endregion
    }
}
