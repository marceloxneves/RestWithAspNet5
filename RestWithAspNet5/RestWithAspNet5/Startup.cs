using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using RestWithAspNet5.Authentication.Configurations;
using RestWithAspNet5.Authentication.Repository;
using RestWithAspNet5.Authentication.Services;
using RestWithAspNet5.Authentication.Services.Implementations;
using RestWithAspNet5.Business;
using RestWithAspNet5.Business.Implementations;
using RestWithAspNet5.Hypermedia.Enricher;
using RestWithAspNet5.Hypermedia.Filters;
using RestWithAspNet5.Model.Context;
using RestWithAspNet5.Repository;
using RestWithAspNet5.Repository.Generic;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Business.Implementations;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestWithAspNet5
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        //Migrations
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            //Log - Serilog - usado em Migrations
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Authentication - ini
            var tokenConfigurations = new TokenConfigurations();

            new ConfigureFromConfigurationOptions<TokenConfigurations>(Configuration.GetSection("TokenConfigurations")).Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
            });
            //Authentication - fim

            //Cors
            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddControllers();

            //Conexão MySql
            var connection = Configuration["MySqlConnection:MySqlConnectionString"];
            services.AddDbContext<MySqlContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

            if (Environment.IsDevelopment())
            {
                MigrateDatabase(connection);
            }

            //Aceitar serialização XML - Content Negociation
            //services.AddMvc(options =>
            //{
            //    options.RespectBrowserAcceptHeader = true;
            //    options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
            //    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
            //}).AddXmlSerializerFormatters();

            //Hateoas
            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
            filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
            services.AddSingleton(filterOptions);


            //Versionamento API
            services.AddApiVersioning();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "REST API's ASP.NET Core 5",
                    Version = "v1",
                    Description = "API Restful with Docker",
                    Contact = new OpenApiContact
                    {
                        Name = "Marcelo",
                        Url = new Uri("https://github.com/marceloxneves")
                    }
                });
            });

            //IOC
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
            services.AddScoped<IBookBusiness, BookBusinessImplementation>();
            services.AddScoped<IFileBusiness, FileBusinessImplementation>();

            //Substituídos por Generics types abaixo
            //services.AddScoped<IPersonRepository, PersonRepositoryImplementation>();            
            //services.AddScoped<IBookRepository, BookRepositoryImplementation>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IPersonRepository, PersonRepository>();

            //IOC - Authentication
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<ITokenService, TokenService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Cors - deve ficar após UseHttpsRedirection e UseRouting e antes de UseEndpoints
            app.UseCors();

            //Swagger - ini
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST API's ASP.NET Core 5 - v1");
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");

            app.UseRewriter(option);
            //Swagger - fim

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //Hateoas
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/v{version=apiVersion}/{id?}");
            });
        }

        private void MigrateDatabase(string connection)
        {
            try
            {
                //var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connection);
                var evolveConnection = new MySqlConnection(connection);
                var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/dataset" },
                    IsEraseDisabled = true
                };
                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error("Database migration failed", ex);
                throw;
            }
        }
    }
}
