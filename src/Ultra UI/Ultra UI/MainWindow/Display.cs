using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ultra
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Display - Vsync
        /// </summary>
        private void cbxVsync_Checked(object sender, RoutedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Video-General]
                    // -------------------------
                    if (VM.DisplayView.Display_Vsync_IsChecked == true)
                    {
                        Configure.ConigFile.cfg.Write("Video-General", "VerticalSync", "True");
                    }
                    else if (VM.DisplayView.Display_Vsync_IsChecked == false)
                    {
                        Configure.ConigFile.cfg.Write("Video-General", "VerticalSync", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Display - Video Extension
        /// </summary>
        private void cbxVideoExtension_Checked(object sender, RoutedEventArgs e)
        {
            // ???
        }

        /// <summary>
        /// On Screen Display
        /// </summary>
        private void cbxOSD_Checked(object sender, RoutedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Core]
                    // -------------------------
                    if (VM.DisplayView.Display_OSD_IsChecked == true)
                    {
                        Configure.ConigFile.cfg.Write("Core", "OnScreenDisplay", "True");
                    }
                    else if (VM.DisplayView.Display_OSD_IsChecked == false)
                    {
                        Configure.ConigFile.cfg.Write("Core", "OnScreenDisplay", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Disable Screensaver
        /// </summary>
        private void cbxScreensaver_Checked(object sender, RoutedEventArgs e)
        {
            // ???
        }

        /// <summary>
        /// Display Defaults - Button
        /// </summary>
        private void btnDisplayDefaults_Click(object sender, RoutedEventArgs e)
        {
            DisplayDefaults();
        }
        public void DisplayDefaults()
        {
            // Resolution
            VM.DisplayView.Display_Resolution_SelectedItem = "960x720";

            // Vsync
            VM.DisplayView.Display_Vsync_IsChecked = true;

            // Video Extension
            VM.DisplayView.Display_VideoExtension_IsChecked = false;

            // View
            VM.DisplayView.Display_View_SelectedItem = "Windowed";

            // OSD
            VM.DisplayView.Display_OSD_IsChecked = true;

            // Screensaver
            //VM.DisplayView.Display_Screensaver_IsChecked = true;
        }


        /// <summary>
        /// Display - View
        /// </summary>
        private void cboView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // Fullscreen
                    if (VM.DisplayView.Display_View_SelectedItem == "Fullscreen")
                    {
                        Configure.ConigFile.cfg.Write("Video-General", "Fullscreen", "True");
                    }
                    // Windowed
                    else if (VM.DisplayView.Display_View_SelectedItem == "Windowed")
                    {
                        Configure.ConigFile.cfg.Write("Video-General", "Fullscreen", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }


        /// <summary>
        /// Resolution
        /// </summary>
        private void cboResolution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Video-General]
                    // -------------------------
                    List<string> splitRes = new List<string>();
                    splitRes = VM.DisplayView.Display_Resolution_SelectedItem.Split('x').ToList();
                    string width = string.Empty;
                    string height = string.Empty;
                    if (splitRes.Count > 1) // null check
                    {
                        width = splitRes[0];
                        height = splitRes[1];
                    }

                    Configure.ConigFile.cfg.Write("Video-General", "ScreenWidth", width);
                    Configure.ConigFile.cfg.Write("Video-General", "ScreenHeight", height);
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }
    }
}
