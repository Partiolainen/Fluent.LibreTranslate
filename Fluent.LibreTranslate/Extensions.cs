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
                await Task.Delay(GlobalLibreTranslateSettings.RateLimitTimeSpan);
                GlobalLibreTranslateSettings.SlowDownLocker.Release();
            });
    }

    /// <summary>
    /// Detects the text language
    /// </summary>
    /// <param name="text">Text to detect</param>
    /// <returns>Translated text</returns>
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
    /// Translates the text from one language to another with source language detection 
    /// </summary>
    /// <param name="text">Text to translate</param>
    /// <param name="target">Target language</param>
    /// <returns>Translated text</returns>
    public static async Task<string> TranslateAsync(this string text, LanguageCode target) =>
        await text.TranslateAsync(LanguageCode.AutoDetect, target);

    /// <summary>
    /// Detects the text language
    /// </summary>
    /// <param name="text">Text to detect</param>
    /// <returns>Translated text</returns>
    public static LanguageCode DetectLanguage(this string text) => AsyncHelper.RunSync(() => DetectLanguageAsync(text));


    /// <summary>
    /// Submit a suggestion to improve a translation
    /// </summary>
    /// <param name="text">Original text</param>
    /// <param name="suggestedTranslation">Suggested translation</param>
    /// <param name="source">Language of original text</param>
    /// <param name="target">Language of suggested translation</param>
    public static async Task SuggestAsync(this string text, string suggestedTranslation, LanguageCode source,
        LanguageCode target)
    {
        try
        {
            await SlowDown();
            await text.BaseUrl()
                .AppendPathSegment("suggest")
                .SetQueryParam("s", suggestedTranslation)
                .SetQueryParam("source", source)
                .SetQueryParam("target", target)
                .PostAsync();
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
    public static string Translate(this string text, LanguageCode source, LanguageCode target) =>
        AsyncHelper.RunSync(() => TranslateAsync(text, source, target));

    /// <summary>
    /// Translates the text from one language to another with source language detection 
    /// </summary>
    /// <param name="text">Text to translate</param>
    /// <param name="target">Target language</param>
    /// <returns>Translated text</returns>
    public static string Translate(this string text, LanguageCode target) =>
        AsyncHelper.RunSync(() => TranslateAsync(text, target));

    /// <summary>
    /// Submit a suggestion to improve a translation
    /// </summary>
    /// <param name="text">Original text</param>
    /// <param name="suggestedTranslation">Suggested translation</param>
    /// <param name="source">Language of original text</param>
    /// <param name="target">Language of suggested translation</param>
    public static void Suggest(this string text, string suggestedTranslation, LanguageCode source, LanguageCode target) =>
        AsyncHelper.RunSync(() => SuggestAsync(text, suggestedTranslation, source, target));
}