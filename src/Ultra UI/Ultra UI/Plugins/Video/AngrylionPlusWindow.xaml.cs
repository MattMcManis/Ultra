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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing.Text;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using ViewModel;
using System.Management;

namespace Ultra
{
    /// <summary>
    /// Interaction logic for AngrylionPlusWindow.xaml
    /// </summary>
    public partial class AngrylionPlusWindow : Window, MainWindow.IPluginWindow
    {
        //public string maxthreads { get; set; }

        public AngrylionPlusWindow()
        {
            InitializeComponent();

            MinWidth = 432;
            MinHeight = 310;

            // Load Control Values
            PluginCfgReader();

            // Set Theme Icon
            MainWindow.SetThemeIcon(this);

            // Max Threads
            string maxthreads = string.Empty;
            foreach (var item in new ManagementObjectSearcher("Select NumberOfLogicalProcessors FROM Win32_ComputerSystem").Get())
            {
                maxthreads = String.Format("{0}", item["NumberOfLogicalProcessors"]);
            }

            int maxthreadsInt = 0;
            int.TryParse(maxthreads, out maxthreadsInt);
            List<string> threadsTotal = new List<string>();

            threadsTotal.Add("All");
            for (var i = 1; i < maxthreadsInt; i++)
            {
                threadsTotal.Add(i.ToString());
            }
            //threadsTotal.Add(threadsTotal.Count.ToString());

            VM.Plugins_Video_AngrylionPlus_View.NumWorkers_Items = new ObservableCollection<string>(threadsTotal);
        }

        /// <summary>
        /// Window Loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Window Closing
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
            // Reset Save ✓ Label
            VM.Plugins_Video_AngrylionPlus_View.Save_Text = "";
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

                        // --------------------------------------------------
                        // [Video-Angrylion-Plus]
                        // --------------------------------------------------

                        // Parallel
                        // # Distribute rendering between multiple processors if True
                        bool parallel = true;
                        bool.TryParse(cfg.Read("Video-Angrylion-Plus", "Parallel").ToLower(), out parallel);
                        VM.Plugins_Video_AngrylionPlus_View.Parallel_IsChecked = parallel;

                        // NumWorkers
                        // # Rendering Workers (0=Use all logical processors)
                        string numbWorkers = cfg.Read("Video-Angrylion-Plus", "NumWorkers");
                        if (numbWorkers == "0")
                        {
                            VM.Plugins_Video_AngrylionPlus_View.NumWorkers_SelectedItem = "All";
                        }
                        else
                        {
                            VM.Plugins_Video_AngrylionPlus_View.NumWorkers_SelectedItem = numbWorkers;
                        }

                        // VI mode
                        // # (0=Filtered, 1=Unfiltered, 2=Depth, 3=Coverage)
                        string viMode = cfg.Read("Video-Angrylion-Plus", "ViMode");
                        switch (viMode)
                        {
                            case "0":
                                VM.Plugins_Video_AngrylionPlus_View.ViMode_SelectedItem = "Filtered";
                                break;

                            case "1":
                                VM.Plugins_Video_AngrylionPlus_View.ViMode_SelectedItem = "Unfiltered";
                                break;

                            case "2":
                                VM.Plugins_Video_AngrylionPlus_View.ViMode_SelectedItem = "Depth";
                                break;

                            case "3":
                                VM.Plugins_Video_AngrylionPlus_View.ViMode_SelectedItem = "Coverage";
                                break;
                        }

                        // Scaling interpolation type
                        // # Scaling interpolation type (0=NN, 1=Linear)
                        string viInterpolation = cfg.Read("Video-Angrylion-Plus", "ViInterpolation");
                        switch (viInterpolation)
                        {
                            case "0":
                                VM.Plugins_Video_AngrylionPlus_View.ViInterpolation_SelectedItem = "Nearest Neighbor";
                                break;

                            case "1":
                                VM.Plugins_Video_AngrylionPlus_View.ViInterpolation_SelectedItem = "Linear";
                                break;
                        }

                        // ViWidescreen
                        // # Use anamorphic 16:9 output mode if True
                        bool viWidescreen = false;
                        bool.TryParse(cfg.Read("Video-Angrylion-Plus", "ViWidescreen").ToLower(), out viWidescreen);
                        VM.Plugins_Video_AngrylionPlus_View.ViWidescreen_IsChecked = viWidescreen;

                        // ViHideOverscan
                        // # Hide overscan area in filteded mode if True
                        bool viHideOverscan = false;
                        bool.TryParse(cfg.Read("Video-Angrylion-Plus", "ViHideOverscan").ToLower(), out viHideOverscan);
                        VM.Plugins_Video_AngrylionPlus_View.ViHideOverscan_IsChecked = viHideOverscan;

                        // DpCompat
                        // # Compatibility mode (0=Fast 1=Moderate 2=Slow)
                        string dpCompat = cfg.Read("Video-Angrylion-Plus", "DpCompat");
                        switch (dpCompat)
                        {
                            case "0":
                                VM.Plugins_Video_AngrylionPlus_View.DpCompat_SelectedItem = "Fast";
                                break;

                            case "1":
                                VM.Plugins_Video_AngrylionPlus_View.DpCompat_SelectedItem = "Moderate";
                                break;

                            case "2":
                                VM.Plugins_Video_AngrylionPlus_View.DpCompat_SelectedItem = "Slow";
                                break;
                        }
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

