using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyAssistant.Core.Data.Context;
using MyAssistant.Core.Data.Entities.Auth;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddLogging();
//builder.Services.AddHttpLogging(o => { });
builder.Services.AddDbContext<MyAssistant.Core.Data.Context.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDb"),
    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MyAssistant • Core",
    });
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    { 
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "Enter the Bearer Authorization string as following: `Generated-JWT-Token`",
        Type = SecuritySchemeType.Http,
        // or
        // Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        //Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
 {
     options.SaveToken = true;
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
         ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
         ClockSkew = TimeSpan.Zero,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]!))
     };
 });

//builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
//{
//    policy
//    .WithOrigins(builder.Configuration.GetSection("AllowedHosts").Get<string[]>()!)
//    .AllowAnyHeader()
//    .AllowAnyMethod()
//    .AllowCredentials();
//}));


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MyAssistant.Core.Service.Ping).Assembly));

var app = builder.Build();


if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "MyAssistant • Core");
        c.OAuthAppName("Movie Manager API");
    });
}


//app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
//app.UseCors("CorsPolicy");
app.MapControllers();

foreach(var service in builder.Services)
{
    Console.WriteLine($"{service.Lifetime} | {service.ServiceType}");
}

app.Run();