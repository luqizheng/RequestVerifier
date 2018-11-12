using System.IO;

namespace RequestVerifier.Signature
{
    public interface ISignature
    {
        string Sign(Stream stream);
        string Sign(byte[] encrypt);
    }
}