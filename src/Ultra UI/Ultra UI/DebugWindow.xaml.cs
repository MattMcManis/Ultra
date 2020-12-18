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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ultra
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        private MainWindow mainwindow = (MainWindow)System.Windows.Application.Current.MainWindow;

        public DebugWindow()
        {
            InitializeComponent();

            this.MinWidth = 648;
            this.MinHeight = 480;
        }

        /// <summary>
        /// Window Loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Plugins
            string video = VM.PluginsView.Video_Items.FirstOrDefault(item => item.Name == VM.PluginsView.Video_SelectedItem)?.FullPath;
            string audio = VM.PluginsView.Audio_Items.FirstOrDefault(item => item.Name == VM.PluginsView.Audio_SelectedItem)?.FullPath;
            string input = VM.PluginsView.Input_Items.FirstOrDefault(item => item.Name == VM.PluginsView.Input_SelectedItem)?.FullPath;
            string rsp = VM.PluginsView.RSP_Items.FirstOrDefault(item => item.Name == VM.PluginsView.RSP_SelectedItem)?.FullPath;

            string configdir = "--configdir " + VM.PathsView.Config_Text; // do not wrap dir in quotes
            string datadir = "--datadir " + VM.PathsView.Data_Text; // do not wrap dir in quotes
            string fullscreen = string.Empty;
            string resolution = string.Empty;
            string osd = string.Empty;
            string cheats = string.Empty;

            // Fullscreen
            if (VM.DisplayView.Display_View_SelectedItem == "Fullscreen")
            {
                fullscreen = "--fullscreen";
            }
            // Windowed
            else if (VM.DisplayView.Display_View_SelectedItem == "Windowed")
            {
                fullscreen = "--windowed";
            }

            // Resolution
            resolution = "--resolution " + VM.DisplayView.Display_Resolution_SelectedItem;

            // OSD
            osd = "--osd";

            // Cheats
            //cheats = "--cheats "; // needs work

            string rom = string.Empty;

            int index = 0;
            if (mainwindow.listViewGames.SelectedItems.Count > 0)
            {
                index = mainwindow.listViewGames.Items.IndexOf(mainwindow.listViewGames.SelectedItems[0]);

                // Get ROM Path
                rom = VM.MainView.Games_Items.Select(item => item.FullPath).ElementAt(index);
            }

            // -------------------------
            // Generate Arguments
            // -------------------------
            List<string> argsList = new List<string>()
            {
                "\"" + MainWindow.SetMupen64PlusExe() + "\"",
                //configdir, // does not work correctly
                //datadir,   // does not work correctly

                //"--nosaveoptions", // will prevent plugins from genereating defaults in mupen64plus.cfg

                resolution,
                fullscreen,
                osd,
                cheats,

                "--gfx "   + "\"" + video + "\"",
                "--audio " + "\"" + audio + "\"",
                "--input " + "\"" + input + "\"",
                "--rsp "   + "\"" + rsp   + "\"",
                "\"" + rom + "\""
            };

            // -------------------------
            // Display Text
            // -------------------------
            VM.MainView.Debug_Text =
                
                "Mupen64Plus:" + "\r\n" +
                MainWindow.SetMupen64PlusExe() +
                "\r\n" +
                MainWindow.SetMupen64PlusDll() +
                "\r\n\r\n" +

                "Plugins" + "\r\n" +
                VM.PluginsView.Video_Items.FirstOrDefault(item => item.Name == VM.PluginsView.Video_SelectedItem)?.FullPath + "\r\n" +
                VM.PluginsView.Audio_Items.FirstOrDefault(item => item.Name == VM.PluginsView.Audio_SelectedItem)?.FullPath + "\r\n" +
                VM.PluginsView.Input_Items.FirstOrDefault(item => item.Name == VM.PluginsView.Input_SelectedItem)?.FullPath + "\r\n" +
                VM.PluginsView.RSP_Items.FirstOrDefault(item => item.Name == VM.PluginsView.RSP_SelectedItem)?.FullPath +
                "\r\n\r\n" +

                "ROM:" + "\r\n" +
                rom +
                "\r\n\r\n" +

                "Arguments:" + "\r\n" +
                string.Join(" ", argsList)
            ;
        }

        /// <summary>
        /// Window Closing
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
        }

    }
}
