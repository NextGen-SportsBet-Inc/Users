using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using SportBetInc.Repositories;
using MassTransit;
using SportBetInc.Consumer;

namespace SportBetInc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load(".env");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<VerifyUserConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri("" + Environment.GetEnvironmentVariable("RabbitMQConnectionURI")), h =>
                    {
                        h.Username("" + Environment.GetEnvironmentVariable("RabbitUser"));
                        h.Password("" + Environment.GetEnvironmentVariable("RabbitPassword"));
                    });

                    cfg.ConfigureEndpoints(context);
                });

            });

            builder.Services.AddControllers();

            // Database context injection
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
            var connectionString = $"Data Source={dbHost};Initial Catalog={dbName}; User ID=sa;Password={dbPassword}; TrustServerCertificate=True";
            builder.Services.AddDbContext<UsersDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            IdentityModelEventSource.ShowPII = true;

            //keycloak
            builder.Configuration.AddEnvironmentVariables();
            builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
            builder.Services
                .AddAuthorization()
                .AddKeycloakAuthorization(options =>
                {
                    options.EnableRolesMapping =
                        RolesClaimTransformationSource.ResourceAccess;
                    options.RolesResource = $"{builder.Configuration["Keycloak:resource"]}";
                })
                .AddAuthorizationBuilder();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SportsBet Users API", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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

                opt.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var home_dir = Environment.GetEnvironmentVariable("HOME_DIR");
                    apiDesc.RelativePath = home_dir + apiDesc.RelativePath;
                    return true;
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
