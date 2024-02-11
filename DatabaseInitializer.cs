using Microsoft.EntityFrameworkCore;
using AppDemo.Data;

public static class DatabaseInitializer
{
    public static void InitializeDatabase(IApplicationBuilder app,IWebHostEnvironment env)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
        var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        context.Database.Migrate();
        }


    }

}