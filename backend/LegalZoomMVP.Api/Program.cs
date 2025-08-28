using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Application.Services;
using LegalZoomMVP.Infrastructure.Data;
using LegalZoomMVP.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
        builder.Services.AddScoped<LegalZoomMVP.Infrastructure.Services.PowerOfAttorneyRepository>();
        builder.Services.AddScoped<LegalZoomMVP.Application.Services.PowerOfAttorneyService>();

builder.Services.AddAuthorization();

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAIAssistantService, AIAssistantService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IPowerOfAttorneyService, PowerOfAttorneyService>();
builder.Services.AddScoped<IAdvocateService, AdvocateService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IAdvocateRepository, AdvocateRepository>();
builder.Services.AddScoped<IPowerOfAttorneyRepository, PowerOfAttorneyRepository>();
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<IJwtConfiguration>(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<JwtConfiguration>>().Value);
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IStripeService, StripeService>();

// External Services
builder.Services.AddHttpClient<OpenAIService>();
builder.Services.AddScoped<OpenAIService>();

// Configure Stripe
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:3004")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

// Add controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "LegalZoom MVP API", 
        Version = "v1",
        Description = "A comprehensive legal document management and AI assistant platform"
    });
    
    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
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
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LegalZoom MVP API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

// Ensure CORS is applied before HTTPS redirection and authentication/authorization
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.EnsureCreatedAsync();
}

await app.RunAsync();