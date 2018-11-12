using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RequestVerifier;

namespace Web.Controllers
{
    public class TestData
    {
        public string Data { get; set; } = "1231232131";
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [SignResponse()]
        [VerifyRequest()]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [SignResponse()]
        [VerifyRequest()]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [SignResponse()]
        [VerifyRequest()]
        public object Post([FromBody] TestData value)
        {
            return new
            {
                success = true,
                code = "0000"
            };
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [SignResponse()]
        [VerifyRequest()]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [SignResponse()]
        [VerifyRequest()]
        public void Delete(int id)
        {
        }
    }
}
