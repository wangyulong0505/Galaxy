using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.EntityFrameworkCore;
using Castle.Facilities.Logging;
using Galaxy.EntityFrameworkCore;
using Galaxy.Web.Configuration;
using Galaxy.Web.Filters;
using Galaxy.Web.Models;
using Galaxy.Web.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Galaxy.Web.Startup
{
    public class Startup
    {
        private readonly RedisCacheConfig cacheProvider;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            cacheProvider = new RedisCacheConfig();
        }
        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region 添加DbContext

            services.AddAbpDbContext<GalaxyDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            });
            //添加对mysql的支持
            /*
            services.AddDbContext<GalaxyDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("MySqlConnection"));
            });
            */
            #endregion

            services.AddSession();
            services.AddOptions();
            
            #region 添加缓存
            /*
            services.AddMemoryCache();
            if (cacheProvider.IsUseRedis)
            {
                services.AddSingleton(typeof(ICacheService), new RedisCacheService(new Microsoft.Extensions.Caching.Redis.RedisCacheOptions
                {
                    Configuration = cacheProvider.ConnectionString,
                    InstanceName = cacheProvider.InstanceName
                }, 0));
            }
            else
            {
                services.AddSingleton<IMemoryCache>(factory =>
                {
                    var cache = new MemoryCache(new MemoryCacheOptions());
                    return cache;
                });
                services.AddSingleton<ICacheService, MemoryCacheService>();
            }
            */
            #endregion

            //添加对appsettings的配置文件的读取
            services.Configure<QQLoginSettings>(Configuration.GetSection("QQLoginSettings"));
            services.Configure<WechatLoginSettings>(Configuration.GetSection("WechatLoginSettings"));
            services.Configure<WeiboLoginSettings>(Configuration.GetSection("WeiboLoginSettings"));
            services.Configure<GithubLoginSettings>(Configuration.GetSection("GithubLoginSettings"));
            //添加对超级管理员配置文件的读取
            services.Configure<SuperAdmin>(Configuration.GetSection("SuperAdmin"));
            //添加jwt配置文件读取
            /*
            services.Configure<JwtSettings>(Configuration);
            JwtSettings settings = new JwtSettings();
            Configuration.Bind("JwtSettings", settings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey))
                };
            });
            */
            //添加登录权限验证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Error/SomethingWrong";
                    options.LoginPath = "/Account/Login";
                });
            //添加WebApi跨域，第一种方式
            services.AddCors();
            //添加Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Galaxy API接口文档",
                    Version = "v1",
                    Description = "RESTful API for Galaxy",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "wangshibang", Email = "wangyulong0505@sina.com", Url = "" }
                });
                options.DocInclusionPredicate((docName, description) => true);
                options.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Galaxy.Application.xml"));
                options.DescribeAllEnumsAsStrings();
                options.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数
            });
            services.AddAntiforgery(option =>
            {
                option.Cookie.Name = "GALAXY-CSRF-COOKIE";
                option.FormFieldName = "GalaxyFieldName";
                option.HeaderName = "GALAXY-CSRF-HEADER";
            });
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            //Configure Abp and Dependency Injection
            return services.AddAbp<GalaxyWebModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); //Initializes ABP framework.
            app.UseSession();
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //添加初始化自定义类DI，便于直接使用IHostEnvironment
            app.UseWkMvcDI();
            app.UseStaticFiles();
            //添加Web Api跨越支持, 第一种方式
            app.UseCors(option =>
                option.WithOrigins("http://www.example.com")
                .AllowAnyHeader()
            );
            //添加Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Galaxy API V1");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
