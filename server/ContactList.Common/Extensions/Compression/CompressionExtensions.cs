using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;

namespace ContactList.Common.Extensions.Compression
{
    public static class CompressionExtensions
    {
        private readonly static string PrefixQlpo = "{0}Qlpo";

        public static Dictionary<string, string> GetZipFiles(this byte[] rawdata, string encoding)
        {
            var documents = new Dictionary<string, string>();

            var sourceEncode = encoding.GetSourceEncoding();

            using (var zipProc = new ZipArchive(new MemoryStream(rawdata), ZipArchiveMode.Read, false, sourceEncode))
            {
                foreach (var entry in zipProc.Entries)
                {
                    var entryOpened = entry.Open();

                    using (var reader = new StreamReader(entryOpened, sourceEncode))
                    {
                        Byte[] bytes;

                        using (var memstream = new MemoryStream())
                        {
                            reader.BaseStream.CopyTo(memstream);
                            bytes = memstream.ToArray();
                        }

                        documents.Add(entry.Name, Convert.ToBase64String(bytes));
                    }
                }
            }
            return documents;
        }

        public static string GetStringByteArray(this string content, string encoding = "ibm850")
        {
            var sourceEncode = encoding.GetSourceEncoding();

            return Convert.ToBase64String(sourceEncode.GetBytes(content));
        }

        public static string GetStringByteArrayCompress(this string content, string encoding = "ibm850")
        {
            if (content.StartsWith(PrefixQlpo))
                return content;

            var sourceEncode = encoding.GetSourceEncoding();

            return Convert.ToBase64String(sourceEncode.GetBytes(content)).Compress();
        }

        public static string GetValueContent(this string content, string encoding = "ibm850")
        {
            var sourceEncode = encoding.GetSourceEncoding();

            string value;

            if (content.StartsWith("Qlpo") || content.StartsWith(PrefixQlpo))
                value = sourceEncode.GetString(Convert.FromBase64String(content.DeCompress()));
            else
                value = sourceEncode.GetString(Convert.FromBase64String(content));

            return new StreamReader(new MemoryStream("utf-8".GetSourceEncoding().GetBytes(value))).ReadToEnd();
        }

        public static bool IsCompacted(this string content)
        {
            return content.StartsWith(PrefixQlpo);
        }

        public static byte[] GetByteArray(this string content)
        {
            if (content.StartsWith("Qlpo") || content.StartsWith(PrefixQlpo))
                content = content.DeCompress();

            return Convert.FromBase64String(content);
        }

        private static string Compress(this string stringToCompress)
        {
            if (string.IsNullOrEmpty(stringToCompress))
                return string.Empty;

            if (stringToCompress.StartsWith("Qlpo") || stringToCompress.StartsWith(PrefixQlpo))
                return stringToCompress;

            var strOut = Convert.ToBase64String(Compress(Encoding.UTF8.GetBytes(stringToCompress)));

            if (!String.IsNullOrEmpty(strOut))
                strOut = "{0}" + strOut;

            return strOut;
        }

        private static string DeCompress(this string stringToDecompress)
        {
            if (string.IsNullOrEmpty(stringToDecompress))
                return string.Empty;

            var encoding = Encoding.Unicode;

            if (stringToDecompress.StartsWith(PrefixQlpo))
            {
                encoding = Encoding.UTF8;
                stringToDecompress = stringToDecompress.Remove(0, 3);
            }

            if (!stringToDecompress.StartsWith("Qlpo"))
                return stringToDecompress;

            string outString;

            try
            {
                var inArr = Convert.FromBase64String(stringToDecompress.Trim());
                outString = encoding.GetString(DeCompress(inArr));
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return outString;
        }

        public static byte[] Compress(byte[] bytesToCompress)
        {
            var ms = new MemoryStream();
            Stream s = new BZip2OutputStream(ms, 1);
            s.Write(bytesToCompress, 0, bytesToCompress.Length);
            s.Close();
            return ms.ToArray();
        }

        public static byte[] DeCompress(byte[] bytesToDecompress)
        {
            var writeData = new byte[4096];

            using (var stream = new BZip2InputStream(new MemoryStream(bytesToDecompress)))
            {
                using (var outStream = new MemoryStream())
                {
                    while (true)
                    {
                        var size = stream.Read(writeData, 0, writeData.Length);
                        if (size > 0)
                            outStream.Write(writeData, 0, size);
                        else
                            break;
                    }

                    return outStream.ToArray();
                }
            }
        }

        public static byte[] GetAnyArray(this object toArray)
        {
            var binFormatter = new BinaryFormatter();

            var mStream = new MemoryStream();

            binFormatter.Serialize(mStream, toArray);

            return mStream.ToArray();
        }

        public static Encoding GetSourceEncoding(this string encoding)
        {
            Encoding sourceEncode = null;

            try
            {
                int intEncoding = 0;

                int.TryParse(encoding, out intEncoding);

                if (intEncoding != 0)
                    sourceEncode = Encoding.GetEncoding(intEncoding);
                else
                    sourceEncode = Encoding.GetEncoding(encoding);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException();
            }

            return sourceEncode;
        }
    }
}
