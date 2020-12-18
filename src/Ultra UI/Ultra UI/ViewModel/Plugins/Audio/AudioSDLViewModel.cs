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
    public class Plugins_Audio_AudioSDL_ViewModel : INotifyPropertyChanged
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
        /// Plugins - AudioSDL ViewModel
        /// </summary>
        public Plugins_Audio_AudioSDL_ViewModel()
        {
            
        }
       
        // --------------------------------------------------
        // Audio SDL Version
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
        // Audio SDL Default Frequency
        // --------------------------------------------------
        // Text
        private string _DefaultFrequency_Text = string.Empty;
        public string DefaultFrequency_Text
        {
            get { return _DefaultFrequency_Text; }
            set
            {
                if (_DefaultFrequency_Text == value)
                {
                    return;
                }

                _DefaultFrequency_Text = value;
                OnPropertyChanged("DefaultFrequency_Text");
            }
        }
        // Controls Enable
        private bool _DefaultFrequency_IsEnabled = true;
        public bool DefaultFrequency_IsEnabled
        {
            get { return _DefaultFrequency_IsEnabled; }
            set
            {
                if (_DefaultFrequency_IsEnabled == value)
                {
                    return;
                }

                _DefaultFrequency_IsEnabled = value;
                OnPropertyChanged("DefaultFrequency_IsEnabled");
            }
        }

        // -------------------------
        // Audio SDL Swap Channels
        // -------------------------
        // Checked
        private bool _SwapChannels_IsChecked;
        public bool SwapChannels_IsChecked
        {
            get { return _SwapChannels_IsChecked; }
            set
            {
                if (_SwapChannels_IsChecked != value)
                {
                    _SwapChannels_IsChecked = value;
                    OnPropertyChanged("SwapChannels_IsChecked");
                }
            }
        }
        // Enabled
        private bool _SwapChannels_IsEnabled = true;
        public bool SwapChannels_IsEnabled
        {
            get { return _SwapChannels_IsEnabled; }
            set
            {
                if (_SwapChannels_IsEnabled == value)
                {
                    return;
                }

                _SwapChannels_IsEnabled = value;
                OnPropertyChanged("SwapChannels_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Audio SDL Primary Buffer Size
        // --------------------------------------------------
        // Text
        private string _PrimaryBufferSize_Text = string.Empty;
        public string PrimaryBufferSize_Text
        {
            get { return _PrimaryBufferSize_Text; }
            set
            {
                if (_PrimaryBufferSize_Text == value)
                {
                    return;
                }

                _PrimaryBufferSize_Text = value;
                OnPropertyChanged("PrimaryBufferSize_Text");
            }
        }
        // Controls Enable
        private bool _PrimaryBufferSize_IsEnabled = true;
        public bool PrimaryBufferSize_IsEnabled
        {
            get { return _PrimaryBufferSize_IsEnabled; }
            set
            {
                if (_PrimaryBufferSize_IsEnabled == value)
                {
                    return;
                }

                _PrimaryBufferSize_IsEnabled = value;
                OnPropertyChanged("PrimaryBufferSize_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Audio SDL Primary Buffer Target
        // --------------------------------------------------
        // Text
        private string _PrimaryBufferTarget_Text = string.Empty;
        public string PrimaryBufferTarget_Text
        {
            get { return _PrimaryBufferTarget_Text; }
            set
            {
                if (_PrimaryBufferTarget_Text == value)
                {
                    return;
                }

                _PrimaryBufferTarget_Text = value;
                OnPropertyChanged("PrimaryBufferTarget_Text");
            }
        }
        // Controls Enable
        private bool _PrimaryBufferTarget_IsEnabled = true;
        public bool PrimaryBufferTarget_IsEnabled
        {
            get { return _PrimaryBufferTarget_IsEnabled; }
            set
            {
                if (_PrimaryBufferTarget_IsEnabled == value)
                {
                    return;
                }

                _PrimaryBufferTarget_IsEnabled = value;
                OnPropertyChanged("PrimaryBufferTarget_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Audio SDL Secondary Buffer Size
        // --------------------------------------------------
        // Text
        private string _SecondaryBufferSize_Text = string.Empty;
        public string SecondaryBufferSize_Text
        {
            get { return _SecondaryBufferSize_Text; }
            set
            {
                if (_SecondaryBufferSize_Text == value)
                {
                    return;
                }

                _SecondaryBufferSize_Text = value;
                OnPropertyChanged("SecondaryBufferSize_Text");
            }
        }
        // Controls Enable
        private bool _SecondaryBufferSize_IsEnabled = true;
        public bool SecondaryBufferSize_IsEnabled
        {
            get { return _SecondaryBufferSize_IsEnabled; }
            set
            {
                if (_SecondaryBufferSize_IsEnabled == value)
                {
                    return;
                }

                _SecondaryBufferSize_IsEnabled = value;
                OnPropertyChanged("SecondaryBufferSize_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Audio SDL Resample
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _Resample_Items = new ObservableCollection<string>
        {
            "src-sinc-best-quality",
            "src-sinc-medium-quality",
            "src-sinc-fastest",
            "src-zero-order-hold",
            "src-linear",
            "speex-fixed-0",
            "speex-fixed-1",
            "speex-fixed-2",
            "speex-fixed-3",
            "speex-fixed-4",
            "speex-fixed-5",
            "speex-fixed-6",
            "speex-fixed-7",
            "speex-fixed-8",
            "speex-fixed-9",
            "speex-fixed-10",
            "trivial"
        };
        public ObservableCollection<string> Resample_Items
        {
            get { return _Resample_Items; }
            set
            {
                _Resample_Items = value;
                OnPropertyChanged("Resample_Items");
            }
        }

        // Selected Index
        private int _Resample_SelectedIndex { get; set; }
        public int Resample_SelectedIndex
        {
            get { return _Resample_SelectedIndex; }
            set
            {
                if (_Resample_SelectedIndex == value)
                {
                    return;
                }

                _Resample_SelectedIndex = value;
                OnPropertyChanged("Resample_SelectedIndex");
            }
        }

        // Selected Item
        private string _Resample_SelectedItem { get; set; }
        public string Resample_SelectedItem
        {
            get { return _Resample_SelectedItem; }
            set
            {
                if (_Resample_SelectedItem == value)
                {
                    return;
                }

                _Resample_SelectedItem = value;
                OnPropertyChanged("Resample_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Resample_IsEnabled = true;
        public bool Resample_IsEnabled
        {
            get { return _Resample_IsEnabled; }
            set
            {
                if (_Resample_IsEnabled == value)
                {
                    return;
                }

                _Resample_IsEnabled = value;
                OnPropertyChanged("Resample_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Audio SDL VolumeControlType
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _VolumeControlType_Items = new ObservableCollection<string>
        {
            "SDL",
            "OSS mixer"
        };
        public ObservableCollection<string> VolumeControlType_Items
        {
            get { return _VolumeControlType_Items; }
            set
            {
                _VolumeControlType_Items = value;
                OnPropertyChanged("VolumeControlType_Items");
            }
        }

        // Selected Index
        private int _VolumeControlType_SelectedIndex { get; set; }
        public int VolumeControlType_SelectedIndex
        {
            get { return _VolumeControlType_SelectedIndex; }
            set
            {
                if (_VolumeControlType_SelectedIndex == value)
                {
                    return;
                }

                _VolumeControlType_SelectedIndex = value;
                OnPropertyChanged("VolumeControlType_SelectedIndex");
            }
        }

        // Selected Item
        private string _VolumeControlType_SelectedItem { get; set; }
        public string VolumeControlType_SelectedItem
        {
            get { return _VolumeControlType_SelectedItem; }
            set
            {
                if (_VolumeControlType_SelectedItem == value)
                {
                    return;
                }

                _VolumeControlType_SelectedItem = value;
                OnPropertyChanged("VolumeControlType_SelectedItem");
            }
        }

        // Controls Enable
        private bool _VolumeControlType_IsEnabled = true;
        public bool VolumeControlType_IsEnabled
        {
            get { return _VolumeControlType_IsEnabled; }
            set
            {
                if (_VolumeControlType_IsEnabled == value)
                {
                    return;
                }

                _VolumeControlType_IsEnabled = value;
                OnPropertyChanged("VolumeControlType_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Audio SDL VolumeAdjust
        // --------------------------------------------------
        // Text
        private string _VolumeAdjust_Text = string.Empty;
        public string VolumeAdjust_Text
        {
            get { return _VolumeAdjust_Text; }
            set
            {
                if (_VolumeAdjust_Text == value)
                {
                    return;
                }

                _VolumeAdjust_Text = value;
                OnPropertyChanged("VolumeAdjust_Text");
            }
        }
        // Controls Enable
        private bool _VolumeAdjust_IsEnabled = true;
        public bool VolumeAdjust_IsEnabled
        {
            get { return _VolumeAdjust_IsEnabled; }
            set
            {
                if (_VolumeAdjust_IsEnabled == value)
                {
                    return;
                }

                _VolumeAdjust_IsEnabled = value;
                OnPropertyChanged("VolumeAdjust_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Audio SDL VolumeDefault
        // --------------------------------------------------
        // Text
        private string _VolumeDefault_Text = string.Empty;
        public string VolumeDefault_Text
        {
            get { return _VolumeDefault_Text; }
            set
            {
                if (_VolumeDefault_Text == value)
                {
                    return;
                }

                _VolumeDefault_Text = value;
                OnPropertyChanged("VolumeDefault_Text");
            }
        }
        // Controls Enable
        private bool _VolumeDefault_IsEnabled = true;
        public bool VolumeDefault_IsEnabled
        {
            get { return _VolumeDefault_IsEnabled; }
            set
            {
                if (_VolumeDefault_IsEnabled == value)
                {
                    return;
                }

                _VolumeDefault_IsEnabled = value;
                OnPropertyChanged("VolumeDefault_IsEnabled");
            }
        }

        // -------------------------
        // Audio SDL AudioSync
        // -------------------------
        // Checked
        private bool _AudioSync_IsChecked;
        public bool AudioSync_IsChecked
        {
            get { return _AudioSync_IsChecked; }
            set
            {
                if (_AudioSync_IsChecked != value)
                {
                    _AudioSync_IsChecked = value;
                    OnPropertyChanged("AudioSync_IsChecked");
                }
            }
        }
        // Enabled
        private bool _AudioSync_IsEnabled = true;
        public bool AudioSync_IsEnabled
        {
            get { return _AudioSync_IsEnabled; }
            set
            {
                if (_AudioSync_IsEnabled == value)
                {
                    return;
                }

                _AudioSync_IsEnabled = value;
                OnPropertyChanged("AudioSync_IsEnabled");
            }
        }

        // --------------------------------------------------
        // AudioSDL Save
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
