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
using System.ComponentModel;
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
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();

            this.MinWidth = 550;
            this.MinHeight = 413;
            //this.MaxWidth = 445;
            //this.MaxHeight = 335;
        }

        /// <summary>
        /// Window Loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VM.MainView.Info_Text =
@"Ultra UI ~ Mupen64Plus Frontend
https://github.com/MattMcManis/Ultra
https://ultraui.github.io
u64ui@protonmail.com

The MIT License

Copyright © 2019 Matt McManis

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Mupen64Plus-Core (GPLv2) 
https://mupen64plus.org 
The authors are:
  • Richard Goedeken (Richard42)
  • Sven Eckelmann (ecsv)
  • John Chadwick (NMN)
  • James Hood (Ebenblues)
  • Scott Gorman (okaygo)
  • Scott Knauert (Tillin9)
  • Jesse Dean (DarkJezter)
  • Louai Al-Khanji (slougi)
  • Bob Forder (orbitaldecay)
  • Jason Espinosa (hasone)
  • Bobby Smiles (bsmiles32)
  • Dorian Fevrier (Narann)
  • Richard Hender (ricrpi)
  • Will Nayes (wnayes)
  • Conchur Navid
  • Gillou68310
  • HyperHacker
  • littleguy77
  • Nebuleon
  • and others.

Special thanks to Milan Nikolic's M64Py (GPLv3) for design inspiration.
http://m64py.sourceforge.net
https://github.com/mupen64plus/mupen64plus-ui-python/blob/master/COPYING

API Interop (MIT) Copyright 2019 © BizHawk Team
https://github.com/TASVideos/BizHawk/tree/master/BizHawk.Emulation.Cores/Consoles/Nintendo/N64
https://github.com/TASVideos/BizHawk/blob/master/LICENSE";
        }

        /// <summary>
        /// Window Closing
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
        }

    }
}
