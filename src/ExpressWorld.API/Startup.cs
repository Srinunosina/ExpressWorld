using AutoMapper;
using ExpressWorld.Application.Handlers;
using ExpressWorld.Application.Repositories;
using ExpressWorld.Application.Services;
using ExpressWorld.Infrastructure.Repositories;
using ExpressWorld.Shared.Adapters;
using ExpressWorld.Shared.Configurations;
using ExpressWorld.Shared.Factories;
using ExpressWorld.Shared.Mappings;
using MediatR;
using System.Reflection;

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
            services.AddAutoMapper(typeof(AutoMapperProfile));
            //services.AddMediatR(cfg =>
            //{
            //    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //});

            services.AddMediatR(typeof(SearchProductsQueryHandler).Assembly);

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            //services.AddTransient<CSVSupplierAdapter>();
            //services.AddTransient<HttpApiSupplierAdapter>();

            var supplierConfigs = Configuration.GetSection("Suppliers").Get<List<SupplierConfig>>();
            services.AddSingleton(supplierConfigs);

            services.AddTransient<AdapterFactory>();

            // Register each supplier adapter based on configuration
            //foreach (var supplier in supplierConfigs)
            //{
            //    RegisterSupplierAdapter(services, supplier);
            //}


            //   services.AddTransient<JSONSupplierAdapter>();

            //services.AddDbContext<ProductDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("ProductDatabase")));

            //services.AddScoped<SqlServerAdapter>();
            //services.AddSingleton<SupplierFactory>();
            //services.AddSingleton<DataSourceManager>();

            // Add services to the container.
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Apply pending migrations and update the database
            // ApplyDBMigrations(app);

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

        private void RegisterSupplierAdapter(IServiceCollection services, SupplierConfig supplierConfig)
        {
            var assembly = Assembly.Load("ExpressWorld.Shared");
            var dtoType = assembly.GetType(supplierConfig.DtoType);
            // Resolve the DTO type from the configuration string
            //var dtoType = Type.GetType(supplierConfig.DtoType);
            if (dtoType == null)
            {
                throw new InvalidOperationException($"DTO type '{supplierConfig.DtoType}' could not be found.");
            }

            // Register the adapter as IProductAdapter with the resolved DTO type
            var adapterType = typeof(JSONSupplierAdapter<>).MakeGenericType(dtoType);

            // Use a factory to supply the file path and inject IMapper
            services.AddTransient(typeof(IProductAdapter), serviceProvider =>
            {
                var mapper = serviceProvider.GetRequiredService<IMapper>();
                return Activator.CreateInstance(adapterType, supplierConfig.SourcePath, mapper);
            });
        }

    }
}
