using System.Diagnostics;

namespace Fluent.LibreTranslate.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        GlobalLibreTranslateSettings.Server = LibreTranslateServer.Libretranslate_de;
        GlobalLibreTranslateSettings.ApiKey = null;
        GlobalLibreTranslateSettings.UseRateLimitControl = true;
    }

    private const string _englishText = "Hello World!";
    private const string _finnishText = "Hei maailma!";
    private const string _fooLanguage = "foo";
    private const int _slowDownTestIterations = 20; //15 requests per minute


    [Test]
    public async Task TestDetectionAsync()
    {
        var language = await _englishText.DetectLanguageAsync();
        Assert.That(language, Is.EqualTo(LanguageCode.English));
    }

    [Test]
    public void TestDetection()
    {
        var language = _englishText.DetectLanguage();
        Assert.That(language, Is.EqualTo(LanguageCode.English));
    }

    [Test]
    public async Task TestTranslationAsync()
    {
        var translatedText = await _englishText.TranslateAsync(LanguageCode.AutoDetect, LanguageCode.Finnish);
        Assert.That(translatedText, Is.Not.Null);
        Assert.That(translatedText, Is.EqualTo(_finnishText));
    }

    [Test]
    public async Task TestAutoTranslationAsync()
    {
        var translatedText = await _englishText.TranslateAsync(LanguageCode.Finnish);
        Assert.That(translatedText, Is.Not.Null);
        Assert.That(translatedText, Is.EqualTo(_finnishText));
    }

    [Test]
    public void TestTranslation()
    {
        var translatedText = _englishText.Translate(LanguageCode.AutoDetect, LanguageCode.Finnish);
        Assert.That(translatedText, Is.Not.Null);
        Assert.That(translatedText, Is.EqualTo(_finnishText));
    }

    [Test]
    public void TestAutoTranslation()
    {
        var translatedText = _englishText.Translate(LanguageCode.Finnish);
        Assert.That(translatedText, Is.Not.Null);
        Assert.That(translatedText, Is.EqualTo(_finnishText));
    }

    [Test]
    public void TestTranslationException()
    {
        var exception = Assert.Throws<ArgumentException>(() => _englishText.Translate(_fooLanguage));
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.Not.Empty);
    }

    [Test]
    public async Task SlowDownTest()
    {
        var watch = Stopwatch.StartNew();
        var random = new Random();
        for (var i = 0; i < _slowDownTestIterations; i++)
        {
            var delay = random.Next(2000);
            await Task.Delay(delay);
            watch.Reset();
            watch.Start();
            await TestAutoTranslationAsync();
            watch.Stop();
            await TestContext.Progress.WriteLineAsync(
                $"SlowDownTest progress: {(i + 1m) / _slowDownTestIterations:P0};" +
                $" delay {delay / 1000m:f1}s; translation time: {watch.Elapsed.Milliseconds / 1000m:f2}");
        }
    }

    [Test]
    public void ParallelTest()
    {
        var taskList = new List<Action>();
        for (var i = 0; i < _slowDownTestIterations; i++)
        {
            var i1 = i;
            taskList.Add((() =>
            {
                var watch = Stopwatch.StartNew();
                TestContext.Progress.WriteLine($"Running instanse #{i1}");
                watch.Start();
                TestAutoTranslation();
                watch.Stop();
                TestContext.Progress.WriteLine($"Instanse #{i1} completed after {watch.Elapsed.Seconds}s");
            }));
        }

        Parallel.Invoke(new ParallelOptions { MaxDegreeOfParallelism = _slowDownTestIterations }, taskList.ToArray());
    }
}