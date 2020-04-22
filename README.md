# Blazor WOL for Windows

## Summary

This application provides a Web UI to sending a "WOL" (Wake up On Lan) magic packet, on Windows OS.

This is a console program based on .NET Core 3.1 and the UI is built on Blazor WebAssembly.

This application can also run as a Windows service.

## Download

Download the latest version of the zip package from ["Release"](https://github.com/jsakamoto/BlazorWOL-for-Windows/releases) page, and extract it to any folder somewhere you prefer.

## Usage

### Launch it by stand-alone mode

You can launch this application from the command line:

```shell
> Blazor WOL.Server
```

After launch it, you can access to that UI via `http://localhost:5000/`.

### Install as a Windows Service

You can install this application as a Windows Service by `install` command, like this:

```shell
> Blazor WOL.Server install
```

Once after you did it, you can manage it from "Windows Service Management Console", such as start the service, and stop the service.

If you want to remove it from Windows Service, you can do it with `uninstall` command, like this:

```shell
> Blazor WOL.Server uninstall
```

### Command line options

#### --urls `<url>`

You can change the URLs to listen.

example:

```shell
> Blazor WOL.Server --urls http://+:8080/
```

#### --application-data-location `<path-to-folder>`

You can change the folder to store the JSON file which contains device list data.

example:

```shell
> Blazor WOL.Server --application-data-location C:\temp
```

#### --base-href `<path>`

You can change the base URL. This option useful for deploy this application behind the reverse proxy for make the URL for this application to sub-path.

example:

```shell
> Blazor WOL.Server --base-url /app/wol/
```

## Configure command-line options when it's installed as a Windows Service

You can configure command-line options for the Windows Service of this application by editing the Windows registry.

Open registry key `HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BlazorWOL` by Windows registry editor ("regedit"), and change `ImagePath` value.

## License

[The Unlicense](LICENSE)