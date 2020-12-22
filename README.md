![https://github.com/MattMcManis/Ultra](https://raw.githubusercontent.com/MattMcManis/Ultra/master/images/logo-ultra-ui-web.png)

# Ultra
Mupen64Plus N64 Emulator Frontend for Windows

* [Overview](#overview)
* [Features](#features)
* [Downloads](#downloads)
* [Setup](#setup)
* [Resources](#resources)
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
* Plugin Configuration Menus for `GLideN64`, `Angrylion Plus`, `Audio SDL`, `Input SDL`, `RSP HLE`, `CXD4`.
* Shortcuts to common folders
* Quick Load & Save State `st0-9`
* Load Save File `.m64p`/`.pj` format
* Save State File `.m64p`/`.pj` format
* Map Keyboard Keys for Controls
* Gamepad Fully Automatic Mode

**What it currently can't do:**
* Gamepad Remap Custom Buttons
* No Plugin Configuration Menus for `Glide64`, `Glide64mk2`, `Rice`, `Arachnoid`, `z64` yet.
* Cheats

&nbsp;

## Downloads
#### Ultra
[Ultra + Mupen64plus Latest Release](https://github.com/MattMcManis/Ultra/releases)

Requires
* Windows 8, 8.1, 10
* [Microsoft .NET Framework 4.5](https://www.microsoft.com/en-us/download/details.aspx?id=30653)
* [Mupen64Plus](https://github.com/mupen64plus/mupen64plus-core/releases)
* [GLideN64 Public Release 4.0](https://github.com/gonetz/GLideN64/releases/tag/Public_Release_4_0)

#### Mupen64Plus
[Latest Release](https://github.com/mupen64plus/mupen64plus-core/releases)


&nbsp;

## Setup

> Notice: This program will write to your existing [mupen64plus.cfg](https://mupen64plus.org/wiki/index.php/FileLocations#Windows_Vista_and_Newer), make a backup before running.

1. **Place Files**
    * Put `Ultra.exe` in the `Mupen64Plus` folder that contains the `mupen64plus.dll`.  
      [Screenshot 1](https://raw.githubusercontent.com/MattMcManis/Ultra/master/docs/Ultra%20Setup/01.png)

2. **Set Paths & Defaults**
    * Run `Ultra.exe`
    * In the `Paths` tab, press the `Defaults All` button to automatically set the paths and controls.  
      Or manually set your `Mupen64Plus` and `Plugin` Paths.  
      [Screenshot 2](https://raw.githubusercontent.com/MattMcManis/Ultra/master/docs/Ultra%20Setup/02.png)
    
    * Set your `ROMs` Path (`.n64`/`.v64`/`.z64` files in subfolders will be detected).

3. **Configure Graphics, Audio, Controls**
    * The `Plugin Configure Windows`, such as `GLideN64`, will not have the values loaded from `mupen64plus.cfg` yet.
    * In the `Plugins` tab, press the `Generate` button to generate the selected plugin default values.  
      [Screenshot 3](https://raw.githubusercontent.com/MattMcManis/Ultra/master/docs/Ultra%20Setup/03.png)  
    
    * When you open a `Plugin Configure Window` the controls will now be loaded with the defaults.  
      [Screenshot 4](https://raw.githubusercontent.com/MattMcManis/Ultra/master/docs/Ultra%20Setup/04.png)
    
4. **Play Game**
    * In the `Games` tab, press the `Rebuild` &#10226; arrow button to generate the list of games.  
      [Screenshot 5](https://raw.githubusercontent.com/MattMcManis/Ultra/master/docs/Ultra%20Setup/06.png)  

    * Play a game with your selected settings.  
      [Screenshot 6](https://raw.githubusercontent.com/MattMcManis/Ultra/master/docs/Ultra%20Setup/07.png)

### Paths

* Ultra UI Config  
  `C:\Users\<username>\AppData\Roaming\Ultra UI\ultra.conf`
* Mupen64Plus Config  
  `C:\Users\<username>\AppData\Roaming\Mupen64Plus\mupen64plus.cfg`
* Saves  
  `C:\Users\<username>\AppData\Roaming\Mupen64Plus\save\`
* Screenshots  
  `C:\Users\<username>\AppData\Roaming\Mupen64Plus\screenshot\`


### Tips

**Plugin Combinations for best results**

- `GLideN64` + `RSP HLE`
- `Angrylion Plus` + `CXD4` (Requires powerful PC)

**Resolution**

- If `Windowed`, select `4:3` aspect ratio to avoid black bars on sides.
- If `Fullscreen`, select your monitors actual aspect ratio, such as `16:9`, or else `4:3` will stretch to fit your screen.
- If you want an extended view, select a `16:9` resolution, and in the Plugin select `Aspect Ratio Force 16:9` or `Widescreen`.
- If game is slow, select a smaller resolution.

&nbsp;


## Resources

**Ultra Wiki**
* [Resources](https://github.com/MattMcManis/Ultra/wiki/Resources)  
* [Troubleshooting](https://github.com/MattMcManis/Ultra/wiki/Troubleshooting)

**Mupen64Plus Wiki**
* [Mupen64Plus](https://mupen64plus.org/wiki/index.php/Mupen64Plus)
* [Console Usage](https://mupen64plus.org/wiki/index.php/UIConsoleUsage)
* [File Locations](https://mupen64plus.org/wiki/index.php/FileLocations)
* [Keyboard Setup](https://mupen64plus.org/wiki/index.php/KeyboardSetup)
* [Controller Setup](https://mupen64plus.org/wiki/index.php/ControllerSetup)
* [Third Party Plugins](https://mupen64plus.org/wiki/index.php/ThirdPartyPlugins) 
* [Recommended Plugin Setups](http://emulation.gametechwiki.com/index.php/Mupen64Plus#Recommended_plugin_setups) 
* [Game Compatibility](https://mupen64plus.org/wiki/index.php/GameCompatibility)

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