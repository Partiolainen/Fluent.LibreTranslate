@echo off
del *.nupkg
dotnet build ../Fluent.LibreTranslate/Fluent.LibreTranslate.csproj -c=Release
dotnet pack ../Fluent.LibreTranslate/Fluent.LibreTranslate.csproj -c=Release -o ./