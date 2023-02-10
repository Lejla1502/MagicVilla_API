using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Serilog;
using MagicVilla_VillaAPI.Repository.IRepository;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
//Enabling Serilog - everything above the level Devug will be logged (to file)
//and the new file will be created every day
Log.Logger =  new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("log/villaLogs.txt", rollingInterval:RollingInterval.Day).CreateLogger();

//telling app to use Serilog instead of default logger
builder.Host.UseSerilog(); 

//the above code will create folder "log" with "villaLogs" file where logs will be stored
*/
builder.Services.AddDbContext<VillaDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection"));
});

builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumRepository, VillaNumRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0); //major and minor version
    options.ReportApiVersions = true;   //displays what version of api are available in the response header
});

//to tell swagger how to use versions for api
builder.Services.AddVersionedApiExplorer(options =>
{
    //used to format api version as groupname
    //VVV- is for the verson name
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;   //instead of v{version} in the url -> v1 or v2
});

//configuring authentication for bearer
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

//builder.Services.AddAuthentication(x =>
//   {
//       x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//       x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    }
//).AddJwtBearer(b =>
//{
//    b.RequireHttpsMetadata = false;
//    b.SaveToken = true;
//    b.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
//        ValidateIssuer = false,
//        ValidateAudience = false
//    };
//});


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
     {
         options.TokenValidationParameters =
             new TokenValidationParameters
             {
                 ValidateAudience = false,
                 ValidateIssuer = false,
                 ValidateActor = false,
                 ValidateLifetime = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
             };
     });

//xonfiguring bearer authentication for swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
            "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo        //this will appear at the left upper corner of swagger
    {
        Version = "v1.0",
        Title = "Magic Villa V1",
        Description = "API to manage Villa",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Dotnetmastery",
            Url = new Uri("https://dotnetmastery.com")
        },
        License =  new OpenApiLicense
        {
            Name = "Example license",
            Url = new Uri("https://example.com/license")
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo  
    {
        Version = "v2.0",
        Title = "Magic Villa V2",
        Description = "API to manage Villa",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Dotnetmastery",
            Url = new Uri("https://dotnetmastery.com")
        },
        License = new OpenApiLicense
        {
            Name = "Example license",
            Url = new Uri("https://example.com/license")
        }
    });
});



builder.Services.AddControllers(option =>
{
    //if the application type is not JSON, display error message
   // option.ReturnHttpNotAcceptable = true;
    //if we define XML as return type in postman, it returns 406 - format type not acceptable

    //but if we want API to support XML formatting, we add
    //AddXmlDataContractSerializerFormatters - adds built in support for XML API
    //and now output of the reqquest can be in XML format

}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters(); //for PATCH method
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<ILogging, Logging>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1"); //default one
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
    });
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
