using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent.LibreTranslate;

public class LanguageCode
{
    private static readonly Dictionary<string, LanguageCode> Instance = new();
    private readonly string Code;

    internal LanguageCode(string code)
    {
        Code = code;
        Instance[Code] = this;
    }

    public static implicit operator LanguageCode(string str)
    {
        return FromString(str);
    }

    public static LanguageCode FromString(string str)
    {
        if (Instance.TryGetValue(str, out var result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException(
                $"{nameof(LanguageCode)} must be one of the followings {string.Join(", ", Instance.Select(x => x.Value.Code))}");
        }
    }

    public override string ToString()
    {
        return $"{Code}";
    }

    public static readonly LanguageCode English = new("en");
    public static readonly LanguageCode Arabic = new("ar");
    public static readonly LanguageCode Azerbaijani = new("az");
    public static readonly LanguageCode Chinese = new("zh");
    public static readonly LanguageCode Czech = new("cs");
    public static readonly LanguageCode Danish = new("da");
    public static readonly LanguageCode Dutch = new("nl");
    public static readonly LanguageCode Esperanto = new("eo");
    public static readonly LanguageCode Finnish = new("fi");
    public static readonly LanguageCode French = new("fr");
    public static readonly LanguageCode German = new("de");
    public static readonly LanguageCode Greek = new("el");
    public static readonly LanguageCode Hebrew = new("he");
    public static readonly LanguageCode Hindi = new("hi");
    public static readonly LanguageCode Hungarian = new("hu");
    public static readonly LanguageCode Indonesian = new("id");
    public static readonly LanguageCode Irish = new("ga");
    public static readonly LanguageCode Italian = new("it");
    public static readonly LanguageCode Japanese = new("ja");
    public static readonly LanguageCode Korean = new("ko");
    public static readonly LanguageCode Persian = new("fa");
    public static readonly LanguageCode Polish = new("pl");
    public static readonly LanguageCode Portuguese = new("pt");
    public static readonly LanguageCode Russian = new("ru");
    public static readonly LanguageCode Slovak = new("sk");
    public static readonly LanguageCode Spanish = new("es");
    public static readonly LanguageCode Swedish = new("sv");
    public static readonly LanguageCode Turkish = new("tr");
    public static readonly LanguageCode Ukranian = new("uk");
    public static readonly LanguageCode Vietnamese = new("vi");
    public static readonly LanguageCode AutoDetect = new("auto");
}