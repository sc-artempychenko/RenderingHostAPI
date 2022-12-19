using Microsoft.EntityFrameworkCore;
using POCRenderingHostAPI.Data;
using POCRenderingHostAPI.Repositories;
using POCRenderingHostAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IRenderingHostQueryRunnerService, RenderingHostQueryRunnerService>();
builder.Services.AddScoped<ISiteQueryRunnerService, SiteQueryRunnerService>();
builder.Services.AddScoped<IRenderingHostRepository, RenderingHostRepository>();
builder.Services.AddSingleton<IHostConfigurationProvider, HostConfigurationProvider>();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("azureDb"));
});

builder.Services.AddSwaggerGen();
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Management API",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("corsapp");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
