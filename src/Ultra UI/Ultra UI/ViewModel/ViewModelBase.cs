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

using Ultra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class VM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string prop)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(prop));
            }
        }

        /// <summary>
        /// ViewModel Base
        /// </summary>
        public VM()
        {

        }

        // --------------------------------------------------
        // Main
        // --------------------------------------------------
        public static MainViewModel MainView { get; set; } = new MainViewModel();

        // --------------------------------------------------
        // Paths
        // --------------------------------------------------
        public static PathsViewModel PathsView { get; set; } = new PathsViewModel();

        // --------------------------------------------------
        // Emulator
        // --------------------------------------------------
        public static EmulatorViewModel EmulatorView { get; set; } = new EmulatorViewModel();

        // --------------------------------------------------
        // Display
        // --------------------------------------------------
        public static DisplayViewModel DisplayView { get; set; } = new DisplayViewModel();


        // --------------------------------------------------
        // Plugins
        // --------------------------------------------------
        public static PluginsViewModel PluginsView { get; set; } = new PluginsViewModel();

        // -------------------------
        // Video
        // -------------------------
        // GLideN64
        public static Plugins_Video_GLideN64_ViewModel Plugins_Video_GLideN64_View { get; set; } = new Plugins_Video_GLideN64_ViewModel();

        // -------------------------
        // Audio
        // -------------------------
        // AudioSDL
        public static Plugins_Audio_AudioSDL_ViewModel Plugins_Audio_AudioSDL_View { get; set; } = new Plugins_Audio_AudioSDL_ViewModel();

        // -------------------------
        // Input
        // -------------------------
        // InputSDL
        public static Plugins_Input_InputSDL_ViewModel Plugins_Input_InputSDL_View { get; set; } = new Plugins_Input_InputSDL_ViewModel();

        // -------------------------
        // RSP
        // -------------------------
        // RSP HLE
        public static Plugins_RSP_RSPHLE_ViewModel Plugins_RSP_RSPHLE_View { get; set; } = new Plugins_RSP_RSPHLE_ViewModel();
        // cxd4 SSSE3
        public static Plugins_RSP_cxd4SSSE3_ViewModel Plugins_RSP_cxd4SSSE3_View { get; set; } = new Plugins_RSP_cxd4SSSE3_ViewModel();

    }
}
