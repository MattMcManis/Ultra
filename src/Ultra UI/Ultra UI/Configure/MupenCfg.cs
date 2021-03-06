﻿/* ----------------------------------------------------------------------
Ultra UI
https://github.com/MattMcManis/Ultra
https://ultraui.github.io
mattmcmanis@outlook.com

The MIT License

Copyright (C) 2019-2020 Matt McManis

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
using ViewModel;

namespace Ultra
{
    public class MupenCfg
    {
        /// <summary>
        /// Read mupen64plus.cfg
        /// </summary>
        public static void ReadMupen64PlusCfg()
        {
            // Check if Paths Config TextBox is Empty
            if (string.IsNullOrWhiteSpace(VM.PathsView.Config_Text))
            {
                MessageBox.Show("Could not read mupen64plus.cfg. Config Path is empty.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }

            // Check if Paths Config TextBox is valid
            if (MainWindow.IsValidPath(VM.PathsView.Config_Text) == false)
            {
                MessageBox.Show("Could not read mupen64plus.cfg. Config Path is not valid.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }

            // Label Notice
            VM.MainView.CfgErrorNotice_Text = "";

            if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")) == false)
            {
                VM.MainView.CfgErrorNotice_Text = "Notice: mupen64plus.cfg not found.";

                return;
            }


            // Start mupen64plus.cfg Read
            Configure.ConigFile cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

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
            catch (Exception ex)
            {
                MessageBox.Show("Problem importing Plugins Path from mupen64plus.cfg." + ex.ToString(),
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }


            // -------------------------
            // Plugins
            // -------------------------
            // LoadPlugins() in MainWindow.Window_Loaded() after ScanPlugins()


            // -------------------------
            // Emulator
            // -------------------------
            try
            {
                // CPU Core
                int cpuCore = 2; // Default Dynamic Recompiler fallback
                int.TryParse(cfg.Read("Core", "R4300Emulator").ToLower(), out cpuCore);
                switch (cpuCore)
                {
                    // Pure Interpreter
                    case 0:
                        VM.EmulatorView.CPU_SelectedItem = "Pure Interpreter";
                        break;

                    // Cached Interpreter
                    case 1:
                        VM.EmulatorView.CPU_SelectedItem = "Cached Interpreter";
                        break;

                    // Dynamic Recompiler
                    case 2:
                        VM.EmulatorView.CPU_SelectedItem = "Dynamic Recompiler";
                        break;
                }

                // DisableSpecRecomp
                bool disableSpecRecomp = true;
                bool.TryParse(cfg.Read("Core", "DisableSpecRecomp").ToLower(), out disableSpecRecomp);
                VM.EmulatorView.Emulator_DisableSpecRecomp_IsChecked = disableSpecRecomp;

                // RandomizeInterrupt
                bool randomizeInterrupt = true;
                bool.TryParse(cfg.Read("Core", "RandomizeInterrupt").ToLower(), out randomizeInterrupt);
                VM.EmulatorView.Emulator_RandomizeInterrupt_IsChecked = randomizeInterrupt;

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
            catch (Exception ex)
            {
                MessageBox.Show("Problem importing Emulator settings from mupen64plus.cfg.\r\n\r\n" + ex.ToString(),
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
                switch (fullscreen)
                {
                    // Fullscreen
                    case true:
                        VM.DisplayView.Display_View_SelectedItem = "Fullscreen";
                        break;

                    // Windowed
                    case false:
                        VM.DisplayView.Display_View_SelectedItem = "Windowed";
                        break;
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
            catch (Exception ex)
            {
                MessageBox.Show("Problem importing Display settings from mupen64plus.cfg." + ex.ToString(),
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }


        /// <summary>
        ///  Load Plugins from mupen64plus.cfg
        /// </summary>
        public static void LoadPlugins(MainWindow mainwindow)
        {
            // Check if Paths Config TextBox is Empty
            if (string.IsNullOrWhiteSpace(VM.PathsView.Config_Text))
            {
                MessageBox.Show("Could not load plugins. Config Path is empty.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }

            // Check if Paths Config TextBox is valid
            if (MainWindow.IsValidPath(VM.PathsView.Config_Text) == false)
            {
                MessageBox.Show("Could not load plugins. Config Path is not valid.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }

            // Check if Cfg File Exists
            if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")) == false)
            {
                MessageBox.Show("Could not load plugins. Config file ultra.conf does not exist.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }

            // Start Cfg File Read
            Configure.ConigFile cfg = null;

            try
            {
                cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

                // Get the .dll file paths from the cfg [UI-Console] plugins section
                // Select those file names in the Plugin ComboBoxes

                // -------------------------
                // Video
                // -------------------------
                List<string> videoPluginNames = VM.PluginsView.Video_Items.Select(item => item.Name).ToList();
                string videoPlugin = Path.GetFileName(cfg.Read("UI-Console", "VideoPlugin"));
                //MessageBox.Show(videoPlugin); //debug

                // Selected
                if (!string.IsNullOrWhiteSpace(videoPlugin))
                {
                    // Default
                    if (videoPluginNames.Contains(videoPlugin))
                    {
                        VM.PluginsView.Video_SelectedItem = videoPlugin; 
                    }
                    // Missing Fallback
                    else
                    {
                        //MessageBox.Show("Missing Fallback"); //debug
                        // Default
                        if (videoPluginNames.Contains("mupen64plus-video-GLideN64.dll"))
                        {
                            //MessageBox.Show("Default"); //debug
                            VM.PluginsView.Video_SelectedItem = "mupen64plus-video-GLideN64.dll";
                        }
                        // Missing
                        else
                        {
                            //MessageBox.Show("Missing"); //debug
                            //VM.PluginsView.Video_SelectedIndex = 0;
                            mainwindow.cboPlugin_Video.SelectedIndex = 0;
                        }
                    }
                }
                // Null Fallback
                else
                {
                    //VM.PluginsView.Video_SelectedIndex = 0;
                    mainwindow.cboPlugin_Video.SelectedIndex = 0;
                }

                // -------------------------
                // Audio
                // -------------------------
                List<string> audioPluginNames = VM.PluginsView.Audio_Items.Select(item => item.Name).ToList();
                string audioPlugin = Path.GetFileName(cfg.Read("UI-Console", "AudioPlugin"));

                // Selected
                if (!string.IsNullOrWhiteSpace(audioPlugin))
                {
                    // Default
                    if (audioPluginNames.Contains(audioPlugin))
                    {
                        VM.PluginsView.Audio_SelectedItem = audioPlugin;
                    }
                    // Missing Fallback
                    else
                    {
                        //MessageBox.Show("Missing Fallback"); //debug
                        // Default
                        if (audioPluginNames.Contains("mupen64plus-audio-sdl.dll"))
                        {
                            //MessageBox.Show("Default"); //debug
                            VM.PluginsView.Audio_SelectedItem = "mupen64plus-audio-sdl.dll";
                        }
                        // Missing
                        else
                        {
                            //MessageBox.Show("Missing"); //debug
                            //VM.PluginsView.Audio_SelectedIndex = 0;
                            mainwindow.cboPlugin_Audio.SelectedIndex = 0;
                        }
                    }
                }
                // Null Fallback
                else
                {
                    //VM.PluginsView.Audio_SelectedIndex = 0;
                    mainwindow.cboPlugin_Audio.SelectedIndex = 0;
                }

                // -------------------------
                // RSP
                // -------------------------
                List<string> rspPluginNames = VM.PluginsView.RSP_Items.Select(item => item.Name).ToList();
                string rspPlugin = Path.GetFileName(cfg.Read("UI-Console", "RSPPlugin"));

                // Selected
                if (!string.IsNullOrWhiteSpace(rspPlugin))
                {
                    // Default
                    if (rspPluginNames.Contains(rspPlugin))
                    {
                        VM.PluginsView.RSP_SelectedItem = rspPlugin;
                    }
                    // Missing Fallback
                    else
                    {
                        //MessageBox.Show("Missing Fallback"); //debug
                        // Default
                        if (rspPluginNames.Contains("mupen64plus-rsp-hle.dll"))
                        {
                            //MessageBox.Show("Default"); //debug
                            VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-hle.dll";
                        }
                        // Missing
                        else
                        {
                            //MessageBox.Show("Missing"); //debug
                            //VM.PluginsView.RSP_SelectedIndex = 0;
                            mainwindow.cboPlugin_RSP.SelectedIndex = 0;
                        }
                    }
                }
                // Null Fallback
                else
                {
                    //VM.PluginsView.RSP_SelectedIndex = 0;
                    mainwindow.cboPlugin_RSP.SelectedIndex = 0;
                }

                // -------------------------
                // Input
                // -------------------------
                List<string> inputPluginNames = VM.PluginsView.Input_Items.Select(item => item.Name).ToList();
                string inputPlugin = Path.GetFileName(cfg.Read("UI-Console", "InputPlugin"));

                // Selected
                if (!string.IsNullOrWhiteSpace(inputPlugin))
                {
                    // Default
                    if (inputPluginNames.Contains(inputPlugin))
                    {
                        VM.PluginsView.Input_SelectedItem = inputPlugin;
                    }
                    // Missing Fallback
                    else
                    {
                        //MessageBox.Show("Missing Fallback"); //debug
                        // Default
                        if (inputPluginNames.Contains("mupen64plus-input-sdl.dll"))
                        {
                            //MessageBox.Show("Default"); //debug
                            VM.PluginsView.Input_SelectedItem = "mupen64plus-input-sdl.dll";
                        }
                        // Missing
                        else
                        {
                            //MessageBox.Show("Missing"); //debug
                            //VM.PluginsView.Input_SelectedIndex = 0;
                            mainwindow.cboPlugin_Input.SelectedIndex = 0;
                        }
                    }
                }
                // Null Fallback
                else
                {
                    //VM.PluginsView.Input_SelectedIndex = 0;
                    mainwindow.cboPlugin_Input.SelectedIndex = 0;
                }
            }
            // Error
            catch (Exception ex)
            {
                MessageBox.Show("Problem importing selected plugins from mupen64plus.cfg.\r\n\r\n" + ex.ToString(),
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Write mupen64plus.cfg
        /// </summary>
        public static void WriteMupen64PlusCfg(string directory,
                                               string filename,
                                               List<Action> actionsToWrite
                                              )
        {
            // -------------------------
            // Check if Directory Exists
            // -------------------------
            if (!Directory.Exists(directory))
            {
                try
                {
                    // Create Config Directory
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create config folder " + directory + ".\n\nMay require Administrator privileges.\r\n\r\n" +
                                    ex.ToString(),
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }

            // -------------------------
            // Save mupen64plus.cfg file if directory exists
            // -------------------------
            // Check if Conf File Exists
            if (Directory.Exists(directory))
            {
                //MessageBox.Show(directory); //debug

                //Configure.ConigFile cfg = null;

                // Access
                if (MainWindow.hasWriteAccessToFolder(directory) == false)
                {
                    // Denied
                    MessageBox.Show("User does not have write access to " + directory,
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    // Start mupen64plus.cfg file write
                    Configure.ConigFile.cfg = new Configure.ConigFile(Path.Combine(directory, filename));

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
        }

    }
}
