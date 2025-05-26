using System.Globalization;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using WhmCalcMaui.Services;
using WhmCalcMaui.Services.Calculations;
using WhmCalcMaui.ViewModels;
using WhmCalcMaui.Views;

namespace WhmCalcMaui
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
                }).RegisterServices().RegisterViewModels().RegisterViews();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // фикс краша на дроиде при использовании конвертора https://github.com/dotnet/maui/issues/25728
#if ANDROID
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("DisableEmojiCompat", (handler, view) =>
                {
                    handler.PlatformView.EmojiCompatEnabled = false;
                }); 
#endif

            return builder.Build();
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            //mauiAppBuilder.Services.AddSingleton<>();
            //mauiAppBuilder.Services.AddTransient<>();
            mauiAppBuilder.Services.AddSingleton<ModListService>();
            mauiAppBuilder.Services.AddSingleton<CalcOutputService>();
            mauiAppBuilder.Services.AddSingleton<IDataAccessService, DataAccessService>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainViewModel>();
            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainView>();
            return mauiAppBuilder;
        }
    }
}
