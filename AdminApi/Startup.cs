using System;
using System.Text;
using AdminApi.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model;
using Serilog;
using Microsoft.EntityFrameworkCore;
using AdminApi.Context.DomainModel;
using AdminApi.Context.Repository;
using AdminApi.IService;
using AdminApi.Service;
using Extensions;
using AutoMapper;
using AdminApi.AutoMapperConfig;


namespace AdminApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Appsettings(Configuration));

            services.AddSingleton<ILogger>(Log.Logger);
            services.AddDbContext<AdminContext>(option =>
            {
                var connectionString = Configuration.GetConnectionString("sqlstr");
                option.UseMySql(connectionString);
            }).AddUnitOfWork<AdminContext>()
            .AddCustomRepository<User, UserRepository>()
            .AddCustomRepository<Role, RoleRepository>()
            .AddCustomRepository<UserRole, UserRoleRepository>();

            //添加AutoMapper
            var automapperConfog = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProFile());
            });

            services.AddSingleton(automapperConfog.CreateMapper());

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddTransient<IRoleService, RoleService>();

            services.AddControllers();
            services.AddCors(options => { options.AddPolicy("CorsPolicy", builder => builder.SetIsOriginAllowed((host) => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()); });

            //401 Unauthorized响应 应该用来表示缺失或错误的认证；
            //403 Forbidden响应 应该在这之后用，当用户被认证后，但用户没有被授权在特定资源上执行操作。

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(option =>
                   {
                       //RequireHttpsMetadata: 限定认证操作是否必须通过https来做
                       option.RequireHttpsMetadata = false;
                       option.SaveToken = true;
                       var token = Configuration.GetSection("TokenParameter").Get<TokenParameter>();

                       option.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = false,
                           ValidateAudience = false,
                           ValidateIssuerSigningKey = true,
                           ValidateLifetime = true,//是否验证失效时间
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                           ValidIssuer = token.Issuer,
                           ClockSkew = TimeSpan.Zero,//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
                       };
                   });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("user", policy => policy.RequireRole("user").Build());//单独角色
                options.AddPolicy("admin", policy => policy.RequireRole("admin").Build());
                options.AddPolicy("userOradmin", policy => policy.RequireRole("user", "admin"));//或的关系
                //options.AddPolicy("userAndadmin", policy => policy.RequireRole("user").RequireRole("admin"));//且的关系
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Admin API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                     {
                        new OpenApiSecurityScheme
                        {
                                Reference = new OpenApiReference
                                {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                }
                        },
                        new string[] {}
                     }
        });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");
            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Admin API v1");
                c.RoutePrefix = "";
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

