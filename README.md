## Install
For now it's not in any package manager repository. Only option is to build it from source.

Clone repository to your local machine:
```sh
git clone https://github.com/tymbaca/pagemerger.git
```

## Build

### tl;dr
```sh
git clone https://github.com/tymbaca/pagemerger.git
cd pagemerger 
dotnet add package DocumentFormat.OpenXml --version 2.20.0
dotnet add package CommandLineParser --version 2.9.1
dotnet publish -o ./result -p:PublishSingleFile=true --self-contained false
cd result
sudo mv pagemerger /usr/local/bin
```

> For build make sure you have those dependancies:
> - [.NET](https://dotnet.microsoft.com/en-us/download) (7 and later)
> - .NET packages:
>   - [CommandLineParser](https://github.com/commandlineparser/commandline) (2.9.1 and later)
>   - [Open-XML-SDK](https://github.com/dotnet/Open-XML-SDK) (2.20.0 and later)

To build project to single executable file go to project directory and run this command:

```sh
dotnet publish -o ./result -p:PublishSingleFile=true --self-contained false
```

> Notice that `-o` option specifies output directory where built binary will be putted. You can choose another directory if you wish.

Then go to output directory and move binary to one of the directories that are in PATH (usually you can put it into `/usr/local/bin`):
```sh
cd result
sudo mv pagemerger /usr/local/bin
```


## Usage

Run program with arguments in following order:
```sh
pagemerger output.docx src_file1.docx src_file2.docx src_file3.docx
```

If you want to set page breaks between sources simply add option `-b` or `--set-page-breaks`:

```sh
pagemerger -b output.docx src_file1.docx src_file2.docx src_file3.docx
```
