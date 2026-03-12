using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using TaskManager.WebApi.DTOs;
using TaskManager.WebApi.Persistence;
using TaskManager.WebApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new OpenApiString(DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd"))
    });
});

builder.Services.AddScoped<IValidator<NewTaskInputModel>, NewTaskInputModelValidator>();
builder.Services.AddScoped<IValidator<EditTaskInputModel>, EditTaskInputModelValidator>();

builder.Services.AddDbContext<TaskManagerDbContext>(opt => opt.UseInMemoryDatabase("TaskManagerDb"));

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
