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
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for CheatsWindow.xaml
    /// </summary>
    public partial class CheatsWindow : Window
    {
        public CheatsWindow(/*string args*/)
        {
            InitializeComponent();

            // Set Theme Icon
            MainWindow.SetThemeIcon(this);
        }

        /// <summary>
        /// Window Loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = @"C:\Path\To\mupen64plus-ui-console.exe";
            p.StartInfo.Arguments = "--cheats list " + "\"C:\\Path\\To\\Game.n64\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            //Console.WriteLine(p.StandardOutput.ReadToEnd());

            //p.WaitForExit();

            //Process p = new Process
            //{
            //    StartInfo = new ProcessStartInfo
            //    {
            //        FileName = "mupen64plus-ui-console.exe",
            //        Arguments = "",
            //        UseShellExecute = false,
            //        RedirectStandardOutput = true,
            //        RedirectStandardError = true,
            //        CreateNoWindow = true
            //    }
            //};

            string cheatsStr = p.StandardOutput.ReadToEnd();
            //string cheatsStr = p.StandardError.ReadToEnd();

            p.WaitForExit();

            List<string> cheatsList = cheatsStr.Split(new[] { /*"\r\n" */ /*Environment.NewLine*/ "UI-Console:    "}, StringSplitOptions.None).ToList();

            //string[] arrLanguagePriority_ItemOrder = cheatsStr.Split(Environment.NewLine);

            //MessageBox.Show(string.Join("\r\n", cheatsList));

            //VM.EmulatorView.Cheats_ListView_Items = new ObservableCollection<string>(cheatsList);
            //List<string> test = new List<string>() { "1", "2", "3" };
            //VM.EmulatorView.Cheats_ListView_Items = new ObservableCollection<string>(test);
            //VM.EmulatorView.Cheats_ListView_Items = new List<string>() { "1", "2", "3" };

            //List<string> cheats = new List<string>();

            for (int i = 2; i < cheatsList.Count; i++)
            {
                lstvCheats.Items.Add(cheatsList[i]);
            };
        }

        /// <summary>
        /// Window Closing
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
        }


        /// <summary>
        /// ListView Cheats Selection Changed
        /// </summary>
        private void lstvCheats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmulatorViewModel.cheatIDs.Add(lstvCheats.SelectedIndex);
        }
    }
}
