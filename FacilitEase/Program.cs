using FacilitEase.Data;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;
using FacilitEase.Repositories;
using FacilitEase.Services;
using Microsoft.EntityFrameworkCore;
using FacilitEase.Models.EntityModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Startup.cs
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IPriorityService, PriorityService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IL3AdminService, L3AdminService>();
builder.Services.AddScoped<IRepository<TBL_TICKET>, Repository<TBL_TICKET>>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ITicketDetailsService, TicketDetailsService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IL1AdminService, L1AdminService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); ;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
