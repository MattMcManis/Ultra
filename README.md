![https://github.com/MattMcManis/Ultra](https://raw.githubusercontent.com/MattMcManis/Ultra/master/images/logo-ultra-ui-web.png)

# Ultra UI
Mupen64Plus N64 Emulator Frontend for Windows

* [Overview](#overview)
* [Features](#features)
* [Downloads](#downloads)
* [Setup](#setup)
* [Troubleshooting](#troubleshooting)
* [Build](#build)

&nbsp;

## Overview

![Ultra UI](https://raw.githubusercontent.com/MattMcManis/Ultra/master/images/Ultra-UI-Screenshot-01.png)

Play N64 games through an easy to use interface.

Special thanks to [M64py](http://m64py.sourceforge.net) for design inspiration.  
API system by [BizHawk](https://github.com/TASVideos/BizHawk).

&nbsp;

## Features
**What it can do:**
* Game Launcher
* Navigate options quickly with tabs
* Edit Mupen64Plus Config
* Plugin Configuration Menus for `GLideN64`, `Audio SDL`, `Input SDL`, `RSP HLE`, & `cxd4-ssse3`.
* Shortcuts to common folders
* Quick Load & Save State `st0-9`
* Load Save File `.m64p`/`.pj` format
* Save State File `.m64p`/`.pj` format
* Map Keyboard Keys for Controls
* Gamepad Fully Automatic Mode

**What it currently can't do:**
* Gamepad Remap Custom Buttons
* No Plugin Configuration Menus for `Glide64`, `Glide64mk2`, `Rice`, `z64`yet.

&nbsp;

## Downloads
#### Releases
https://github.com/MattMcManis/Ultra/releases

#### Requires
* Windows
* [Microsoft .NET Framework 4.5](https://www.microsoft.com/en-us/download/details.aspx?id=30653)
* [Mupen64Plus](https://github.com/mupen64plus/mupen64plus-core/releases)
* [GLideN64 Public Release 4.0](https://github.com/gonetz/GLideN64/releases/tag/Public_Release_4_0)

&nbsp;

## Setup

> Notice: This program will write to your existing `mupen64plus.cfg`, keep a backup before running.

1. **Files**
    * Put `Ultra.exe` in the `Mupen64Plus` folder that contains the `mupen64plus.dll`.

2. **Set the Paths & Defaults**
    * Run `Ultra.exe`
    * Press the `Defaults All` button to automatically set the controls.
    * Or manually set your `Mupen64Plus` and `Plugin` Paths.
    * Set your `ROMs` Path.
    * In the Games Tab, press the rebuild arrow button to generate the list of games.

3. **Configure the Graphics, Audio, Controls**
    * The `Plugin Configure Windows`, such as `GLideN64`, will not have the values loaded from `mupen64plus.cfg` yet.
    * In the `Plugins Tab`, press the `Generate` button to generate the selected plugin default values.
    * When you open a `Plugin Configure Window` the controls will now be loaded with the defaults.
    * Play a game with your desired settings.

### Paths

* Ultra UI Config `C:\Users\<username>\AppData\Roaming\Ultra UI\ultra.conf`
* Mupen64Plus Config `C:\Users\<username>\AppData\Roaming\Mupen64Plus\mupen64plus.cfg`
* Saves `C:\Users\<username>\AppData\Roaming\Mupen64Plus\save\`
* Screenshots `C:\Users\<username>\AppData\Roaming\Mupen64Plus\screenshot\`

&nbsp;

## Wiki

* [Mupen64Plus](https://mupen64plus.org/wiki/index.php/Mupen64Plus)
* [Console Usage](https://mupen64plus.org/wiki/index.php/UIConsoleUsage)
* [File Locations](https://mupen64plus.org/wiki/index.php/FileLocations)
* [Keyboard Setup](https://mupen64plus.org/wiki/index.php/KeyboardSetup)
* [Controller Setup](https://mupen64plus.org/wiki/index.php/ControllerSetup)
* [Third Party Plugins](https://mupen64plus.org/wiki/index.php/ThirdPartyPlugins)
* [Recommended Plugin Setups](http://emulation.gametechwiki.com/index.php/Mupen64Plus#Recommended_plugin_setups)
* [Game Compatibility](https://mupen64plus.org/wiki/index.php/GameCompatibility)

&nbsp;

## Troubleshooting

[Troubleshooting Wiki](https://github.com/MattMcManis/Ultra/wiki/Troubleshooting)

&nbsp;

## Build
Visual Studio 2015
<br />
WPF, C#, XAML
<br />
Visual C++ 19.0 Compiler

&nbsp;

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=VTUE7KQ8RS3DN) 

Thank you for your support.