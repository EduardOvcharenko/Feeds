using Feeds.Data;
using Feeds.JwtToken;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;


namespace Feeds
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

            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")));

            AddJwtServices(services);

            services.AddMemoryCache();
            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<IFeedRepository, FeedRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<ICacheRefresher, CacheRefresher>();

            services.AddMvc()
                 .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                 .AddJsonOptions(options => {
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Feeds API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Feeds API V1");
            });
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

        }

        private void AddJwtServices(IServiceCollection services)
        {
            var signingKey = new SigningSymmetricKey(Configuration["SigningSecurityKey"]);
            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

            var encryptionEncodingKey = new EncryptingSymmetricKey(Configuration["EncodingSecurityKey"]);
            services.AddSingleton<IJwtEncryptingEncodingKey>(encryptionEncodingKey);

            IJwtSigningDecodingKey signingDecodingKey = signingKey;
            IJwtEncryptingDecodingKey encryptingDecodingKey = encryptionEncodingKey;
            services
                .AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = Configuration["JwtSchemeName"];
                    options.DefaultChallengeScheme = Configuration["JwtSchemeName"];
                })
                .AddJwtBearer(Configuration["JwtSchemeName"], jwtBearerOptions => {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingDecodingKey.GetKey(),
                        TokenDecryptionKey = encryptingDecodingKey.GetKey(),

                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JwtIssuer"],

                        ValidateAudience = true,
                        ValidAudience = Configuration["JwtAudience"],

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });
        }
    }
}
