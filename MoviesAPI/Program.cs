using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesAPI;
using MoviesAPI.Services;
using MoviesAPI.Utilities;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
});


var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins")!.Split(","); //if we have multiple urls

builder.Services.AddCors(options=>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy
            .WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("total-records-count");
        });

    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer("name=DefaultConnection",
sqlServer => sqlServer.UseNetTopologySuite()));

builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

builder.Services.AddSingleton(provider => new MapperConfiguration(config =>
{
    var geometryFactory = provider.GetRequiredService<GeometryFactory>();
    config.AddProfile(new AutoMapperProfiles(geometryFactory));
}).CreateMapper());

builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.AddTransient<IFileStorage, AzureFileStorage>();

builder.Services.AddTransient<IFileStorage,LocalFileStorage>();

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();
//if (app.Environment.IsDevelopment())
//{
//  app.UseSwagger();
 // app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

app.Run();
