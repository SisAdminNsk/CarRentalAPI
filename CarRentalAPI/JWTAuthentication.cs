using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarRentalAPI
{
    public static class AuthenticationExtentions
    {
        public static void AddApiAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {

            var key = Encoding.ASCII.GetBytes(configuration.GetRequiredSection("jwtoptions:SecretKey").Value);

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
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["auth_cookies"];

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(

                options =>
                {
                    options.AddPolicy("admin",
                        policy =>
                        {
                            policy.RequireRole("admin");

                        });

                    options.AddPolicy("user",
                        policy =>
                        {
                            policy.RequireRole("admin", "user");

                        });
                }
             );
        }
    }
}
