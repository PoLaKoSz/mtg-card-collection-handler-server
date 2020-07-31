using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MagicApi.Models;
using MagicApi.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using MagicApi.Middlewares;

namespace MagicApi
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
            services.AddDbContext<CardContext>(opt =>
            opt.UseInMemoryDatabase("CardList"));
            services.AddControllers();
            services.AddDbContext<MTGContext>(opt =>
            opt.UseNpgsql("Host=localhost;Database=MTGDatabase;Username=postgres;Password=mksm20"));
            services.AddCors(options =>
        {
            options.AddPolicy("MainPolicy",
                builder =>
                {
                    builder.WithOrigins("https://localhost:3000")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowAnyOrigin();
                    builder.WithOrigins("https://localhost:5001")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowAnyOrigin();
                });
        });
            services.AddTokenAuthentication(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
