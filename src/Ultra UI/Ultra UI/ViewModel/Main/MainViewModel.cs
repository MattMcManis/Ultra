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
using System.Windows;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
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
        /// Main ViewModel
        /// </summary>
        public MainViewModel()
        {
            Window_Width = 480;
            Window_Height = 500;

            // Defaults
            Info_Text = string.Empty;
            Debug_Text = string.Empty;
            CfgErrorNotice_Text = string.Empty;

            UpdateAutoCheck_IsChecked = true;
            UpdateAutoCheck_IsEnabled = true;

            StateSlot0_IsChecked = true;
            StateSlot1_IsChecked = false;
            StateSlot2_IsChecked = false;
            StateSlot3_IsChecked = false;
            StateSlot4_IsChecked = false;
            StateSlot5_IsChecked = false;
            StateSlot6_IsChecked = false;
            StateSlot7_IsChecked = false;
            StateSlot8_IsChecked = false;
            StateSlot9_IsChecked = false;
        }


        // Ultra UI Current Version
        public static Version currentVersion;
        // Ultra UI GitHub Latest Version
        public static Version latestVersion;
        // Alpha, Beta, Stable
        public static string currentBuildPhase = "alpha";
        public static string latestBuildPhase;
        public static string[] splitVersionBuildPhase;


        // -------------------------
        // Window Top
        // -------------------------
        // Value
        //private double _Window_Position_Top = 0;
        //public double Window_Position_Top
        //{
        //    get { return _Window_Position_Top; }
        //    set
        //    {
        //        if (_Window_Position_Top == value)
        //        {
        //            return;
        //        }

        //        _Window_Position_Top = value;
        //        OnPropertyChanged("Window_Position_Top");
        //    }
        //}

        // -------------------------
        // Window Left
        // -------------------------
        //private double _Window_Position_Left = 0;
        //public double Window_Position_Left
        //{
        //    get { return _Window_Position_Left; }
        //    set
        //    {
        //        if (_Window_Position_Left == value)
        //        {
        //            return;
        //        }

        //        _Window_Position_Left = value;
        //        OnPropertyChanged("Window_Position_Left");
        //    }
        //}

        // -------------------------
        // Window Width
        // -------------------------
        // Value
        private double _Window_Width;
        public double Window_Width
        {
            get { return _Window_Width; }
            set
            {
                if (_Window_Width == value)
                {
                    return;
                }

                _Window_Width = value;
                OnPropertyChanged("Window_Width");
            }
        }

        // -------------------------
        // Window Height
        // -------------------------
        // Value
        private double _Window_Height;
        public double Window_Height
        {
            get { return _Window_Height; }
            set
            {
                if (_Window_Height == value)
                {
                    return;
                }

                _Window_Height = value;
                OnPropertyChanged("Window_Height");
            }
        }

        // -------------------------
        // Window Maximized
        // -------------------------
        // Value
        //private bool _Window_IsMaximized;
        //public bool Window_IsMaximized
        //{
        //    get { return _Window_IsMaximized; }
        //    set
        //    {
        //        if (_Window_IsMaximized == value)
        //        {
        //            return;
        //        }

        //        _Window_IsMaximized = value;
        //        OnPropertyChanged("Window_IsMaximized");
        //    }
        //}


        // -------------------------
        // Window State
        // -------------------------
        // Value
        //private WindowState _Window_State = WindowState.Normal;
        //public WindowState Window_State
        //{
        //    get { return _Window_State; }
        //    set
        //    {
        //        if (_Window_State == value)
        //        {
        //            return;
        //        }

        //        _Window_State = value;
        //        OnPropertyChanged("Window_State");
        //    }
        //}


        // -------------------------
        // Window Title
        // -------------------------
        // Text
        private string _TitleVersion;
        public string TitleVersion
        {
            get { return _TitleVersion; }
            set
            {
                if (value != _TitleVersion)
                {
                    _TitleVersion = value;
                    OnPropertyChanged("TitleVersion");
                }
            }
        }


        //// -------------------------
        //// Window Icon
        //// -------------------------
        //// Text
        //private string _WindowIcon;
        //public string WindowIcon
        //{
        //    get { return _WindowIcon; }
        //    set
        //    {
        //        if (value != _WindowIcon)
        //        {
        //            _WindowIcon = value;
        //            OnPropertyChanged("WindowIcon");
        //        }
        //    }
        //}


        // -------------------------
        // Update Auto Check - Toggle
        // -------------------------
        // Checked
        private bool _UpdateAutoCheck_IsChecked;
        public bool UpdateAutoCheck_IsChecked
        {
            get { return _UpdateAutoCheck_IsChecked; }
            set
            {
                if (_UpdateAutoCheck_IsChecked != value)
                {
                    _UpdateAutoCheck_IsChecked = value;
                    OnPropertyChanged("UpdateAutoCheck_IsChecked");
                }
            }
        }

        // Text
        //private string _UpdateAutoCheck_Text;
        //public string UpdateAutoCheck_Text
        //{
        //    get { return _UpdateAutoCheck_Text; }
        //    set
        //    {
        //        if (_UpdateAutoCheck_Text == value)
        //        {
        //            return;
        //        }

        //        _UpdateAutoCheck_Text = value;
        //        OnPropertyChanged("UpdateAutoCheck_Text");
        //    }
        //}

        // Enabled
        private bool _UpdateAutoCheck_IsEnabled;
        public bool UpdateAutoCheck_IsEnabled
        {
            get { return _UpdateAutoCheck_IsEnabled; }
            set
            {
                if (_UpdateAutoCheck_IsEnabled == value)
                {
                    return;
                }

                _UpdateAutoCheck_IsEnabled = value;
                OnPropertyChanged("UpdateAutoCheck_IsEnabled");
            }
        }


        // --------------------------------------------------
        // Info
        // --------------------------------------------------
        // Text
        private string _Info_Text;
        public string Info_Text
        {
            get { return _Info_Text; }
            set
            {
                if (_Info_Text == value)
                {
                    return;
                }

                _Info_Text = value;
                OnPropertyChanged("Info_Text");
            }
        }

        // --------------------------------------------------
        // Debug
        // --------------------------------------------------
        // Text
        private string _Debug_Text;
        public string Debug_Text
        {
            get { return _Debug_Text; }
            set
            {
                if (_Debug_Text == value)
                {
                    return;
                }

                _Debug_Text = value;
                OnPropertyChanged("Debug_Text");
            }
        }

        // --------------------------------------------------
        // Cfg Missing Notice
        // --------------------------------------------------
        // Text
        private string _CfgErrorNotice_Text;
        public string CfgErrorNotice_Text
        {
            get { return _CfgErrorNotice_Text; }
            set
            {
                if (_CfgErrorNotice_Text == value)
                {
                    return;
                }

                _CfgErrorNotice_Text = value;
                OnPropertyChanged("CfgErrorNotice_Text");
            }
        }

        // -------------------------
        // Games
        // -------------------------
        public partial class Games
        {
            // Full Path
            public string FullPath { get; set; }
            // Directory
            public string Directory { get; set; } 
            // Name
            public string Name { get; set; }
        }

        public static ObservableCollection<Games> _Games_Items = new ObservableCollection<Games>();
        public ObservableCollection<Games> Games_Items
        {
            get { return _Games_Items; }
            set
            {
                _Games_Items = value;
                OnPropertyChanged("Games_Items");
            }
        }

        // Selected Items
        private List<string> _Games_SelectedItems = new List<string>();
        public List<string> Games_SelectedItems
        {
            get { return _Games_SelectedItems; }
            set
            {
                _Games_SelectedItems = value;
                OnPropertyChanged("Games_SelectedItems");
            }
        }

        // Selected Index
        private int _Games_SelectedIndex;
        public int Games_SelectedIndex
        {
            get { return _Games_SelectedIndex; }
            set
            {
                if (_Games_SelectedIndex == value)
                {
                    return;
                }

                _Games_SelectedIndex = value;
                OnPropertyChanged("Games_SelectedIndex");
            }
        }

        // Selected Item
        private string _Games_SelectedItem;
        public string Games_SelectedItem
        {
            get { return _Games_SelectedItem; }
            set
            {
                if (_Games_SelectedItem == value)
                {
                    return;
                }

                _Games_SelectedItem = value;
                OnPropertyChanged("Games_SelectedItem");
            }
        }


        // --------------------------------------------------
        // Cfg
        // --------------------------------------------------
        // Text
        private string _Cfg_Text;
        public string Cfg_Text
        {
            get { return _Cfg_Text; }
            set
            {
                if (value != _Cfg_Text)
                {
                    _Cfg_Text = value;
                    OnPropertyChanged("Cfg_Text");
                }
            }
        }

        // -------------------------
        // StateSlot0
        // -------------------------
        // Checked
        private bool _StateSlot0_IsChecked;
        public bool StateSlot0_IsChecked
        {
            get { return _StateSlot0_IsChecked; }
            set
            {
                if (_StateSlot0_IsChecked != value)
                {
                    _StateSlot0_IsChecked = value;
                    OnPropertyChanged("StateSlot0_IsChecked");
                }
            }
        }
        // Enabled
        private bool _StateSlot0_IsEnabled;
        public bool StateSlot0_IsEnabled
        {
            get { return _StateSlot0_IsEnabled; }
            set
            {
                if (_StateSlot0_IsEnabled == value)
                {
                    return;
                }

                _StateSlot0_IsEnabled = value;
                OnPropertyChanged("StateSlot0_IsEnabled");
            }
        }

        // -------------------------
        // StateSlot1
        // -------------------------
        // Checked
        private bool _StateSlot1_IsChecked;
        public bool StateSlot1_IsChecked
        {
            get { return _StateSlot1_IsChecked; }
            set
            {
                if (_StateSlot1_IsChecked != value)
                {
                    _StateSlot1_IsChecked = value;
                    OnPropertyChanged("StateSlot1_IsChecked");
                }
            }
        }
        // Enabled
        private bool _StateSlot1_IsEnabled;
        public bool StateSlot1_IsEnabled
        {
            get { return _StateSlot1_IsEnabled; }
            set
            {
                if (_StateSlot1_IsEnabled == value)
                {
                    return;
                }

                _StateSlot1_IsEnabled = value;
                OnPropertyChanged("StateSlot1_IsEnabled");
            }
        }

        // -------------------------
        // StateSlot2
        // -------------------------
        // Checked
        private bool _StateSlot2_IsChecked;
        public bool StateSlot2_IsChecked
        {
            get { return _StateSlot2_IsChecked; }
            set
            {
                if (_StateSlot2_IsChecked != value)
                {
                    _StateSlot2_IsChecked = value;
                    OnPropertyChanged("StateSlot2_IsChecked");
                }
            }
        }
        // Enabled
        private bool _StateSlot2_IsEnabled;
        public bool StateSlot2_IsEnabled
        {
            get { return _StateSlot2_IsEnabled; }
            set
            {
                if (_StateSlot2_IsEnabled == value)
                {
                    return;
                }

                _StateSlot2_IsEnabled = value;
                OnPropertyChanged("StateSlot2_IsEnabled");
            }
        }

        // -------------------------
        // StateSlot3
        // -------------------------
        // Checked
        private bool _StateSlot3_IsChecked;
        public bool StateSlot3_IsChecked
        {
            get { return _StateSlot3_IsChecked; }
            set
            {
                if (_StateSlot3_IsChecked != value)
                {
                    _StateSlot3_IsChecked = value;
                    OnPropertyChanged("StateSlot3_IsChecked");
                }
            }
        }
        // Enabled
        private bool _StateSlot3_IsEnabled;
        public bool StateSlot3_IsEnabled
        {
            get { return _StateSlot3_IsEnabled; }
            set
            {
                if (_StateSlot3_IsEnabled == value)
                {
                    return;
                }

                _StateSlot3_IsEnabled = value;
                OnPropertyChanged("StateSlot3_IsEnabled");
            }
        }

        // -------------------------
        // StateSlot4
        // -------------------------
        // Checked
        private bool _StateSlot4_IsChecked;
        public bool StateSlot4_IsChecked
        {
            get { return _StateSlot4_IsChecked; }
            set
            {
                if (_StateSlot4_IsChecked != value)
                {
                    _StateSlot4_IsChecked = value;
                    OnPropertyChanged("StateSlot4_IsChecked");
                }
            }
        }
        // Enabled
        private bool _StateSlot4_IsEnabled;
        public bool StateSlot4_IsEnabled
        {
            get { return _StateSlot4_IsEnabled; }
            set
            {
                if (_StateSlot4_IsEnabled == value)
                {
                    return;
                }

                _StateSlot4_IsEnabled = value;
                OnPropertyChanged("StateSlot4_IsEnabled");
            }
        }

        // -------------------------
        // StateSlot5
        // -------------------------
        // Checked
        private bool _StateSlot5_IsChecked;
        public bool StateSlot5_IsChecked
        {
            get { return _StateSlot5_IsChecked; }
            set
            {
                if (_StateSlot5_IsChecked != value)
                {
                    _StateSlot5_IsChecked = value;
                    OnPropertyChanged("StateSlot5_IsChecked");
                }
            }
        }
        // Enabled
        private bool _StateSlot5_IsEnabled;
        public bool StateSlot5_IsEnabled
        {
            get { return _StateSlot5_IsEnabled; }
            set
            {
                if (_StateSlot5_IsEnabled == value)
                {
                    return;
                }

                _StateSlot5_IsEnabled = value;
                OnPropertyChanged("StateSlot5_IsEnabled");
            }
        }

        // -------------------------
        // StateSlot6
        // -------------------------
        // Checked
        private bool _StateSlot6_IsChecked;
        public bool StateSlot6_IsChecked
        {
            get { return _StateSlot6_IsChecked; }
            set
            {
                if (_StateSlot6_IsChecked != value)
                {
                    _StateSlot6_IsChecked = value;
                    OnPropertyChanged("StateSlot6_IsChecked");
                }
            }
        }
        // Enabled
        private bool _StateSlot6_IsEnabled;
        public bool StateSlot6_IsEnabled
        {
            get { return _StateSlot6_IsEnabled; }
            set
            {
                if (_StateSlot6_IsEnabled == value)
                {
                    return;
                }

                _StateSlot6_IsEnabled = value;
                OnPropertyChanged("StateSlot6_IsEnabled");
            }
        }

        // -------------------------
        // StateSlot7
        // -------------------------
        // Checked
        private bool _StateSlot7_IsChecked;
        public bool StateSlot7_IsChecked
        {
            get { return _StateSlot7_IsChecked; }
            set
            {
                if (_StateSlot7_IsChecked != value)
                {
                    _StateSlot7_IsChecked = value;
                    OnPropertyChanged("StateSlot7_IsChecked");
                }
            }
        }
        // Enabled
        private bool _StateSlot7_IsEnabled;
        public bool StateSlot7_IsEnabled
        {
            get { return _StateSlot7_IsEnabled; }
            set
            {
                if (_StateSlot7_IsEnabled == value)
                {
                    return;
                }

                _StateSlot7_IsEnabled = value;
                OnPropertyChanged("StateSlot7_IsEnabled");
            }
        }

        // -------------------------
        // StateSlot8
        // -------------------------
        // Checked
        private bool _StateSlot8_IsChecked;
        public bool StateSlot8_IsChecked
        {
            get { return _StateSlot8_IsChecked; }
            set
            {
                if (_StateSlot8_IsChecked != value)
                {
                    _StateSlot8_IsChecked = value;
                    OnPropertyChanged("StateSlot8_IsChecked");
                }
            }
        }
        // Enabled
        private bool _StateSlot8_IsEnabled;
        public bool StateSlot8_IsEnabled
        {
            get { return _StateSlot8_IsEnabled; }
            set
            {
                if (_StateSlot8_IsEnabled == value)
                {
                    return;
                }

                _StateSlot8_IsEnabled = value;
                OnPropertyChanged("StateSlot8_IsEnabled");
            }
        }

        // -------------------------
        // StateSlot9
        // -------------------------
        // Checked
        private bool _StateSlot9_IsChecked;
        public bool StateSlot9_IsChecked
        {
            get { return _StateSlot9_IsChecked; }
            set
            {
                if (_StateSlot9_IsChecked != value)
                {
                    _StateSlot9_IsChecked = value;
                    OnPropertyChanged("StateSlot9_IsChecked");
                }
            }
        }
        // Enabled
        private bool _StateSlot9_IsEnabled;
        public bool StateSlot9_IsEnabled
        {
            get { return _StateSlot9_IsEnabled; }
            set
            {
                if (_StateSlot9_IsEnabled == value)
                {
                    return;
                }

                _StateSlot9_IsEnabled = value;
                OnPropertyChanged("StateSlot9_IsEnabled");
            }
        }

    }

}
