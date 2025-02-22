using AccountsBackend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using AccountsBackend.BusinessLogic;
using AccountsBackend.BusinessLogic.Mappings;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
//using System.Web.Http.Cors;
using System.Web.Http;
using DotNetEnv;
using System.Collections;

namespace AccountsBackend.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var httpConfig = new HttpConfiguration();

        httpConfig.MapHttpAttributeRoutes();
        httpConfig.Routes.MapHttpRoute(
            name: "DefaultApi", 
            routeTemplate: "api/{controller}/{id}", 
            defaults: new {id = RouteParameter.Optional});

        var builder = WebApplication.CreateBuilder(args);

        var solutionDirectory = Directory.GetCurrentDirectory();
        var envFilePath = Path.Combine(solutionDirectory, "..", "configuration.env");
        Console.WriteLine($"Env file path: {envFilePath}");

        Console.WriteLine("Env file exists. Loading variables.");
        Env.Load(envFilePath);

        builder.Configuration["JWTCONFIG__ISSUER"] = Environment.GetEnvironmentVariable("JWTCONFIG__ISSUER");
        builder.Configuration["JWTCONFIG__AUDIENCE"] = Environment.GetEnvironmentVariable("JWTCONFIG__AUDIENCE");
        builder.Configuration["JWTCONFIG__KEY"] = Environment.GetEnvironmentVariable("JWTCONFIG__KEY");
        builder.Configuration["CONNECTION_STRING__ACCOUNTDB"] = Environment.GetEnvironmentVariable("CONNECTION_STRING__ACCOUNTDB");

        var configuration = builder.Configuration;

        configuration.AddEnvironmentVariables();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDataAccess(configuration);
        builder.Services.AddBusinessLogic();

        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddControllers();

        builder.Services.AddSpaStaticFiles(config => 
        {
            config.RootPath = "../accounts-frontend/dist";
        });

        builder.Services.AddCors(option => 
        {
            option.AddPolicy("MyCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader();
            });
        });

        builder.Services.AddAutoMapper(cfg => 
        {
            cfg.AddProfile<AssemblyMappingProfile>();
        }, typeof(AssemblyMappingProfile).Assembly);

        builder.Services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = false;  
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["JWTCONFIG__ISSUER"],
                ValidAudience = builder.Configuration["JWTCONFIG__AUDIENCE"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTCONFIG__KEY"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
            
            opt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["cool-cookies"];

                    return Task.CompletedTask;
                }
            };
        });
    
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddSwaggerGen(opt => 
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Введите ваш JWT Access token",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            opt.AddSecurityDefinition("Bearer", jwtSecurityScheme);
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {jwtSecurityScheme, Array.Empty<string>()}
            });
        });

        var app = builder.Build();

        app.UseStaticFiles();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSpaStaticFiles();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseWebSockets();
        app.UseSpa(spa => 
        {
            spa.Options.SourcePath = "../accounts-frontend";

            if(app.Environment.IsDevelopment())
            {
                spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
            }
        });

        app.UseHttpsRedirection();
        
        app.Run();
    }
}
