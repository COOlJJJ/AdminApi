using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace AdminApi
{
    public class Program
    {
        /// <summary>
        /// 基础的用户管理和分配角色 以及token登录&Docker容器化&Nginx反向代理
        /// 1.dockerfile 直接添加 但是因为解决方案和项目不在一个目录 要copy到上一层
        /// 2.docker build -t adminapi . 构建镜像
        /// 3.docker save adminapi -o d:/api.tar 导出镜像
        /// 4.docker load --input d:/api.tar 导入镜像
        /// 5.docker run -it -d --rm -p 8081:80 --name api1 adminapi 运行容器
        /// 6.反向代理 负载均衡
        /// upstream webServer{
        //    server 127.0.0.1:5011;
        //    server 127.0.0.1:5012;
        //    server 127.0.0.1:5013;
        //}

        //    server {
        //    listen       5010;
        //    server_name localhost;
        //    location / {
        //        proxy_pass http://webServer;
        //    }
        //}
        //
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Create the Serilog logger, and configure the sinks
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .Enrich.FromLogContext()
               .WriteTo.Console()
               .WriteTo.File("./log/log.txt", LogEventLevel.Information)
               .CreateLogger();
            // Wrap creating and running the host in a try-catch block
            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

