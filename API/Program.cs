using API.Helpers;
using API.Hubs;
using API.Middlewares;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services.Orders;
using Infrastucture.Data;
using Infrastucture.Services.CacheService;
using Infrastucture.Services.EmailService;
using Infrastucture.Services.SeatHoldService;
using Infrastucture.Services.TheMovieDatabase;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var scheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        };
        
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", scheme);
        
        foreach (var operation in document.Paths.Values.SelectMany(p => p.Operations))
        {
            operation.Value.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme } }] = Array.Empty<string>()
            });
        }
        return Task.CompletedTask;
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
{
    var connString = builder.Configuration.GetConnectionString("Redis");
    if (connString == null) throw new Exception("Redis connection string not found");
    var configuration = ConfigurationOptions.Parse(connString, true);
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<ICacheService, RedisService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpClient<TmdbService>(client => 
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddIdentityApiEndpoints<AppUser>()
   .AddEntityFrameworkStores<StoreContext>();
builder.Services
   .AddFluentEmail("info@cinemax.com", "Cinemax");
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISeatHoldService, SeatHoldService>();
builder.Services.AddSingleton<IShowtimeNotifier, ShowtimeNotifier>(); 
builder.Services.AddSignalR();

var mailSettings = builder.Configuration.GetSection("MailSettings");
builder.Services
   .AddFluentEmail(mailSettings["Email"], mailSettings["DisplayName"])
   .AddSmtpSender(
        mailSettings["Host"], 
        int.Parse(mailSettings["Port"]), 
        mailSettings["UserName"], 
        mailSettings["Password"]
    );

builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowCredentials()
               .WithOrigins("http://localhost:8100", "https://localhost:8100"));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGroup("api/account").MapIdentityApi<AppUser>();
app.MapControllers();
app.MapHub<ShowtimeHub>("/hubs/showtime");

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    var tmdbService = services.GetRequiredService<TmdbService>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    //await context.Database.EnsureDeletedAsync();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context, tmdbService, userManager);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

app.Run();
