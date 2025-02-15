namespace ProjectMovieBookDB;

public class Program
{
    /// <summary>
    /// Основен метод на приложението, който стартира ASP.NET Core уеб приложението.
    /// </summary>
    /// <param name="args">Аргументи от командния ред.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Добавяне на услуги в контейнера.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Конфигуриране на HTTP заявките.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // Стандартната стойност за HSTS е 30 дни. Може да искате да промените това за производствени сценарии.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}
