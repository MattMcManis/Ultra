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
using System.Windows;
using System.Windows.Controls;
using ViewModel;

namespace Ultra
{
    public partial class MainWindow : Window
    {
        private static Dictionary<string, IPluginWindow> _window;

        private static void InitializeWindows()
        {
            _window = new Dictionary<string, IPluginWindow>
            {
                // Video
                { "GLideN64",   new GLideN64Window() },
                { "Glide64mk2",   new Glide64mk2Window() },
                { "Angrylion Plus",   new AngrylionPlusWindow() },

                // Audio
                { "Audio SDL",   new AudioSDLWindow() },

                // Input
                { "Input SDL",   new InputSDLWindow() },

                // RSP
                { "RSPHLE",   new RSPHLEWindow() },
                { "CXD4",   new RSPcxd4Window() },
                //{ "CXD4 SSE2",   new RSPcxd4SSE2Window() },
                //{ "CXD4 SSSE3",   new RSPcxd4SSSE3Window() },
            };
        }

        public interface IPluginWindow
        {
        }

        // Plugin Window
        //public static PluginWindow pluginWindow;
        private static Boolean IsPluginWindowOpened = false;
        public void OpenPluginWindow(Window pluginWindow, string pluginName)
        {
            // Check if Window is already open
            if (IsPluginWindowOpened) return;

            // Start Window
            pluginWindow = (Window)_window[pluginName];

            // Only allow 1 Window instance
            pluginWindow.ContentRendered += delegate { IsPluginWindowOpened = true; };
            pluginWindow.Closed += delegate { IsPluginWindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (pluginWindow.Width > Width)
            {
                pluginWindow.Left = Left - ((pluginWindow.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                pluginWindow.Left = Math.Max((Left + (Width - pluginWindow.Width) / 2), Left);
            }

            pluginWindow.Top = Math.Max((Top + (Height - pluginWindow.Height) / 2), Top);

            // Open Window
            pluginWindow.Show();
        }



        /// <summary>
        /// Plugin Video - Button
        /// </summary>
        private void btnPlugin_Video_Configure_Click(object sender, RoutedEventArgs e)
        {
            OpenVideoConfigureWindow();
        }

        /// <summary>
        /// Open Video Configure Window
        /// </summary>
        public void OpenVideoConfigureWindow()
        {
            InitializeWindows();

            // Open
            switch (VM.PluginsView.Video_SelectedItem)
            {
                // GLideN64
                case "mupen64plus-video-GLideN64.dll":
                    OpenPluginWindow((Window)_window["GLideN64"], "GLideN64");
                    break;

                // Glide64mk2
                //case "mupen64plus-video-glide64mk2.dll":
                //    OpenPluginWindow((Window)_window["Glide64mk2"], "Glide64mk2");
                //    break;

                // Angrylion Plus
                case "mupen64plus-video-angrylion-plus.dll":
                    OpenPluginWindow((Window)_window["Angrylion Plus"], "Angrylion Plus");
                    break;

                // Unknown
                default:
                    MessageBox.Show("Cannot configure " + VM.PluginsView.Video_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                    break;
            }
        }


        /// <summary>
        /// Plugin Audio - Button
        /// </summary>
        private void btnPlugin_Audio_Configure_Click(object sender, RoutedEventArgs e)
        {
            OpenAudioConfigureWindow();
        }

        /// <summary>
        /// Open Audio Configure Window
        /// </summary>
        public void OpenAudioConfigureWindow()
        {
            InitializeWindows();

            // Open
            switch (VM.PluginsView.Audio_SelectedItem)
            {
                // Audio SDL
                case "mupen64plus-audio-sdl.dll":
                    OpenPluginWindow((Window)_window["Audio SDL"], "Audio SDL");
                    break;

                // Unknown
                default:
                    MessageBox.Show("Cannot configure " + VM.PluginsView.Audio_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                    break;
            }
        }


        /// <summary>
        /// Plugin RSP - Button
        /// </summary>
        private void btnPlugin_RSP_Configure_Click(object sender, RoutedEventArgs e)
        {
            OpenRSPConfigureWindow();
        }

        /// <summary>
        /// Open RSP Configure Window
        /// </summary>
        public void OpenRSPConfigureWindow()
        {
            InitializeWindows();

            // Open
            switch (VM.PluginsView.RSP_SelectedItem)
            {
                // RSP HLE
                case "mupen64plus-rsp-hle.dll":
                    OpenPluginWindow((Window)_window["RSPHLE"], "RSPHLE");
                    break;

                // CXD4
                case "mupen64plus-rsp-cxd4.dll":
                    OpenPluginWindow((Window)_window["CXD4"], "CXD4");
                    break;

                // CXD4 SSE2
                case "mupen64plus-rsp-cxd4-sse2.dll":
                    OpenPluginWindow((Window)_window["CXD4"], "CXD4");
                    //OpenPluginWindow((Window)_window["CXD4 SSE2"], "CXD4 SSE2");
                    break;

                // CXD4 SSSE3
                case "mupen64plus-rsp-cxd4-ssse3.dll":
                    OpenPluginWindow((Window)_window["CXD4"], "CXD4");
                    //OpenPluginWindow((Window)_window["CXD4 SSSE3"], "CXD4 SSSE3");
                    break;

                // Unknown
                default:
                    MessageBox.Show("Cannot configure " + VM.PluginsView.RSP_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                               "Notice",
                               MessageBoxButton.OK,
                               MessageBoxImage.Information);
                    break;
            }
        }


        /// <summary>
        /// Plugin Input - Button
        /// </summary>
        private void btnPlugin_Input_Configure_Click(object sender, RoutedEventArgs e)
        {
            OpenInputConfigureWindow();
        }

        /// <summary>
        /// Open Input Configure Window
        /// </summary>
        public void OpenInputConfigureWindow()
        {
            InitializeWindows();

            // Open
            switch (VM.PluginsView.Input_SelectedItem)
            {
                // Input SDL
                case "mupen64plus-input-sdl.dll":
                    OpenPluginWindow((Window)_window["Input SDL"], "Input SDL");
                    break;

                // Unknown
                default:
                    MessageBox.Show("Cannot configure " + VM.PluginsView.Input_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                    break;
            }
        }


        /// <summary>
        /// Plugins Defaults - Button
        /// </summary>
        // Button
        private void btnPluginsDefaults_Click(object sender, RoutedEventArgs e)
        {
            PluginDefaults();
        }

        public static void PluginDefaults()
        {
            //MessageBox.Show(string.Join("\r\n", VM.PluginsView.Video_Items));
            // -------------------------
            // Video
            // -------------------------
            if (VM.PluginsView.Video_Items != null &&
                VM.PluginsView.Video_Items.Count > 0)
            {
                List<string> videoPluginNames = VM.PluginsView.Video_Items.Select(item => item.Name).ToList();
                //MessageBox.Show(string.Join("\r\n", videoPluginNames)); //debug
                // GLideN64
                if (videoPluginNames.Contains("mupen64plus-video-GLideN64.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-GLideN64.dll";
                }
                // Glide64mk2
                else if (videoPluginNames.Contains("mupen64plus-video-glide64mk2.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-glide64mk2.dll";
                }
                // Glide64
                else if (videoPluginNames.Contains("mupen64plus-video-glide64.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-glide64.dll";
                }
                // Rice
                else if (videoPluginNames.Contains("mupen64plus-video-rice.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-rice.dll";
                }
                // Angrylion Plus
                else if (videoPluginNames.Contains("mupen64plus-video-angrylion-plus.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-angrylion-plus.dll";
                }
                // z64
                else if (videoPluginNames.Contains("mupen64plus-video-z64.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-z64.dll";
                }
                // Arachnoid
                else if (videoPluginNames.Contains("mupen64plus-video-arachnoid.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-arachnoid.dll";
                }
                // First
                else
                {
                    VM.PluginsView.Video_SelectedIndex = -1;
                }
            }

            // -------------------------
            // Audio
            // -------------------------
            if (VM.PluginsView.Audio_Items != null &&
                VM.PluginsView.Audio_Items.Count > 0)
            {
                List<string> audioPluginNames = VM.PluginsView.Audio_Items.Select(item => item.Name).ToList();

                // Audio SDL
                if (audioPluginNames.Contains("mupen64plus-audio-sdl.dll"))
                {
                    VM.PluginsView.Audio_SelectedItem = "mupen64plus-audio-sdl.dll";
                }
                // First
                else
                {
                    VM.PluginsView.Audio_SelectedIndex = -1;
                }
            }

            // -------------------------
            // Input
            // -------------------------
            if (VM.PluginsView.Input_Items != null &&
                VM.PluginsView.Input_Items.Count > 0)
            {
                List<string> inputPluginNames = VM.PluginsView.Input_Items.Select(item => item.Name).ToList();

                // Input SDL
                if (inputPluginNames.Contains("mupen64plus-input-sdl.dll"))
                {
                    VM.PluginsView.Input_SelectedItem = "mupen64plus-input-sdl.dll";
                }
                // First
                else
                {
                    VM.PluginsView.Input_SelectedIndex = -1;
                }
            }

            // -------------------------
            // RSP
            // -------------------------
            if (VM.PluginsView.RSP_Items != null &&
                VM.PluginsView.RSP_Items.Count > 0)
            {
                List<string> rspPluginNames = VM.PluginsView.RSP_Items.Select(item => item.Name).ToList();

                // RSP HLE
                if (rspPluginNames.Contains("mupen64plus-rsp-hle.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-hle.dll";
                }
                // CXD4
                else if (rspPluginNames.Contains("mupen64plus-rsp-cxd4.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-cxd4.dll";
                }
                // CXD4 SSE2
                else if (rspPluginNames.Contains("mupen64plus-rsp-cxd4-sse2.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-cxd4-sse2.dll";
                }
                // CXD4 SSSE3
                else if (rspPluginNames.Contains("mupen64plus-rsp-cxd4-ssse3.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-cxd4-ssse3.dll";
                }
                // z64
                else if (rspPluginNames.Contains("mupen64plus-rsp-z64.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-z64.dll";
                }
                // z64 HLE
                else if (rspPluginNames.Contains("mupen64plus-rsp-z64-hlevideo.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-z64-hlevideo.dll";
                }
                // First
                else
                {
                    VM.PluginsView.RSP_SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// Plugin - Video
        /// </summary>
        private void cboPlugin_Video_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    if (!string.IsNullOrEmpty(VM.PluginsView.Video_SelectedItem))
                    {
                        // -------------------------
                        // [UI-Console]
                        // -------------------------
                        Configure.ConigFile.cfg.Write("UI-Console", "VideoPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.Video_SelectedItem) + "\"");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        //public static void Plugin_Video_SelectionChanged()
        //{
        //    // Write to mupen64plus.cfg
        //    List<Action> actionsToWrite = new List<Action>
        //    {
        //        new Action(() =>
        //        {
        //            if (!string.IsNullOrEmpty(VM.PluginsView.Video_SelectedItem))
        //            {
        //                // -------------------------
        //                // [UI-Console]
        //                // -------------------------
        //                Configure.ConigFile.cfg.Write("UI-Console", "VideoPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.Video_SelectedItem) + "\"");
        //            }
        //        }),
        //    };

        //    MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
        //                                 "mupen64plus.cfg",        // Filename
        //                                 actionsToWrite            // Actions to write
        //                                );
        //}

        /// <summary>
        /// Plugin - Audio
        /// </summary>
        private void cboPlugin_Audio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    if (!string.IsNullOrEmpty(VM.PluginsView.Audio_SelectedItem))
                    {
                        // -------------------------
                        // [UI-Console]
                        // -------------------------
                        Configure.ConigFile.cfg.Write("UI-Console", "AudioPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.Audio_SelectedItem) + "\"");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Input
        /// </summary>
        private void cboPlugin_Input_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    if (!string.IsNullOrEmpty(VM.PluginsView.Input_SelectedItem))
                    {
                        // -------------------------
                        // [UI-Console]
                        // -------------------------
                        Configure.ConigFile.cfg.Write("UI-Console", "InputPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.Input_SelectedItem) + "\"");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// RSP
        /// </summary>
        private void cboPlugin_RSP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    if (!string.IsNullOrEmpty(VM.PluginsView.RSP_SelectedItem))
                    {
                        // -------------------------
                        // [UI-Console]
                        // -------------------------
                        Configure.ConigFile.cfg.Write("UI-Console", "RspPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.RSP_SelectedItem) + "\"");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }
    }
}
