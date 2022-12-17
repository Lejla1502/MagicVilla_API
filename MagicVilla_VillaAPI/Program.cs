var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(option =>
{
    //if the application type is not JSON, display error message
    option.ReturnHttpNotAcceptable = true;
    //if we define XML as return type in postman, it returns 406 - format type not acceptable

    //but if we want API to support XML formatting, we add
    //AddXmlDataContractSerializerFormatters - adds built in support for XML API
    //and now output of the reqquest can be in XML format

}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters(); //for PATCH method
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
