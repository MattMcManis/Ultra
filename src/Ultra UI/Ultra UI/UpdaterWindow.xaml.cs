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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using ViewModel;
// Disable XML Comment warnings
#pragma warning disable 1591
#pragma warning disable 1587
#pragma warning disable 1570

namespace Ultra
{
    /// <summary>
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        // Web Downloads
        public static ManualResetEvent waiter = new ManualResetEvent(false); // Download one at a time

        // Progress Label Info
        public static string progressInfo;

        // Unzip CMD Arguments
        public static string extractArgs;


        public UpdateWindow()
        {
            InitializeComponent();

            // Start Download as soon as Update Window opens
            StartDownload();
        }

        /// <summary>
        /// Close
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Close();
        }

        // -----------------------------------------------
        // Download Handlers
        // -----------------------------------------------
        // -------------------------
        // Progress Changed
        // -------------------------
        public void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Progress Info
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                this.labelProgressInfo.Content = progressInfo;
            }));

            // Progress Bar
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                this.progressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
            }));
        }

        // -------------------------
        // Download Complete
        // -------------------------
        public void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Set the waiter Release
            // Must be here
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                waiter.Set();
            }));
        }

        // -------------------------
        // Check For Internet Connection
        // -------------------------
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool CheckForInternetConnection()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }



        // -------------------------
        // Ultra Self-Update Download
        // -------------------------
        public void StartDownload()
        {
            // Start New Thread
            Thread worker = new Thread(() =>
            {
                // -------------------------
                // Download
                // -------------------------
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons

                WebClient wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.UserAgent, "Ultra (https://github.com/MattMcManis/Ultra) " + " v" + MainViewModel.currentVersion + "-" + MainViewModel.currentBuildPhase + " Update");
                //wc.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8"); //error
                //wc.Headers.Add("accept-language", "en-US,en;q=0.9"); //error
                //wc.Headers.Add("dnt", "1"); //error
                //wc.Headers.Add("upgrade-insecure-requests", "1"); //error
                //wc.Headers.Add("accept-encoding", "gzip, deflate, br"); //error

                waiter = new ManualResetEvent(false); //start a new waiter for next pass (clicking update again)

                Uri url = new Uri("https://github.com/MattMcManis/Ultra/releases/download/" + "v" + Convert.ToString(MainViewModel.latestVersion) + "-" + MainViewModel.latestBuildPhase.Trim() + "/Ultra.zip"); // v1.0.0.0-alpha/Ultra.zip

                //Process.Start(url.ToString()); //debug
                //MessageBox.Show(url.ToString()); /debug

                // Delete old Ultra.zip file if it was left in %temp%
                if (File.Exists(MainWindow.tempDir + "Ultra.zip"))
                {
                    try
                    {
                        File.Delete(MainWindow.tempDir + "Ultra.zip");
                    }
                    catch
                    {

                    }
                }

                // Async
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
                wc.DownloadFileAsync(url, MainWindow.tempDir + "Ultra.zip");

                // Progress Info
                progressInfo = "Downloading Ultra...";

                // Wait for Download to finish
                waiter.WaitOne();

                // -------------------------
                // Extract
                // -------------------------
                // Progress Info
                progressInfo = "Extracting Ultra...";

                List<string> extractArgs = new List<string>() {
                    // Powershell Launch Parameters
                    "-nologo -noprofile -command",
                    "$Host.UI.RawUI.WindowSize = New-Object System.Management.Automation.Host.Size (80, 30); ",
                    "$Host.UI.RawUI.BufferSize = New-Object System.Management.Automation.Host.Size (80, 30); ",
                    // Message
                    "Write-Host \"Updating Ultra to version " + Convert.ToString(MainViewModel.latestVersion) + ".\";",
                    "Write-Host \"`nPlease wait for program to close.\";",
                    // Wait
                    "timeout 3;",
                    // Extract
                    "$shell = new-object -com shell.application;",
                    "$zip = $shell.NameSpace('" + MainWindow.tempDir + "Ultra.zip');",
                    //"foreach ($item in $zip.items()) {$shell.Namespace('" + MainWindow.appDir + "').CopyHere($item, 0x14)};", //all files
                    "foreach ($item in $zip.items()) {$name = [string]$item.Name; if ($name -match 'Ultra.exe') {$shell.Namespace('" + MainWindow.appRootDir + "').CopyHere($item, 0x14)}};",
                    // Delete Temp
                    "Write-Host \"`nDeleting Temp File\";",
                    "del " + "\"" + MainWindow.tempDir + "Ultra.zip" + "\";",
                    // Complete
                    "Write-Host \"`nUpdate Complete. Relaunching Ultra.\";",
                    "timeout 3;",
                    // Relaunch Ultra
                    "& '" + MainWindow.appRootDir + "Ultra.exe'",
                };

                // Join List with Spaces
                string arguments = string.Join(" ", extractArgs.Where(s => !string.IsNullOrEmpty(s)));

                // Start
                Process.Start("powershell.exe", arguments);

                // Close Ultra before updating exe
                Application.Current.Shutdown();
            });


            // Start Download Thread
            //
            worker.Start();
        }
    }
}
