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
using Sat.Recruitment.Core.Entities;

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
            services.Configure<UnitOfWork>(options => configuration.GetSection("ApiSettings:SecretKey").Bind(options));
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

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

            /*---------ADD CACHE SERVICE----------*/
            //services.AddResponseCaching();

            /*---------AUTHENTICATION SUPPORT WITH .NET IDENTITY----------*/
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<SatRecruitmentContext>();

            /*---------ADD CORS CONFIGURATION----------*/
            services.AddCors(p => p.AddPolicy("CORSPolicy", build =>
            {
                //CHANGE WITHORIGINS PARAMS TO SPECIFY THE DOMAINS THAT CAN CONSUME THIS API.
                build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            /*---------ADD AUTHENTICATION CONFIGURATION----------*/
            var key = configuration.GetValue<string>("ApiSettings:SecretKey");
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

            //Facebook Authentication
            //services.AddAuthentication()
            //  .AddFacebook(options => {
            //    options.AppId = "3301164923542188";
            //    options.AppSecret = "7e8f986c7f5907abf122ac61c0d1b4b1";
            //}).AddGoogle(options => {
            //    options.ClientId = "370954348970-1n9sst0i3qemdes51emnu0h7q14fr4u6.apps.googleusercontent.com";
            //    options.ClientSecret = "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";
            //}).AddMicrosoftAccount(options => {
            //    options.ClientId = "707a9dfc-2b98-44a5-8c82-0128d128f774";
            //    options.ClientSecret = "2FX8Q~43D6vfmXBCBO5l.WApSRVQLOmnHgzTcdj6";
            //});

            //Authorization based on Policies
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
            //    options.AddPolicy("Registrado", policy => policy.RequireRole("Registrado"));
            //    options.AddPolicy("Usuario", policy => policy.RequireRole("Usuario"));
            //    options.AddPolicy("UsuarioYAdministrador", policy => policy.RequireRole("Administrador").RequireRole("Usuario"));

            //    //Claims Policy
            //    options.AddPolicy("AdministradorCrear", policy => policy.RequireRole("Administrador").RequireClaim("Create", "True"));
            //    options.AddPolicy("AdministradorEditarBorrar", policy => policy.RequireRole("Administrador").RequireClaim("Edit", "True").RequireClaim("Delete", "True"));
            //    options.AddPolicy("AdministradorCrearEditarBorrar", policy => policy.RequireRole("Administrador").RequireClaim("Create", "True")
            //    .RequireClaim("Edit", "True").RequireClaim("Delete", "True"));
            //});

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
                    "Ex: \"Bearer t0k3nH3r3\"",
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
