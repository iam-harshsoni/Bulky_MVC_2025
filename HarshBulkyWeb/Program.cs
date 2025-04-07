using HarshBulky.DataAccess.Data;
using HarshBulky.DataAccess.Repository;
using HarshBulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using HarshBulky.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure ApplicationDbContext to use SQL Server database
// Register ApplicationDbContext with SQL Server as the database provider
// Ensure the connection string is properly set in the appsettings.json file
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddRazorPages();

/*
    Have to add  .AddDefaultTokenProviders(); to prevent the error 'NotSupportedException: No IUserTwoFactorTokenProvider<TUser> named 'Default' is registered.'
    While using 'AddDefaultIdentity' its been taken care, but here we are using Custom AddIdentiy, so we will get this above error, and for that we need to add this  .AddDefaultTokenProviders(); 

    Error occured because while registering, its generating the EmailConfirmationToken and for that rewuires  .AddDefaultTokenProviders();
 
 */
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Make sure to always add this 'ConfigureApplicationCookie' after AddIdentity, otherwise it wont work.
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<IEmailSender>(new EmailSender(
    smtpHost: "smtp.gmail.com",
    smtpPort: 587,
    smtpUser: "your-email@gmail.com",
    smtpPass: "your-password" // Replace this with your Gmail password
));

// Add logging (this is usually added by default in newer templates)
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();


