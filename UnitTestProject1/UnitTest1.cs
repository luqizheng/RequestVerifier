using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RequestVerifier.Signature;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private ISignature Create()
        {
            return new AesMd5Signature("1234567890123456", "09876543210987654321098765432121", Encoding.UTF8);
            ;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var client = new HttpClient();

            var resp =
                client.PostAsync("https://localhost:44331/api/values", GetContent())
                    .Result;
            var status = resp.StatusCode;

            var content = resp.Content.ReadAsStringAsync().Result;
            if (content.Length != 0)
            {
                var signature = Create();
                var d = signature.Sign(Encoding.UTF8.GetBytes(content));

                var sign = resp.Headers.GetValues("sign");
                Assert.AreEqual(d, sign.First());
            }
        }

        public HttpContent GetContent()
        {
            var postData = JsonConvert.SerializeObject(new { data = "test" });
            var httpContent = new StringContent(postData, Encoding.UTF8,
                "application/json");

            httpContent.Headers.Add("sign", Create().Sign(Encoding.UTF8.GetBytes(postData)));
            return httpContent;
        }
      
    }
}