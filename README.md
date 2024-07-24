# myAuthWebApi
A sample for header token authentication


# Sample
sample project:source/myAuthWebApi/*

## Step 1. Add a New Class to handle auth 
```

HeaderTokenAuthHandler

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
