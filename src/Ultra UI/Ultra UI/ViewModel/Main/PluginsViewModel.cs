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
using Ultra;

namespace ViewModel
{
    public class PluginsViewModel : INotifyPropertyChanged
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
        public PluginsViewModel()
        {
            //Video_SelectedIndex = 0;
            Video_IsEnabled = true;
            Audio_IsEnabled = true;
            Input_IsEnabled = true;
            RSP_IsEnabled = true;

            PluginsErrorNotice_Text = string.Empty;
        }

        // --------------------------------------------------
        // Plugins Location Error Notice
        // --------------------------------------------------
        // Text
        private string _PluginsErrorNotice_Text = string.Empty;
        public string PluginsErrorNotice_Text
        {
            get { return _PluginsErrorNotice_Text; }
            set
            {
                if (_PluginsErrorNotice_Text == value)
                {
                    return;
                }

                _PluginsErrorNotice_Text = value;
                OnPropertyChanged("PluginsErrorNotice_Text");
            }
        }


        // --------------------------------------------------
        // Video
        // --------------------------------------------------
        // Items Source
        public class Video
        {
            public string FullPath { get; set; }
            public string Directory { get; set; }
            public string Name { get; set; }
        }

        public ObservableCollection<Video> _Video_Items = new ObservableCollection<Video>();
        public ObservableCollection<Video> Video_Items
        {
            get { return _Video_Items; }
            set
            {
                _Video_Items = value;
                OnPropertyChanged("Video_Items");
            }
        }

        //private ObservableCollection<string> _Video_Items = new ObservableCollection<string>();
        //public ObservableCollection<string> Video_Items
        //{
        //    get { return _Video_Items; }
        //    set
        //    {
        //        _Video_Items = value;
        //        OnPropertyChanged("Video_Items");
        //    }
        //}

        // Selected Index
        private int _Video_SelectedIndex;
        public int Video_SelectedIndex
        {
            get { return _Video_SelectedIndex; }
            set
            {
                if (_Video_SelectedIndex == value)
                {
                    return;
                }

                _Video_SelectedIndex = value;
                OnPropertyChanged("Video_SelectedIndex");

                //MainWindow.Plugin_Video_SelectionChanged();
            }
        }

