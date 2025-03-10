using Application;
using Application.AutoMapper;
using DAL.SqlServer;// Contains AddSqlServerServices extension and UnitOfWork
using MediatR;
using Microsoft.OpenApi.Models;
using RestaurantManagement.Middlewares;
using RestaurantManagement.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Please enter valid token here ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
            new List<string>()
          }
        });
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(typeof(Application.CQRS.Users.Handlers.Register.Handler).Assembly);
builder.Services.AddAuthenticationService(builder.Configuration);
//builder.Services.ConfigureCors();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var conn = builder.Configuration.GetConnectionString("myconn");
builder.Services.AddSqlServerServices(conn);
builder.Services.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowCors");
app.UseMiddleware<ExceptionHandlerMiddleware>();
//app.UseMiddleware<RateLimitMiddleware>(2 , TimeSpan.FromMinutes(1));

app.MapControllers();

app.Run();