                        // --------------------------------------------------
                        // [Video-Angrylion-Plus]
                        // --------------------------------------------------
                        // Parallel
                        // # Distribute rendering between multiple processors if True
                        if (VM.Plugins_Video_AngrylionPlus_View.Parallel_IsChecked == true)
                        {
                            cfg.Write("Video-Angrylion-Plus", "Parallel", "True");
                        }
                        else if (VM.Plugins_Video_AngrylionPlus_View.Parallel_IsChecked == false)
                        {
                            cfg.Write("Video-Angrylion-Plus", "Parallel", "False");
                        }

                        // NumWorkers
                        // Rendering Workers (0=Use all logical processors)
                        if (VM.Plugins_Video_AngrylionPlus_View.NumWorkers_SelectedItem == "All")
                        {
                            cfg.Write("Video-Angrylion-Plus", "NumWorkers", "0");
                        }
                        else
                        {
                            cfg.Write("Video-Angrylion-Plus", "NumWorkers", VM.Plugins_Video_AngrylionPlus_View.NumWorkers_SelectedItem);
                        }
                        

                        // ViMode
                        // (0=Filtered, 1=Unfiltered, 2=Depth, 3=Coverage)
                        switch (VM.Plugins_Video_AngrylionPlus_View.ViMode_SelectedItem)
                        {
                            case "Filtered":
                                cfg.Write("Video-Angrylion-Plus", "ViMode", "0");
                                break;

                            case "Unfiltered":
                                cfg.Write("Video-Angrylion-Plus", "ViMode", "1");
                                break;

                            case "Depth":
                                cfg.Write("Video-Angrylion-Plus", "ViMode", "2");
                                break;

                            case "Coverage":
                                cfg.Write("Video-Angrylion-Plus", "ViMode", "3");
                                break;
                        }

                        // ViInterpolation
                        // Scaling interpolation type (0=NN, 1=Linear)
                        switch (VM.Plugins_Video_AngrylionPlus_View.ViInterpolation_SelectedItem)
                        {
                            case "Nearest Neighbor":
                                cfg.Write("Video-Angrylion-Plus", "ViInterpolation", "0");
                                break;

                            case "Linear":
                                cfg.Write("Video-Angrylion-Plus", "ViInterpolation", "1");
                                break;
                        }

                        // ViWidescreen
                        // # Use anamorphic 16:9 output mode if True
                        if (VM.Plugins_Video_AngrylionPlus_View.ViWidescreen_IsChecked == true)
                        {
                            cfg.Write("Video-Angrylion-Plus", "ViWidescreen", "True");
                        }
                        else if (VM.Plugins_Video_AngrylionPlus_View.ViWidescreen_IsChecked == false)
                        {
                            cfg.Write("Video-Angrylion-Plus", "ViWidescreen", "False");
                        }

                        // ViHideOverscan
                        // # Hide overscan area in filteded mode if True
                        if (VM.Plugins_Video_AngrylionPlus_View.ViHideOverscan_IsChecked == true)
                        {
                            cfg.Write("Video-Angrylion-Plus", "ViHideOverscan", "True");
                        }
                        else if (VM.Plugins_Video_AngrylionPlus_View.ViHideOverscan_IsChecked == false)
                        {
                            cfg.Write("Video-Angrylion-Plus", "ViHideOverscan", "False");
                        }

                        // DpCompat
                        // Compatibility mode (0=Fast 1=Moderate 2=Slow)
                        switch (VM.Plugins_Video_AngrylionPlus_View.DpCompat_SelectedItem)
                        {
                            case "Fast":
                                cfg.Write("Video-Angrylion-Plus", "DpCompat", "0");
                                break;

                            case "Moderate":
                                cfg.Write("Video-Angrylion-Plus", "DpCompat", "1");
                                break;

                            case "Slow":
                                cfg.Write("Video-Angrylion-Plus", "DpCompat", "2");
                                break;
                        }

                        // -------------------------
                        // Save Complete
                        // -------------------------
                        VM.Plugins_Video_AngrylionPlus_View.Save_Text = "✓";
                    }
                    // Error
                    catch
                    {
                        VM.Plugins_Video_AngrylionPlus_View.Save_Text = "Error";

                        MessageBox.Show("Could not save to mupen64plus.cfg.",
                                        "Write Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
                // Error
                else
                {
                    MessageBox.Show("Could not find mupen64plus.cfg.",
                                    "Write Error",
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
        /// Defaults
        /// </summary>
        private void btnDefaults_Click(object sender, RoutedEventArgs e)
        {
            VM.Plugins_Video_AngrylionPlus_View.Parallel_IsChecked = true;
            VM.Plugins_Video_AngrylionPlus_View.NumWorkers_SelectedItem = "All";
            VM.Plugins_Video_AngrylionPlus_View.ViMode_SelectedItem = "Filtered";
            VM.Plugins_Video_AngrylionPlus_View.ViInterpolation_SelectedItem = "Linear";
            VM.Plugins_Video_AngrylionPlus_View.ViWidescreen_IsChecked = false;
            VM.Plugins_Video_AngrylionPlus_View.ViHideOverscan_IsChecked = false;
            VM.Plugins_Video_AngrylionPlus_View.DpCompat_SelectedItem = "Moderate";

            // Reset Save ✓ Label
            VM.Plugins_Video_AngrylionPlus_View.Save_Text = "";
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
