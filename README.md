# Fluent.LibreTranslate

## C# Fluent sync/async translation via LibreTranslate
[![NuGet](https://img.shields.io/nuget/v/Fluent.LibreTranslate.svg)](https://www.nuget.org/packages/Fluent.LibreTranslate/)
### Installation
`Install-Package Fluent.LibreTranslate -Version 1.0.0.1`
### Using
```csharp
using LibreTranslate.Net;
```
### Usage
```csharp
GlobalLibreTranslateSettings.Server = LibreTranslateServer.Libretranslate_de;

Console.WriteLine(await "Hello, World!".TranslateAsync(LanguageCode.Finnish));
```
### Output:
```
Hei, maailma!
```
### Custom LibreTranslate server URL:
```csharp
GlobalLibreTranslateSettings.Server = "http://localhost:5000";
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
