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

using System.Diagnostics;
using System.IO;
using System.Windows;
using ViewModel;

namespace Ultra
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Set Mupen64Plus Exe
        /// </summary>
        public static string SetMupen64PlusExe()
        {
            // mupen64plus-ui-console.exe
            if (File.Exists(Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus-ui-console.exe")))
            {
                return Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus-ui-console.exe");
            }
            // mupen64plus.exe
            else if (File.Exists(Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus.exe")))
            {
                return Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus.exe");
            }
            // mupen64.exe
            else if (File.Exists(Path.Combine(VM.PathsView.Mupen_Text, "mupen64.exe")))
            {
                return Path.Combine(VM.PathsView.Mupen_Text, "mupen64.exe");
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Set Mupen64Plus dll
        /// </summary>
        public static string SetMupen64PlusDll()
        {
            // mupen64plus-ui-console.exe
            if (File.Exists(Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus.dll")))
            {
                return Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus.dll");
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Path Mupen64Plus - Browse
        /// </summary>
        private void btnBrowsePathMupen_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.PathsView.Mupen_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.PathsView.Mupen_Text = VM.PathsView.Mupen_Text.Replace(@"\\", @"\");
            }

            //// Open Select File Window
            //Microsoft.Win32.OpenFileDialog selectFile = new Microsoft.Win32.OpenFileDialog();

            //// Show Dialog Box
            //Nullable<bool> result = selectFile.ShowDialog();

            //// Process Dialog Box
            //if (result == true)
            //{
            //    if (File.Exists(selectFile.FileName))
            //    {
            //        VM.PathsView.Mupen_Text = selectFile.FileName;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Could not find " + selectFile.FileName,
            //                        "Error",
            //                        MessageBoxButton.OK,
            //                        MessageBoxImage.Error);
            //    }

            //}
        }


        /// <summary>
        /// Path Config - Browse
        /// </summary>
        private void btnBrowsePathConfig_Click(object sender, RoutedEventArgs e)
        {
            // Config Path cannot be changed from %AppData%

            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.PathsView.Config_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.PathsView.Config_Text = VM.PathsView.Config_Text.TrimEnd('\\') + @"\".Replace(@"\\", @"\");
            }
        }

        /// <summary>
        /// Path Plugins
        /// </summary>
        private void btnBrowsePathPlugins_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.PathsView.Plugins_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.PathsView.Plugins_Text = VM.PathsView.Plugins_Text.Replace(@"\\", @"\");
            }
        }

        /// <summary>
        /// Path Data
        /// </summary>
        private void btnBrowsePathData_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.PathsView.Data_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.PathsView.Data_Text = VM.PathsView.Data_Text.Replace(@"\\", @"\");
            }
        }

        /// <summary>
        /// Path ROMs
        /// </summary>
        private void btnBrowsePathROMs_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.PathsView.ROMs_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.PathsView.ROMs_Text = VM.PathsView.ROMs_Text.Replace(@"\\", @"\");
            }
        }

        /// <summary>
        /// Open Mupen64Plus Path
        /// </summary>
        private void btnOpenMupen64PlusExePath_Click(object sender, RoutedEventArgs e)
        {
            //ExplorePath(Path.GetDirectoryName(VM.PathsView.Mupen_Text));
            ExplorePath(VM.PathsView.Mupen_Text);
        }

        /// <summary>
        /// Open Config Path
        /// </summary>
        private void btnOpenConfigPath_Click(object sender, RoutedEventArgs e)
        {
            //ExplorePath(VM.PathsView.Config_Text);

            if (Directory.Exists(VM.PathsView.Config_Text))
            {
                Process.Start("explorer.exe", VM.PathsView.Config_Text);
            }
            else
            {
                MessageBox.Show(VM.PathsView.Config_Text + " does not yet exist.\n\nPlease run mupen64plus-ui-console.exe or launch a game to automatically create it.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Open Plugins Path
        /// </summary>
        private void btnOpenPluginsPath_Click(object sender, RoutedEventArgs e)
        {
            ExplorePath(VM.PathsView.Plugins_Text);
        }

        /// <summary>
        /// Open Data Path
        /// </summary>
        private void btnOpenDataPath_Click(object sender, RoutedEventArgs e)
        {
            ExplorePath(VM.PathsView.Data_Text);
        }

        /// <summary>
        /// Open ROMs Path
        /// </summary>
        private void btnOpenROMsPath_Click(object sender, RoutedEventArgs e)
        {
            ExplorePath(VM.PathsView.ROMs_Text);
        }

        /// <summary>
        /// Defaults all - Button
        /// </summary>
        private void btnPathsDefaultsAll_Click(object sender, RoutedEventArgs e)
        {
            // Paths
            PathDefaults();

            // Plugins
            // Reload Contains PluginsDefaults()
            PluginsReload();

            // Emulator
            EmulatorDefaults();

            // Display
            DisplayDefaults();
        }

        /// <summary>
        /// Path Defaults - Button
        /// </summary>
        private void btnPathsDefaults_Click(object sender, RoutedEventArgs e)
        {
            PathDefaults();
        }
        public void PathDefaults()
        {
            //string mupen64plusExeFolder = string.Empty;

            // -------------------------
            // Mupen64Plus Exe Path
            // -------------------------
            // -------------------------
            // Path TextBox is Empty
            // Check if Ultra.exe is in the Mupen64Plus folder 
            // -------------------------
            if (File.Exists(Path.Combine(appRootDir, "mupen64plus-ui-console.exe")) ||
                File.Exists(Path.Combine(appRootDir, "mupen64plus.exe")) ||
                File.Exists(Path.Combine(appRootDir, "mupen64.exe")))
            {
                VM.PathsView.Mupen_Text = appRootDir;
            }
            // -------------------------
            // Exe Not Found
            // -------------------------
            else
            {
                MessageBox.Show("Could not find Mupen64Plus exe.\n\nPlease place Ultra.exe in the Mupen64Plus folder.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }

            // -------------------------
            // Cofig Path
            // -------------------------
            // Hardcoded %AppData%\Mupen64Plus
            VM.PathsView.Config_Text = appDataRoamingDir + @"Mupen64Plus\";

            // -------------------------
            // Plugins Path
            // -------------------------
            if (IsValidPath(VM.PathsView.Mupen_Text))
            {
                // Check for 'plugins' folder
                if (Directory.Exists(VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\" + "plugins"))
                {
                    VM.PathsView.Plugins_Text = VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\" + @"plugins\";
                }
                // Use mupen64plus-ui-console.exe root folder for Plugins
                else
                {
                    VM.PathsView.Plugins_Text = VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\";
                }
            }

            // -------------------------
            // Data
            // -------------------------
            if (IsValidPath(VM.PathsView.Mupen_Text))
            {
                // Check for 'data' folder
                if (Directory.Exists(VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\" + "data"))
                {
                    VM.PathsView.Data_Text = VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\" + @"data\";
                }
                // Use mupen64plus-ui-console.exe root folder for Data
                else
                {
                    VM.PathsView.Data_Text = VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\";
                }
            }
        }
    }
}
