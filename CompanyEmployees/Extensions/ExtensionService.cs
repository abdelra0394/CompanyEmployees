using System.Runtime.CompilerServices;

namespace CompanyEmployees.Extenstions
{
    public static class ExtensionService
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
    }
}
