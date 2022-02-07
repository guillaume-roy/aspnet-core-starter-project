using System.Reflection;

using MediatR;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using ServerApi.Authentication;
using ServerApi.Filters;

using ServerCore.Configurations;
using ServerCore.Services;

using ServerInfrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);
var enableSwagger = builder.Configuration.GetValue<bool>("EnableSwagger");

{
    builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
    builder.Services.AddScoped(typeof(IUserSession), typeof(UserSession));
    builder.Services.AddScoped(typeof(CookieService));
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());
    builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = builder.Configuration.GetValue<string>("Authentication:Cookie:Name");
            options.ExpireTimeSpan = TimeSpan.FromDays(builder.Configuration.GetValue<int>("Authentication:Cookie:ExpirationDays"));
            options.SlidingExpiration = true;
            options.Cookie.Domain = builder.Configuration.GetValue<string>("Authentication:Cookie:Domain");
            options.Cookie.Path = "/api";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.Cookie.IsEssential = true;
            options.EventsType = typeof(CookieEventHandler);
        });

    builder.Services.AddScoped<CookieEventHandler>();

    builder.Services.AddLogging(config =>
    {
        config.ClearProviders();

        config.AddConfiguration(builder.Configuration.GetSection("Logging"));
        config.AddDebug();
        config.AddEventSourceLogger();

        if (builder.Environment.IsDevelopment())
        {
            config.AddConsole();
        }
    });
    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.InstrumentationKey = builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString");
        options.DeveloperMode = builder.Environment.IsDevelopment();
        options.EnableDebugLogger = builder.Environment.IsDevelopment();
    });

    if (enableSwagger)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    builder.Services.AddCore();
    builder.Services.AddInfrastructure(builder.Configuration);
}

var app = builder.Build();

{
    if (enableSwagger)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseAuthentication();
    app.UseAuthorization();

    app.Run();
}
