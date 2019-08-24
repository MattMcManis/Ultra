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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Ultra
{
    public class Game
    {
        //private MainWindow mainwindow = (MainWindow)System.Windows.Application.Current.MainWindow;

        /// <summary>
        /// Play
        /// </summary>
        /// <remarks>
        /// Runs Threaded Launch() method
        /// </remarks>
        public static void Play(string rom)
        {
            // -------------------------
            // Set Mupen64Plus dll 
            // -------------------------
            MainWindow.mupen64plusDll = MainWindow.SetMupen64PlusDll();

            // -------------------------
            // Check for Mupen64Plus
            // -------------------------
            //if (!string.IsNullOrEmpty(MainWindow.mupen64plusExe))
            if (!string.IsNullOrEmpty(MainWindow.mupen64plusDll))
            {
                //if (File.Exists(MainWindow.mupen64plusExe))
                if (File.Exists(MainWindow.mupen64plusDll))
                {
                    // Check for Plugins
                    if (!string.IsNullOrEmpty(VM.PluginsView.Video_SelectedItem) &&
                        !string.IsNullOrEmpty(VM.PluginsView.Audio_SelectedItem) &&
                        !string.IsNullOrEmpty(VM.PluginsView.Input_SelectedItem) &&
                        !string.IsNullOrEmpty(VM.PluginsView.RSP_SelectedItem)
                        )
                    {
                        // Check for ROM
                        if (File.Exists(rom))
                        {
                            // Enable Menu Items
                            MainWindow.EnableMenuItems();

                            // -------------------------
                            // Load ROM into memory
                            // -------------------------
                            byte[] romBuffer = File.ReadAllBytes(rom);

                            // -------------------------
                            // Set Plugins 
                            // -------------------------
                            // (Must be here, after Import Mupen64PlusCfg, which loads Plugins Path)
                            string videoPlugin = VM.PluginsView.Video_Items.FirstOrDefault(item => item.Name == VM.PluginsView.Video_SelectedItem)?.FullPath;
                            string audioPlugin = VM.PluginsView.Audio_Items.FirstOrDefault(item => item.Name == VM.PluginsView.Audio_SelectedItem)?.FullPath;
                            string inputPlugin = VM.PluginsView.Input_Items.FirstOrDefault(item => item.Name == VM.PluginsView.Input_SelectedItem)?.FullPath;
                            string rspPlugin = VM.PluginsView.RSP_Items.FirstOrDefault(item => item.Name == VM.PluginsView.RSP_SelectedItem)?.FullPath;
                            //MessageBox.Show(video); //debug

                            // -------------------------
                            // Set Window Resolution
                            // -------------------------
                            List<string> resolution = new List<string>();
                            resolution = VM.DisplayView.Display_Resolution_SelectedItem.Split('x').ToList();

                            int windowWidth = 0;
                            int.TryParse(resolution[0], out windowWidth);

                            int windowHeight = 0;
                            int.TryParse(resolution[1], out windowHeight);

                            // -------------------------
                            // Reset the Label Notice
                            // -------------------------
                            var task = Task.Factory.StartNew(new Action(CfgExitsCheck));

                            // -------------------------
                            // Launch Game
                            // -------------------------
                            Thread t = new Thread(() => 
                                PlayProcess(romBuffer,
                                            videoPlugin,
                                            audioPlugin,
                                            inputPlugin,
                                            rspPlugin,
                                            windowWidth,
                                            windowHeight
                                            )
                            );
                            t.Start();
                        }
                        // ROM Error
                        else
                        {
                            MessageBox.Show(rom + " does not exist.",
                                            "Notice",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                        }
                    }
                    // Plugin Not Set
                    else
                    {
                        MessageBox.Show("Not all Plugins are set.",
                                        "Notice",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }
                }
                // Mupen64Plus dll Error
                else
                {
                    MessageBox.Show("Could not find " + MainWindow.mupen64plusDll,
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }
            }
            // Mupen64Plus Path Error
            else
            {
                MessageBox.Show("Mupen64Plus Path not set.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }

        }
        /// <summary>
        /// Play Process
        /// </summary>
        public static void PlayProcess(byte[] romBuffer,
                                       string videoPlugin,
                                       string audioPlugin,
                                       string inputPlugin,
                                       string rspPlugin,
                                       int windowWidth,
                                       int windowHeight
                                       )
        {
            // Launch Game
            Mupen64PlusAPI.api = new Mupen64PlusAPI();
            Mupen64PlusAPI.api.Launch(
                    romBuffer,
                    videoPlugin,
                    audioPlugin,
                    inputPlugin,
                    rspPlugin,
                    windowWidth,
                    windowHeight
            );

            // Waits until Launch() Mupen64Plus exists

            // Close Mupe64Plus
            Mupen64PlusAPI.api.Dispose();
            Mupen64PlusAPI.api = null;
            GC.Collect();

            // Disable Menu Items
            MainWindow.DisableMenuItems();
        }


        /// <summary>
        /// Cfg Exists check
        /// </summary>
        /// <remarks>
        /// After 3 seconds Reset the "Notice: mupen64plus.cfg not found" if it exists
        /// </remarks>
        public static void /*async Task<int>*/ CfgExitsCheck()
        {
            //int count = 0;
            //await Task.Factory.StartNew(() =>
            //{
                // Sleep 5 seconds
                Thread.Sleep(5000);

                if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
                {
                    VM.MainView.CfgErrorNotice_Text = "";
                }
            //});

            //return count;
        }

    }
}
