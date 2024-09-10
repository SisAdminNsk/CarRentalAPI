using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarRentalAPI
{
    public static class AuthenticationExtentions
    {
        public static void AddApiAuthenticationAndAuthorization(this IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes("77f3cfde-e19b-4ed1-8cf8-df3eaa49be44");

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
