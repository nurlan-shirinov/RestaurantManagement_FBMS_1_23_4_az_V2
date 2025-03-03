using Application;
using Application.AutoMapper;
using DAL.SqlServer;// Contains AddSqlServerServices extension and UnitOfWork
using MediatR;
using RestaurantManagement.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(typeof(Application.CQRS.Users.Handlers.Register.Handler).Assembly);

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var conn = builder.Configuration.GetConnectionString("myconn");
builder.Services.AddSqlServerServices(conn);

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<RateLimitMiddleware>(2 , TimeSpan.FromMinutes(1));

app.MapControllers();

app.Run();

