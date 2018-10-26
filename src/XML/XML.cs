using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Microsoft.Xna.Framework
{
    public static class XML
    {
        public static T Deserialize<T>(Stream stream)
        {
            T result;
            try
            {
                XmlSerializer xms = new XmlSerializer(typeof(T));
                result = (T)xms.Deserialize(stream);
            }
            catch
            {
                result = default(T);
            }
            return result;
        }

        public static void Serialize<T>(T serialized, Stream outStream)
        {
            try
            {
                XmlSerializer xms = new XmlSerializer(typeof(T));
                xms.Serialize(outStream, serialized);
            }
            catch (InvalidOperationException ex)
            {
                FNALoggerEXT.LogError("XML serialization threw an exception: " + ex.InnerException.Message);
                throw ex.InnerException;
            }
        }
    }
}
