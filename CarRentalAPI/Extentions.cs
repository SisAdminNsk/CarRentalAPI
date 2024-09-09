namespace CarRentalAPI
{
    public static class Extentions
    {
        public static void AllowAllOrigins(this IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy("AllowAllOrigins", builder => {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
    }
}
