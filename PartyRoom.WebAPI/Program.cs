using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PartyRoom.Infrastructure.Data;
using PartyRoom.WebAPI;
using PartyRoom.WebAPI.Extensions;
using PartyRoom.WebAPI.MappingProfiles.RoomMapping;
using PartyRoom.WebAPI.MappingProfiles.UserMapping;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Добавьте описание для авторизации JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    c.EnableAnnotations();
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("PartyRoom.Infrastructure")));

builder.Services.AddCustomIdentity();
builder.Services.AddCustomAuthentication(jwtSettings);
builder.Services.AddCustomAuthorization();
builder.Services.AddAutoMapper(typeof(UserMappingProfile), typeof(RoomMappingProfile));
builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddHostedService<RoomLogicBackgroundService>();



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PartyRoom API v1");
        c.DocumentTitle = "PartyRoom API Documentation";
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
