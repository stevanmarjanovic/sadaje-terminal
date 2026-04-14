# Sada Je Terminal

Text-based 5-minute-step clock in Serbian for terminal use.

<img width="709" height="544" alt="Screenshot 2026-04-12 at 21 45 17" src="https://github.com/user-attachments/assets/3ba24cf6-fceb-4e37-84e0-99c10bebe7ea" />

## Usage

```
sadaje [options]
```

|Option              | Description                                                                                |
|--------------------|--------------------------------------------------------------------------------------------|
|`--dark`            | **Set dark mode**                                                                          |
|`--rounded`         | **Make box corners rounded**                                                               |
|`--color=<color>`   | **Set the color of the text**<br>black, blue, green, cyan, red, magenta, yellow, white     |
|`--margin=<margin>` | **Set margin between border and content**<br>This is called padding actually. Use integers |
|`--hide-time`       | **Hide the current time**                                                                  |
|`--hide-copyright`  | **Hide the copyright notice**                                                              |
|`--debug`           | **Show some debug data**                                                                   |
|`--help`            | **Show this help message**                                                                 |


## Build

This app was designed to be built as a Native AOT. All the scripts below were made for ease of publishing, but they
essentially just run one script.

```shell
dotnet publish ./SadaJeTerminal.csproj -c Release -r <RID>
```

### Runtime Identifiers

| OS          | Architecture | RID           |
|-------------|--------------|---------------|
| **macOS**   | ARM          | `osx-arm64`   |
|             | x64          | `osx-x64`     |
| **Linux**   | ARM          | `linux-arm64` |
|             | x64          | `linux-x64`   |
| **Windows** | ARM          | `win-arm64`   |
|             | x64          | `win-x64`     |


### MacOS

This creates both `arm64` and `x64` binaries for macOS in the `./dist` folder.

```shell
./publish-osx.sh
```

### Windows

This creates both `arm64` and `x64` binaries for Windows in the `./dist` folder.

#### Command Line
```shell
publish.bat
```

#### PowerShell
```shell
./publish.ps1
```

### Linux

This creates both `arm64` and `x64` binaries for Linux-based operating systems in the `./dist` folder.

```shell
./publish-linux.sh
```
