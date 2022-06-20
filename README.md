# Fluent.LibreTranslate

## C# Fluent sync/async translation via LibreTranslate
[![NuGet](https://img.shields.io/nuget/v/Fluent.LibreTranslate.svg)](https://www.nuget.org/packages/Fluent.LibreTranslate/)
### Installation
`dotnet add package Fluent.LibreTranslate`
### Using
```csharp
using Fluent.LibreTranslate;
```
### Usage
```csharp
GlobalLibreTranslateSettings.Server = LibreTranslateServer.Libretranslate_de;
GlobalLibreTranslateSettings.ApiKey = null; // if need an apiKey 
GlobalLibreTranslateSettings.UseRateLimitControl = true; //to avoid "429 Too Many Requests" exception
GlobalLibreTranslateSettings.RateLimitTimeSpan = TimeSpan.FromSeconds(4); //depends on server configuration, default 4 seconds

Console.WriteLine(await "Hello, World!".TranslateAsync(LanguageCode.Finnish));
```
### Output:
```
Hei, maailma!
```
### Custom LibreTranslate server URL:
```csharp
GlobalLibreTranslateSettings.Server = new LibreTranslateServer("http://localhost:5000");
```
### Methods
```csharp
Task<LanguageCode> DetectLanguageAsync(this string text);
Task<string> TranslateAsync(this string text, LanguageCode source, LanguageCode target);
Task<string> TranslateAsync(this string text, LanguageCode target);

LanguageCode DetectLanguage(this string text);
string Translate(this string text, LanguageCode source, LanguageCode target);
string Translate(this string text, LanguageCode target);
```
