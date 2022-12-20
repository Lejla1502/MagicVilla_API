using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

builder.Services.AddAutoMapper(typeof(MappingConfig));

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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
