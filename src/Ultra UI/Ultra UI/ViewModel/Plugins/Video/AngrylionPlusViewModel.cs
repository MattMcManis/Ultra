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
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class Plugins_Video_AngrylionPlus_ViewModel : INotifyPropertyChanged
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
        /// Plugins - AngrylionPlus ViewModel
        /// </summary>
        public Plugins_Video_AngrylionPlus_ViewModel()
        {

        }


        // ----------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// AngrylionPlus Plugin
        /// </summary>
        // ----------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------

        // --------------------------------------------------
        // Version
        // --------------------------------------------------
        //// Text
        //private string _Version_Text = string.Empty;
        //public string Version_Text
        //{
        //    get { return _Version_Text; }
        //    set
        //    {
        //        if (_Version_Text == value)
        //        {
        //            return;
        //        }

        //        _Version_Text = value;
        //        OnPropertyChanged("Version_Text");
        //    }
        //}
        //// Controls Enable
        //private bool _Version_IsEnabled = false;
        //public bool Version_IsEnabled
        //{
        //    get { return _Version_IsEnabled; }
        //    set
        //    {
        //        if (_Version_IsEnabled == value)
        //        {
        //            return;
        //        }

        //        _Version_IsEnabled = value;
        //        OnPropertyChanged("Version_IsEnabled");
        //    }
        //}

        // -------------------------
        // Parallel
        // -------------------------
        // Checked
        private bool _Parallel_IsChecked;
        public bool Parallel_IsChecked
        {
            get { return _Parallel_IsChecked; }
            set
            {
                if (_Parallel_IsChecked != value)
                {
                    _Parallel_IsChecked = value;
                    OnPropertyChanged("Parallel_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Parallel_IsEnabled = true;
        public bool Parallel_IsEnabled
        {
            get { return _Parallel_IsEnabled; }
            set
            {
                if (_Parallel_IsEnabled == value)
                {
                    return;
                }

                _Parallel_IsEnabled = value;
                OnPropertyChanged("Parallel_IsEnabled");
            }
        }


        // --------------------------------------------------
        // NumWorkers
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _NumWorkers_Items = new ObservableCollection<string>();
        public ObservableCollection<string> NumWorkers_Items
        {
            get { return _NumWorkers_Items; }
            set
            {
                _NumWorkers_Items = value;
                OnPropertyChanged("NumWorkers_Items");
            }
        }

        // Selected Index
        private int _NumWorkers_SelectedIndex { get; set; }
        public int NumWorkers_SelectedIndex
        {
            get { return _NumWorkers_SelectedIndex; }
            set
            {
                if (_NumWorkers_SelectedIndex == value)
                {
                    return;
                }

                _NumWorkers_SelectedIndex = value;
                OnPropertyChanged("NumWorkers_SelectedIndex");
            }
        }

        // Selected Item
        private string _NumWorkers_SelectedItem { get; set; }
        public string NumWorkers_SelectedItem
        {
            get { return _NumWorkers_SelectedItem; }
            set
            {
                if (_NumWorkers_SelectedItem == value)
                {
                    return;
                }

                _NumWorkers_SelectedItem = value;
                OnPropertyChanged("NumWorkers_SelectedItem");
            }
        }

        // Controls Enable
        private bool _NumWorkers_IsEnabled = true;
        public bool NumWorkers_IsEnabled
        {
            get { return _NumWorkers_IsEnabled; }
            set
            {
                if (_NumWorkers_IsEnabled == value)
                {
                    return;
                }

                _NumWorkers_IsEnabled = value;
                OnPropertyChanged("NumWorkers_IsEnabled");
            }
        }

        // --------------------------------------------------
        // ViMode
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _ViMode_Items = new ObservableCollection<string>
        {
            "Filtered",
            "Unfiltered",
            "Depth",
            "Coverage"
        };
        public ObservableCollection<string> ViMode_Items
        {
            get { return _ViMode_Items; }
            set
            {
                _ViMode_Items = value;
                OnPropertyChanged("ViMode_Items");
            }
        }

        // Selected Index
        private int _ViMode_SelectedIndex { get; set; }
        public int ViMode_SelectedIndex
        {
            get { return _ViMode_SelectedIndex; }
            set
            {
                if (_ViMode_SelectedIndex == value)
                {
                    return;
                }

                _ViMode_SelectedIndex = value;
                OnPropertyChanged("ViMode_SelectedIndex");
            }
        }

        // Selected Item
        private string _ViMode_SelectedItem { get; set; }
        public string ViMode_SelectedItem
        {
            get { return _ViMode_SelectedItem; }
            set
            {
                if (_ViMode_SelectedItem == value)
                {
                    return;
                }

                _ViMode_SelectedItem = value;
                OnPropertyChanged("ViMode_SelectedItem");
            }
        }

        // Controls Enable
        private bool _ViMode_IsEnabled = true;
        public bool ViMode_IsEnabled
        {
            get { return _ViMode_IsEnabled; }
            set
            {
                if (_ViMode_IsEnabled == value)
                {
                    return;
                }

                _ViMode_IsEnabled = value;
                OnPropertyChanged("ViMode_IsEnabled");
            }
        }

        // --------------------------------------------------
        // ViInterpolation
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _ViInterpolation_Items = new ObservableCollection<string>
        {
            "Nearest Neighbor",
            "Linear"
        };
        public ObservableCollection<string> ViInterpolation_Items
        {
            get { return _ViInterpolation_Items; }
            set
            {
                _ViInterpolation_Items = value;
                OnPropertyChanged("ViInterpolation_Items");
            }
        }

        // Selected Index
        private int _ViInterpolation_SelectedIndex { get; set; }
        public int ViInterpolation_SelectedIndex
        {
            get { return _ViInterpolation_SelectedIndex; }
            set
            {
                if (_ViInterpolation_SelectedIndex == value)
                {
                    return;
                }

                _ViInterpolation_SelectedIndex = value;
                OnPropertyChanged("ViInterpolation_SelectedIndex");
            }
        }

        // Selected Item
        private string _ViInterpolation_SelectedItem { get; set; }
        public string ViInterpolation_SelectedItem
        {
            get { return _ViInterpolation_SelectedItem; }
            set
            {
                if (_ViInterpolation_SelectedItem == value)
                {
                    return;
                }

                _ViInterpolation_SelectedItem = value;
                OnPropertyChanged("ViInterpolation_SelectedItem");
            }
        }

        // Controls Enable
        private bool _ViInterpolation_IsEnabled = true;
        public bool ViInterpolation_IsEnabled
        {
            get { return _ViInterpolation_IsEnabled; }
            set
            {
                if (_ViInterpolation_IsEnabled == value)
                {
                    return;
                }

                _ViInterpolation_IsEnabled = value;
                OnPropertyChanged("ViInterpolation_IsEnabled");
            }
        }

        // -------------------------
        // ViWidescreen
        // -------------------------
        // Checked
        private bool _ViWidescreen_IsChecked;
        public bool ViWidescreen_IsChecked
        {
            get { return _ViWidescreen_IsChecked; }
            set
            {
                if (_ViWidescreen_IsChecked != value)
                {
                    _ViWidescreen_IsChecked = value;
                    OnPropertyChanged("ViWidescreen_IsChecked");
                }
            }
        }
        // Enabled
        private bool _ViWidescreen_IsEnabled = true;
        public bool ViWidescreen_IsEnabled
        {
            get { return _ViWidescreen_IsEnabled; }
            set
            {
                if (_ViWidescreen_IsEnabled == value)
                {
                    return;
                }

                _ViWidescreen_IsEnabled = value;
                OnPropertyChanged("ViWidescreen_IsEnabled");
            }
        }

        // -------------------------
        // ViHideOverscan
        // -------------------------
        // Checked
        private bool _ViHideOverscan_IsChecked;
        public bool ViHideOverscan_IsChecked
        {
            get { return _ViHideOverscan_IsChecked; }
            set
            {
                if (_ViHideOverscan_IsChecked != value)
                {
                    _ViHideOverscan_IsChecked = value;
                    OnPropertyChanged("ViHideOverscan_IsChecked");
                }
            }
        }
        // Enabled
        private bool _ViHideOverscan_IsEnabled = true;
        public bool ViHideOverscan_IsEnabled
        {
            get { return _ViHideOverscan_IsEnabled; }
            set
            {
                if (_ViHideOverscan_IsEnabled == value)
                {
                    return;
                }

                _ViHideOverscan_IsEnabled = value;
                OnPropertyChanged("ViHideOverscan_IsEnabled");
            }
        }


        // --------------------------------------------------
        // DpCompat
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _DpCompat_Items = new ObservableCollection<string>
        {
            "Fast",
            "Moderate",
            "Slow"
        };
        public ObservableCollection<string> DpCompat_Items
        {
            get { return _DpCompat_Items; }
            set
            {
                _DpCompat_Items = value;
                OnPropertyChanged("DpCompat_Items");
            }
        }

        // Selected Index
        private int _DpCompat_SelectedIndex { get; set; }
        public int DpCompat_SelectedIndex
        {
            get { return _DpCompat_SelectedIndex; }
            set
            {
                if (_DpCompat_SelectedIndex == value)
                {
                    return;
                }

                _DpCompat_SelectedIndex = value;
                OnPropertyChanged("DpCompat_SelectedIndex");
            }
        }

        // Selected Item
        private string _DpCompat_SelectedItem { get; set; }
        public string DpCompat_SelectedItem
        {
            get { return _DpCompat_SelectedItem; }
            set
            {
                if (_DpCompat_SelectedItem == value)
                {
                    return;
                }

                _DpCompat_SelectedItem = value;
                OnPropertyChanged("DpCompat_SelectedItem");
            }
        }

        // Controls Enable
        private bool _DpCompat_IsEnabled = true;
        public bool DpCompat_IsEnabled
        {
            get { return _DpCompat_IsEnabled; }
            set
            {
                if (_DpCompat_IsEnabled == value)
                {
                    return;
                }

                _DpCompat_IsEnabled = value;
                OnPropertyChanged("DpCompat_IsEnabled");
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
