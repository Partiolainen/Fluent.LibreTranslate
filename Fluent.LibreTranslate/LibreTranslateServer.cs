using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluent.LibreTranslate;

public class LibreTranslateServer
{
    private static readonly Dictionary<string, LibreTranslateServer> Instance = new();
    internal readonly string Url;

    internal LibreTranslateServer(string url)
    {
        Url = url;
        Instance[Url] = this;
    }

    public static implicit operator LibreTranslateServer(string str)
    {
        return FromString(str);
    }

    public static LibreTranslateServer FromString(string str)
    {
        if (Instance.TryGetValue(str, out var result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException(
                $"{nameof(LibreTranslateServer)} must be one of the followings {string.Join(", ", Instance.Select(x => x.Key))}");
        }
    }

    public override string ToString()
    {
        return $"{Url}";
    }

    //mirrors source: https://github.com/LibreTranslate/LibreTranslate#mirrors
    public static LibreTranslateServer Libretranslate_com => new("https://libretranslate.com/");
    public static LibreTranslateServer Libretranslate_de => new("https://libretranslate.de/");
    public static LibreTranslateServer Translate_argosopentech_com => new("https://translate.argosopentech.com/");
    public static LibreTranslateServer Translate_api_skitzen_com => new("https://translate.api.skitzen.com/");
    public static LibreTranslateServer Libretranslate_pussthecat_org => new("https://libretranslate.pussthecat.org/");
    public static LibreTranslateServer Translate_fortytwo_it_com => new("https://translate.fortytwo-it.com/");
    public static LibreTranslateServer Translate_terraprint_co => new("https://translate.terraprint.co/");
}