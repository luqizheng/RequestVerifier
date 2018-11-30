using System.Security.Cryptography;
using System.Text;

namespace RequestVerifier.Signature
{
    /// <summary>
    /// </summary>
    public class TripleDesMd5Signature : SymmetricAlgorithmSignature
    {
        public TripleDesMd5Signature(string iv, string key, Encoding encoding) : base(iv, key, encoding)
        {
        }

        protected override SymmetricAlgorithm Create()
        {
            var c = TripleDES.Create();
            c.IV = IV;
            c.Key = c.Key;
            return c;
        }
    }
}