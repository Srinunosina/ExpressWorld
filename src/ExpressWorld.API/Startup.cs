using ExpressWorld.API.Middleware;
using ExpressWorld.Application.Handlers;
using ExpressWorld.Application.Repositories;
using ExpressWorld.Application.Services;
using ExpressWorld.Infrastructure.Repositories;
using ExpressWorld.Shared.Configurations;
using ExpressWorld.Shared.Factories;
using ExpressWorld.Shared.Mappings;
using MediatR;
using Microsoft.Extensions.Options;

namespace ExpressWorld.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IHostEnvironment _hostEnvironment;
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var supplierConfigs = Configuration.GetSection("Suppliers").Get<List<SupplierConfig>>();
            //services.AddSingleton(supplierConfigs);

            // Register the SupplierConfig to allow hot reload
            services.Configure<List<SupplierConfig>>(Configuration.GetSection("Suppliers"));

            // Register IOptionsMonitor for SupplierConfig
            services.AddSingleton<IOptionsMonitor<List<SupplierConfig>>, OptionsMonitor<List<SupplierConfig>>>();
            services.AddSingleton<AdapterFactory>();

            services.AddAutoMapper(typeof(AutoMapperProfile));            
            services.AddMediatR(typeof(SearchProductsQueryHandler).Assembly);
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            // Add services to the container.
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
       }
    }
}
