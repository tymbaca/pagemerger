## Build

> For build make sure you have those dependancies:
> - [.NET](https://dotnet.microsoft.com/en-us/download) (7 and later)
> - .NET packages:
>   - [CommandLineParser](https://github.com/commandlineparser/commandline) (2.9.1 and later)
>   - [Open-XML-SDK](https://github.com/dotnet/Open-XML-SDK) (2.20.0 and later)

To build project to single executable file simply run this command from project directory:
```sh
dotnet publish -p:PublishSingleFile=true --self-contained false
```

It will build and also *print the folder* where executable is placed:

```sh
PageMerger -> /.../PageMerger/bin/Debug/net7.0/osx-x64/PageMerger.dll
PageMerger -> /.../PageMerger/bin/Debug/net7.0/osx-x64/publish/
```

Or use `-o` option with following path to specify the output directory:

```sh
dotnet publish -o example/directory -p:PublishSingleFile=true --self-contained false
```

