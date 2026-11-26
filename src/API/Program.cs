using System.Text;
using Zin.API.Filter;
using Zin.API.Secret;
using Zin.Infra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
public partial class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddGrpc();
        builder.Services.AddScoped<JWTSecret>();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddExceptionHandler<ExceptionFilter>();

        builder.Services.AddOpenApi(
            options =>
            {
                options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                    {
                        document.Components ??= new OpenApiComponents(); 
                        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            Description = "Enter your JWT Access Token."
                        };
                        document.Info.Title = "Zin RMM";
                        document.Info.Version = "1.0.0";
                        return Task.CompletedTask;
                    }
                );
            }
        );
        
        builder.Services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(
            options =>
            {
                string SecretKey = builder.Configuration["Jwt:SecretKey"]!;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[JWTSecret.SessionCookie];
                        return Task.CompletedTask;
                    },
                    OnForbidden = async (context) =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new { error = "Forbidden" });
                    },
                    OnChallenge = async (context) =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            error = "Unauthorized",
                        });
                    }
                };
            }
        );
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers(
            options =>
            {
                options.Filters.Add<ResponseFilter>();
            }
        );

        builder.Services.AddProblemDetails();
        
        WebApplication app = builder.Build();

        if (!app.Environment.IsProduction())
        {
            app.MapOpenApi();
        }

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        app.MapGrpcService<GDeviceService>();

        app.MapFallback("{*path}", ExceptionFilter.NotFound);
        app.Run();
    }
}