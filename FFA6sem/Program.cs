using DAL.Entities;
using DAL.Interfaces;
using DAL.Repository;
using FFA6sem.Data;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Services;
using FFA6sem.Model.Util;
using FFA6sem.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ninject;
using Serilog.Events;
using Serilog;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

var loggerConfiguration = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, buffered: false);

Log.Logger = loggerConfiguration.CreateLogger();

builder.Logging.AddSerilog();

// Add services to the container.

// внедрить зависимости
var kernel = new StandardKernel(new NinjectRegistrations(), new ServiceModule(builder.Configuration));
IDbCrud crudService                            = kernel.Get<IDbCrud>();
IBudgetService budgetService                   = kernel.Get<IBudgetService>();
IExpenseService expenseService                 = kernel.Get<IExpenseService>();
IExpenseTypeService expenseTypeService         = kernel.Get<IExpenseTypeService>();
IIncomeService incomeService                   = kernel.Get<IIncomeService>();
IIncomeTypeService incomeTypeService           = kernel.Get<IIncomeTypeService>();
IPlannedExpensesService plannedExpensesService = kernel.Get<IPlannedExpensesService>();
IPlannedIncomesService plannedIncomesService   = kernel.Get<IPlannedIncomesService>();
IReportService reportService                   = kernel.Get<IReportService>();
ITimePeriodService timePeriodService           = kernel.Get<ITimePeriodService>();
IUserService userService                       = kernel.Get<IUserService>();
IDbRepos dbRepos                               = kernel.Get<IDbRepos>();

builder.Services.AddScoped<IDbRepos, DbRepos>();
builder.Services.AddScoped<IDbCrud, DbDataOperation>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IExpenseTypeService, ExpenseTypeService>();
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<IIncomeTypeService, IncomeTypeService>();
builder.Services.AddScoped<IPlannedExpensesService, PlannedExpensesService>();
builder.Services.AddScoped<IPlannedIncomesService, PlannedIncomesService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ITimePeriodService, TimePeriodService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<FFAContext>();

builder.Services.AddDbContext<FFAContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddControllersAsServices();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "FFA6Sem";
    options.LoginPath = "/";
    options.AccessDeniedPath = "/";
    options.LogoutPath = "/";
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
    // ¬озвращать 401 при вызове недоступных методов дл€ роли
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await RolesSeed.CreateUserRoles(scope.ServiceProvider);
    
    var ffacontext = scope.ServiceProvider.GetRequiredService<FFAContext>();
    await UsersSeed.UsersSeedAsync(ffacontext, scope.ServiceProvider);

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