        // Selected Item
        private string _Video_SelectedItem;
        public string Video_SelectedItem 
        {
            get { return _Video_SelectedItem; }
            set
            {
                if (_Video_SelectedItem == value)
                {
                    return;
                }

                _Video_SelectedItem = value;
                OnPropertyChanged("Video_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Video_IsEnabled;
        public bool Video_IsEnabled
        {
            get { return _Video_IsEnabled; }
            set
            {
                if (_Video_IsEnabled == value)
                {
                    return;
                }

                _Video_IsEnabled = value;
                OnPropertyChanged("Video_IsEnabled");
            }
        }


        // --------------------------------------------------
        // Audio
        // --------------------------------------------------
        // Items Source
        public class Audio
        {
            public string FullPath { get; set; }
            public string Directory { get; set; }
            public string Name { get; set; }
        }

        public ObservableCollection<Audio> _Audio_Items = new ObservableCollection<Audio>();
        public ObservableCollection<Audio> Audio_Items
        {
            get { return _Audio_Items; }
            set
            {
                _Audio_Items = value;
                OnPropertyChanged("Audio_Items");
            }
        }

        //private ObservableCollection<string> _Audio_Items = new ObservableCollection<string>();
        //public ObservableCollection<string> Audio_Items
        //{
        //    get { return _Audio_Items; }
        //    set
        //    {
        //        _Audio_Items = value;
        //        OnPropertyChanged("Audio_Items");
        //    }
        //}

        // Selected Index
        private int _Audio_SelectedIndex;
        public int Audio_SelectedIndex
        {
            get { return _Audio_SelectedIndex; }
            set
            {
                if (_Audio_SelectedIndex == value)
                {
                    return;
                }

                _Audio_SelectedIndex = value;
                OnPropertyChanged("Audio_SelectedIndex");
            }
        }

        // Selected Item
        private string _Audio_SelectedItem;
        public string Audio_SelectedItem
        {
            get { return _Audio_SelectedItem; }
            set
            {
                if (_Audio_SelectedItem == value)
                {
                    return;
                }

                _Audio_SelectedItem = value;
                OnPropertyChanged("Audio_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Audio_IsEnabled;
        public bool Audio_IsEnabled
        {
            get { return _Audio_IsEnabled; }
            set
            {
                if (_Audio_IsEnabled == value)
                {
                    return;
                }

                _Audio_IsEnabled = value;
                OnPropertyChanged("Audio_IsEnabled");
            }
        }


        // --------------------------------------------------
        // Input
        // --------------------------------------------------
        // Items Source
        public class Input
        {
            public string FullPath { get; set; }
            public string Directory { get; set; }
            public string Name { get; set; }
        }

        public ObservableCollection<Input> _Input_Items = new ObservableCollection<Input>();
        public ObservableCollection<Input> Input_Items
        {
            get { return _Input_Items; }
            set
            {
                _Input_Items = value;
                OnPropertyChanged("Input_Items");
            }
        }

        //private ObservableCollection<string> _Input_Items = new ObservableCollection<string>();
        //public ObservableCollection<string> Input_Items
        //{
        //    get { return _Input_Items; }
        //    set
        //    {
        //        _Input_Items = value;
        //        OnPropertyChanged("Input_Items");
        //    }
        //}

        // Selected Index
        private int _Input_SelectedIndex;
        public int Input_SelectedIndex
        {
            get { return _Input_SelectedIndex; }
            set
            {
                if (_Input_SelectedIndex == value)
                {
                    return;
                }

                _Input_SelectedIndex = value;
                OnPropertyChanged("Input_SelectedIndex");
            }
        }

        // Selected Item
        private string _Input_SelectedItem;
        public string Input_SelectedItem
        {
            get { return _Input_SelectedItem; }
            set
            {
                if (_Input_SelectedItem == value)
                {
                    return;
                }

                _Input_SelectedItem = value;
                OnPropertyChanged("Input_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Input_IsEnabled;
        public bool Input_IsEnabled
        {
            get { return _Input_IsEnabled; }
            set
            {
                if (_Input_IsEnabled == value)
                {
                    return;
                }

                _Input_IsEnabled = value;
                OnPropertyChanged("Input_IsEnabled");
            }
        }


        // --------------------------------------------------
        // RSP
        // --------------------------------------------------
        // Items Source
        public class RSP
        {
            public string FullPath { get; set; }
            public string Directory { get; set; }
            public string Name { get; set; }
        }

        public ObservableCollection<RSP> _RSP_Items = new ObservableCollection<RSP>();
        public ObservableCollection<RSP> RSP_Items
        {
            get { return _RSP_Items; }
            set
            {
                _RSP_Items = value;
                OnPropertyChanged("RSP_Items");
            }
        }

        //private ObservableCollection<string> _RSP_Items = new ObservableCollection<string>();
        //public ObservableCollection<string> RSP_Items
        //{
        //    get { return _RSP_Items; }
        //    set
        //    {
        //        _RSP_Items = value;
        //        OnPropertyChanged("RSP_Items");
        //    }
        //}

        // Selected Index
        private int _RSP_SelectedIndex;
        public int RSP_SelectedIndex
        {
            get { return _RSP_SelectedIndex; }
            set
            {
                if (_RSP_SelectedIndex == value)
                {
                    return;
                }

                _RSP_SelectedIndex = value;
                OnPropertyChanged("RSP_SelectedIndex");
            }
        }

        // Selected Item
        private string _RSP_SelectedItem;
        public string RSP_SelectedItem
        {
            get { return _RSP_SelectedItem; }
            set
            {
                if (_RSP_SelectedItem == value)
                {
                    return;
                }

                _RSP_SelectedItem = value;
                OnPropertyChanged("RSP_SelectedItem");
            }
        }

        // Controls Enable
        private bool _RSP_IsEnabled;
        public bool RSP_IsEnabled
        {
            get { return _RSP_IsEnabled; }
            set
            {
                if (_RSP_IsEnabled == value)
                {
                    return;
                }

                _RSP_IsEnabled = value;
                OnPropertyChanged("RSP_IsEnabled");
            }
        }

    }

}
