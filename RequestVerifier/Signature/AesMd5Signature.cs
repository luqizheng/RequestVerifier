using System.Security.Cryptography;
using System.Text;

namespace RequestVerifier.Signature
{
    /// <summary>
    /// </summary>
    public class AesMd5Signature : SymmetricAlgorithmSignature
    {
        public AesMd5Signature(string iv, string key, Encoding encoding) : base(iv, key, encoding)
        {
        }


        protected override SymmetricAlgorithm Create()
        {
            var md = Aes.Create();
            md.IV = IV;
            md.Key = md.Key;
            return md;
        }
    }
}