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
    /// Interaction logic for AudioSDLWindow.xaml
    /// </summary>
    public partial class AudioSDLWindow : Window, MainWindow.IPluginWindow
    {
        public AudioSDLWindow()
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
        ///  Window Closing
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
            // Reset Save ✓ Label
            VM.Plugins_Audio_AudioSDL_View.Save_Text = "";
        }


        /// <summary>
        ///  Plugin Cfg Reader
        /// </summary>
        /// <remarks>
        ///  Import Control Values from mupen64plus.cfg
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

                        // Version
                        VM.Plugins_Audio_AudioSDL_View.Version_Text = cfg.Read("Audio-SDL", "Version");

                        // Default Frequency
                        VM.Plugins_Audio_AudioSDL_View.DefaultFrequency_Text = cfg.Read("Audio-SDL", "DEFAULT_FREQUENCY");

                        // Swap Channels
                        bool swapChannels = false;
                        bool.TryParse(cfg.Read("Audio-SDL", "SWAP_CHANNELS").ToLower(), out swapChannels);
                        VM.Plugins_Audio_AudioSDL_View.SwapChannels_IsChecked = swapChannels;

                        // Primary Buffer Size
                        VM.Plugins_Audio_AudioSDL_View.PrimaryBufferSize_Text = cfg.Read("Audio-SDL", "PRIMARY_BUFFER_SIZE");

                        // Primary Buffer Target
                        VM.Plugins_Audio_AudioSDL_View.PrimaryBufferTarget_Text = cfg.Read("Audio-SDL", "PRIMARY_BUFFER_TARGET");

                        // Secondary Buffer Size
                        VM.Plugins_Audio_AudioSDL_View.SecondaryBufferSize_Text = cfg.Read("Audio-SDL", "SECONDARY_BUFFER_SIZE");

                        // Resample
                        VM.Plugins_Audio_AudioSDL_View.Resample_SelectedItem = cfg.Read("Audio-SDL", "RESAMPLE");

                        // Volume Control Type
                        string volControlType = cfg.Read("Audio-SDL", "VOLUME_CONTROL_TYPE");
                        if (volControlType == "1")
                        {
                            VM.Plugins_Audio_AudioSDL_View.VolumeControlType_SelectedItem = "SDL";
                        }
                        else if (volControlType == "2")
                        {
                            VM.Plugins_Audio_AudioSDL_View.VolumeControlType_SelectedItem = "OSS mixer";
                        }

                        // Volume Adjust
                        VM.Plugins_Audio_AudioSDL_View.VolumeAdjust_Text = cfg.Read("Audio-SDL", "VOLUME_ADJUST");

                        // Volume Default
                        VM.Plugins_Audio_AudioSDL_View.VolumeDefault_Text = cfg.Read("Audio-SDL", "VOLUME_DEFAULT");

                        // Audio Sync
                        bool audioSync = true;
                        bool.TryParse(cfg.Read("Audio-SDL", "AUDIO_SYNC").ToLower(), out audioSync);
                        VM.Plugins_Audio_AudioSDL_View.AudioSync_IsChecked = audioSync;
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
        ///  Save
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Check if Paths Config TextBox is Empty
            if (MainWindow.IsValidPath(VM.PathsView.Config_Text))
            {
                // Check if Cfg File Exists
                if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
                {
                    // Save to mupen64plus.cfg
                    Configure.ConigFile cfg = null;

                    try
                    {
                        cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

                        // -------------------------
                        // [Audio-SDL]
                        // -------------------------
                        // Version
                        cfg.Write("Audio-SDL", "Version", VM.Plugins_Audio_AudioSDL_View.Version_Text);

                        // Default Frequency
                        cfg.Write("Audio-SDL", "DEFAULT_FREQUENCY", VM.Plugins_Audio_AudioSDL_View.DefaultFrequency_Text);

                        // Swap Channels
                        if (VM.Plugins_Audio_AudioSDL_View.SwapChannels_IsChecked == true)
                        {
                            cfg.Write("Audio-SDL", "SWAP_CHANNELS", "True");
                        }
                        else if (VM.Plugins_Audio_AudioSDL_View.SwapChannels_IsChecked == false)
                        {
                            cfg.Write("Audio-SDL", "SWAP_CHANNELS", "False");
                        }

                        // Primary Buffer Size
                        cfg.Write("Audio-SDL", "PRIMARY_BUFFER_SIZE", VM.Plugins_Audio_AudioSDL_View.PrimaryBufferSize_Text);

                        // Primary Buffer Target
                        cfg.Write("Audio-SDL", "PRIMARY_BUFFER_TARGET", VM.Plugins_Audio_AudioSDL_View.PrimaryBufferTarget_Text);

                        // Secondary Buffer Size
                        cfg.Write("Audio-SDL", "SECONDARY_BUFFER_SIZE", VM.Plugins_Audio_AudioSDL_View.SecondaryBufferSize_Text);

                        // Resample
                        cfg.Write("Audio-SDL", "RESAMPLE", "\"" + VM.Plugins_Audio_AudioSDL_View.Resample_SelectedItem + "\"");

                        // Volume Control Type
                        if (VM.Plugins_Audio_AudioSDL_View.VolumeControlType_SelectedItem == "SDL")
                        {
                            cfg.Write("Audio-SDL", "VOLUME_CONTROL_TYPE", "1");
                        }
                        else if (VM.Plugins_Audio_AudioSDL_View.VolumeControlType_SelectedItem == "OSS mixer")
                        {
                            cfg.Write("Audio-SDL", "VOLUME_CONTROL_TYPE", "2");
                        }

                        // Volume Adjust
                        cfg.Write("Audio-SDL", "VOLUME_ADJUST", VM.Plugins_Audio_AudioSDL_View.VolumeAdjust_Text);

                        // Volume Default
                        cfg.Write("Audio-SDL", "VOLUME_DEFAULT", VM.Plugins_Audio_AudioSDL_View.VolumeDefault_Text);

                        // Audio Sync
                        if (VM.Plugins_Audio_AudioSDL_View.AudioSync_IsChecked == true)
                        {
                            cfg.Write("Audio-SDL", "AUDIO_SYNC", "True");
                        }
                        else if (VM.Plugins_Audio_AudioSDL_View.AudioSync_IsChecked == false)
                        {
                            cfg.Write("Audio-SDL", "AUDIO_SYNC", "False");
                        }


                        // -------------------------
                        // Save Complete
                        // -------------------------
                        VM.Plugins_Audio_AudioSDL_View.Save_Text = "✓";
                    }
                    // Error
                    catch
                    {
                        VM.Plugins_Audio_AudioSDL_View.Save_Text = "Error";

                        MessageBox.Show("Could not save to mupen64plus.cfg.",
                                        "Write Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
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
        ///  Defaults
        /// </summary>
        private void btnDefaults_Click(object sender, RoutedEventArgs e)
        {
            //VM.Plugins_Audio_AudioSDL_View.Version_Text = "1";
            VM.Plugins_Audio_AudioSDL_View.DefaultFrequency_Text = "32767";
            VM.Plugins_Audio_AudioSDL_View.SwapChannels_IsChecked = false;
            VM.Plugins_Audio_AudioSDL_View.PrimaryBufferSize_Text = "16384";
            VM.Plugins_Audio_AudioSDL_View.PrimaryBufferTarget_Text = "2048";
            VM.Plugins_Audio_AudioSDL_View.SecondaryBufferSize_Text = "1024";
            VM.Plugins_Audio_AudioSDL_View.Resample_SelectedItem = "trivial";
            VM.Plugins_Audio_AudioSDL_View.VolumeControlType_SelectedItem = "SDL";
            VM.Plugins_Audio_AudioSDL_View.VolumeAdjust_Text = "5";
            VM.Plugins_Audio_AudioSDL_View.VolumeDefault_Text = "80";

            // Reset Save ✓ Label
            VM.Plugins_Audio_AudioSDL_View.Save_Text = "";
        }


        /// <summary>
        ///  Close
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
