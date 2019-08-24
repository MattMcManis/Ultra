/* ----------------------------------------------------------------------
Ultra UI
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ultra
{
    public class Configure
    {
        public static string theme;

        /// <summary>
        /// Config File Reader
        /// </summary>
        /// License MIT
        // https://code.msdn.microsoft.com/windowsdesktop/Reading-and-Writing-Values-85084b6a
        public partial class ConigFile
        {
            public static ConigFile cfg;
            public static ConigFile conf;

            public string path { get; private set; }


            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            private static extern int GetPrivateProfileString(string section, string key,
            string defaultValue, StringBuilder value, int size, string filePath);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
            static extern int GetPrivateProfileString(string section, string key, string defaultValue,
                [In, Out] char[] value, int size, string filePath);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            private static extern int GetPrivateProfileSection(string section, IntPtr keyValue,
            int size, string filePath);

            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool WritePrivateProfileString(string section, string key,
                string value, string filePath);

            public static int capacity = 512;

            public ConigFile(string configPath)
            {
                path = configPath;
            }

            public bool Write(string section, string key, string value)
            {
                bool result = WritePrivateProfileString(section, key, value, path);
                return result;
            }

            public string Read(string section, string key)
            {
                var value = new StringBuilder(capacity);
                GetPrivateProfileString(section, key, string.Empty, value, value.Capacity, path);
                return value.ToString();
            }
        }


        /// <summary>
        /// Read ultra.conf file
        /// </summary>
        public static void ReadUltraConf(string directory,
                                         string filename,
                                         List<Action> actionsToRead/*MainWindow mainwindow*/)
        {
            // Failed Imports
            List<string> listFailedImports = new List<string>();

            // -------------------------
            // Start Cofig File Read
            // -------------------------
            //ConigFile conf = null;

            try
            {
                // Check if ultra.conf file exists in %AppData%\Utara UI\
                if (File.Exists(Path.Combine(directory, filename)))
                {
                    Configure.ConigFile.conf = new ConigFile(Path.Combine(directory, filename));

                    //MessageBox.Show(MainWindow.ultraConfFile); //debug

                    // Write each action in the list
                    foreach (Action Read in actionsToRead)
                    {
                        Read();
                    }
                }
            }

            // Error Loading ultra.conf
            //
            catch
            {
                // Delete ultra.conf and Restart
                // Check if ultra.conf Exists
                if (File.Exists(MainWindow.ultraConfFile))
                {
                    // Yes/No Dialog Confirmation
                    //
                    MessageBoxResult result = MessageBox.Show(
                        "Could not load ultra.conf. \n\nDelete config and reload defaults?",
                        "Error",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Error);
                    switch (result)
                    {
                        // Create
                        case MessageBoxResult.Yes:
                            File.Delete(MainWindow.ultraConfFile);

                            // Reload Control Defaults
                            // 
                            // 
                            // 

                            // Restart Program
                            Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                            break;

                        // Use Default
                        case MessageBoxResult.No:
                            // Force Shutdown
                            System.Windows.Forms.Application.ExitThread();
                            Environment.Exit(0);
                            return;
                    }
                }
                // If ultra.conf Not Found
                else
                {
                    MessageBox.Show("No previous ultra.conf file found.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                    return;
                }
            }
        }


        /// <summary>
        /// Write ultra.conf
        /// </summary>
        public static void WriteUltraConf(string directory, 
                                           string filename,
                                           List<Action> actionsToWrite
                                           )
        {
            // -------------------------
            // Check if Directory Exists
            // -------------------------
            if (!Directory.Exists(directory))
            {
                try
                {
                    // Create Config Directory
                    Directory.CreateDirectory(directory);
                }
                catch
                {
                    MessageBox.Show("Could not create config folder " + directory + ".\n\nMay require Administrator privileges.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }

            // -------------------------
            // Save ultra.conf file
            // -------------------------
            if (Directory.Exists(directory))
            {
                //MessageBox.Show(path); //debug

                // Access
                if (MainWindow.hasWriteAccessToFolder(directory) == true)
                {
                    //ConigFile conf = null;

                    try
                    {
                        // Start ultra.conf File Write
                        Configure.ConigFile.conf = new ConigFile(Path.Combine(directory, filename));

                        // Write each action in the list
                        foreach (Action Write in actionsToWrite)
                        {
                            Write();
                        }
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Could not save " + filename + " to " + directory/*MainWindow.ultraConfDir*//*path*/,
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
                // Denied
                else
                {
                    MessageBox.Show("User does not have write access to " + directory/*MainWindow.ultraConfDir*/,
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }
            }
        }

    }
}
