using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TicTacToeTest.Data;

namespace TicTacToeTest
{
    public class Startup
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb; Database=TicTacToeStorage; Trusted_Connection=True;";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TicTacToeDbContext>(options => options.UseSqlServer(ConnectionString));
            services.AddScoped<IDataStore, GameDataStore>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
