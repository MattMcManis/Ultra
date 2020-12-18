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
using System.Text;
using System.Windows;

namespace Ultra
{
    /// <summary>
    /// Interaction logic for CfgEditor.xaml
    /// </summary>
    public partial class CfgEditorWindow : Window
    {
        public CfgEditorWindow()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Window Loaded
        /// </summary>
        private static StreamReader sr;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"))) //mupen64plus.cfg
            {
                sr = new StreamReader(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));
                VM.MainView.Cfg_Text = sr.ReadToEnd();

                //MessageBox.Show(VM.MainView.Cfg_Text); //debug
            }
        }

        /// <summary>
        /// Window Closing
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
            // Exit
            sr.Close();
            sr.Dispose();
        }

        /// <summary>
        /// Save
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            sr.Close();
            sr.Dispose();

            if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"))) //mupen64plus.cfg
            {
                File.WriteAllText(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"), VM.MainView.Cfg_Text, Encoding.Unicode);

                this.Close();
            }
            else
            {
                MessageBox.Show("Cannot save mupen64plus.cfg. File does not exist.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

    }
}
