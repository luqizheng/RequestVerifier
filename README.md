# RequestVerifier
verify signature  of request body and sign the response body

# Quick Start

```csharp
   public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            // add request verifier.
            services.AddVerifyRequestSignature("sign", () => new AesMd5Signature("1234567890123456",
                "09876543210987654321098765432121", Encoding.UTF8));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //use the response signature.
            app.UseResponseSignature();
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
   ``` 
   
```csharp
   //in Controller of MVC
   
        [HttpPut][HttpPost] only for put or post 
        [SignResponse()] //sign body of  response in middleware.
        [VerifyRequest()] // verify the post/put body data in ActionFilter.
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
   ```
# How to make a new signature method

extent ISignature 
