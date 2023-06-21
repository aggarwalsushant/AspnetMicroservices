using Catalog.API.Data;
using Catalog.API.Repositories;

namespace Catalog.API;

public class Startup
{
  public IServiceCollection RegisterServices(IServiceCollection services)
  {
    services.AddControllers();
    services.AddSwaggerGen();


    services.AddScoped<ICatalogContext, CatalogContext>();
    services.AddScoped<IProductRepository, ProductRepository>();

    return services;
  }

  public void SetupMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
  {
    // Configure the HTTP request pipeline.
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
    }

    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
  }
}
