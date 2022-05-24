using Microsoft.EntityFrameworkCore;
using WebAPI_Nwind.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var connectionString = builder.Configuration.GetConnectionString("BDNorthwind");
//builder.Services.AddDbContext<NorthwindContext>(x => x.UseMySql(ServerVersion.Parse("8.0.28-mysql")));

var connectionString = builder.Configuration.GetConnectionString("BDNorthwind");
builder.Services.AddDbContext<NorthwindContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5501");
        }
        );
});


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

app.UseCors();

app.Run();
