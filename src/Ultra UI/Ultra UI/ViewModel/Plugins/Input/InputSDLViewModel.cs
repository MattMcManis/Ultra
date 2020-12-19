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
    public class Plugins_Input_InputSDL_ViewModel : INotifyPropertyChanged
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
        /// Plugins Input - InputSDL ViewModel
        /// </summary>
        public Plugins_Input_InputSDL_ViewModel()
        {
            Controller_SelectedItem = "1";
        }

        // --------------------------------------------------
        // DPad R
        // --------------------------------------------------
        // Text
        private string _DPad_R_Text = string.Empty;
        public string DPad_R_Text
        {
            get { return _DPad_R_Text; }
            set
            {
                if (_DPad_R_Text == value)
                {
                    return;
                }

                _DPad_R_Text = value;
                OnPropertyChanged("DPad_R_Text");
            }
        }
        // Controls Checked
        private bool _DPad_R_IsChecked;
        public bool DPad_R_IsChecked
        {
            get { return _DPad_R_IsChecked; }
            set
            {
                if (_DPad_R_IsChecked == value)
                {
                    return;
                }

                _DPad_R_IsChecked = value;
                OnPropertyChanged("DPad_R_IsChecked");
            }
        }
        // Controls Enable
        private bool _DPad_R_IsEnabled = true;
        public bool DPad_R_IsEnabled
        {
            get { return _DPad_R_IsEnabled; }
            set
            {
                if (_DPad_R_IsEnabled == value)
                {
                    return;
                }

                _DPad_R_IsEnabled = value;
                OnPropertyChanged("DPad_R_IsEnabled");
            }
        }

        // --------------------------------------------------
        // DPad L
        // --------------------------------------------------
        // Text
        private string _DPad_L_Text = string.Empty;
        public string DPad_L_Text
        {
            get { return _DPad_L_Text; }
            set
            {
                if (_DPad_L_Text == value)
                {
                    return;
                }

                _DPad_L_Text = value;
                OnPropertyChanged("DPad_L_Text");
            }
        }
        // Controls Checked
        private bool _DPad_L_IsChecked;
        public bool DPad_L_IsChecked
        {
            get { return _DPad_L_IsChecked; }
            set
            {
                if (_DPad_L_IsChecked == value)
                {
                    return;
                }

                _DPad_L_IsChecked = value;
                OnPropertyChanged("DPad_L_IsChecked");
            }
        }
        // Controls Enable
        private bool _DPad_L_IsEnabled = true;
        public bool DPad_L_IsEnabled
        {
            get { return _DPad_L_IsEnabled; }
            set
            {
                if (_DPad_L_IsEnabled == value)
                {
                    return;
                }

                _DPad_L_IsEnabled = value;
                OnPropertyChanged("DPad_L_IsEnabled");
            }
        }

        // --------------------------------------------------
        // DPad D
        // --------------------------------------------------
        // Text
        private string _DPad_D_Text = string.Empty;
        public string DPad_D_Text
        {
            get { return _DPad_D_Text; }
            set
            {
                if (_DPad_D_Text == value)
                {
                    return;
                }

                _DPad_D_Text = value;
                OnPropertyChanged("DPad_D_Text");
            }
        }
        // Controls Checked
        private bool _DPad_D_IsChecked;
        public bool DPad_D_IsChecked
        {
            get { return _DPad_D_IsChecked; }
            set
            {
                if (_DPad_D_IsChecked == value)
                {
                    return;
                }

                _DPad_D_IsChecked = value;
                OnPropertyChanged("DPad_D_IsChecked");
            }
        }
        // Controls Enable
        private bool _DPad_D_IsEnabled = true;
        public bool DPad_D_IsEnabled
        {
            get { return _DPad_D_IsEnabled; }
            set
            {
                if (_DPad_D_IsEnabled == value)
                {
                    return;
                }

                _DPad_D_IsEnabled = value;
                OnPropertyChanged("DPad_D_IsEnabled");
            }
        }

        // --------------------------------------------------
        // DPad U
        // --------------------------------------------------
        // Text
        private string _DPad_U_Text = string.Empty;
        public string DPad_U_Text
        {
            get { return _DPad_U_Text; }
            set
            {
                if (_DPad_U_Text == value)
                {
                    return;
                }

                _DPad_U_Text = value;
                OnPropertyChanged("DPad_U_Text");
            }
        }
        // Controls Checked
        private bool _DPad_U_IsChecked;
        public bool DPad_U_IsChecked
        {
            get { return _DPad_U_IsChecked; }
            set
            {
                if (_DPad_U_IsChecked == value)
                {
                    return;
                }

                _DPad_U_IsChecked = value;
                OnPropertyChanged("DPad_U_IsChecked");
            }
        }
        // Controls Enable
        private bool _DPad_U_IsEnabled = true;
        public bool DPad_U_IsEnabled
        {
            get { return _DPad_U_IsEnabled; }
            set
            {
                if (_DPad_U_IsEnabled == value)
                {
                    return;
                }

                _DPad_U_IsEnabled = value;
                OnPropertyChanged("DPad_U_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Start
        // --------------------------------------------------
        // Text
        private string _Start_Text = string.Empty;
        public string Start_Text
        {
            get { return _Start_Text; }
            set
            {
                if (_Start_Text == value)
                {
                    return;
                }

                _Start_Text = value;
                OnPropertyChanged("Start_Text");
            }
        }
        // Controls Checked
        private bool _Start_IsChecked;
        public bool Start_IsChecked
        {
            get { return _Start_IsChecked; }
            set
            {
                if (_Start_IsChecked == value)
                {
                    return;
                }

                _Start_IsChecked = value;
                OnPropertyChanged("Start_IsChecked");
            }
        }
        // Controls Enable
        private bool _Start_IsEnabled = true;
        public bool Start_IsEnabled
        {
            get { return _Start_IsEnabled; }
            set
            {
                if (_Start_IsEnabled == value)
                {
                    return;
                }

                _Start_IsEnabled = value;
                OnPropertyChanged("Start_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Z Trig
        // --------------------------------------------------
        // Text
        private string _Z_Trig_Text = string.Empty;
        public string Z_Trig_Text
        {
            get { return _Z_Trig_Text; }
            set
            {
                if (_Z_Trig_Text == value)
                {
                    return;
                }

                _Z_Trig_Text = value;
                OnPropertyChanged("Z_Trig_Text");
            }
        }
        // Controls Checked
        private bool _Z_Trig_IsChecked;
        public bool Z_Trig_IsChecked
        {
            get { return _Z_Trig_IsChecked; }
            set
            {
                if (_Z_Trig_IsChecked == value)
                {
                    return;
                }

                _Z_Trig_IsChecked = value;
                OnPropertyChanged("Z_Trig_IsChecked");
            }
        }
        // Controls Enable
        private bool _Z_Trig_IsEnabled = true;
        public bool Z_Trig_IsEnabled
        {
            get { return _Z_Trig_IsEnabled; }
            set
            {
                if (_Z_Trig_IsEnabled == value)
                {
                    return;
                }

                _Z_Trig_IsEnabled = value;
                OnPropertyChanged("Z_Trig_IsEnabled");
            }
        }

        // --------------------------------------------------
        // B Button
        // --------------------------------------------------
        // Text
        private string _B_Button_Text = string.Empty;
        public string B_Button_Text
        {
            get { return _B_Button_Text; }
            set
            {
                if (_B_Button_Text == value)
                {
                    return;
                }

                _B_Button_Text = value;
                OnPropertyChanged("B_Button_Text");
            }
        }
        // Controls Checked
        private bool _B_Button_IsChecked;
        public bool B_Button_IsChecked
        {
            get { return _B_Button_IsChecked; }
            set
            {
                if (_B_Button_IsChecked == value)
                {
                    return;
                }

                _B_Button_IsChecked = value;
                OnPropertyChanged("B_Button_IsChecked");
            }
        }
        // Controls Enable
        private bool _B_Button_IsEnabled = true;
        public bool B_Button_IsEnabled
        {
            get { return _B_Button_IsEnabled; }
            set
            {
                if (_B_Button_IsEnabled == value)
                {
                    return;
                }

                _B_Button_IsEnabled = value;
                OnPropertyChanged("B_Button_IsEnabled");
            }
        }

        // --------------------------------------------------
        // A Button
        // --------------------------------------------------
        // Text
        private string _A_Button_Text = string.Empty;
        public string A_Button_Text
        {
            get { return _A_Button_Text; }
            set
            {
                if (_A_Button_Text == value)
                {
                    return;
                }

                _A_Button_Text = value;
                OnPropertyChanged("A_Button_Text");
            }
        }
        // Controls Checked
        private bool _A_Button_IsChecked;
        public bool A_Button_IsChecked
        {
            get { return _A_Button_IsChecked; }
            set
            {
                if (_A_Button_IsChecked == value)
                {
                    return;
                }

                _A_Button_IsChecked = value;
                OnPropertyChanged("A_Button_IsChecked");
            }
        }
        // Controls Enable
        private bool _A_Button_IsEnabled = true;
        public bool A_Button_IsEnabled
        {
            get { return _A_Button_IsEnabled; }
            set
            {
                if (_A_Button_IsEnabled == value)
                {
                    return;
                }

                _A_Button_IsEnabled = value;
                OnPropertyChanged("A_Button_IsEnabled");
            }
        }

        // --------------------------------------------------
        // C Button R
        // --------------------------------------------------
        // Text
        private string _C_Button_R_Text = string.Empty;
        public string C_Button_R_Text
        {
            get { return _C_Button_R_Text; }
            set
            {
                if (_C_Button_R_Text == value)
                {
                    return;
                }

                _C_Button_R_Text = value;
                OnPropertyChanged("C_Button_R_Text");
            }
        }
        // Controls Checked
        private bool _C_Button_R_IsChecked;
        public bool C_Button_R_IsChecked
        {
            get { return _C_Button_R_IsChecked; }
            set
            {
                if (_C_Button_R_IsChecked == value)
                {
                    return;
                }

                _C_Button_R_IsChecked = value;
                OnPropertyChanged("C_Button_R_IsChecked");
            }
        }
        // Controls Enable
        private bool _C_Button_R_IsEnabled = true;
        public bool C_Button_R_IsEnabled
        {
            get { return _C_Button_R_IsEnabled; }
            set
            {
                if (_C_Button_R_IsEnabled == value)
                {
                    return;
                }

                _C_Button_R_IsEnabled = value;
                OnPropertyChanged("C_Button_R_IsEnabled");
            }
        }

        // --------------------------------------------------
        // C Button L
        // --------------------------------------------------
        // Text
        private string _C_Button_L_Text = string.Empty;
        public string C_Button_L_Text
        {
            get { return _C_Button_L_Text; }
            set
            {
                if (_C_Button_L_Text == value)
                {
                    return;
                }

                _C_Button_L_Text = value;
                OnPropertyChanged("C_Button_L_Text");
            }
        }
        // Controls Checked
        private bool _C_Button_L_IsChecked;
        public bool C_Button_L_IsChecked
        {
            get { return _C_Button_L_IsChecked; }
            set
            {
                if (_C_Button_L_IsChecked == value)
                {
                    return;
                }

                _C_Button_L_IsChecked = value;
                OnPropertyChanged("C_Button_L_IsChecked");
            }
        }
        // Controls Enable
        private bool _C_Button_L_IsEnabled = true;
        public bool C_Button_L_IsEnabled
        {
            get { return _C_Button_L_IsEnabled; }
            set
            {
                if (_C_Button_L_IsEnabled == value)
                {
                    return;
                }

                _C_Button_L_IsEnabled = value;
                OnPropertyChanged("C_Button_L_IsEnabled");
            }
        }

        // --------------------------------------------------
        // C Button D
        // --------------------------------------------------
        // Text
        private string _C_Button_D_Text = string.Empty;
        public string C_Button_D_Text
        {
            get { return _C_Button_D_Text; }
            set
            {
                if (_C_Button_D_Text == value)
                {
                    return;
                }

                _C_Button_D_Text = value;
                OnPropertyChanged("C_Button_D_Text");
            }
        }
        // Controls Checked
        private bool _C_Button_D_IsChecked;
        public bool C_Button_D_IsChecked
        {
            get { return _C_Button_D_IsChecked; }
            set
            {
                if (_C_Button_D_IsChecked == value)
                {
                    return;
                }

                _C_Button_D_IsChecked = value;
                OnPropertyChanged("C_Button_D_IsChecked");
            }
        }
        // Controls Enable
        private bool _C_Button_D_IsEnabled = true;
        public bool C_Button_D_IsEnabled
        {
            get { return _C_Button_D_IsEnabled; }
            set
            {
                if (_C_Button_D_IsEnabled == value)
                {
                    return;
                }

                _C_Button_D_IsEnabled = value;
                OnPropertyChanged("C_Button_D_IsEnabled");
            }
        }

        // --------------------------------------------------
        // C Button U
        // --------------------------------------------------
        // Text
        private string _C_Button_U_Text = string.Empty;
        public string C_Button_U_Text
        {
            get { return _C_Button_U_Text; }
            set
            {
                if (_C_Button_U_Text == value)
                {
                    return;
                }

                _C_Button_U_Text = value;
                OnPropertyChanged("C_Button_U_Text");
            }
        }
        // Controls Checked
        private bool _C_Button_U_IsChecked;
        public bool C_Button_U_IsChecked
        {
            get { return _C_Button_U_IsChecked; }
            set
            {
                if (_C_Button_U_IsChecked == value)
                {
                    return;
                }

                _C_Button_U_IsChecked = value;
                OnPropertyChanged("C_Button_U_IsChecked");
            }
        }
        // Controls Enable
        private bool _C_Button_U_IsEnabled = true;
        public bool C_Button_U_IsEnabled
        {
            get { return _C_Button_U_IsEnabled; }
            set
            {
                if (_C_Button_U_IsEnabled == value)
                {
                    return;
                }

                _C_Button_U_IsEnabled = value;
                OnPropertyChanged("C_Button_U_IsEnabled");
            }
        }

        // --------------------------------------------------
        // R Trig
        // --------------------------------------------------
        // Text
        private string _R_Trig_Text = string.Empty;
        public string R_Trig_Text
        {
            get { return _R_Trig_Text; }
            set
            {
                if (_R_Trig_Text == value)
                {
                    return;
                }

                _R_Trig_Text = value;
                OnPropertyChanged("R_Trig_Text");
            }
        }
        // Controls Checked
        private bool _R_Trig_IsChecked;
        public bool R_Trig_IsChecked
        {
            get { return _R_Trig_IsChecked; }
            set
            {
                if (_R_Trig_IsChecked == value)
                {
                    return;
                }

                _R_Trig_IsChecked = value;
                OnPropertyChanged("R_Trig_IsChecked");
            }
        }
        // Controls Enable
        private bool _R_Trig_IsEnabled = true;
        public bool R_Trig_IsEnabled
        {
            get { return _R_Trig_IsEnabled; }
            set
            {
                if (_R_Trig_IsEnabled == value)
                {
                    return;
                }

                _R_Trig_IsEnabled = value;
                OnPropertyChanged("R_Trig_IsEnabled");
            }
        }

        // --------------------------------------------------
        // L Trig
        // --------------------------------------------------
        // Text
        private string _L_Trig_Text = string.Empty;
        public string L_Trig_Text
        {
            get { return _L_Trig_Text; }
            set
            {
                if (_L_Trig_Text == value)
                {
                    return;
                }

                _L_Trig_Text = value;
                OnPropertyChanged("L_Trig_Text");
            }
        }
        // Controls Checked
        private bool _L_Trig_IsChecked;
        public bool L_Trig_IsChecked
        {
            get { return _L_Trig_IsChecked; }
            set
            {
                if (_L_Trig_IsChecked == value)
                {
                    return;
                }

                _L_Trig_IsChecked = value;
                OnPropertyChanged("L_Trig_IsChecked");
            }
        }
        // Controls Enable
        private bool _L_Trig_IsEnabled = true;
        public bool L_Trig_IsEnabled
        {
            get { return _L_Trig_IsEnabled; }
            set
            {
                if (_L_Trig_IsEnabled == value)
                {
                    return;
                }

                _L_Trig_IsEnabled = value;
                OnPropertyChanged("L_Trig_IsEnabled");
            }
        }

        // --------------------------------------------------
        // X Axis L
        // --------------------------------------------------
        // Text
        private string _X_Axis_L_Text = string.Empty;
        public string X_Axis_L_Text
        {
            get { return _X_Axis_L_Text; }
            set
            {
                if (_X_Axis_L_Text == value)
                {
                    return;
                }

                _X_Axis_L_Text = value;
                OnPropertyChanged("X_Axis_L_Text");
            }
        }
        // Controls Checked
        private bool _X_Axis_L_IsChecked;
        public bool X_Axis_L_IsChecked
        {
            get { return _X_Axis_L_IsChecked; }
            set
            {
                if (_X_Axis_L_IsChecked == value)
                {
                    return;
                }

                _X_Axis_L_IsChecked = value;
                OnPropertyChanged("X_Axis_L_IsChecked");
            }
        }
        // Controls Enable
        private bool _X_Axis_L_IsEnabled = true;
        public bool X_Axis_L_IsEnabled
        {
            get { return _X_Axis_L_IsEnabled; }
            set
            {
                if (_X_Axis_L_IsEnabled == value)
                {
                    return;
                }

                _X_Axis_L_IsEnabled = value;
                OnPropertyChanged("X_Axis_L_IsEnabled");
            }
        }

        // --------------------------------------------------
        // X Axis R
        // --------------------------------------------------
        // Text
        private string _X_Axis_R_Text = string.Empty;
        public string X_Axis_R_Text
        {
            get { return _X_Axis_R_Text; }
            set
            {
                if (_X_Axis_R_Text == value)
                {
                    return;
                }

                _X_Axis_R_Text = value;
                OnPropertyChanged("X_Axis_R_Text");
            }
        }
        // Controls Checked
        private bool _X_Axis_R_IsChecked;
        public bool X_Axis_R_IsChecked
        {
            get { return _X_Axis_R_IsChecked; }
            set
            {
                if (_X_Axis_R_IsChecked == value)
                {
                    return;
                }

                _X_Axis_R_IsChecked = value;
                OnPropertyChanged("X_Axis_R_IsChecked");
            }
        }
        // Controls Enable
        private bool _X_Axis_R_IsEnabled = true;
        public bool X_Axis_R_IsEnabled
        {
            get { return _X_Axis_R_IsEnabled; }
            set
            {
                if (_X_Axis_R_IsEnabled == value)
                {
                    return;
                }

                _X_Axis_R_IsEnabled = value;
                OnPropertyChanged("X_Axis_R_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Y Axis D
        // --------------------------------------------------
        // Text
        private string _Y_Axis_D_Text = string.Empty;
        public string Y_Axis_D_Text
        {
            get { return _Y_Axis_D_Text; }
            set
            {
                if (_Y_Axis_D_Text == value)
                {
                    return;
                }

                _Y_Axis_D_Text = value;
                OnPropertyChanged("Y_Axis_D_Text");
            }
        }
        // Controls Checked
        private bool _Y_Axis_D_IsChecked;
        public bool Y_Axis_D_IsChecked
        {
            get { return _Y_Axis_D_IsChecked; }
            set
            {
                if (_Y_Axis_D_IsChecked == value)
                {
                    return;
                }

                _Y_Axis_D_IsChecked = value;
                OnPropertyChanged("Y_Axis_D_IsChecked");
            }
        }
        // Controls Enable
        private bool _Y_Axis_D_IsEnabled = true;
        public bool Y_Axis_D_IsEnabled
        {
            get { return _Y_Axis_D_IsEnabled; }
            set
            {
                if (_Y_Axis_D_IsEnabled == value)
                {
                    return;
                }

                _Y_Axis_D_IsEnabled = value;
                OnPropertyChanged("Y_Axis_D_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Y Axis U
        // --------------------------------------------------
        // Text
        private string _Y_Axis_U_Text = string.Empty;
        public string Y_Axis_U_Text
        {
            get { return _Y_Axis_U_Text; }
            set
            {
                if (_Y_Axis_U_Text == value)
                {
                    return;
                }

                _Y_Axis_U_Text = value;
                OnPropertyChanged("Y_Axis_U_Text");
            }
        }
        // Controls Checked
        private bool _Y_Axis_U_IsChecked;
        public bool Y_Axis_U_IsChecked
        {
            get { return _Y_Axis_U_IsChecked; }
            set
            {
                if (_Y_Axis_U_IsChecked == value)
                {
                    return;
                }

                _Y_Axis_U_IsChecked = value;
                OnPropertyChanged("Y_Axis_U_IsChecked");
            }
        }
        // Controls Enable
        private bool _Y_Axis_U_IsEnabled = true;
        public bool Y_Axis_U_IsEnabled
        {
            get { return _Y_Axis_U_IsEnabled; }
            set
            {
                if (_Y_Axis_U_IsEnabled == value)
                {
                    return;
                }

                _Y_Axis_U_IsEnabled = value;
                OnPropertyChanged("Y_Axis_U_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Mempak
        // --------------------------------------------------
        // Text
        private string _Mempak_Text = string.Empty;
        public string Mempak_Text
        {
            get { return _Mempak_Text; }
            set
            {
                if (_Mempak_Text == value)
                {
                    return;
                }

                _Mempak_Text = value;
                OnPropertyChanged("Mempak_Text");
            }
        }
        // Controls Checked
        private bool _Mempak_IsChecked;
        public bool Mempak_IsChecked
        {
            get { return _Mempak_IsChecked; }
            set
            {
                if (_Mempak_IsChecked == value)
                {
                    return;
                }

                _Mempak_IsChecked = value;
                OnPropertyChanged("Mempak_IsChecked");
            }
        }
        // Controls Enable
        private bool _Mempak_IsEnabled = true;
        public bool Mempak_IsEnabled
        {
            get { return _Mempak_IsEnabled; }
            set
            {
                if (_Mempak_IsEnabled == value)
                {
                    return;
                }

                _Mempak_IsEnabled = value;
                OnPropertyChanged("Mempak_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Rumblepak
        // --------------------------------------------------
        // Text
        private string _Rumblepak_Text = string.Empty;
        public string Rumblepak_Text
        {
            get { return _Rumblepak_Text; }
            set
            {
                if (_Rumblepak_Text == value)
                {
                    return;
                }

                _Rumblepak_Text = value;
                OnPropertyChanged("Rumblepak_Text");
            }
        }
        // Controls Checked
        private bool _Rumblepak_IsChecked;
        public bool Rumblepak_IsChecked
        {
            get { return _Rumblepak_IsChecked; }
            set
            {
                if (_Rumblepak_IsChecked == value)
                {
                    return;
                }

                _Rumblepak_IsChecked = value;
                OnPropertyChanged("Rumblepak_IsChecked");
            }
        }
        // Controls Enable
        private bool _Rumblepak_IsEnabled = true;
        public bool Rumblepak_IsEnabled
        {
            get { return _Rumblepak_IsEnabled; }
            set
            {
                if (_Rumblepak_IsEnabled == value)
                {
                    return;
                }

                _Rumblepak_IsEnabled = value;
                OnPropertyChanged("Rumblepak_IsEnabled");
            }
        }

       
        // --------------------------------------------------
        // Input Controller
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _Controller_Items = new ObservableCollection<string>
        {
            "1",
            "2",
            "3",
            "4"
        };
        public ObservableCollection<string> Controller_Items
        {
            get { return _Controller_Items; }
            set
            {
                _Controller_Items = value;
                OnPropertyChanged("Controller_Items");
            }
        }

        // Selected Index
        private int _Controller_SelectedIndex { get; set; }
        public int Controller_SelectedIndex
        {
            get { return _Controller_SelectedIndex; }
            set
            {
                if (_Controller_SelectedIndex == value)
                {
                    return;
                }

                _Controller_SelectedIndex = value;
                OnPropertyChanged("Controller_SelectedIndex");
            }
        }

        // Selected Item
        private string _Controller_SelectedItem { get; set; }
        public string Controller_SelectedItem
        {
            get { return _Controller_SelectedItem; }
            set
            {
                if (_Controller_SelectedItem == value)
                {
                    return;
                }

                _Controller_SelectedItem = value;
                OnPropertyChanged("Controller_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Controller_IsEnabled = true;
        public bool Controller_IsEnabled
        {
            get { return _Controller_IsEnabled; }
            set
            {
                if (_Controller_IsEnabled == value)
                {
                    return;
                }

                _Controller_IsEnabled = value;
                OnPropertyChanged("Controller_IsEnabled");
            }
        }


        // -------------------------
        // Plugged
        // -------------------------
        // Checked
        private bool _Plugged_IsChecked;
        public bool Plugged_IsChecked
        {
            get { return _Plugged_IsChecked; }
            set
            {
                if (_Plugged_IsChecked != value)
                {
                    _Plugged_IsChecked = value;
                    OnPropertyChanged("Plugged_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Plugged_IsEnabled = true;
        public bool Plugged_IsEnabled
        {
            get { return _Plugged_IsEnabled; }
            set
            {
                if (_Plugged_IsEnabled == value)
                {
                    return;
                }

                _Plugged_IsEnabled = value;
                OnPropertyChanged("Plugged_IsEnabled");
            }
        }

        // -------------------------
        // Mouse
        // -------------------------
        // Checked
        private bool _Mouse_IsChecked;
        public bool Mouse_IsChecked
        {
            get { return _Mouse_IsChecked; }
            set
            {
                if (_Mouse_IsChecked != value)
                {
                    _Mouse_IsChecked = value;
                    OnPropertyChanged("Mouse_IsChecked");
                }
            }
        }
        // Enabled
        private bool _Mouse_IsEnabled = true;
        public bool Mouse_IsEnabled
        {
            get { return _Mouse_IsEnabled; }
            set
            {
                if (_Mouse_IsEnabled == value)
                {
                    return;
                }

                _Mouse_IsEnabled = value;
                OnPropertyChanged("Mouse_IsEnabled");
            }
        }


        // --------------------------------------------------
        // Input Plugin
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _Plugin_Items = new ObservableCollection<string>
        {
            "None",
            "Mem Pak",
            "Rumble Pak",
        };
        public ObservableCollection<string> Plugin_Items
        {
            get { return _Plugin_Items; }
            set
            {
                _Plugin_Items = value;
                OnPropertyChanged("Plugin_Items");
            }
        }

        // Selected Index
        private int _Plugin_SelectedIndex { get; set; }
        public int Plugin_SelectedIndex
        {
            get { return _Plugin_SelectedIndex; }
            set
            {
                if (_Plugin_SelectedIndex == value)
                {
                    return;
                }

                _Plugin_SelectedIndex = value;
                OnPropertyChanged("Plugin_SelectedIndex");
            }
        }

        // Selected Item
        private string _Plugin_SelectedItem { get; set; }
        public string Plugin_SelectedItem
        {
            get { return _Plugin_SelectedItem; }
            set
            {
                if (_Plugin_SelectedItem == value)
                {
                    return;
                }

                _Plugin_SelectedItem = value;
                OnPropertyChanged("Plugin_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Plugin_IsEnabled = true;
        public bool Plugin_IsEnabled
        {
            get { return _Plugin_IsEnabled; }
            set
            {
                if (_Plugin_IsEnabled == value)
                {
                    return;
                }

                _Plugin_IsEnabled = value;
                OnPropertyChanged("Plugin_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Input Device
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _Device_Items = new ObservableCollection<string>
        {
            "Keyboard/Mouse",
            "Gamepad"
        };
        public ObservableCollection<string> Device_Items
        {
            get { return _Device_Items; }
            set
            {
                _Device_Items = value;
                OnPropertyChanged("Device_Items");
            }
        }

        // Selected Index
        private int _Device_SelectedIndex { get; set; }
        public int Device_SelectedIndex
        {
            get { return _Device_SelectedIndex; }
            set
            {
                if (_Device_SelectedIndex == value)
                {
                    return;
                }

                _Device_SelectedIndex = value;
                OnPropertyChanged("Device_SelectedIndex");
            }
        }

        // Selected Item
        private string _Device_SelectedItem { get; set; }
        public string Device_SelectedItem
        {
            get { return _Device_SelectedItem; }
            set
            {
                if (_Device_SelectedItem == value)
                {
                    return;
                }

                _Device_SelectedItem = value;
                OnPropertyChanged("Device_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Device_IsEnabled = true;
        public bool Device_IsEnabled
        {
            get { return _Device_IsEnabled; }
            set
            {
                if (_Device_IsEnabled == value)
                {
                    return;
                }

                _Device_IsEnabled = value;
                OnPropertyChanged("Device_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Input Mode
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _Mode_Items = new ObservableCollection<string>
        {
            "Manual",
            "Fully Automatic"
        };
        public ObservableCollection<string> Mode_Items
        {
            get { return _Mode_Items; }
            set
            {
                _Mode_Items = value;
                OnPropertyChanged("Mode_Items");
            }
        }

        // Selected Index
        private int _Mode_SelectedIndex { get; set; }
        public int Mode_SelectedIndex
        {
            get { return _Mode_SelectedIndex; }
            set
            {
                if (_Mode_SelectedIndex == value)
                {
                    return;
                }

                _Mode_SelectedIndex = value;
                OnPropertyChanged("Mode_SelectedIndex");
            }
        }

        // Selected Item
        private string _Mode_SelectedItem { get; set; }
        public string Mode_SelectedItem
        {
            get { return _Mode_SelectedItem; }
            set
            {
                if (_Mode_SelectedItem == value)
                {
                    return;
                }

                _Mode_SelectedItem = value;
                OnPropertyChanged("Mode_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Mode_IsEnabled = true;
        public bool Mode_IsEnabled
        {
            get { return _Mode_IsEnabled; }
            set
            {
                if (_Mode_IsEnabled == value)
                {
                    return;
                }

                _Mode_IsEnabled = value;
                OnPropertyChanged("Mode_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Input Analog Deadzone X
        // --------------------------------------------------
        // Text
        private string _AnalogDeadzoneX_Text = string.Empty;
        public string AnalogDeadzoneX_Text
        {
            get { return _AnalogDeadzoneX_Text; }
            set
            {
                if (_AnalogDeadzoneX_Text == value)
                {
                    return;
                }

                _AnalogDeadzoneX_Text = value;
                OnPropertyChanged("AnalogDeadzoneX_Text");
            }
        }
        // Controls Enable
        private bool _AnalogDeadzoneX_IsEnabled = true;
        public bool AnalogDeadzoneX_IsEnabled
        {
            get { return _AnalogDeadzoneX_IsEnabled; }
            set
            {
                if (_AnalogDeadzoneX_IsEnabled == value)
                {
                    return;
                }

                _AnalogDeadzoneX_IsEnabled = value;
                OnPropertyChanged("AnalogDeadzoneX_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Analog Deadzone Y
        // --------------------------------------------------
        // Text
        private string _AnalogDeadzoneY_Text = string.Empty;
        public string AnalogDeadzoneY_Text
        {
            get { return _AnalogDeadzoneY_Text; }
            set
            {
                if (_AnalogDeadzoneY_Text == value)
                {
                    return;
                }

                _AnalogDeadzoneY_Text = value;
                OnPropertyChanged("AnalogDeadzoneY_Text");
            }
        }
        // Controls Enable
        private bool _AnalogDeadzoneY_IsEnabled = true;
        public bool AnalogDeadzoneY_IsEnabled
        {
            get { return _AnalogDeadzoneY_IsEnabled; }
            set
            {
                if (_AnalogDeadzoneY_IsEnabled == value)
                {
                    return;
                }

                _AnalogDeadzoneY_IsEnabled = value;
                OnPropertyChanged("AnalogDeadzoneY_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Analog Peak X
        // --------------------------------------------------
        // Text
        private string _AnalogPeakX_Text = string.Empty;
        public string AnalogPeakX_Text
        {
            get { return _AnalogPeakX_Text; }
            set
            {
                if (_AnalogPeakX_Text == value)
                {
                    return;
                }

                _AnalogPeakX_Text = value;
                OnPropertyChanged("AnalogPeakX_Text");
            }
        }
        // Controls Enable
        private bool _AnalogPeakX_IsEnabled = true;
        public bool AnalogPeakX_IsEnabled
        {
            get { return _AnalogPeakX_IsEnabled; }
            set
            {
                if (_AnalogPeakX_IsEnabled == value)
                {
                    return;
                }

                _AnalogPeakX_IsEnabled = value;
                OnPropertyChanged("AnalogPeakX_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Analog Peak Y
        // --------------------------------------------------
        // Text
        private string _AnalogPeakY_Text = string.Empty;
        public string AnalogPeakY_Text
        {
            get { return _AnalogPeakY_Text; }
            set
            {
                if (_AnalogPeakY_Text == value)
                {
                    return;
                }

                _AnalogPeakY_Text = value;
                OnPropertyChanged("AnalogPeakY_Text");
            }
        }
        // Controls Enable
        private bool _AnalogPeakY_IsEnabled = true;
        public bool AnalogPeakY_IsEnabled
        {
            get { return _AnalogPeakY_IsEnabled; }
            set
            {
                if (_AnalogPeakY_IsEnabled == value)
                {
                    return;
                }

                _AnalogPeakY_IsEnabled = value;
                OnPropertyChanged("AnalogPeakY_IsEnabled");
            }
        }


        // --------------------------------------------------
        // Save label
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
