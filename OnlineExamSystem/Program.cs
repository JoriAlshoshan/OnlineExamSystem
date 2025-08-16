using FluentAssertions.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Data;
using OnlineExamSystem.Models;
using OnlineExamSystem.Services;
using OnlineExamSystem.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IExamService, ExamService>();
builder.Services.AddTransient<IQnAService, QnAService>();
builder.Services.AddTransient<IAccountService, AccountService>();

builder.Services.AddIdentity<UsersApp, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();
await SeedService.SeedDatabase(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Welcome}/{id?}");

app.Run();
