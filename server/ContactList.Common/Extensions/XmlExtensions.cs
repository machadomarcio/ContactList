using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ContactList.Common.Extensions
{
    public static class XmlExtensions
    {
        private static Encoding XmlEncoder = new UTF8Encoding(false);
        private static Dictionary<Type, XmlSerializer> serializers = new Dictionary<Type, XmlSerializer>();
        private static object _lock = new object();

        private static readonly XmlWriterSettings DefaultSettings = new XmlWriterSettings()
        {
            NewLineHandling = NewLineHandling.None,
            Indent = false,
            OmitXmlDeclaration = true,
            Encoding = new UTF8Encoding(false)
        };

        private static XmlSerializer GetSerializerFor(Type type)
        {
            lock (_lock)
            {
                if (serializers.ContainsKey(type)) return serializers[type];
                var newSerializer = new XmlSerializer(type);
                serializers.Add(type, newSerializer);

                return serializers[type];
            }
        }

        /// <summary>
        /// Method for serialize objects
        /// </summary>
        /// <param name="source">xml for serialize</param>
        /// <returns>object in xml of type string</returns>
        public static string XmlSerialize(this object source)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(ms, DefaultSettings))
                {
                    var nameSpaceEmpty = new XmlSerializerNamespaces();
                    nameSpaceEmpty.Add("", "");

                    var serial = GetSerializerFor(source.GetType());

                    serial.Serialize(writer, source, nameSpaceEmpty);

                    writer.Flush();

                    return XmlEncoder.GetString(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Method for deserialize objects
        /// </summary>
        /// <param name="xml">xml for deserialize</param>
        /// <returns>the object deserialized</returns>
        public static T XmlDeSerialize<T>(this string xml)
        {
            if (string.IsNullOrEmpty(xml)) return default(T);
            try
            {
                return Deserialize<T>(xml);
            }
            catch (Exception)
            {
                var xmlDecoded = HttpUtility.HtmlDecode(xml).Replace("&", "&amp;");

                return Deserialize<T>(xmlDecoded);
            }
        }

        private static T Deserialize<T>(string xml)
        {
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                var serial = GetSerializerFor(typeof(T));

                return (T)serial.Deserialize(reader);
            }
        }

        public static XmlDeclaration GetOrCreateXmlDeclaration(this XmlDocument xmlDocument)
        {
            XmlDeclaration xmlDeclaration = null;
            if (xmlDocument.HasChildNodes)
                xmlDeclaration = xmlDocument.FirstChild as XmlDeclaration;

            if (xmlDeclaration != null)
                return xmlDeclaration;

            xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", null, null);

            var root = xmlDocument.DocumentElement;
            xmlDocument.InsertBefore(xmlDeclaration, root);
            return xmlDeclaration;
        }
    }
}