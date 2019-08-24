/* ----------------------------------------------------------------------
Ultra UI
https://github.com/MattMcManis/Ultra
https://ultraui.github.io
mattmcmanis@outlook.com

The MIT License

Copyright (C) 2019 Matt McManis

Permission is hereby granted, free of charge, to any person obtaining a 
copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the 
Software is furnished to do so, subject to the following conditions:
    
The above copyright notice and this permission notice shall be included in 
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
---------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ultra
{
    public class MupenCfg
    {
        /// <summary>
        /// Read mupen64plus.cfg
        /// </summary>
        public static void ReadMupen64PlusCfg()
        {
            // Start Cfg File Read
            Configure.ConigFile cfg = null;

            //MessageBox.Show(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")); //debug

            if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
            {
                // Label Notice
                VM.MainView.CfgErrorNotice_Text = "";

                cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

                // -------------------------
                // Paths
                // -------------------------
                try
                {
                    // Plugins
                    VM.PathsView.Plugins_Text = cfg.Read("UI-Console", "PluginDir");

                    // Change Plugins default path "\." to full path "C:\Path\To\Plugins\"
                    if (VM.PathsView.Plugins_Text == @"\.")
                    {
                        VM.PathsView.Plugins_Text = MainWindow.appRootDir;
                    }
                }
                catch
                {
                    MessageBox.Show("Problem importing Plugins Path from mupen64plus.cfg.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }

                // -------------------------
                // Emulator
                // -------------------------
                try
                {
                    // CPU Core
                    int cpuCore = 2; // Default Dynamic Recompiler fallback
                    int.TryParse(cfg.Read("Core", "R4300Emulator").ToLower(), out cpuCore);

                    // Pure Interpreter
                    if (cpuCore == 0)
                    {
                        VM.EmulatorView.Emulator_PureInterpreter_IsChecked = true;
                    }
                    // Cached Interpreter
                    else if (cpuCore == 1)
                    {
                        VM.EmulatorView.Emulator_CachedInterpreter_IsChecked = true;
                    }
                    // Dynamic Recompiler
                    else if (cpuCore == 2)
                    {
                        VM.EmulatorView.Emulator_DynamicRecompiler_IsChecked = true;
                    }

                    // No Compiled Jump
                    bool noCompiledJump = false;
                    bool.TryParse(cfg.Read("Core", "NoCompiledJump").ToLower(), out noCompiledJump);
                    VM.EmulatorView.Emulator_NoCompiledJump_IsChecked = noCompiledJump;

                    // Disable Extra Memory
                    bool disableExtraMem = false;
                    bool.TryParse(cfg.Read("Core", "DisableExtraMem").ToLower(), out disableExtraMem);
                    VM.EmulatorView.Emulator_DisableExtraMemory_IsChecked = disableExtraMem;

                    // Delay SI
                    bool delaySI = true;
                    bool.TryParse(cfg.Read("Core", "DelaySI").ToLower(), out delaySI);
                    VM.EmulatorView.Emulator_DelaySI_IsChecked = delaySI;

                    // Cycles
                    VM.EmulatorView.Emulator_Cycles_SelectedItem = cfg.Read("Core", "CountPerOp");
                }
                catch
                {
                    MessageBox.Show("Problem importing Emulator settings from mupen64plus.cfg.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }

                // -------------------------
                // Display
                // -------------------------
                try
                {
                    // Resolution
                    string screenWidth = cfg.Read("Video-General", "ScreenWidth");
                    string screenHeight = cfg.Read("Video-General", "ScreenHeight");
                    VM.DisplayView.Display_Resolution_SelectedItem = screenWidth + "x" + screenHeight;

                    // Vsync
                    bool vsync = true;
                    bool.TryParse(cfg.Read("Video-General", "VerticalSync").ToLower(), out vsync);
                    VM.DisplayView.Display_Vsync_IsChecked = vsync;

                    // View
                    bool fullscreen = true;
                    bool.TryParse(cfg.Read("Video-General", "Fullscreen").ToLower(), out fullscreen);
                    // Fullscreen
                    if (fullscreen == true)
                    {
                        VM.DisplayView.Display_View_SelectedItem = "Fullscreen";
                    }
                    // Windowed
                    else if (fullscreen == false)
                    {
                        VM.DisplayView.Display_View_SelectedItem = "Windowed";
                    }

                    // OSD
                    bool osd = true;
                    bool.TryParse(cfg.Read("Core", "OnScreenDisplay").ToLower(), out osd);
                    VM.DisplayView.Display_OSD_IsChecked = osd;

                    // Disable Screensaver
                    //bool disableScreensaver = true;
                    //bool.TryParse(cfg.Read("Core", "xxxxxx").ToLower(), out disableScreensaver);
                    //VM.DisplayView.Display_Screensaver_IsChecked = disableScreensaver;

                    // Label Notice
                    VM.MainView.CfgErrorNotice_Text = "";
                }
                catch
                {
                    MessageBox.Show("Problem importing Display settings from mupen64plus.cfg.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }

            }
            // Missing
            else
            {
                // Label Notice
                VM.MainView.CfgErrorNotice_Text = "Notice: mupen64plus.cfg not found.";
            }
        }


        /// <summary>
        ///  Load Plugins from mupen64plus.cfg
        /// </summary>
        public static void LoadPlugins()
        {
            // Start Cfg File Read
            Configure.ConigFile cfg = null;

            // Check if Paths Config TextBox is Empty
            if (!string.IsNullOrEmpty(VM.PathsView.Config_Text))
            {
                // Check if Cfg File Exists
                if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
                {
                    try
                    {
                        cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

                        // Get the .dll file paths from the cfg [UI-Console] plugins section
                        // Select those file names in the Plugin ComboBoxes

                        // Video
                        VM.PluginsView.Video_SelectedItem = Path.GetFileName(cfg.Read("UI-Console", "VideoPlugin"));

                        // Audio
                        VM.PluginsView.Audio_SelectedItem = Path.GetFileName(cfg.Read("UI-Console", "AudioPlugin"));

                        // RSP
                        VM.PluginsView.RSP_SelectedItem = Path.GetFileName(cfg.Read("UI-Console", "RspPlugin"));

                        // Input
                        VM.PluginsView.Input_SelectedItem = Path.GetFileName(cfg.Read("UI-Console", "InputPlugin"));
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Problem importing selected plugins from mupen64plus.cfg.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
            }
            // Path not found
            //else
            //{
            //    MessageBox.Show("Config Path is empty.",
            //                    "Notice",
            //                    MessageBoxButton.OK,
            //                    MessageBoxImage.Warning);
            //}
        }


        /// <summary>
        /// Write mupen64plus.cfg
        /// </summary>
        public static void WriteMupen64PlusCfg(string directory,
                                               string filename,
                                               List<Action> actionsToWrite
                                               /*string path*/)
        {
            // -------------------------
            // Check if Directory Exists
            // -------------------------
            if (!Directory.Exists(directory/*path*/))
            {
                try
                {
                    // Create Config Directory
                    Directory.CreateDirectory(directory/*path*/);
                }
                catch
                {
                    MessageBox.Show("Could not create config folder " + directory/*path*/ + ".\n\nMay require Administrator privileges.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }

            // -------------------------
            // Save mupen64plus.cfg file if directory exists
            // -------------------------
            // Check if Paths Config TextBox is Empty
            //if (!string.IsNullOrEmpty(VM.PathsView.Config_Text))
            //{
            // Check if Conf File Exists
            if (Directory.Exists(directory/*path*/))
            {
                //MessageBox.Show(path); //debug

                //Configure.ConigFile cfg = null;

                // Access
                if (MainWindow.hasWriteAccessToFolder(directory/*path*/) == true)
                {
                    try
                    {
                        // Start conf File Write
                        Configure.ConigFile.cfg = new Configure.ConigFile(Path.Combine(directory, filename)/*path + "mupen64plus.cfg"*/);

                        // Write each action in the list
                        foreach (Action Write in actionsToWrite)
                        {
                            Write();
                        }
                    }

                    // Error
                    catch
                    {
                        MessageBox.Show("Could not save " + filename + " to " + directory,
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
                // Denied
                else
                {
                    MessageBox.Show("User does not have write access to " + directory/*+ path*/,
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }

            }
            //}
            // Path not found
            //else
            //{
            //    MessageBox.Show("Config Path is empty.",
            //                    "Notice",
            //                    MessageBoxButton.OK,
            //                    MessageBoxImage.Warning);
            //}
        }
        

    }
}
