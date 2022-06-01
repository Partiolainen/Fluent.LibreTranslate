using System.Threading;

namespace Fluent.LibreTranslate;

public static class GlobalLibreTranslateSettings
{
    public static LibreTranslateServer Server = LibreTranslateServer.Libretranslate_com;
    public static string ApiKey = null;
    public static bool UseRateLimitControl = true;

    internal static SemaphoreSlim SlowDownLocker = new(1);
}