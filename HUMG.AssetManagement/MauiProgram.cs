using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microcharts.Maui;
using Microsoft.Extensions.Logging.Debug;

namespace HUMG.AssetManagement
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Logging.AddDebug();


            return builder.Build();
        }
    }
}