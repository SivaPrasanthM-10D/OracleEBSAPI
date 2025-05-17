using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OracleEBSAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAd", options);

        options.TokenValidationParameters.RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception is SecurityTokenExpiredException)
                {
                    throw new UnauthorizedAccessException("Token has expired. Please log in again.");
                }
                else if (context.Exception is SecurityTokenInvalidSignatureException)
                {
                    throw new UnauthorizedAccessException("Invalid token signature. Please check your token.");
                }
                else if (context.Exception is SecurityTokenException)
                {
                    throw new UnauthorizedAccessException("Invalid token. Please provide a valid token.");
                }

                throw new UnauthorizedAccessException("Authentication failed. Please check your credentials.");
            },
            OnChallenge = context =>
            {
                throw new UnauthorizedAccessException("You are not authenticated. Please log in.");
            },
            OnForbidden = context =>
            {
                throw new UnauthorizedAccessException("You are not authorized to access this resource.");
            }
        };
    },
    options => builder.Configuration.Bind("AzureAd", options));


builder.Services.AddControllers(options =>
{
    // Remove JSON formatters if you want XML only
    options.InputFormatters.Clear();
    options.OutputFormatters.Clear();
})
.AddXmlSerializerFormatters();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OracleEBS API",
        Version = "v1",
        Description = "OracleEBS API Gateway for managing employee-related operations",
    });

    // Add security definition for Azure AD JWT Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your Azure AD token."
    });

    // Add security requirement for all endpoints
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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
