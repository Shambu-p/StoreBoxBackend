using StoreBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
