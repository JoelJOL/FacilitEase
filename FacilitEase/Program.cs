using FacilitEase.Data;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FacilitEase.Models.EntityModels;
using FacilitEase.Hubs;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using FacilitEase.NewFolder4;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
           .AddJsonOptions(options =>
           {
               options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
           });

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();
var jwtAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
    options.AddPolicy("Manager", policy => policy.RequireRole("Manager"));
    options.AddPolicy("DepartmentHead", policy => policy.RequireRole("DepartmentHead"));
    options.AddPolicy("L1Admin", policy => policy.RequireRole("L1Admin"));
    options.AddPolicy("L2Admin", policy => policy.RequireRole("L2Admin"));
    options.AddPolicy("L3Admin", policy => policy.RequireRole("L3Admin"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtAudience,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeDetailRepository, EmployeeDetailRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IManagerService, ManagerService>();
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
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IRepository<TBL_TICKET>, Repository<TBL_TICKET>>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ITicketDetailsService, TicketDetailsService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IL1AdminService, L1AdminService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddScoped<MailJetService>();
builder.Services.AddHostedService<EscalationHostedService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); ;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

//configuring the http request for signal R
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAngularDev");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub"); // Map the NotificationHub
});

app.UseCors("CorsPolicy");
app.UseMiddleware<LogMiddleware>();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});
  
app.UseCors("AllowLocalhost");
  
app.UseHttpsRedirection();


app.UseAuthorization();


app.MapControllers();

app.Run();