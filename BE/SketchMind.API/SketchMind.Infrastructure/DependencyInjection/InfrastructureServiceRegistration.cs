using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SketchMind.Application.Contracts.AI;
using SketchMind.Application.Repos;
using SketchMind.Infrastructure.AI.Embedding;
using SketchMind.Infrastructure.AI.Embedding.DTOs;
using SketchMind.Infrastructure.Data;
using SketchMind.Infrastructure.Data.Repos;
using SketchMind.Infrastructure.Data.VectorStore.Mongo;
using SketchMind.Infrastructure.Identity;
using System.Net.Http.Headers;



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


            AddMongoConfiguration(services, configuration);

            AddHuggingFaceConfiguration(services, configuration);

            AddHttpClientConfigurationForHuggingFace(services, configuration);


            services.AddScoped<IMaterialRepository, MaterialRepository>();

            return services;
        }

        private static IServiceCollection AddMongoConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbOptions>(configuration.GetSection("MongoDB"));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var options = sp
                    .GetRequiredService<IOptions<MongoDbOptions>>()
                    .Value;

                return new MongoClient(options.ConnectionString);
            });

            services.AddSingleton<IMongoDatabase>(sp =>
            {
                var options = sp
                    .GetRequiredService<IOptions<MongoDbOptions>>()
                    .Value;

                var client = sp.GetRequiredService<IMongoClient>();

                return client.GetDatabase(options.DatabaseName);
            });


            services.AddScoped<IVectorStore, MongoVectorStore>();

            return services;

        }

        private static IServiceCollection AddHuggingFaceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HuggingFaceOptions>(configuration.GetSection("HuggingFace"));
            return services;

        }

        private static IServiceCollection AddHttpClientConfigurationForHuggingFace(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IEmbeddingGenerator, HuggingFaceEmbeddingGenerator>((sp, client) =>
            {
                var options = sp
           .GetRequiredService<IOptions<HuggingFaceOptions>>()
           .Value;
                
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", options.ApiKey);
            });

            return services;
        }
       



    }
}
