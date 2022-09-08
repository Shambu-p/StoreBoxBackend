using StoreBackend.Data;
using Microsoft.EntityFrameworkCore;
using StoreBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme{
        
        Description = "Standard Authorization scheme using Brearer Scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey

    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();

});


var _jwt_settings = builder.Configuration.GetSection("AuthSettings");
builder.Services.Configure<AuthSettings>(_jwt_settings);

var _authKey = builder.Configuration.GetValue<string>("AuthSettings:securityKey");
builder.Services.AddAuthentication(item => {
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item => {
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters() {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// builder.Services.AddDbContext<asp_dbContext>(options => {
//     options.UseMySql(builder.Configuration.GetConnectionString("my_db"),
//     Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
// });

builder.Services.AddDbContext<StoreBackendContext>(options => {
    options.UseMySql(builder.Configuration.GetConnectionString("my_db"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
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
