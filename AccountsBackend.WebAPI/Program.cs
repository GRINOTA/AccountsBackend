using AccountsBackend.Data;
using AccountsBackend.BusinesLogic;
using System.Reflection;
using AccountsBackend.BusinesLogic.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;

        builder.Services.AddDataAccess(configuration);
        builder.Services.AddBusinessLogic();
        builder.Services.AddControllers();
        builder.Services.AddAutoMapper(cfg => 
        {
            cfg.AddProfile<AssemblyMappingProfile>();
        }, typeof(AssemblyMappingProfile).Assembly);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.Run();
    }
}
