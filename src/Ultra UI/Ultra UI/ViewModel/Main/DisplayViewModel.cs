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

namespace ViewModel
{
    public class DisplayViewModel : INotifyPropertyChanged
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
        /// Plugins ViewModel
        /// </summary>
        public DisplayViewModel()
        {
            //Display_View_SelectedItem = "Windowed";
            //Display_Resolution_SelectedItem = "960x720";
            Display_Resolution_IsEnabled = true;
            Display_View_IsEnabled = true;
            Display_Vsync_IsEnabled = true;
            Display_VideoExtension_IsEnabled = true;
            Display_Screensaver_IsEnabled = false;
            Display_OSD_IsEnabled = true;
        }


        // --------------------------------------------------
        // Resolution
        // --------------------------------------------------
        // Items Source
        public class Resolution
        {
            public string Name { get; set; }
            public bool Category { get; set; }
        }

        public ObservableCollection<Resolution> _Display_Resolution_Items = new ObservableCollection<Resolution>()
        {
            // 4:3
            new Resolution() { Name = "4:3",       Category = true  },
            new Resolution() { Name = "8192x6144", Category = false },
            new Resolution() { Name = "7680x5760", Category = false },
            new Resolution() { Name = "4096x3072", Category = false },
            new Resolution() { Name = "3840x2880", Category = false },
            new Resolution() { Name = "2133x1600", Category = false },
            new Resolution() { Name = "1920x1440", Category = false },
            new Resolution() { Name = "1600x1200", Category = false },
            new Resolution() { Name = "1440x1080", Category = false },
            new Resolution() { Name = "1280x960",  Category = false },
            new Resolution() { Name = "1024x768",  Category = false },
            new Resolution() { Name = "960x720",   Category = false },
            new Resolution() { Name = "800x600",   Category = false },
            new Resolution() { Name = "768x576",   Category = false },
            new Resolution() { Name = "640x480",   Category = false },
            //new Resolution() { Name = "600x450", Category = false },
            //new Resolution() { Name = "400x300", Category = false },
            new Resolution() { Name = "320x240",   Category = false },

            // 16:9
            new Resolution() { Name = "16:9",       Category = true  },
            new Resolution() { Name = "8192x4608",  Category = false },
            new Resolution() { Name = "7680x4320",  Category = false },
            new Resolution() { Name = "4096x2304",  Category = false },
            new Resolution() { Name = "3840x2160",  Category = false },
            new Resolution() { Name = "2560x1440",  Category = false },
            new Resolution() { Name = "1920x1080",  Category = false },
            new Resolution() { Name = "1600x900",   Category = false },
            new Resolution() { Name = "1366x768",   Category = false },
            new Resolution() { Name = "1280x720",   Category = false },
            new Resolution() { Name = "1024x576",   Category = false },
            new Resolution() { Name = "960x540",    Category = false },
            new Resolution() { Name = "854x480",    Category = false },
            new Resolution() { Name = "640x360",    Category = false },
            new Resolution() { Name = "426x240",    Category = false },

            // 16:10
            new Resolution() { Name = "16:10",     Category = true  },
            new Resolution() { Name = "3840x2400", Category = false },
            new Resolution() { Name = "2560x1600", Category = false },
            new Resolution() { Name = "1920x1200", Category = false },
            new Resolution() { Name = "1680x1050", Category = false },
            new Resolution() { Name = "1440x900",  Category = false },
            new Resolution() { Name = "1280x800",  Category = false },
            new Resolution() { Name = "960x600",   Category = false },
            new Resolution() { Name = "640x400",   Category = false },
            new Resolution() { Name = "320x200",   Category = false },
        };
        public ObservableCollection<Resolution> Display_Resolution_Items
        {
            get { return _Display_Resolution_Items; }
            set
            {
                _Display_Resolution_Items = value;
                OnPropertyChanged("Display_Resolution_Items");
            }
        }
        //// Items Source
        //private List<string> _Display_Resolution_Items = new List<string>()
        //{
        //    "4096x3072",
        //    "3840x2880",
        //    "2133x1600",
        //    "1920x1440",
        //    "1600x1200",
        //    "1440x1080",
        //    "1280x960",
        //    "1024x768",
        //    "960x720",
        //    "800x600",
        //    "768x576",
        //    "640x480",
        //    "600x450",
        //    "400x300",
        //    "320x240"
        //};
        //public List<string> Display_Resolution_Items
        //{
        //    get { return _Display_Resolution_Items; }
        //    set
        //    {
        //        _Display_Resolution_Items = value;
        //        OnPropertyChanged("Display_Resolution_Items");
        //    }
        //}

        // Selected Index
        private int _Display_Resolution_SelectedIndex;
        public int Display_Resolution_SelectedIndex
        {
            get { return _Display_Resolution_SelectedIndex; }
            set
            {
                if (_Display_Resolution_SelectedIndex == value)
                {
                    return;
                }

                _Display_Resolution_SelectedIndex = value;
                OnPropertyChanged("Display_Resolution_SelectedIndex");
            }
        }

