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
using ViewModel;

namespace Ultra
{
    public partial class MainWindow : Window 
    {
        /// <summary>
        /// Menu Item - Update
        /// </summary>
        public static UpdateWindow updatewindow;
        private Boolean IsUpdateWindowOpened = false;
        private void Update_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // -------------------------
            // Proceed if Internet Connection
            // -------------------------
            if (UpdateWindow.CheckForInternetConnection() == true)
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                WebClient wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.UserAgent, "Ultra (https://github.com/MattMcManis/Ultra) " + " v" + MainViewModel.currentVersion + "-" + MainViewModel.currentBuildPhase + " Update Check");
                wc.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                wc.Headers.Add("accept-language", "en-US,en;q=0.9");
                wc.Headers.Add("dnt", "1");
                wc.Headers.Add("upgrade-insecure-requests", "1");
                //wc.Headers.Add("accept-encoding", "gzip, deflate, br"); //error

                // -------------------------
                // Parse GitHub version file
                // -------------------------
                string parseLatestVersion = string.Empty;

                try
                {
                    parseLatestVersion = wc.DownloadString("https://raw.githubusercontent.com/MattMcManis/Ultra/master/version");
                }
                catch
                {
                    MessageBox.Show("GitHub version file not found.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation);

                    return;
                }

                // -------------------------
                // Split Version & Build Phase by dash
                // -------------------------
                if (!string.IsNullOrEmpty(parseLatestVersion) &&
                    !string.IsNullOrWhiteSpace(parseLatestVersion))
                {
                    try
                    {
                        // Split Version and Build Phase
                        MainViewModel.splitVersionBuildPhase = Convert.ToString(parseLatestVersion).Split('-');

                        // Set Version Number
                        MainViewModel.latestVersion = new Version(MainViewModel.splitVersionBuildPhase[0]); //number
                        MainViewModel.latestBuildPhase = MainViewModel.splitVersionBuildPhase[1]; //alpha
                    }
                    catch
                    {
                        MessageBox.Show("Problem reading version.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);

                        return;
                    }

                    // Debug
                    //MessageBox.Show(Convert.ToString(latestVersion));
                    //MessageBox.Show(latestBuildPhase);

                    // -------------------------
                    // Check if Ultra is the Latest Version
                    // -------------------------
                    // Update Available
                    if (MainViewModel.latestVersion > MainViewModel.currentVersion)
                    {
                        // Yes/No Dialog Confirmation
                        //
                        MessageBoxResult result = MessageBox.Show("v" + Convert.ToString(MainViewModel.latestVersion) + "-" + MainViewModel.latestBuildPhase + "\n\nDownload Update?",
                                                                  "Update Available",
                                                                  MessageBoxButton.YesNo
                                                                  );
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                // Check if Window is already open
                                if (IsUpdateWindowOpened) return;

                                // Start Window
                                updatewindow = new UpdateWindow();

                                // Keep in Front
                                updatewindow.Owner = Window.GetWindow(this);

                                // Only allow 1 Window instance
                                updatewindow.ContentRendered += delegate { IsUpdateWindowOpened = true; };
                                updatewindow.Closed += delegate { IsUpdateWindowOpened = false; };

                                // Position Relative to MainWindow
                                // Keep from going off screen
                                updatewindow.Left = Math.Max((Left + (Width - updatewindow.Width) / 2), Left);
                                updatewindow.Top = Math.Max((Top + (Height - updatewindow.Height) / 2), Top);

                                // Open Window
                                updatewindow.Show();
                                break;
                            case MessageBoxResult.No:
                                break;
                        }
                    }

                    // Update Not Available
                    //
                    else if (MainViewModel.latestVersion <= MainViewModel.currentVersion)
                    {
                        MessageBox.Show("This version is up to date.",
                                        "Notice",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);

                        return;
                    }

                    // Unknown
                    //
                    else // null
                    {
                        MessageBox.Show("Could not find download. Try updating manually.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);

                        return;
                    }
                }

                // Version is Null
                //
                else
                {
                    MessageBox.Show("GitHub version file returned empty.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);

                    return;
                }
            }

            // No Internet Connection
            //
            else
            {
                MessageBox.Show("Could not detect Internet Connection.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }
        }


        /// <summary>
        /// Update Available Check
        /// </summary>
        public static async Task<int> UpdateAvailableCheck()
        {
            int count = 0;
            if (VM.MainView.UpdateAutoCheck_IsChecked == true)
            {
                await Task.Run(() =>
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    WebClient wc = new WebClient();
                    wc.Headers.Add(HttpRequestHeader.UserAgent, "Ultra (https://github.com/MattMcManis/Ultra)" + " v" + MainViewModel.currentVersion + "-" + MainViewModel.currentBuildPhase + " Update Check");
                    wc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                    wc.Headers.Add("Accept-Language", "en-US,en;q=0.9");
                    wc.Headers.Add("dnt", "1");
                    wc.Headers.Add("Upgrade-Insecure-Requests", "1");
                    //wc.Headers.Add("accept-encoding", "gzip, deflate, br"); //error

                    // -------------------------
                    // Parse GitHub version file
                    // -------------------------
                    string parseLatestVersion = string.Empty;

                    try
                    {
                        parseLatestVersion = wc.DownloadString("https://raw.githubusercontent.com/MattMcManis/Ultra/master/version");
                    }
                    catch
                    {
                        return;
                    }

                    // -------------------------
                    // Split Version & Build Phase by dash
                    // -------------------------
                    if (!string.IsNullOrWhiteSpace(parseLatestVersion)) //null check
                    {
                        try
                        {
                            // Split Version and Build Phase
                            MainViewModel.splitVersionBuildPhase = Convert.ToString(parseLatestVersion).Split('-');

                            // Set Version Number
                            MainViewModel.latestVersion = new Version(MainViewModel.splitVersionBuildPhase[0]); //number
                            MainViewModel.latestBuildPhase = MainViewModel.splitVersionBuildPhase[1]; //alpha
                        }
                        catch
                        {
                            return;
                        }

                        // Check if Ultra is the Latest Version
                        // Update Available
                        if (MainViewModel.latestVersion > MainViewModel.currentVersion)
                        {
                            VM.MainView.TitleVersion = VM.MainView.TitleVersion + " ~ Update Available: " + "(" + Convert.ToString(MainViewModel.latestVersion) + "-" + MainViewModel.latestBuildPhase + ")";
                        }
                        // Update Not Available
                        else if (MainViewModel.latestVersion <= MainViewModel.currentVersion)
                        {
                            return;
                        }
                    }
                });
            }

            return count;
        }


        /// <summary>
        /// Update Available Checkbox
        /// </summary>
        private void cbxUpdateAutoCheck_Checked(object sender, RoutedEventArgs e)
        {
            VM.MainView.UpdateAutoCheck_IsChecked = true;
        }

        private void cbxUpdateAutoCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            VM.MainView.UpdateAutoCheck_IsChecked = false;
        }

    }
}
