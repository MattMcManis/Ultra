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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra
{
    public class PathsViewModel : INotifyPropertyChanged
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
        /// Paths ViewModel
        /// </summary>
        public PathsViewModel()
        {
            
        }


        // --------------------------------------------------
        // Mupen64Plus Folder - TextBox
        // --------------------------------------------------
        // Text
        private string _Mupen_Text = string.Empty;
        public string Mupen_Text
        {
            get { return _Mupen_Text; }
            set
            {
                if (_Mupen_Text == value)
                {
                    return;
                }

                _Mupen_Text = value;
                OnPropertyChanged("Mupen_Text");
            }
        }

        // --------------------------------------------------
        // Path Config - TextBox
        // --------------------------------------------------
        // Text
        private string _Config_Text = string.Empty;
        public string Config_Text
        {
            get { return _Config_Text; }
            set
            {
                if (_Config_Text == value)
                {
                    return;
                }

                _Config_Text = value;
                OnPropertyChanged("Config_Text");
            }
        }

        // --------------------------------------------------
        // Path Plugins - TextBox
        // --------------------------------------------------
        // Text
        private string _Plugins_Text = string.Empty;
        public string Plugins_Text
        {
            get { return _Plugins_Text; }
            set
            {
                if (_Plugins_Text == value)
                {
                    return;
                }

                _Plugins_Text = value;
                OnPropertyChanged("Plugins_Text");
            }
        }

        // --------------------------------------------------
        // Path Data - TextBox
        // --------------------------------------------------
        // Text
        private string _Data_Text = string.Empty;
        public string Data_Text
        {
            get { return _Data_Text; }
            set
            {
                if (_Data_Text == value)
                {
                    return;
                }

                _Data_Text = value;
                OnPropertyChanged("Data_Text");
            }
        }

        // --------------------------------------------------
        // Path ROMs - TextBox
        // --------------------------------------------------
        // Text
        private string _ROMs_Text = string.Empty;
        public string ROMs_Text
        {
            get { return _ROMs_Text; }
            set
            {
                if (_ROMs_Text == value)
                {
                    return;
                }

                _ROMs_Text = value;
                OnPropertyChanged("ROMs_Text");
            }
        }

    }
}
