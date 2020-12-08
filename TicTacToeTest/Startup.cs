using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeTest.Data;

namespace TicTacToeTest
{
    public class Startup
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb; Database=TicTacToeStorage; Trusted_Connection=True;";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TicTacToeStorage>(options => options.UseSqlServer(ConnectionString));
            services.AddScoped<IRepository, GameRepository>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
