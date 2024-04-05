using Microsoft.AspNetCore.Authentication.Cookies;

namespace App
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      // 1. Create the web application builder
      var builder = WebApplication.CreateBuilder(args);

      // 2. Add services (dependencies) to the application
      builder.Services.AddControllersWithViews();
      builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
      // builder.Services.AddHttpContextAccessor();

      // 3. Establish the database connection and create the database context.
      var database = new DatabaseService(
        builder.Configuration.GetSection("MongoDB").Get<DatabaseSettings>()!
      );
      await database.InitAsync();

      builder.Services.AddSingleton(database);
      builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      // 4. Build the web application
      var app = builder.Build();

      // 5. Configure the HTTP request pipeline (middleware)

      // 5a. Error Handling (only in non-development environments)
      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/Home/Error"); // Redirects to a generic error page
      }

      // 5b. Serving Static Files
      app.UseStaticFiles(); // Enables serving files like CSS, JavaScript, and images

      // 5c. Routing
      app.UseRouting(); // Adds middleware to handle URL routing 

      // 5d. Authentication/Authorization
      app.UseAuthentication();
      app.UseAuthorization();

      // 5e. Defines the default routing pattern
      app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

      // 6. Start the web application and listen for requests
      app.Run();
    }
  }
}