using app.domain.Abstract;
using app.domain.Data.Utils.Configure;
using app.domain.Services;
using BusinesDAL.Abstract;
using BusinesDAL.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddScoped<IFacultyBusinessService, FacultyBusinessService>();
        builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();

        builder.Services.AddScoped<ISpecialityBusinessService, SpecialityBusinessService>();
        builder.Services.AddScoped<ISpecialityRepository, SpecialityRepository>();

        DatabaseConfigure.Build(builder.Services);
        builder.Services.AddSwaggerGen();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "app.web.view v1"));
        }

        //app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
        });
        app.Run();
    }
}