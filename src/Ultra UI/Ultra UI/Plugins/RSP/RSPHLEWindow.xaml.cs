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

using System.ComponentModel;
using System.IO;
using System.Windows;
using ViewModel;

namespace Ultra
{
    /// <summary>
    /// Interaction logic for RSPHLEWindow.xaml
    /// </summary>
    public partial class RSPHLEWindow : Window, MainWindow.IPluginWindow
    {
        public RSPHLEWindow()
        {
            InitializeComponent();

            // Set Theme Icon
            MainWindow.SetThemeIcon(this);

            // Load Control Values
            PluginCfgReader();
        }

        /// <summary>
        /// Window Loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Window Unloaded
        /// </summary>
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Window Closing
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
            // Reset Save ✓ Label
            VM.Plugins_RSP_RSPHLE_View.Save_Text = "";
        }

        /// <summary>
        /// Plugin Cfg Reader
        /// </summary>
        /// <remarks>
        /// Import Control Values from mupen64plus.cfg
        /// </remarks>
        private static void PluginCfgReader()
        {
            // Check if Paths Config TextBox is Empty
            if (MainWindow.IsValidPath(VM.PathsView.Config_Text))
            {
                // Check if Cfg File Exists
                if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
                {
                    // Start Cfg File Read
                    Configure.ConigFile cfg = null;

                    try
                    {
                        cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

                        // -------------------------
                        // [Rsp-HLE]
                        // -------------------------

                        // Version
                        VM.Plugins_RSP_RSPHLE_View.Version_Text = cfg.Read("Rsp-HLE", "Version");

                        // RSP Fallback
                        VM.Plugins_RSP_RSPHLE_View.RSPFallback_Text = cfg.Read("Rsp-HLE", "RspFallback");

                        // Display List To Graphics Plugin
                        bool displayListToGraphicsPlugin = true;
                        bool.TryParse(cfg.Read("Rsp-HLE", "DisplayListToGraphicsPlugin").ToLower(), out displayListToGraphicsPlugin);
                        VM.Plugins_RSP_RSPHLE_View.DisplayListToGraphicsPlugin_IsChecked = displayListToGraphicsPlugin;

                        //  Audio List To Audio Plugin
                        bool audioListToAudioPlugin = false;
                        bool.TryParse(cfg.Read("Rsp-HLE", "AudioListToAudioPlugin").ToLower(), out audioListToAudioPlugin);
                        VM.Plugins_RSP_RSPHLE_View.AudioListToAudioPlugin_IsChecked = audioListToAudioPlugin;
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Problem reading mupen64plus.cfg.",
                                        "Read Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
                // Error
                else
                {
                    MessageBox.Show("Could not find mupen64plus.cfg.",
                                    "Read Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            // Path not found
            else
            {
                MessageBox.Show("Config Path is empty.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
            {
                // Save to mupen64plus.cfg
                Configure.ConigFile cfg = null;

                try
                {
                    cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

                    // Version
                    cfg.Write("Rsp-HLE", "Version", VM.Plugins_RSP_RSPHLE_View.Version_Text);

                    // RSP Fallback
                    cfg.Write("Rsp-HLE", "RspFallback", "\"" + VM.Plugins_RSP_RSPHLE_View.RSPFallback_Text + "\"");

                    // Display List To Graphics Plugin
                    if (VM.Plugins_RSP_RSPHLE_View.DisplayListToGraphicsPlugin_IsChecked == true)
                    {
                        cfg.Write("Rsp-HLE", "DisplayListToGraphicsPlugin", "True");
                    }
                    else if (VM.Plugins_RSP_RSPHLE_View.DisplayListToGraphicsPlugin_IsChecked == false)
                    {
                        cfg.Write("Rsp-HLE", "DisplayListToGraphicsPlugin", "False");
                    }

                    //  Audio List To Audio Plugin
                    if (VM.Plugins_RSP_RSPHLE_View.AudioListToAudioPlugin_IsChecked == true)
                    {
                        cfg.Write("Rsp-HLE", "AudioListToAudioPlugin", "True");
                    }
                    else if (VM.Plugins_RSP_RSPHLE_View.AudioListToAudioPlugin_IsChecked == false)
                    {
                        cfg.Write("Rsp-HLE", "AudioListToAudioPlugin", "False");
                    }


                    // -------------------------
                    // Save Complete
                    // -------------------------
                    VM.Plugins_RSP_RSPHLE_View.Save_Text = "✓";
                }
                // Error
                catch
                {
                    VM.Plugins_RSP_RSPHLE_View.Save_Text = "Error";

                    MessageBox.Show("Could not save to mupen64plus.cfg.",
                                    "Write Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }


        /// <summary>
        /// Defaults
        /// </summary>
        private void btnDefaults_Click(object sender, RoutedEventArgs e)
        {
            //VM.Plugins_RSP_RSPHLE_View.Version_Text = "1";
            VM.Plugins_RSP_RSPHLE_View.RSPFallback_Text = "";
            VM.Plugins_RSP_RSPHLE_View.DisplayListToGraphicsPlugin_IsChecked = true;
            VM.Plugins_RSP_RSPHLE_View.AudioListToAudioPlugin_IsChecked = false;

            // Reset Save ✓ Label
            VM.Plugins_RSP_RSPHLE_View.Save_Text = "";
        }


        /// <summary>
        /// Close
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
