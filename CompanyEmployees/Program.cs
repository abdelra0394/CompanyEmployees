using CompanyEmployees.Extensions;
using CompanyEmployees.Extenstions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

namespace CompanyEmployees
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            // Add services to the container.
            builder.Services.ConfigureCors();
            builder.Services.CongfigureIISIntegration();
            builder.Services.ConfigureLoggerService();
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.AddControllers(
                config =>
                {
                    config.RespectBrowserAcceptHeader = true;
                    config.ReturnHttpNotAcceptable = true;
                }).AddXmlSerializerFormatters()
                .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);
            builder.Services.AddAutoMapper(typeof(Program));
            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILoggerManager>();
            app.ConfigureExceptionHandler(logger);

            if (app.Environment.IsProduction())
                app.UseHsts();
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseCors("CorsPolicy");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
