﻿using CapaDatos.Implementacion.auth.Implementacion;
using CapaDatos.Implementacion.Cliente.Implementacion;
using CapaDatos.Implementacion.Listas.Implementacion;
using CapaDatos.Interza.auth.Interfaz;
using CapaDatos.Interza.Cliente.Interfaze;
using CapaDatos.Interza.Listas.Interfaz;
using CapaNegocio.Implementacion.auth.Implementacion;
using CapaNegocio.Implementacion.Clientes.Implementacion;
using CapaNegocio.Implementacion.Listas.Implementacion;
using CapaNegocio.Interfaz.auth.Interfaz;
using CapaNegocio.Interfaz.Clientes.Interfaze;
using CapaNegocio.Interfaz.Listas.Interfaz;
using FinancieraApi.jtw;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FinancieraApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static WebApplication InicializarApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);
            return app;
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });
            builder.Services.AddSingleton<TokenGenerator>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

            builder.Services.AddScoped<IloginCapaDatos, clsLoginCapaDatos>();
            builder.Services.AddScoped<IloginCapaNegocios, clsLoginCapaNegocios>();

            builder.Services.AddScoped<IListasCombosCapaDatos, clsListasCombos>();
            builder.Services.AddScoped<IListasCapaNegocios, clsListasCombosCapaNegocios>();




            builder.Services.AddScoped<IClienteCapaDatos, clsClienteCapaDatos>();
            builder.Services.AddScoped<IClienteCapaNegocios, clsClienteCapaNegocios>();





        }
        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(
                    c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Api");
                        c.DefaultModelsExpandDepth(-1);
                        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    });
            }


            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("AllowAllOrigins");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=home}/{action=index}/{id?}"
                    );
            });

            // app.MapControllers();

            app.Run();
        }
    }
}
