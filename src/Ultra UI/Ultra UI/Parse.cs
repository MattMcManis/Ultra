/* ----------------------------------------------------------------------
Ultra UI
Copyright (C) 2019 Matt McManis
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
    public class Parse
    {
        // 1. Press Reload Button to scan PC Directory for ROMs
        // 2. Saves ROMs List to ultra.conf [ROMs]
        // 3. Application Parses ultra.conf for ROMs at startup
        // 4. Parsing ultra.conf is faster than Scanning PC Directory every time application starts up

        /// <summary>
        /// Scan Game Files from PC
        /// </summary>
        public static void ScanGameFiles()
        {
            // Check if Paths ROMs TextBox is Empty
            if (MainWindow.IsValidPath(VM.PathsView.ROMs_Text))
            {
                // Check if Directory exists
                if (Directory.Exists(VM.PathsView.ROMs_Text.TrimEnd('\\') + @"\"))
                {
                    List<string> romsList = new List<string>();

                    // Scan PC Directory
                    try
                    {
                        romsList = Directory.GetFiles(VM.PathsView.ROMs_Text, "*.*", SearchOption.AllDirectories)
                                   .Where(file => file.ToLower().EndsWith("n64") || file.ToLower().EndsWith("v64") || file.ToLower().EndsWith("z64"))
                                   .ToList();

                        // ROM files not found
                        if (romsList.Count <= 0)
                        {
                            MessageBox.Show("N64 ROM files (.n64, .v64, .z64) not found.",
                                            "Notice",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                        }
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Could not scan ROMs folder.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }

                    // -------------------------
                    // Write Game Paths to ultra.conf
                    // -------------------------
                    Configure.ConigFile conf = null; // Initialize Read & Write

                    try
                    {
                        // Read
                        conf = new Configure.ConigFile(MainWindow.ultraConfFile);
                    }
                    // Error
                    catch
                    {
                        //MessageBox.Show("Could not save ROMs list to ultra.conf.",
                        //                "Error",
                        //                MessageBoxButton.OK,
                        //                MessageBoxImage.Error);
                    }

                    // Only if conf can be read/written
                    if (conf != null)
                    {
                        // -------------------------
                        // [ROMs]
                        // -------------------------
                        // Clear ROMs List
                        List<string> romsConfList = new List<string>();

                        // Count ROM Paths in ultra.conf
                        for (int i = 0; i < 999; i++)
                        {
                            // Read
                            romsConfList.Add(conf.Read("ROMs", i.ToString()));
                        }
                        romsConfList = romsConfList.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                        // -------------------------
                        // Write ROM File Paths to ultra.conf
                        // -------------------------
                        // ultra.conf actions to write
                        List<Action> actionsToWrite = new List<Action>
                        {
                            // Clear Missing ROM Paths
                            new Action(() =>
                            {
                                for (int i = 0; i < romsConfList.Count; i++)
                                {
                                    Configure.ConigFile.conf.Write("ROMs", i.ToString(), string.Empty);
                                }
                            }),

                            // Write ROM Paths
                            new Action(() =>
                            {
                                for (int i = 0; i < romsList.Count; i++)
                                {
                                    Configure.ConigFile.conf.Write("ROMs", i.ToString(), romsList[i]);
                                }
                            }),
                        };

                        // Save Config
                        Configure.WriteUltraConf(MainWindow.ultraConfDir, // Directory: %AppData%\Ultra UI\
                                                 "ultra.conf",            // Filename
                                                 actionsToWrite           // Actions to write
                                                 );
                    }
                    
                }
                // ROM Path Error
                else
                {
                    MessageBox.Show("ROMs folder does not exist.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            // ROM Path Error
            else
            {
                MessageBox.Show("ROMs Path is empty.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }


        /// <summary>
        /// Parse Games List from ultra.conf
        /// </summary>
        public static void ParseGamesList()
        {
            // -------------------------
            // Parse ultra.conf
            // -------------------------
            Configure.ConigFile conf = null;

            List<string> romsList = new List<string>();

            // [ROMs]
            try
            {
                conf = new Configure.ConigFile(MainWindow.ultraConfFile);

                for (int i = 0; i < 999; i++)
                {
                    // Read
                    // Add Game Paths from ultra.conf to List
                    romsList.Add(conf.Read("ROMs", i.ToString()));

                    romsList = romsList.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                }
            }
            catch
            {
                MessageBox.Show("Could not read ROMs list from ultra.conf.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            // -------------------------
            // View Games List
            // -------------------------
            try
            {
                // Clear Games List to prevent doubling up
                if (VM.MainView.Games_Items != null)
                {
                    VM.MainView.Games_Items.Clear();
                }

                // Alphabetize
                romsList.Sort();

                // Add ROMs to Games ListView
                for (int i = 0; i < romsList.Count; i++)
                {
                    VM.MainView.Games_Items.Add(new MainViewModel.Games()
                    {
                        FullPath = romsList[i],
                        Directory = System.IO.Path.GetDirectoryName(romsList[i]),
                        Name = System.IO.Path.GetFileNameWithoutExtension(romsList[i])
                    });
                }
            }
            catch
            {
                //Console.WriteLine("Problem parsing Games List. Please try another Proxy.");
                MessageBox.Show("Problem reading Games List.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Scan Plugins
        /// </summary>
        public static void ScanPlugins()
        {
            // Check if Paths Plugins TextBox is Empty
            if (MainWindow.IsValidPath(VM.PathsView.Plugins_Text))
            {
                // Check if Directory exists
                if (Directory.Exists(VM.PathsView.Plugins_Text))
                {
                    // Label Notice Clear
                    VM.PluginsView.PluginsErrorNotice_Text = "";

                    // -------------------------
                    // Found Plugins
                    // -------------------------
                    // Video
                    List<string> foundVideoPluginsList = new List<string>();
                    // Audio
                    List<string> foundAudioPluginsList = new List<string>();
                    // Input
                    List<string> foundInputPluginsList = new List<string>();
                    // RSP
                    List<string> foundRSPPluginsList = new List<string>();

                    // -------------------------
                    // Scan PC Directory
                    // -------------------------
                    try
                    {
                        // Video
                        foundVideoPluginsList = Directory
                                               .EnumerateFiles(VM.PathsView.Plugins_Text)
                                               //.Select(System.IO.Path.GetFileName)
                                               .Select(System.IO.Path.GetFullPath)
                                               .Where(file => file.ToLower().Contains("mupen64plus-video-"))
                                               .Where(file => file.ToLower().EndsWith("dll"))
                                               .ToList();

                        // Audio
                        foundAudioPluginsList = Directory
                                               .EnumerateFiles(VM.PathsView.Plugins_Text)
                                               //.Select(System.IO.Path.GetFileName)
                                               .Select(System.IO.Path.GetFullPath)
                                               .Where(file => file.ToLower().Contains("mupen64plus-audio-"))
                                               .Where(file => file.ToLower().EndsWith("dll"))
                                               .ToList();

                        // Input
                        foundInputPluginsList = Directory
                                               .EnumerateFiles(VM.PathsView.Plugins_Text)
                                               //.Select(System.IO.Path.GetFileName)
                                               .Select(System.IO.Path.GetFullPath)
                                               .Where(file => file.ToLower().Contains("mupen64plus-input-"))
                                               .Where(file => file.ToLower().EndsWith("dll"))
                                               .ToList();

                        // RSP
                        foundRSPPluginsList = Directory
                                              .EnumerateFiles(VM.PathsView.Plugins_Text)
                                              //.Select(System.IO.Path.GetFileName)
                                              .Select(System.IO.Path.GetFullPath)
                                              .Where(file => file.ToLower().Contains("mupen64plus-rsp-"))
                                              .Where(file => file.ToLower().EndsWith("dll"))
                                              .ToList();

                        //MessageBox.Show(string.Join("\r\n", foundVideoPluginsList)); //debug
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Could not scan Plugins folder.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }

                    // -------------------------
                    // Clear before adding items again
                    // -------------------------
                    // Video
                    if (VM.PluginsView.Video_Items != null &&
                        VM.PluginsView.Video_Items.Count > 0)
                    {
                        VM.PluginsView.Video_Items.Clear();
                    }
                    // Audio
                    if (VM.PluginsView.Audio_Items != null &&
                        VM.PluginsView.Audio_Items.Count > 0)
                    {
                        VM.PluginsView.Audio_Items.Clear();
                    }
                    // Input
                    if (VM.PluginsView.Input_Items != null &&
                        VM.PluginsView.Input_Items.Count > 0)
                    {
                        VM.PluginsView.Input_Items.Clear();
                    }
                    // RSP
                    if (VM.PluginsView.RSP_Items != null &&
                        VM.PluginsView.RSP_Items.Count > 0)
                    {
                        VM.PluginsView.RSP_Items.Clear();
                    }

                    // --------------------------------------------------
                    // Add Plugins to ComboBoxes
                    // --------------------------------------------------
                    // -------------------------
                    // Video
                    // -------------------------
                    try
                    {
                        if (foundVideoPluginsList.Count > 0)
                        {
                            for (int i = 0; i < foundVideoPluginsList.Count; i++)
                            {
                                //VM.PluginsView.Video_Items.Add(foundVideoPluginsList[i]);

                                VM.PluginsView.Video_Items.Add(
                                        new PluginsViewModel.Video()
                                        {
                                            FullPath = foundVideoPluginsList[i],
                                            Directory = Path.GetDirectoryName(foundVideoPluginsList[i]),
                                            Name = Path.GetFileName(foundVideoPluginsList[i])
                                        }
                                    );
                            }
                        }
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Problem adding Video Plugin dll's to dropdown menu.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }

                    // -------------------------
                    // Audio
                    // -------------------------
                    try
                    {
                        if (foundAudioPluginsList.Count > 0)
                        {
                            for (int i = 0; i < foundAudioPluginsList.Count; i++)
                            {
                                //VM.PluginsView.Audio_Items.Add(foundAudioPluginsList[i]);

                                VM.PluginsView.Audio_Items.Add(
                                        new PluginsViewModel.Audio()
                                        {
                                            FullPath = foundAudioPluginsList[i],
                                            Directory = Path.GetDirectoryName(foundAudioPluginsList[i]),
                                            Name = Path.GetFileName(foundAudioPluginsList[i])
                                        }
                                    );
                            }
                        }
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Problem adding Audio Plugin dll's to dropdown menu.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }

                    // -------------------------
                    // Input
                    // -------------------------
                    try
                    {
                        if (foundInputPluginsList.Count > 0)
                        {
                            for (int i = 0; i < foundInputPluginsList.Count; i++)
                            {
                                //VM.PluginsView.Input_Items.Add(foundInputPluginsList[i]);

                                VM.PluginsView.Input_Items.Add(
                                        new PluginsViewModel.Input()
                                        {
                                            FullPath = foundInputPluginsList[i],
                                            Directory = Path.GetDirectoryName(foundInputPluginsList[i]),
                                            Name = Path.GetFileName(foundInputPluginsList[i])
                                        }
                                    );
                            }
                        }
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Problem adding Input Plugin dll's to dropdown menu.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }

                    // -------------------------
                    // RSP
                    // -------------------------
                    try
                    {
                        if (foundRSPPluginsList.Count > 0)
                        {
                            for (int i = 0; i < foundRSPPluginsList.Count; i++)
                            {
                                //VM.PluginsView.RSP_Items.Add(foundRSPPluginsList[i]);

                                VM.PluginsView.RSP_Items.Add(
                                        new PluginsViewModel.RSP()
                                        {
                                            FullPath = foundRSPPluginsList[i],
                                            Directory = Path.GetDirectoryName(foundRSPPluginsList[i]),
                                            Name = Path.GetFileName(foundRSPPluginsList[i])
                                        }
                                    );
                            }
                        }
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Problem adding RSP Plugin dll's to dropdown menu.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
                // Error
                else
                {
                    // mupen64plus.cfg must be loaded first in oder to detect Plugins Path
                    // Plugins Path is read from mupen64plus.cfg
                    if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
                    {
                        VM.PluginsView.PluginsErrorNotice_Text = "Error: Could not locate Plugins folder.";
                        //MessageBox.Show("Could not locate Plugins folder.",
                        //                "Error",
                        //                MessageBoxButton.OK,
                        //                MessageBoxImage.Error);
                    }
                }
            }

        }



    }
}
