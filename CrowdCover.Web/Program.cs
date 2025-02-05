using CrowdCover.Web.Client;
using CrowdCover.Web.Controllers;
using CrowdCover.Web.Data;
using CrowdCover.Web.Models;
using CrowdCover.Web.Models.Sharpsports;
using CrowdCover.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity configuration with default IdentityUser and IdentityRole for MVC and API
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders(); // Token provider for password resets, email confirmations, etc.

// Load the JWT key from configuration
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("JWT Key is missing in configuration.");
}

// Configure JWT authentication without re-registering cookie-based authentication
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Add CORS policy to allow any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Configure MVC views and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add SharpSportsClient and configure HttpClient
builder.Services.AddHttpClient<SharpSportsClient>(client =>
{
    client.BaseAddress = new Uri("https://api.sharpsports.io/");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler());

builder.Services.AddSingleton<IEmailSender>(new EmailSender(
       smtpServer: "smtp.gmail.com",
       smtpPort: 587,
       smtpUser: "team@crowdcover.live",  // Your email
       smtpPass: "jsio avks iouy wyky"      // Your password
   ));

builder.Services.AddScoped<SharpSportsClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(SharpSportsClient));

    // Fetch live keys from appsettings.json
    var livePublicKey = configuration["Sharpsports:LivePublicKey"];
    var livePrivateKey = configuration["Sharpsports:LivePrivateKey"];

    // Ensure the keys are not null or empty
    if (string.IsNullOrWhiteSpace(livePublicKey) || string.IsNullOrWhiteSpace(livePrivateKey))
    {
        throw new InvalidOperationException("SharpSports Live API keys are missing in the configuration.");
    }

    return new SharpSportsClient(httpClient, livePublicKey, livePrivateKey);
});

// Register other services
builder.Services.AddScoped<IBettorService, BettorService>();
builder.Services.AddScoped<BetSlipService, BetSlipService>();
builder.Services.AddScoped<BettorAccountService, BettorAccountService>();
builder.Services.AddScoped<BettorService, BettorService>();
builder.Services.AddScoped<AccountController>();


// Register RoomAccessService
builder.Services.AddScoped<RoomAccessService>();

// Add OData services for API
builder.Services.AddControllers()
    .AddOData(options =>
    {
        var odataBuilder = new ODataConventionModelBuilder();
        odataBuilder.EntitySet<DynamicDataVariable>("DynamicData");
        odataBuilder.EntitySet<StreamingRoom>("StreamingRooms");
        odataBuilder.EntitySet<Bettor>("Bettors");
        odataBuilder.EntitySet<BetSlip>("BetSlips");
        odataBuilder.EntitySet<Bet>("Bets");
        odataBuilder.EntitySet<Event>("Events");
        odataBuilder.EntitySet<BettorAccount>("BettorAccounts");
        options.AddRouteComponents("odata", odataBuilder.GetEdmModel());
    });

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CrowdCover API",
        Description = "The CrowdCover Back-end Services",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license")
        }
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMigrationsEndPoint();
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable CORS
app.UseCors("AllowAnyOrigin");

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map default controller route for MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();