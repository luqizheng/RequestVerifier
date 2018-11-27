using System.Linq;
using System.Text;

namespace RequestVerifier
{
    public class SignatureSetting
    {
        /// <summary>
        ///     header name, default to sign"
        /// </summary>
        public string Header { get; set; } = "sign";

        /// <summary>
        ///     if Methods contains "get"
        ///     querystring should be verified it.
        /// </summary>
        public string[] Methods { get; set; } = { "put", "post" };

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public bool Support(string method)
        {
            return Methods.Any(f => method.ToLower() == f);
        }
    }
}