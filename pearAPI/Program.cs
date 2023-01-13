using Microsoft.EntityFrameworkCore;
using pearAPI.Database;
using pearAPI.Models; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); //CORS
});
builder.Services.AddDbContext<PearDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("connString")));


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

app.UseCors("Policies");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

