using Microsoft.EntityFrameworkCore;
using Test_API_New.BusinessLogicLayer.Middleware;
using Test_API_New.BusinessLogicLayer.Services;
using Test_API_New.DataAccessLayer.Context;
using Test_API_New.DataAccessLayer.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreDBContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConStr"))
);
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandlingMiddleware();  // middlewre

app.MapControllers();

app.Run();
