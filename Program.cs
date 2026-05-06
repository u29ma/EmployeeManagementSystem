using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AccountDa>();
builder.Services.AddScoped<AnnouncementDa>();
builder.Services.AddScoped<AttendanceDa>();
builder.Services.AddScoped<DashboardDa>();
builder.Services.AddScoped<DepartmentDa>();
builder.Services.AddScoped<EmployeeDa>();
builder.Services.AddScoped<LeaveManagementDa>();
builder.Services.AddScoped<PayrollDa>();
builder.Services.AddScoped<ReportsDa>();

builder.Services.AddDistributedMemoryCache(); // required
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

//RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

//builder.Services.AddAuthentication("CookieAuth")
//    .AddCookie("CookieAuth", options =>
//    {
//        options.LoginPath = "/Account/Login";
//        options.AccessDeniedPath = "/Account/AccessDenied";
//    });

//builder.Services.AddAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
