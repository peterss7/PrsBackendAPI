using Microsoft.EntityFrameworkCore;
using Entities;
using PrsBackendAPI6.Extensions;
using Microsoft.AspNetCore.Mvc;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureRepositoryWrapper();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<ValidationFilterAttribute>();
        builder.Services.Configure<ApiBehaviorOptions>(options
            => options.SuppressModelStateInvalidFilter = true);




        var configuration = builder.Configuration;
        var connectionString = configuration.GetConnectionString("PrsContext");

        builder.Services.AddDbContext<RepositoryContext>(options =>
            options.UseSqlServer(connectionString)
        );


        var app = builder.Build();

        


        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    $"/swagger/v1/swagger.json", "V1");
            });

        }



        app.UseHttpsRedirection();

        //app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();


        app.MapControllers();
        app.Run();
    }
}