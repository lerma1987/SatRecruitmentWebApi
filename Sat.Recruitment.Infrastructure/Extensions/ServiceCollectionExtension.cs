using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Sat.Recruitment.Core.Interfaces;
using Sat.Recruitment.Core.Services;
using Sat.Recruitment.Infrastructure.Data;
using Sat.Recruitment.Infrastructure.Repositories;
using System.Text;

namespace Sat.Recruitment.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SatRecruitmentContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("SatRecruitmentConn"))
            );

            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UserAuthRepository>(options => configuration.GetSection("ApiSettings:Secret").Bind(options));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserTypeService, UserTypeService>();
            services.AddTransient<IUserAuthService, UserAuthService>();
            services.AddTransient<IUserAuthRepository, UserAuthRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();            

            /*---------Add CORS configuration----------*/
            services.AddCors(p => p.AddPolicy("CORSPolicy", build =>
            {
                //Change WithOrigins params to specify the domains that can consume this API.
                build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); 
            }));

            /*---------Add Authentication configuration----------*/
            var key = configuration.GetValue<string>("ApiSettings:Secret");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string xmlFileName)
        {
            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "David Lerma API", Version = "v1" });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                doc.IncludeXmlComments(xmlPath);
                doc.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authentication JWT using Bearer scheme. \r\n\r\n" +
                    "Type 'Bearer' word followed by an [space] then the Token \r\n\r\n" +
                    "Ex: \"Bearer t0k3nH3r3xxxxx\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                doc.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }
    }
}
