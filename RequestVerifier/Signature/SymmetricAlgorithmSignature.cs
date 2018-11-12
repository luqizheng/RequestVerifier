using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RequestVerifier.Signature
{
    public abstract class SymmetricAlgorithmSignature : ISignature
    {
        private readonly Encoding _encoding;

        protected SymmetricAlgorithmSignature(string iv, string key, Encoding encoding)
        {
            _encoding = encoding;
            Key = Encoding.UTF8.GetBytes(key);
            IV = Encoding.UTF8.GetBytes(iv);
            ;
        }

        /// <summary>
        /// </summary>
        public byte[] IV { get; set; }

        /// <summary>
        /// </summary>
        public byte[] Key { get; set; }

        public string Sign(Stream stream)
        {


            using (var md = Create())
            {
                using (var mStream = new MemoryStream())
                {
                    mStream.Position = 0;
                    var bytes = mStream.ToArray();
                    using (var cStream = new CryptoStream(mStream, Get(md), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytes, 0, bytes.Length);
                        cStream.FlushFinalBlock();
                        var ret = mStream.ToArray();
                        return ret.ToMd5();
                    }
                }
            }
        }

        public string Sign(byte[] bytes)
        {
            using (var md = Create())
            {
                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, Get(md), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytes, 0, bytes.Length);
                        cStream.FlushFinalBlock();
                        var ret = mStream.ToArray();
                        return ret.ToMd5();
                    }
                }
            }
        }

        protected abstract SymmetricAlgorithm Create();

        private ICryptoTransform Get(SymmetricAlgorithm f)
        {
            return f.CreateEncryptor(Key, IV);
        }
    }
}