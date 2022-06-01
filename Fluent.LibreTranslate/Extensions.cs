using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluent.LibreTranslate.Models;
using Flurl;
using Flurl.Http;

namespace Fluent.LibreTranslate;

public static class Extensions
{
    private static Url BaseUrl(this string text) => GlobalLibreTranslateSettings.Server.Url
        .SetQueryParam("q", text)
        .SetQueryParam("api_key", GlobalLibreTranslateSettings.ApiKey);

    private static async Task SlowDown()
    {
        if (GlobalLibreTranslateSettings.UseRateLimitControl)
            await GlobalLibreTranslateSettings.SlowDownLocker.WaitAsync();
    }

    private static void SlowDownRelease()
    {
        if (GlobalLibreTranslateSettings.UseRateLimitControl)
            _ = Task.Run(async () =>
            {
                await Task.Delay(4000);
                GlobalLibreTranslateSettings.SlowDownLocker.Release();
            });
    }

    public static async Task<LanguageCode> DetectLanguageAsync(this string text)
    {
        try
        {
            await SlowDown();
            var detect = await text.BaseUrl()
                .AppendPathSegment("detect")
                .PostAsync()
                .ReceiveJson<IEnumerable<DetectLanguageResponse>>();
            return detect.OrderByDescending(x => x.Confidence).Select(x => x.Language).FirstOrDefault();
        }
        finally
        {
            SlowDownRelease();
        }
    }

    /// <summary>
    /// Translates the text from one language to another
    /// </summary>
    /// <param name="text">Text to translate</param>
    /// <param name="source">Source language</param>
    /// <param name="target">Target language</param>
    /// <returns>Translated text</returns>
    public static async Task<string> TranslateAsync(this string text, LanguageCode source, LanguageCode target)
    {
        try
        {
            await SlowDown();
            var translate = await text.BaseUrl()
                .AppendPathSegment("translate")
                .SetQueryParam("source", source)
                .SetQueryParam("target", target)
                .PostAsync()
                .ReceiveJson<TranslateResponse>();
            return translate.TranslatedText;
        }
        finally
        {
            SlowDownRelease();
        }
    }

    /// <summary>
    /// Translates the text from one language to another
    /// </summary>
    /// <param name="text">Text to translate</param>
    /// <param name="target">Target language</param>
    /// <returns>Translated text</returns>
    public static async Task<string> TranslateAsync(this string text, LanguageCode target) =>
        await text.TranslateAsync(LanguageCode.AutoDetect, target);

    public static LanguageCode DetectLanguage(this string text) => AsyncHelper.RunSync(() => DetectLanguageAsync(text));

    public static string Translate(this string text, LanguageCode source, LanguageCode target) =>
        AsyncHelper.RunSync(() => TranslateAsync(text, source, target));

    public static string Translate(this string text, LanguageCode target) =>
        AsyncHelper.RunSync(() => TranslateAsync(text, target));
}