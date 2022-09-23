using Microsoft.EntityFrameworkCore;
using Registro.Interfaces;
using Registro.Models;
using Registro.Services;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "https://sistema-de-cadastro-intelitrader-rn4r.vercel.app").AllowAnyHeader().AllowAnyMethod();
                  
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserContext>(
options => options.UseSqlServer("Data Source = bancoDados;Initial Catalog = Usuarios;User ID=SA;Password=Thomas.19983"));

builder.Services.AddScoped<IUserService, UserServices>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();



app.MapControllers();

app.Run();
