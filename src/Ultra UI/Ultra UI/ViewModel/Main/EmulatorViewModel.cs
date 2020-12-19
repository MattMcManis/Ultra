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
    public class EmulatorViewModel : INotifyPropertyChanged
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
        /// Emulator ViewModel
        /// </summary>
        public EmulatorViewModel()
        {

        }

        // -------------------------
        // Cycles
        // -------------------------
        // Items Source
        private ObservableCollection<string> _CPU_Items = new ObservableCollection<string>
        {
            "Pure Interpreter",
            "Cached Interpreter",
            "Dynamic Recompiler",
        };
        public ObservableCollection<string> CPU_Items
        {
            get { return _CPU_Items; }
            set
            {
                _CPU_Items = value;
                OnPropertyChanged("CPU_Items");
            }
        }

        // Selected Index
        private int _CPU_SelectedIndex { get; set; }
        public int CPU_SelectedIndex
        {
            get { return _CPU_SelectedIndex; }
            set
            {
                if (_CPU_SelectedIndex == value)
                {
                    return;
                }

                _CPU_SelectedIndex = value;
                OnPropertyChanged("CPU_SelectedIndex");
            }
        }

        // Selected Item
        private string _CPU_SelectedItem { get; set; }
        public string CPU_SelectedItem
        {
            get { return _CPU_SelectedItem; }
            set
            {
                if (_CPU_SelectedItem == value)
                {
                    return;
                }

                _CPU_SelectedItem = value;
                OnPropertyChanged("CPU_SelectedItem");
            }
        }

        // Controls Enable
        private bool _CPU_IsEnabled = true;
        public bool CPU_IsEnabled
        {
            get { return _CPU_IsEnabled; }
            set
            {
                if (_CPU_IsEnabled == value)
                {
                    return;
                }

                _CPU_IsEnabled = value;
                OnPropertyChanged("CPU_IsEnabled");
            }
        }

        // -------------------------
        // Pure Interpreter
        // -------------------------
        // Checked
        private bool _Emulator_PureInterpreter_IsChecked;
        public bool Emulator_PureInterpreter_IsChecked
        {
            get { return _Emulator_PureInterpreter_IsChecked; }
            set
            {
                if (_Emulator_PureInterpreter_IsChecked != value)
                {
                    _Emulator_PureInterpreter_IsChecked = value;
                    OnPropertyChanged("Emulator_PureInterpreter_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Emulator_PureInterpreter_IsEnabled = false;
        public bool Emulator_PureInterpreter_IsEnabled
        {
            get { return _Emulator_PureInterpreter_IsEnabled; }
            set
            {
                if (_Emulator_PureInterpreter_IsEnabled == value)
                {
                    return;
                }

                _Emulator_PureInterpreter_IsEnabled = value;
                OnPropertyChanged("Emulator_PureInterpreter_IsEnabled");
            }
        }

        // -------------------------
        // Cached Interpreter
        // -------------------------
        // Checked
        private bool _Emulator_CachedInterpreter_IsChecked;
        public bool Emulator_CachedInterpreter_IsChecked
        {
            get { return _Emulator_CachedInterpreter_IsChecked; }
            set
            {
                if (_Emulator_CachedInterpreter_IsChecked != value)
                {
                    _Emulator_CachedInterpreter_IsChecked = value;
                    OnPropertyChanged("Emulator_CachedInterpreter_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Emulator_CachedInterpreter_IsEnabled = false;
        public bool Emulator_CachedInterpreter_IsEnabled
        {
            get { return _Emulator_CachedInterpreter_IsEnabled; }
            set
            {
                if (_Emulator_CachedInterpreter_IsEnabled == value)
                {
                    return;
                }

                _Emulator_CachedInterpreter_IsEnabled = value;
                OnPropertyChanged("Emulator_CachedInterpreter_IsEnabled");
            }
        }

        // -------------------------
        // Dynamic Recompiler
        // -------------------------
        // Checked
        private bool _Emulator_DynamicRecompiler_IsChecked;
        public bool Emulator_DynamicRecompiler_IsChecked
        {
            get { return _Emulator_DynamicRecompiler_IsChecked; }
            set
            {
                if (_Emulator_DynamicRecompiler_IsChecked != value)
                {
                    _Emulator_DynamicRecompiler_IsChecked = value;
                    OnPropertyChanged("Emulator_DynamicRecompiler_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Emulator_DynamicRecompiler_IsEnabled = false;
        public bool Emulator_DynamicRecompiler_IsEnabled
        {
            get { return _Emulator_DynamicRecompiler_IsEnabled; }
            set
            {
                if (_Emulator_DynamicRecompiler_IsEnabled == value)
                {
                    return;
                }

                _Emulator_DynamicRecompiler_IsEnabled = value;
                OnPropertyChanged("Emulator_DynamicRecompiler_IsEnabled");
            }
        }

        // -------------------------
        // DisableSpecRecomp
        // -------------------------
        // Checked
        private bool _Emulator_DisableSpecRecomp_IsChecked;
        public bool Emulator_DisableSpecRecomp_IsChecked
        {
            get { return _Emulator_DisableSpecRecomp_IsChecked; }
            set
            {
                if (_Emulator_DisableSpecRecomp_IsChecked != value)
                {
                    _Emulator_DisableSpecRecomp_IsChecked = value;
                    OnPropertyChanged("Emulator_DisableSpecRecomp_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Emulator_DisableSpecRecomp_IsEnabled = true;
        public bool Emulator_DisableSpecRecomp_IsEnabled
        {
            get { return _Emulator_DisableSpecRecomp_IsEnabled; }
            set
            {
                if (_Emulator_DisableSpecRecomp_IsEnabled == value)
                {
                    return;
                }

                _Emulator_DisableSpecRecomp_IsEnabled = value;
                OnPropertyChanged("Emulator_DisableSpecRecomp_IsEnabled");
            }
        }

        // -------------------------
        // RandomizeInterrupt
        // -------------------------
        // Checked
        private bool _Emulator_RandomizeInterrupt_IsChecked;
        public bool Emulator_RandomizeInterrupt_IsChecked
        {
            get { return _Emulator_RandomizeInterrupt_IsChecked; }
            set
            {
                if (_Emulator_RandomizeInterrupt_IsChecked != value)
                {
                    _Emulator_RandomizeInterrupt_IsChecked = value;
                    OnPropertyChanged("Emulator_RandomizeInterrupt_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Emulator_RandomizeInterrupt_IsEnabled = true;
        public bool Emulator_RandomizeInterrupt_IsEnabled
        {
            get { return _Emulator_RandomizeInterrupt_IsEnabled; }
            set
            {
                if (_Emulator_RandomizeInterrupt_IsEnabled == value)
                {
                    return;
                }

                _Emulator_RandomizeInterrupt_IsEnabled = value;
                OnPropertyChanged("Emulator_RandomizeInterrupt_IsEnabled");
            }
        }

        // -------------------------
        // NoCompiledJump
        // -------------------------
        // Checked
        private bool _Emulator_NoCompiledJump_IsChecked;
        public bool Emulator_NoCompiledJump_IsChecked
        {
            get { return _Emulator_NoCompiledJump_IsChecked; }
            set
            {
                if (_Emulator_NoCompiledJump_IsChecked != value)
                {
                    _Emulator_NoCompiledJump_IsChecked = value;
                    OnPropertyChanged("Emulator_NoCompiledJump_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Emulator_NoCompiledJump_IsEnabled = false;
        public bool Emulator_NoCompiledJump_IsEnabled
        {
            get { return _Emulator_NoCompiledJump_IsEnabled; }
            set
            {
                if (_Emulator_NoCompiledJump_IsEnabled == value)
                {
                    return;
                }

                _Emulator_NoCompiledJump_IsEnabled = value;
                OnPropertyChanged("Emulator_NoCompiledJump_IsEnabled");
            }
        }

        // -------------------------
        // DisableExtraMemory
        // -------------------------
        // Checked
        private bool _Emulator_DisableExtraMemory_IsChecked;
        public bool Emulator_DisableExtraMemory_IsChecked
        {
            get { return _Emulator_DisableExtraMemory_IsChecked; }
            set
            {
                if (_Emulator_DisableExtraMemory_IsChecked != value)
                {
                    _Emulator_DisableExtraMemory_IsChecked = value;
                    OnPropertyChanged("Emulator_DisableExtraMemory_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Emulator_DisableExtraMemory_IsEnabled = false;
        public bool Emulator_DisableExtraMemory_IsEnabled
        {
            get { return _Emulator_DisableExtraMemory_IsEnabled; }
            set
            {
                if (_Emulator_DisableExtraMemory_IsEnabled == value)
                {
                    return;
                }

                _Emulator_DisableExtraMemory_IsEnabled = value;
                OnPropertyChanged("Emulator_DisableExtraMemory_IsEnabled");
            }
        }

        // -------------------------
        // DelaySI
        // -------------------------
        // Checked
        private bool _Emulator_DelaySI_IsChecked;
        public bool Emulator_DelaySI_IsChecked
        {
            get { return _Emulator_DelaySI_IsChecked; }
            set
            {
                if (_Emulator_DelaySI_IsChecked != value)
                {
                    _Emulator_DelaySI_IsChecked = value;
                    OnPropertyChanged("Emulator_DelaySI_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Emulator_DelaySI_IsEnabled = true;
        public bool Emulator_DelaySI_IsEnabled
        {
            get { return _Emulator_DelaySI_IsEnabled; }
            set
            {
                if (_Emulator_DelaySI_IsEnabled == value)
                {
                    return;
                }

                _Emulator_DelaySI_IsEnabled = value;
                OnPropertyChanged("Emulator_DelaySI_IsEnabled");
            }
        }


        // -------------------------
        // Cycles
        // -------------------------
        // Items Source
        private ObservableCollection<string> _Emulator_Cycles_Items = new ObservableCollection<string>
        {
            "0",
            "1",
            "2",
            "3",
            "4"
        };
        public ObservableCollection<string> Emulator_Cycles_Items
        {
            get { return _Emulator_Cycles_Items; }
            set
            {
                _Emulator_Cycles_Items = value;
                OnPropertyChanged("Emulator_Cycles_Items");
            }
        }

        // Selected Index
        private int _Emulator_Cycles_SelectedIndex { get; set; }
        public int Emulator_Cycles_SelectedIndex
        {
            get { return _Emulator_Cycles_SelectedIndex; }
            set
            {
                if (_Emulator_Cycles_SelectedIndex == value)
                {
                    return;
                }

                _Emulator_Cycles_SelectedIndex = value;
                OnPropertyChanged("Emulator_Cycles_SelectedIndex");
            }
        }

        // Selected Item
        private string _Emulator_Cycles_SelectedItem { get; set; }
        public string Emulator_Cycles_SelectedItem
        {
            get { return _Emulator_Cycles_SelectedItem; }
            set
            {
                if (_Emulator_Cycles_SelectedItem == value)
                {
                    return;
                }

                _Emulator_Cycles_SelectedItem = value;
                OnPropertyChanged("Emulator_Cycles_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Emulator_Cycles_IsEnabled = true;
        public bool Emulator_Cycles_IsEnabled
        {
            get { return _Emulator_Cycles_IsEnabled; }
            set
            {
                if (_Emulator_Cycles_IsEnabled == value)
                {
                    return;
                }

                _Emulator_Cycles_IsEnabled = value;
                OnPropertyChanged("Emulator_Cycles_IsEnabled");
            }
        }


        // -------------------------
        // Cheats
        // -------------------------
        public static List<int> cheatIDs = new List<int>();

        //// Items Source
        //private ObservableCollection<string> _Cheats_ListView_Items = new ObservableCollection<string>();
        //public ObservableCollection<string> Cheats_ListView_Items
        //{
        //    get { return _Cheats_ListView_Items; }
        //    set
        //    {
        //        _Cheats_ListView_Items = value;
        //        OnPropertyChanged("Cheats_ListView_Items");
        //    }
        //}
        // Items Source
        private List<string> _Cheats_ListView_Items = new List<string>();
        public List<string> Cheats_ListView_Items
        {
            get { return _Cheats_ListView_Items; }
            set
            {
                _Cheats_ListView_Items = value;
                OnPropertyChanged("Cheats_ListView_Items");
            }
        }
        // Selected Items
        private List<string> _Cheats_ListView_SelectedItems = new List<string>();
        public List<string> Cheats_ListView_SelectedItems
        {
            get { return _Cheats_ListView_SelectedItems; }
            set
            {
                _Cheats_ListView_SelectedItems = value;
                OnPropertyChanged("Cheats_ListView_SelectedItems");
            }
        }
        // Selected Index
        private int _Cheats_ListView_SelectedIndex { get; set; }
        public int Cheats_ListView_SelectedIndex
        {
            get { return _Cheats_ListView_SelectedIndex; }
            set
            {
                if (_Cheats_ListView_SelectedIndex == value)
                {
                    return;
                }

                _Cheats_ListView_SelectedIndex = value;
                OnPropertyChanged("Cheats_ListView_SelectedIndex");
            }
        }
        private double _Cheats_ListView_Opacity { get; set; }
        public double Cheats_ListView_Opacity
        {
            get { return _Cheats_ListView_Opacity; }
            set
            {
                if (_Cheats_ListView_Opacity == value)
                {
                    return;
                }

                _Cheats_ListView_Opacity = value;
                OnPropertyChanged("Cheats_ListView_Opacity");
            }
        }
        // Controls Enable
        public bool _Cheats_ListView_IsEnabled = true;
        public bool Cheats_ListView_IsEnabled
        {
            get { return _Cheats_ListView_IsEnabled; }
            set
            {
                if (_Cheats_ListView_IsEnabled == value)
                {
                    return;
                }

                _Cheats_ListView_IsEnabled = value;
                OnPropertyChanged("Cheats_ListView_IsEnabled");
            }
        }

    }
}
