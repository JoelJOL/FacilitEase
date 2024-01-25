using FacilitEase.Data;
using FacilitEase.Repositories;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;
using FacilitEase.Repositories;
using FacilitEase.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DB_STRING")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

builder.Services.AddScoped<IL3AdminService, L3AdminService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularDev");

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseCors("AllowAngularDev");

app.MapControllers();

app.Run();
