using Microsoft.Extensions.Logging;
using BookTracker2.Data;
using BookTracker2.ViewModels;
using BookTracker2.Views;
using CommunityToolkit.Maui;

namespace BookTracker2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "booktracker.db");
            builder.Services.AddSingleton<DatabaseService>(new DatabaseService(dbPath));

            builder.Services.AddTransient<BooksViewModel>();
            builder.Services.AddTransient<BookDetailViewModel>();

            builder.Services.AddTransient<BooksPage>();
            builder.Services.AddTransient<BookDetailPage>();

            builder.Services.AddTransient<StatsViewModel>();
            builder.Services.AddTransient<StatsPage>();

            builder.Services.AddHttpClient<GoogleBooksService>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
