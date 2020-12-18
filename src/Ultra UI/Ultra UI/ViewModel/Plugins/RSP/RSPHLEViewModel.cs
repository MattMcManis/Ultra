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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra
{
    public class Plugins_RSP_RSPHLE_ViewModel : INotifyPropertyChanged
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
        /// Plugins RPS - RSP HLE ViewModel
        /// </summary>
        public Plugins_RSP_RSPHLE_ViewModel()
        {
            
        }

        

        // --------------------------------------------------
        // Version
        // --------------------------------------------------
        // Text
        private string _Version_Text = string.Empty;
        public string Version_Text
        {
            get { return _Version_Text; }
            set
            {
                if (_Version_Text == value)
                {
                    return;
                }

                _Version_Text = value;
                OnPropertyChanged("Version_Text");
            }
        }
        // Controls Enable
        private bool _Version_IsEnabled = true;
        public bool Version_IsEnabled
        {
            get { return _Version_IsEnabled; }
            set
            {
                if (_Version_IsEnabled == value)
                {
                    return;
                }

                _Version_IsEnabled = value;
                OnPropertyChanged("Version_IsEnabled");
            }
        }

        // --------------------------------------------------
        // RSPFallback
        // --------------------------------------------------
        // Text
        private string _RSPFallback_Text = string.Empty;
        public string RSPFallback_Text
        {
            get { return _RSPFallback_Text; }
            set
            {
                if (_RSPFallback_Text == value)
                {
                    return;
                }

                _RSPFallback_Text = value;
                OnPropertyChanged("RSPFallback_Text");
            }
        }
        // Controls Enable
        private bool _RSPFallback_IsEnabled = true;
        public bool RSPFallback_IsEnabled
        {
            get { return _RSPFallback_IsEnabled; }
            set
            {
                if (_RSPFallback_IsEnabled == value)
                {
                    return;
                }

                _RSPFallback_IsEnabled = value;
                OnPropertyChanged("RSPFallback_IsEnabled");
            }
        }

        // -------------------------
        // DisplayListToGraphicsPlugin
        // -------------------------
        // Checked
        private bool _DisplayListToGraphicsPlugin_IsChecked;
        public bool DisplayListToGraphicsPlugin_IsChecked
        {
            get { return _DisplayListToGraphicsPlugin_IsChecked; }
            set
            {
                if (_DisplayListToGraphicsPlugin_IsChecked != value)
                {
                    _DisplayListToGraphicsPlugin_IsChecked = value;
                    OnPropertyChanged("DisplayListToGraphicsPlugin_IsChecked");
                }
            }
        }
        // Enabled
        private bool _DisplayListToGraphicsPlugin_IsEnabled = true;
        public bool DisplayListToGraphicsPlugin_IsEnabled
        {
            get { return _DisplayListToGraphicsPlugin_IsEnabled; }
            set
            {
                if (_DisplayListToGraphicsPlugin_IsEnabled == value)
                {
                    return;
                }

                _DisplayListToGraphicsPlugin_IsEnabled = value;
                OnPropertyChanged("DisplayListToGraphicsPlugin_IsEnabled");
            }
        }

        // -------------------------
        // AudioListToAudioPlugin
        // -------------------------
        // Checked
        private bool _AudioListToAudioPlugin_IsChecked;
        public bool AudioListToAudioPlugin_IsChecked
        {
            get { return _AudioListToAudioPlugin_IsChecked; }
            set
            {
                if (_AudioListToAudioPlugin_IsChecked != value)
                {
                    _AudioListToAudioPlugin_IsChecked = value;
                    OnPropertyChanged("AudioListToAudioPlugin_IsChecked");
                }
            }
        }
        // Enabled
        private bool _AudioListToAudioPlugin_IsEnabled = true;
        public bool AudioListToAudioPlugin_IsEnabled
        {
            get { return _AudioListToAudioPlugin_IsEnabled; }
            set
            {
                if (_AudioListToAudioPlugin_IsEnabled == value)
                {
                    return;
                }

                _AudioListToAudioPlugin_IsEnabled = value;
                OnPropertyChanged("AudioListToAudioPlugin_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Save
        // --------------------------------------------------
        // Text
        private string _Save_Text = string.Empty;
        public string Save_Text
        {
            get { return _Save_Text; }
            set
            {
                if (_Save_Text == value)
                {
                    return;
                }

                _Save_Text = value;
                OnPropertyChanged("Save_Text");
            }
        }
        // Controls Enable
        private bool _Save_IsEnabled = true;
        public bool Save_IsEnabled
        {
            get { return _Save_IsEnabled; }
            set
            {
                if (_Save_IsEnabled == value)
                {
                    return;
                }

                _Save_IsEnabled = value;
                OnPropertyChanged("Save_IsEnabled");
            }
        }

    }
}
