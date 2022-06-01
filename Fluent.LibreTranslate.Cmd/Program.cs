namespace Fluent.LibreTranslate.Cmd;

internal class Program
{
    private static async Task Main(string[] args)
    {
        GlobalLibreTranslateSettings.Server = LibreTranslateServer.Libretranslate_de;

        if (args.Any())
        {
            Console.WriteLine(await string.Join(" ", args).TranslateAsync(LanguageCode.Finnish));
            return;
        }

        Console.WriteLine(await "Hello, World!".TranslateAsync(LanguageCode.Finnish));
    }
}