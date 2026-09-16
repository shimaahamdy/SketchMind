using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SketchMind.Infrastructure.Data;
using SketchMind.Infrastructure.Identity;


namespace SketchMind.Infrastructure.DependencyInjection
{
    // create class that get services registeed in infrastructure layer so API no nothing about conext, identity and any other details 
    // api only know there is method called AddInfrastructureServices that will register all services in infrastructure layer
    public static class InfrastructureServiceRegistration
    {
        // extension method that will be called in api by servicesCollection to register services in it
        // configuraiton to read info from appsettings.json file
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // register DbContext using sql server
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // register identity services 
            // core as we don't need UI just mandatory users , roles data and password configuration
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<IdentityRole<int>>()        
                .AddEntityFrameworkStores<AppDbContext>(); // identiy will use appDbcontext to store on database
   



            return services;
        }
    }
}
