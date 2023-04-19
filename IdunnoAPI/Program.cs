using IdunnoAPI.DAL;
using IdunnoAPI.Extensions;
using Microsoft.EntityFrameworkCore;

namespace IdunnoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuth(builder.Configuration);
            builder.Services.AddControllers();

            builder.Services.AddDbContext<IdunnoDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddServices();

            if (builder.Environment.IsDevelopment()) // allowing Idunno project to access API in development.
            {
                builder.Services.ConfigureCors();
            }

            var app = builder.Build();

            app.Configure();

            app.Run();
        }
    }
}