        // Selected Item
        private string _Display_Resolution_SelectedItem;
        public string Display_Resolution_SelectedItem
        {
            get { return _Display_Resolution_SelectedItem; }
            set
            {
                if (_Display_Resolution_SelectedItem == value)
                {
                    return;
                }

                _Display_Resolution_SelectedItem = value;
                OnPropertyChanged("Display_Resolution_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Display_Resolution_IsEnabled;
        public bool Display_Resolution_IsEnabled
        {
            get { return _Display_Resolution_IsEnabled; }
            set
            {
                if (_Display_Resolution_IsEnabled == value)
                {
                    return;
                }

                _Display_Resolution_IsEnabled = value;
                OnPropertyChanged("Display_Resolution_IsEnabled");
            }
        }

        // --------------------------------------------------
        // View
        // --------------------------------------------------
        // Items Source
        private List<string> _Display_View_Items = new List<string>()
        {
            "Fullscreen",
            "Windowed"
        };
        public List<string> Display_View_Items
        {
            get { return _Display_View_Items; }
            set
            {
                _Display_View_Items = value;
                OnPropertyChanged("Display_View_Items");
            }
        }

        // Selected Index
        private int _Display_View_SelectedIndex;
        public int Display_View_SelectedIndex
        {
            get { return _Display_View_SelectedIndex; }
            set
            {
                if (_Display_View_SelectedIndex == value)
                {
                    return;
                }

                _Display_View_SelectedIndex = value;
                OnPropertyChanged("Display_View_SelectedIndex");
            }
        }

        // Selected Item
        private string _Display_View_SelectedItem;
        public string Display_View_SelectedItem
        {
            get { return _Display_View_SelectedItem; }
            set
            {
                if (_Display_View_SelectedItem == value)
                {
                    return;
                }

                _Display_View_SelectedItem = value;
                OnPropertyChanged("Display_View_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Display_View_IsEnabled;
        public bool Display_View_IsEnabled
        {
            get { return _Display_View_IsEnabled; }
            set
            {
                if (_Display_View_IsEnabled == value)
                {
                    return;
                }

                _Display_View_IsEnabled = value;
                OnPropertyChanged("Display_View_IsEnabled");
            }
        }

        // -------------------------
        // Vsync
        // -------------------------
        // Checked
        private bool _Display_Vsync_IsChecked;
        public bool Display_Vsync_IsChecked
        {
            get { return _Display_Vsync_IsChecked; }
            set
            {
                if (_Display_Vsync_IsChecked != value)
                {
                    _Display_Vsync_IsChecked = value;
                    OnPropertyChanged("Display_Vsync_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Display_Vsync_IsEnabled;
        public bool Display_Vsync_IsEnabled
        {
            get { return _Display_Vsync_IsEnabled; }
            set
            {
                if (_Display_Vsync_IsEnabled == value)
                {
                    return;
                }

                _Display_Vsync_IsEnabled = value;
                OnPropertyChanged("Display_Vsync_IsEnabled");
            }
        }

        // -------------------------
        // Video Extension
        // -------------------------
        // Checked
        private bool _Display_VideoExtension_IsChecked;
        public bool Display_VideoExtension_IsChecked
        {
            get { return _Display_VideoExtension_IsChecked; }
            set
            {
                if (_Display_VideoExtension_IsChecked != value)
                {
                    _Display_VideoExtension_IsChecked = value;
                    OnPropertyChanged("Display_VideoExtension_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Display_VideoExtension_IsEnabled;
        public bool Display_VideoExtension_IsEnabled
        {
            get { return _Display_VideoExtension_IsEnabled; }
            set
            {
                if (_Display_VideoExtension_IsEnabled == value)
                {
                    return;
                }

                _Display_VideoExtension_IsEnabled = value;
                OnPropertyChanged("Display_VideoExtension_IsEnabled");
            }
        }

        // -------------------------
        // OSD
        // -------------------------
        // Checked
        private bool _Display_OSD_IsChecked;
        public bool Display_OSD_IsChecked
        {
            get { return _Display_OSD_IsChecked; }
            set
            {
                if (_Display_OSD_IsChecked != value)
                {
                    _Display_OSD_IsChecked = value;
                    OnPropertyChanged("Display_OSD_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Display_OSD_IsEnabled;
        public bool Display_OSD_IsEnabled
        {
            get { return _Display_OSD_IsEnabled; }
            set
            {
                if (_Display_OSD_IsEnabled == value)
                {
                    return;
                }

                _Display_OSD_IsEnabled = value;
                OnPropertyChanged("Display_OSD_IsEnabled");
            }
        }

        // -------------------------
        // Screensaver
        // -------------------------
        // Checked
        private bool _Display_Screensaver_IsChecked;
        public bool Display_Screensaver_IsChecked
        {
            get { return _Display_Screensaver_IsChecked; }
            set
            {
                if (_Display_Screensaver_IsChecked != value)
                {
                    _Display_Screensaver_IsChecked = value;
                    OnPropertyChanged("Display_Screensaver_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Display_Screensaver_IsEnabled;
        public bool Display_Screensaver_IsEnabled
        {
            get { return _Display_Screensaver_IsEnabled; }
            set
            {
                if (_Display_Screensaver_IsEnabled == value)
                {
                    return;
                }

                _Display_Screensaver_IsEnabled = value;
                OnPropertyChanged("Display_Screensaver_IsEnabled");
            }
        }

    }
}
