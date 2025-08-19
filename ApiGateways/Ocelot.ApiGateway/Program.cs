using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

//Ocelot configuration
builder.Host.ConfigureAppConfiguration((evn, config) =>
{
    config.AddJsonFile($"ocelot.{evn.HostingEnvironment.EnvironmentName}.json", true, true);
});

builder.Services.AddControllers();

var authScheme = "SportShopGatewayAuthScheme";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(authScheme, options =>
    // .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8009";
        //to communicate between other containers with identity server container
        if (builder.Environment.IsDevelopment())
            options.MetadataAddress = "http://identityserver:8080/.well-known/openid-configuration";
        options.Audience = "SportShopGateway";
        options.RequireHttpsMetadata = false;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello Ocelot");
            });
        });

await app.UseOcelot();
await app.RunAsync();