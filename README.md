# myAuthWebApi
A sample for token-based authentication


# Sample
sample project:source/myAuthWebApi/*

## Step 1. Add a New Class to handle auth token
HeaderTokenAuthHandler
```

    public class HeaderTokenAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public HeaderTokenAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
            {
            }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Header-Token"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }

            var authHeader = Request.Headers["Header-Token"].ToString();
            if (authHeader != "your_password")
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }

            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "user_id"),
            new Claim(ClaimTypes.Name, "mike"),
        };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
```

```

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "HeaderTokenScheme";
    options.DefaultChallengeScheme = "HeaderTokenScheme";
})
   .AddScheme<AuthenticationSchemeOptions, HeaderTokenAuthHandler>("HeaderTokenScheme", null);
```

## Step 2. program.cs to add swagger doc header

```
builder.Services.AddSwaggerGen(c =>
 {
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

     // Define the custom header parameter
     c.AddSecurityDefinition("HeaderTokenScheme", new OpenApiSecurityScheme
     {
         In = ParameterLocation.Header,
         Name = "Header-Token",
         Type = SecuritySchemeType.ApiKey,
         Description = "Header-Token for authentication"
     });

     // Add the security requirement to include the header in requests
     c.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "HeaderTokenScheme"
                 }
             },
             new string[] { }
         }
     });
});


```


## Step 3. add Authrize on webapi
```
        [HttpPost("Update")]        
        [Authorize(AuthenticationSchemes = "HeaderTokenScheme")]
        public ObjectResult Update(int id,string name)
        {
            var obj= new { id, name };
            var json=JsonConvert.SerializeObject(obj);
            return Ok("Update successful,json="+json);
        }
```
