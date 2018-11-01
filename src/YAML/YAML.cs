using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Microsoft.Xna.Framework
{
    public static class YAML
    {
        private static IDeserializer deserializer;
        private static ISerializer serializer;

        static YAML()
        {
            deserializer = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build();
            serializer = new SerializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build();
        }

        public static T Deserialize<T>(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                return deserializer.Deserialize<T>(sr.ReadToEnd());
            }
        }

        public static void Serialize<T>(T obj, Stream stream)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                serializer.Serialize(sw, obj);
            }
        }
    }
}
