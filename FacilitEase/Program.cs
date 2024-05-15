using FacilitEase.ServiceRegistry;
using FacilitEase.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.AddConnectionString();

builder.AddApplicationAuthentication();

builder.AddCommonServices();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.AddServices();

var app = builder.Build();

app.AddMiddlewares();

app.Run();