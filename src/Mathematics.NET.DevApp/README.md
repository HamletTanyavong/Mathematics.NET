# Mathematics.NET Development Application

This application is to be used only for quick testing and verification; set it as the startup item or object.

## Tracking Files

Because we do not want changes made to this application to be tracked, use the commands
```
git update-index --assume-unchanged src/Mathematics.NET.DevApp/Mathematics.NET.DevApp.csproj
```
and
```
git update-index --assume-unchanged src/Mathematics.NET.DevApp/Program.cs
```
To begin tracking changes again, use the same command but with the option `--no-assume-unchanged`.
