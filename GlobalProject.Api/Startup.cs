using Autofac;
using GlobalProject.Api.Middleware;
using GlobalProject.Infrastructure;
using GlobalProject.Infrastructure.Log;
using GlobalProject.Services.IService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Web;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Utilities.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GlobalProject.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var build = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile("skyapm.json")
             .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
             .AddEnvironmentVariables();
            Configuration = build.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //忽略Model中为null的属性
                //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                //设置本地时间而非UTC时间
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            }).AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null); ;
            services.AddSkyApmExtensions();
            //services.AddSkyAPM(ext => ext.AddAspNetCoreHosting());            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GlobalProject.Api", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                string path = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(path);
                string modelPath = Path.Combine(AppContext.BaseDirectory, "GlobalProject.Model.xml");
                c.IncludeXmlComments(modelPath);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });            
            //阿里云
            var ossConfig = Configuration.GetSection("AliyunOssConfig");
            services.AddAliyunOss(options =>
            {
                options.AccessKey = ossConfig["AccessKey"];
                options.AccessSecret = ossConfig["AccessSecret"];
                options.Bucket = ossConfig["Bucket"];
                options.EndPoint = ossConfig["EndPoint"];
                options.UrlForView = ossConfig["UrlForView"];
            });
            EngineContext.initialize(new GeneralEngine(services.BuildServiceProvider()));
            services.AddCors(c =>
            {
                c.AddPolicy("GlobalProject", option => option
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST")
                );
            });
            services.AddSmsService(Configuration);
            // 关闭netcore自动处理参数校验机制   
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            Assembly serviceInterface = Assembly.Load("GlobalProject.Services");
            //接口层所在程序集命名空间
            Assembly service = Assembly.Load("GlobalProject.Services");
            builder.RegisterAssemblyTypes(service, serviceInterface)
              //.Where(p => p.Name.EndsWith("Service"))
              .AsImplementedInterfaces()
             //.InstancePerDependency()
             .InstancePerLifetimeScope();
            Assembly repositoryInterface = Assembly.Load("GlobalProject.Repository");
            //接口层所在程序集命名空间
            Assembly repository = Assembly.Load("GlobalProject.Repository");
            builder.RegisterAssemblyTypes(repository, repositoryInterface)
              //.Where(p => p.Name.EndsWith("Repository"))
              .AsImplementedInterfaces()
             .InstancePerLifetimeScope();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                var tableService=app.ApplicationServices.GetService<ITableService>();
                string contentRootPath = env.ContentRootPath;
                var basePath=contentRootPath.Replace("GlobalProject.Api", "");
                tableService.CreateModelByTable($"{basePath}GlobalProject.Repository\\Model", "GlobalProject.Repository.Model");
            }
            //app.UseExceptionHandler();
            app.UseNlog("nlog.config");
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GlobalProject.Api v1"));
            app.UseCors("GlobalProject");           
            //app.UseGolobalException();
            //NLogBuilder.ConfigureNLog("nlog.config");            
            app.UseHttpsRedirection();
            app.UseRouting();            
            app.UseAuthorization();            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
