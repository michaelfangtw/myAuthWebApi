using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using myAuthWebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "HeaderTokenScheme";
    options.DefaultChallengeScheme = "HeaderTokenScheme";
})
   .AddScheme<AuthenticationSchemeOptions, HeaderTokenAuthHandler>("HeaderTokenScheme", null);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
