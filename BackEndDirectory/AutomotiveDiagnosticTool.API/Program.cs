using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutomotiveDiagnosticTool.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);





builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddAuthentication(options =>
{   
    
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    
}).AddJwtBearer(options =>
 {
     var key = Encoding.UTF8.GetBytes("sshht secret key");
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = false,
         ValidateAudience = false,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
       
         IssuerSigningKey = new SymmetricSecurityKey(key)
     };
    
     options.Events = new JwtBearerEvents
     {
         OnTokenValidated = context =>
         {
             Console.WriteLine("Token Validated Successfully");
             return Task.CompletedTask;
         },
         OnAuthenticationFailed = context =>
         {
             Console.WriteLine($"Authentication Failed: {context.Exception}");
             return Task.CompletedTask;
         }
     };
 });

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Automotive Diagnostic Tool API", Version = "v1" });
    // Include the JWT authentication configuration in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication();

builder.Services.AddEndpointsApiExplorer();




var app = builder.Build();


app.UseCors("AllowAll");
app.MapGet("/generateToken", () =>
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes("sshht secret key");
    var tokenDescriptor = new SecurityTokenDescriptor()
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "Admin"),
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey (key),SecurityAlgorithms.HmacSha256),
    };
    var token = tokenHandler.CreateToken (tokenDescriptor);
    var tokenString = tokenHandler.WriteToken(token);

    return Results.Text(tokenString);
});




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
  
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Automotive Diagnostic Tool API v1");
    });
}
app.UseCors("AllowAll"); 


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


