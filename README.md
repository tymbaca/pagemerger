## Install
For now it's not in any package manager repository. You can either install `.deb` package from latest release (if you have Linux machine) or build it yourself.

Download `.deb` package that mathes your machine architecture (see [releases page](https://github.com/tymbaca/pagemerger/releases)). Then run command:
```sh
dpkg -i pagemerger_<...>.deb  // <— rename as `.deb` package you downloaded
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

Clone repository to your local machine:
```sh
git clone https://github.com/tymbaca/pagemerger.git
```

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

## Uninstall
There are two ways to uninstall: 
- You can do `dpkg -r pagemerger` (if you installed it as a `.deb` package)
- Or simply delete it from `/usr/local/bin` (or whatever directory you placed it after build): `sudo rm /usr/local/bin/pagemerger`

## Usage

Run program with arguments in following order:
```sh
pagemerger output.docx src_file1.docx src_file2.docx src_file3.docx
```

If you want to set page breaks between sources simply add option `-b` or `--set-page-breaks`:

```sh
pagemerger -b output.docx src_file1.docx src_file2.docx src_file3.docx
```
