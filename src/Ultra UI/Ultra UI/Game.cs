/* ----------------------------------------------------------------------
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Ultra
{
    public class Game
    {
        // Order of Operations
        // 1. Play
        //     2. Mupen64Plus Dll Check
        //     3. ROM Check
        //     4. Set Plugins   
        //     5. PlayProcess (New Task)
        //         6. Launch
        //             7. Initiate Mupen64Plus API
        //             8. Launch Game
        //         9. Dispose Thread

        //private MainWindow mainwindow = (MainWindow)System.Windows.Application.Current.MainWindow;

        //public static Thread m64pEmulator { get; set; }
        public static Task m64pEmulator { get; set; }

        /// <summary>
        /// Play
        /// </summary>
        /// <remarks>
        /// Runs Threaded Launch() method
        /// </remarks>
        //public static async void Play(string rom)
        //{
        //    Task<int> task = PlayAsync(rom);
        //    int count = await task;
        //}
        public static void Play(string rom)
        //public static async Task<int> PlayAsync(string rom)
        {
            //int count = 0;
            //await Task.Factory.StartNew(() =>
            //{
                // -------------------------
                // Set Mupen64Plus dll 
                // -------------------------
                MainWindow.mupen64plusDll = MainWindow.SetMupen64PlusDll();

                // -------------------------
                // Error Halts
                // -------------------------
                // Mupen64Plus DLL not set
                if (string.IsNullOrEmpty(MainWindow.mupen64plusDll) ||
                    string.IsNullOrWhiteSpace(MainWindow.mupen64plusDll))
                {
                    MessageBox.Show("Mupen64Plus Path not set.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                    return;
                }

                // Mupen64Plus DLL Path not valid
                if (MainWindow.IsValidPath(MainWindow.mupen64plusDll) == false)
                {
                    MessageBox.Show("Mupen64Plus Path is not valid.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                    return;
                }

                // Mupen64Plus DLL does not exist
                if (!File.Exists(MainWindow.mupen64plusDll))
                {
                    MessageBox.Show("Could not find " + MainWindow.mupen64plusDll,
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                    return;
                }

                // Check if Plugins are set
                if (string.IsNullOrEmpty(VM.PluginsView.Video_SelectedItem) ||
                    string.IsNullOrEmpty(VM.PluginsView.Audio_SelectedItem) ||
                    string.IsNullOrEmpty(VM.PluginsView.Input_SelectedItem) ||
                    string.IsNullOrEmpty(VM.PluginsView.RSP_SelectedItem)
                    )
                {
                    MessageBox.Show("Not all Plugins are set.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                    return;
                }

                // Check if ROM exists
                // String Empty
                if (string.IsNullOrEmpty(rom))
                {
                    MessageBox.Show("ROM is empty",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                    return;
                }
                // Does not exist
                if (!File.Exists(rom))
                {
                    MessageBox.Show(rom + " does not exist.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                    return;
                }

                // Check if ROM is valid
                // Check if file size is 0
                long romFileSize = new FileInfo(rom).Length;
                if (romFileSize == 0)
                {
                    MessageBox.Show("Not a valid ROM file.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                    return;
                }

                // -------------------------
                // Load ROM into memory
                // -------------------------
                byte[] romBuffer = File.ReadAllBytes(rom);

                // Check if ROM is empty
                if (romBuffer == null || romBuffer.Length == 0)
                {
                    MessageBox.Show("Problem reading ROM.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                    return;
                }

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
                int windowWidth = 0;
                int windowHeight = 0;
                if (!string.IsNullOrEmpty(VM.DisplayView.Display_Resolution_SelectedItem))
                {
                    List<string> resolution = new List<string>();
                    resolution = VM.DisplayView.Display_Resolution_SelectedItem.Split('x').ToList();

                    windowWidth = 0;
                    int.TryParse(resolution[0], out windowWidth);

                    windowHeight = 0;
                    int.TryParse(resolution[1], out windowHeight);
                }
                // Safe Defaults
                else
                {
                    windowWidth = 960;
                    windowHeight = 720;
                }

                // -------------------------
                // Reset the Label Notice
                // -------------------------
                var task = Task.Factory.StartNew(new Action(CfgExitsCheck));

                // -------------------------
                // Enable Menu Items
                // -------------------------
                MainWindow.EnableMenuItems();

                // -------------------------
                // Launch Game
                // -------------------------
                //Thread t = new Thread(() =>
                //m64pEmulator = new Thread(() =>
                //    PlayProcess(romBuffer,
                //                    videoPlugin,
                //                    audioPlugin,
                //                    inputPlugin,
                //                    rspPlugin,
                //                    windowWidth,
                //                    windowHeight
                //                   )
                //    );
                //m64pEmulator.Start();


            m64pEmulator = Task.Run(() => PlayProcess(romBuffer,
                                    videoPlugin,
                                    audioPlugin,
                                    inputPlugin,
                                    rspPlugin,
                                    windowWidth,
                                    windowHeight
                                   )

            );

            //PlayProcess(romBuffer,
            //            videoPlugin,
            //            audioPlugin,
            //            inputPlugin,
            //            rspPlugin,
            //            windowWidth,
            //            windowHeight
            //           );
            //});

            //return count;
        }

        /// <summary>
        /// Play Process (Thread)
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
                    "play",
                    romBuffer,
                    videoPlugin,
                    audioPlugin,
                    inputPlugin,
                    rspPlugin,
                    windowWidth,
                    windowHeight
            );

            //Mupen64PlusAPI.api.AsyncExecuteEmulator();

            // Waits until Launch() Mupen64Plus exits

            // Close Mupe64Plus Window
            // Note: Closing does not Dispose(), you must call it manually
            // Only dispose if exited from window
            // and not called from the Stop button
            if (MainWindow.stopped == false)
            {
                Mupen64PlusAPI.api.Dispose();
            }

            // Reset the Stopped trigger
            MainWindow.stopped = false;

            // Reset API
            Mupen64PlusAPI.api = null; 

            GC.Collect();

            // Disable Menu Items
            MainWindow.DisableMenuItems();
        }


        /// <summary>
        /// Cfg Exists check
        /// </summary>
        /// <remarks>
        /// After 5 seconds Reset the "Notice: mupen64plus.cfg not found" if it exists
        /// </remarks>
        private static void CfgExitsCheck()
        {
            // Wait 5 seconds to allow mupen64plus.cfg to be created
            Thread.Sleep(5000);

            // Update Label Notice
            if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
            {
                VM.MainView.CfgErrorNotice_Text = "";
            }
        }

    }
}
