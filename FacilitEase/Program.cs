using FacilitEase.Data;
using FacilitEase.Hubs;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;
using FacilitEase.Middleware;


var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
    });

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddHttpClient();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
                 .AllowCredentials();
    });
});
string connectionString = Env.GetString("ConnectionStrings__DefaultConnection");
var jwtKey = Env.GetString("JWT__Key");
var jwtIssuer = Env.GetString("JWT__Issuer");
var jwtAudience = Env.GetString("JWT__Audience");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));






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
builder.Services.AddScoped<IL3AdminService, L3AdminService>();
builder.Services.AddScoped<IPriorityService, PriorityService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
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
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAzureRoleManagementService, AzureRoleManagementService>();
builder.Services.AddScoped<ISLAService, SLAService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddHostedService<NotificationService>();
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddScoped<MailJetService>();
builder.Services.AddHostedService<EscalationHostedService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuring the HTTP request for SignalR
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}





/*app.UseTokenValidationMiddleware("https://login.microsoftonline.com/5b751804-232f-410d-bb2f-714e3bb466eb/v2.0", "d7104f84-ab29-436f-8f06-82fcf8d81381");
*/
app.UseRouting();
app.UseCors("AllowAngularDev");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowAngularDev"); 
});

//app.UseMiddleware<LogMiddleware>();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});


app.UseAuthorization();
app.UseHttpsRedirection();



app.Run();
