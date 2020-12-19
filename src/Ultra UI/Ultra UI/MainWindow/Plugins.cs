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
            // -------------------------
            // Open
            // -------------------------
            if (VM.PluginsView.Video_SelectedItem == "mupen64plus-video-GLideN64.dll")
            {
                OpenGLideN64Window();
            }
            // -------------------------
            // Deny
            // -------------------------
            else
            {
                MessageBox.Show("Cannot currently configure " + VM.PluginsView.Video_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        // GLideN64
        public static GLideN64Window gliden64Window;
        private Boolean IsGLideN64WindowOpened = false;
        public void OpenGLideN64Window()
        {
            // Check if Window is already open
            if (IsGLideN64WindowOpened) return;

            // Start Window
            gliden64Window = new GLideN64Window();

            // Only allow 1 Window instance
            gliden64Window.ContentRendered += delegate { IsGLideN64WindowOpened = true; };
            gliden64Window.Closed += delegate { IsGLideN64WindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (gliden64Window.Width > Width)
            {
                gliden64Window.Left = Left - ((gliden64Window.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                gliden64Window.Left = Math.Max((Left + (Width - gliden64Window.Width) / 2), Left);
            }

            gliden64Window.Top = Math.Max((Top + (Height - gliden64Window.Height) / 2), Top);

            // Open Window
            gliden64Window.Show();
        }

        // Glide64mk2
        public static Glide64mk2Window glide64mk2Window;
        private Boolean IsGlide64mk2WindowOpened = false;
        public void OpenGlide64mk2Window()
        {
            // Check if Window is already open
            if (IsGlide64mk2WindowOpened) return;

            // Start Window
            glide64mk2Window = new Glide64mk2Window();

            // Only allow 1 Window instance
            glide64mk2Window.ContentRendered += delegate { IsGlide64mk2WindowOpened = true; };
            glide64mk2Window.Closed += delegate { IsGlide64mk2WindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (glide64mk2Window.Width > Width)
            {
                glide64mk2Window.Left = Left - ((glide64mk2Window.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                glide64mk2Window.Left = Math.Max((Left + (Width - glide64mk2Window.Width) / 2), Left);
            }

            glide64mk2Window.Top = Math.Max((Top + (Height - glide64mk2Window.Height) / 2), Top);

            // Open Window
            glide64mk2Window.Show();
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
            // -------------------------
            // Open
            // -------------------------
            if (VM.PluginsView.Audio_SelectedItem == "mupen64plus-audio-sdl.dll")
            {
                OpenAudioSDLWindow();
            }
            // -------------------------
            // Deny
            // -------------------------
            else
            {
                MessageBox.Show("Cannot currently configure " + VM.PluginsView.Audio_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        // Audio SDL
        public static AudioSDLWindow audioSDLWindow;
        private Boolean IsAudioSDLWindowOpened = false;
        public void OpenAudioSDLWindow()
        {
            // Check if Window is already open
            if (IsAudioSDLWindowOpened) return;

            // Start Window
            audioSDLWindow = new AudioSDLWindow();

            // Only allow 1 Window instance
            audioSDLWindow.ContentRendered += delegate { IsAudioSDLWindowOpened = true; };
            audioSDLWindow.Closed += delegate { IsAudioSDLWindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (audioSDLWindow.Width > Width)
            {
                audioSDLWindow.Left = Left - ((audioSDLWindow.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                audioSDLWindow.Left = Math.Max((Left + (Width - audioSDLWindow.Width) / 2), Left);
            }

            audioSDLWindow.Top = Math.Max((Top + (Height - audioSDLWindow.Height) / 2), Top);

            // Open Window
            audioSDLWindow.Show();
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
            // Open
            switch (VM.PluginsView.RSP_SelectedItem)
            {
                // RSP HLE
                case "mupen64plus-rsp-hle.dll":
                    OpenRSPHLEWindow();
                    break;

                // cxd4 SSSE3
                case "mupen64plus-rsp-cxd4-ssse3.dll":
                    OpenRSPcxd4SSSE3Window();
                    break;

                // Deny
                default:
                    MessageBox.Show("Cannot currently configure " + VM.PluginsView.RSP_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                               "Notice",
                               MessageBoxButton.OK,
                               MessageBoxImage.Information);
                    break;
            }
        }

        /// <summary>
        /// RSPHLE Window
        /// </summary>
        public static RSPHLEWindow rsphleWindow;
        private Boolean IsRSPHLEWindowOpened = false;
        public void OpenRSPHLEWindow()
        {
            // Check if Window is already open
            if (IsRSPHLEWindowOpened) return;

            // Start Window
            rsphleWindow = new RSPHLEWindow();

            // Only allow 1 Window instance
            rsphleWindow.ContentRendered += delegate { IsRSPHLEWindowOpened = true; };
            rsphleWindow.Closed += delegate { IsRSPHLEWindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (rsphleWindow.Width > Width)
            {
                rsphleWindow.Left = Left - ((rsphleWindow.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                rsphleWindow.Left = Math.Max((Left + (Width - rsphleWindow.Width) / 2), Left);
            }

            rsphleWindow.Top = Math.Max((Top + (Height - rsphleWindow.Height) / 2), Top);

            // Open Window
            rsphleWindow.Show();
        }

        /// <summary>
        /// RSP cxd4 SSSE3 Window
        /// </summary>
        public static RSPcxd4SSSE3Window cxd4SSSE3;
        private Boolean IsRSPcxd4SSSE3WindowOpened = false;
        public void OpenRSPcxd4SSSE3Window()
        {
            // Check if Window is already open
            if (IsRSPcxd4SSSE3WindowOpened) return;

            // Start Window
            cxd4SSSE3 = new RSPcxd4SSSE3Window();

            // Only allow 1 Window instance
            cxd4SSSE3.ContentRendered += delegate { IsRSPcxd4SSSE3WindowOpened = true; };
            cxd4SSSE3.Closed += delegate { IsRSPcxd4SSSE3WindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (cxd4SSSE3.Width > Width)
            {
                cxd4SSSE3.Left = Left - ((cxd4SSSE3.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                cxd4SSSE3.Left = Math.Max((Left + (Width - cxd4SSSE3.Width) / 2), Left);
            }

            cxd4SSSE3.Top = Math.Max((Top + (Height - cxd4SSSE3.Height) / 2), Top);

            // Open Window
            cxd4SSSE3.Show();
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
            // -------------------------
            // Open
            // -------------------------
            if (VM.PluginsView.Input_SelectedItem == "mupen64plus-input-sdl.dll")
            {
                OpenInputWindow();
            }
            // -------------------------
            // Deny
            // -------------------------
            else
            {
                MessageBox.Show("Cannot currently configure " + VM.PluginsView.Input_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        public static InputSDLWindow inputSDLWindow;
        private Boolean IsInputSDLWindowOpened = false;
        public void OpenInputWindow()
        {
            // Check if Window is already open
            if (IsInputSDLWindowOpened) return;

            // Start Window
            inputSDLWindow = new InputSDLWindow();

            // Only allow 1 Window instance
            inputSDLWindow.ContentRendered += delegate { IsInputSDLWindowOpened = true; };
            inputSDLWindow.Closed += delegate { IsInputSDLWindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (inputSDLWindow.Width > Width)
            {
                inputSDLWindow.Left = Left - ((inputSDLWindow.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                inputSDLWindow.Left = Math.Max((Left + (Width - inputSDLWindow.Width) / 2), Left);
            }

            inputSDLWindow.Top = Math.Max((Top + (Height - inputSDLWindow.Height) / 2), Top);

            // Open Window
            inputSDLWindow.Show();
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
            // -------------------------
            // Video
            // -------------------------
            if (VM.PluginsView.Video_Items != null &&
                VM.PluginsView.Video_Items.Count > 0)
            {
                List<string> videoPluginNames = VM.PluginsView.Video_Items.Select(item => item.Name).ToList();

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
                    VM.PluginsView.Video_SelectedIndex = 0;
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
                    VM.PluginsView.Audio_SelectedIndex = 0;
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
                    VM.PluginsView.Input_SelectedIndex = 0;
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
                // cxd4 SSSE3
                else if (rspPluginNames.Contains("mupen64plus-rsp-cxd4-ssse3.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-cxd4-ssse3.dll";
                }
                // cxd4 SSE2
                else if (rspPluginNames.Contains("mupen64plus-rsp-cxd4-sse2.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-cxd4-sse2.dll";
                }
                // cxd4
                else if (rspPluginNames.Contains("mupen64plus-rsp-cxd4.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-cxd4.dll";
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
                    VM.PluginsView.RSP_SelectedIndex = 0;
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
                        Configure.ConigFile.cfg.Write("UI-Console", "VideoPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.Video_SelectedItem)/*VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + VM.PluginsView.Video_SelectedItem*/ + "\"");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

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
                        Configure.ConigFile.cfg.Write("UI-Console", "AudioPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.Audio_SelectedItem)/*VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + VM.PluginsView.Audio_SelectedItem*/ + "\"");
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
                        Configure.ConigFile.cfg.Write("UI-Console", "InputPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.Input_SelectedItem)/*VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + VM.PluginsView.Audio_SelectedItem*/ + "\"");
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
                        Configure.ConigFile.cfg.Write("UI-Console", "RspPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.RSP_SelectedItem)/*VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + VM.PluginsView.Audio_SelectedItem*/ + "\"");
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
