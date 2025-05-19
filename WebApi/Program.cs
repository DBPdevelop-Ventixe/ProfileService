
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ProfileService>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProfileConnection"));
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});
app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<ProfileService>();
app.MapGet("/", () => "gRPC profile server is running.");
app.MapControllers();

app.Run();
