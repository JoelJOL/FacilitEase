using DotNetEnv;
using FacilitEase.Contracts.RepositoryContracts;
using FacilitEase.Contracts.ServiceContracts;
using FacilitEase.Data;
using FacilitEase.Hubs;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using System.Text;

namespace FacilitEase.ServiceRegistry
{
    public static class ServiceRegistry
    {
        #region Services
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            ApplicationServices(builder.Services);
            builder.ConfigureSwagger();
            return builder;
        }
        #endregion

        #region Common Services
        /// <summary>
        /// Adding Common Services to web application
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddCommonServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSignalR();
            builder.Services.AddControllers();
            builder.Services.AddHttpClient();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
                });

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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            builder.Services.AddHostedService<EscalationHostedService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<MailJetService>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            return builder;
        }
        #endregion

        #region Swagger
        /// <summary>
        /// Configuring the Swagger
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Using the Authorization header with the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "FacilitEase API(" + environment + ")", Version = "v2.0.0" });
                options.AddSecurityDefinition("Bearer", securitySchema);
                options.OperationFilter<AuthorizationOperationFilter>();
            });


            return builder;
        }
        #endregion

        #region Application Services
        /// <summary>
        /// Dependency injection of Services
        /// </summary>
        /// <param name="services"></param>
        public static void ApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeDetailRepository, EmployeeDetailRepository>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IL3AdminService, L3AdminService>();
            services.AddScoped<IPriorityService, PriorityService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRepository<TBL_TICKET>, Repository<TBL_TICKET>>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IRepository<TBL_TICKET>, Repository<TBL_TICKET>>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ITicketDetailsService, TicketDetailsService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IL1AdminService, L1AdminService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<InvoiceService, InvoiceService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IAzureRoleManagementService, AzureRoleManagementService>();
            services.AddScoped<ISLAService, SLAService>();
            services.AddScoped<IL1AdminService, L1AdminService>();
            services.AddScoped<IEmailToTicketProcessor, EmailToTicketProcessor>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<TicketService>();
        }
        #endregion

        #region Common Middleware
        /// <summary>
        /// Configuring common middleware
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication AddMiddlewares(this WebApplication app)
        {
            string applicationAuthority = Env.GetString("Application_Authority");
            string applicationAudience = Env.GetString("Application_Audience");

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseCors("AllowAngularDev");
            app.MapControllers();
            app.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowAngularDev");
            app.UseTokenValidationMiddleware(applicationAuthority, applicationAudience);
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<LogMiddleware>();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            return app;
        }
        #endregion

        #region Application Authentication
        /// <summary>
        /// Adding Authentication
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddApplicationAuthentication(this WebApplicationBuilder builder)
        {
            Env.Load();
            var jwtKey = Env.GetString("JWT_Key");
            var jwtIssuer = Env.GetString("JWT_Issuer");
            var jwtAudience = Env.GetString("JWT_Audience");
            builder.Configuration["Jwt:Key"] = jwtKey;
            builder.Configuration["Jwt:Issuer"] = jwtIssuer;
            builder.Configuration["Jwt:Audience"] = jwtAudience;

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

            return builder;
        }
        #endregion

        #region Connection String
        /// <summary>
        /// Adding of Connection String
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddConnectionString(this WebApplicationBuilder builder)
        {
            Env.Load();

            string connectionString = Env.GetString("ConnectionStrings__DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            return builder;
        }
        #endregion
    }
}