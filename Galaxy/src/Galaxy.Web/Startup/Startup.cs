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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authorization;
using Galaxy.Web.Attributes;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

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
            services.AddAbpDbContext<GalaxyDbContext>(options =>
            {
                DbContextOptionsConfigurer.ConfigureMySql(options.DbContextOptions, options.ConnectionString);
            });
            */
            #endregion

            #region 添加对Session的支持

            services.AddSession();

            #endregion

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

            #region 添加对配置文件的读取

            services.AddOptions();
            services.Configure<QQLoginSettings>(Configuration.GetSection("QQLoginSettings"));
            services.Configure<WechatLoginSettings>(Configuration.GetSection("WechatLoginSettings"));
            services.Configure<WeiboLoginSettings>(Configuration.GetSection("WeiboLoginSettings"));
            services.Configure<GithubLoginSettings>(Configuration.GetSection("GithubLoginSettings"));
            //添加对超级管理员配置文件的读取
            services.Configure<SuperAdmin>(Configuration.GetSection("SuperAdmin"));

            #endregion

            #region 添加Jwt权限认证

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

            #endregion

            #region 添加Cookie权限验证

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Error/SomethingWrong";
                    options.LoginPath = "/Account/Login";
                });

            #endregion

            #region 添加WebApi跨域

            services.AddCors();

            #endregion

            #region  添加SwaggerUI

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

            #endregion

            #region 注册IAuthorizationPolicyProvider和IApplicationModelProvider
            /*
            services.TryAdd(ServiceDescriptor.Transient<IAuthorizationPolicyProvider, ResourceAuthorizationPolicyProvider>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ResourceApplicationModelProvider>());
            */
            #endregion

            #region 添加页面安全验证

            services.AddAntiforgery(option =>
            {
                option.Cookie.Name = "GALAXY-CSRF-COOKIE";
                option.FormFieldName = "GalaxyFieldName";
                option.HeaderName = "GALAXY-CSRF-HEADER";
            });

            #endregion

            #region 添加MVC支持

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            #endregion

            #region 配置Abp和依赖注入

            return services.AddAbp<GalaxyWebModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
            });

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            #region 使用Abp

            app.UseAbp(); //Initializes ABP framework.
            
            #endregion

            #region 使用Session

            app.UseSession();

            #endregion

            #region 使用授权

            app.UseAuthentication();

            #endregion

            #region 开发环境&生产环境

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            #endregion

            #region 添加初始化自定义类DI，便于直接使用IHostEnvironment

            app.UseWkMvcDI();

            #endregion

            #region 使用静态文件

            app.UseStaticFiles();

            #endregion

            #region 使用CORS跨域API访问

            app.UseCors(option =>
                option.WithOrigins("http://www.example.com")
                .AllowAnyHeader()
            );

            #endregion

            #region 使用SwaggerUI

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Galaxy API V1");
            });

            #endregion

            #region 使用MVC路由

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            #endregion
        }
    }
}
