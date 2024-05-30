

using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(c =>
				{
					c.Cookie.Name = "EpeyUserCookie";
					c.LoginPath = "/Account/Login";
					c.LogoutPath = "/Account/Logout";
					c.AccessDeniedPath = "/Account/AccessDenied";
					c.ExpireTimeSpan = TimeSpan.FromSeconds(200);
				});


builder.Services.AddPersistenceServices();


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

app.UseAuthorization();


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
