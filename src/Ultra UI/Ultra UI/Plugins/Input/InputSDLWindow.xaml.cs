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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Ultra
{
    /// <summary>
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class InputSDLWindow : Window
    {
        // Startup
        // Read Cfg
        // 1. ButtonCfgReader() Import Key int values from mupen64plus.cfg
        // 2. PluginCfgReader() Import Control Values
        // Display Key
        // 3. ButtonDisplayer() Display Key Letters on Toggle Buttons

        // Set Key
        // 1. User presses Toggle Button
        // 2. KeyEnumConverter() Converts System.Windows.Input Key Enum to Mupen64Plus Input SDL Plugin Format
        // Display Key
        // 3. ButtonDisplayer() Display Key Letters on Toggle Buttons
        // 4. IntToKeyConverter() Converts Mupen64Plus Input SDL Plugin Format int to Keyboard Letter/Symbol


        /// <summary>
        /// Input SDL Window
        /// </summary>
        public InputSDLWindow()
        {
            InitializeComponent();

            //MaxWidth = 600;
            //MaxHeight = 600;
            MinWidth = 600;
            MinHeight = 600;
        }

        /// <summary>
        /// Window Loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // -------------------------
            // Event Handlers
            // -------------------------
            // Attach SelectionChanged Handlers
            // Prevent Bound ComboBox from firing SelectionChanged Event at application startup

            // Controller
            //cboController.SelectionChanged += cboController_SelectionChanged;

            // Mode
            cboMode.SelectionChanged += cboMode_SelectionChanged;

            // Device
            cboDevice.SelectionChanged += cboDevice_SelectionChanged;

            // -------------------------
            // Check if Device Gamepad or Keyboard and disable Mode ComboBox
            // Check if Mode Fully Automatic is on and disable All Buttons
            // -------------------------
            DeviceMode();
            FullyAutomatic();
        }

        /// <summary>
        /// Window Unloaded
        /// </summary>
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Window Closing
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
            // Reset Save ✓ Label
            VM.Plugins_Input_InputSDL_View.Save_Text = "";
        }

        // ----------------------------------------------------------------------------------------------------
        //
        // Mupen64Plus Input SDL Plugin uses System.Windows.Forms Keys Enum values, not System.Windows.Input
        // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=netframework-4.8
        //
        // ----------------------------------------------------------------------------------------------------

        // D-Pad
        private static string DPad_R;
        private static string DPad_L;
        private static string DPad_D;
        private static string DPad_U;

        // Start
        private static string Start;

        // Letter Buttons
        private static string Z_Trig;
        private static string B_Button;
        private static string A_Button;

        // C-Pad
        private static string C_Button_R;
        private static string C_Button_L;
        private static string C_Button_D;
        private static string C_Button_U;

        // Triggers
        private static string R_Trig;
        private static string L_Trig;

        // Analog
        private static string X_Axis; // combined L & R
        private static string Y_Axis; // combined U & D
        private static string X_Axis_R;
        private static string X_Axis_L;
        private static string Y_Axis_D;
        private static string Y_Axis_U;

        // Accessories
        private static string MemPak;
        private static string RumblePak;


        /// <summary>
        /// Plugin Cfg Reader
        /// </summary>
        /// <remarks>
        /// Import Control Values
        /// </remarks>
        private static void PluginCfgReader()
        {
            // Controller Number
            string num = VM.Plugins_Input_InputSDL_View.Controller_SelectedItem;

            //MessageBox.Show(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")); //debug

            // Check if Paths Config TextBox is Empty
            if (MainWindow.IsValidPath(VM.PathsView.Config_Text))
            {
                // Check if Cfg File Exists
                if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
                {
                    // Start Cfg File Read
                    Configure.ConigFile cfg = null;

                    try
                    {
                        cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

                        // -------------------------
                        // Plugin
                        // -------------------------
                        string plugin = cfg.Read("Input-SDL-Control" + num, "plugin");
                        // None
                        if (plugin == "1")
                        {
                            VM.Plugins_Input_InputSDL_View.Plugin_SelectedItem = "None";
                        }
                        // Mem Pak
                        else if (plugin == "2")
                        {
                            VM.Plugins_Input_InputSDL_View.Plugin_SelectedItem = "Mem Pak";
                        }
                        // Rumble Pak
                        else if (plugin == "3")
                        {
                            VM.Plugins_Input_InputSDL_View.Plugin_SelectedItem = "Rumble Pak";
                        }

                        // -------------------------
                        // Device
                        // -------------------------
                        string device = cfg.Read("Input-SDL-Control" + num, "device");
                        // Keyboard/Mouse
                        if (device == "-1")
                        {
                            VM.Plugins_Input_InputSDL_View.Device_SelectedItem = "Keyboard/Mouse";
                        }
                        // Gamepad
                        else if (device == "0" || plugin == "1" || plugin == "2" || plugin == "3" || plugin == "4")
                        {
                            VM.Plugins_Input_InputSDL_View.Device_SelectedItem = "Gamepad";
                        }

                        // -------------------------
                        // Mode
                        // -------------------------
                        string mode = cfg.Read("Input-SDL-Control" + num, "mode");
                        // Manual
                        if (mode == "0")
                        {
                            VM.Plugins_Input_InputSDL_View.Mode_SelectedItem = "Manual";
                        }
                        // Auto with named SDL Device
                        else if (mode == "1")
                        {
                            VM.Plugins_Input_InputSDL_View.Mode_SelectedItem = "Auto with named SDL Device";
                        }
                        // Fully Automatic
                        else if (mode == "2")
                        {
                            VM.Plugins_Input_InputSDL_View.Mode_SelectedItem = "Fully Automatic";
                        }

                        // -------------------------
                        // Controller
                        // -------------------------
                        // Plugged
                        bool plugged = false;
                        bool.TryParse(cfg.Read("Input-SDL-Control" + num, "plugged").ToLower(), out plugged);
                        VM.Plugins_Input_InputSDL_View.Plugged_IsChecked = plugged;

                        // Mouse
                        bool mouse = false;
                        bool.TryParse(cfg.Read("Input-SDL-Control" + num, "mouse").ToLower(), out mouse);
                        VM.Plugins_Input_InputSDL_View.Mouse_IsChecked = mouse;

                        // -------------------------
                        // Analog Stick
                        // -------------------------
                        // Analog Deadzone
                        List<string> splitAnalogDeadzone = new List<string>();
                        splitAnalogDeadzone = cfg.Read("Input-SDL-Control" + num, "AnalogDeadzone").Split(',').ToList();
                        string analogDeadzoneX = string.Empty;
                        string analogDeadzoneY = string.Empty;
                        if (splitAnalogDeadzone.Count > 1) // null check
                        {
                            analogDeadzoneX = splitAnalogDeadzone[0];
                            analogDeadzoneY = splitAnalogDeadzone[1];
                        }

                        VM.Plugins_Input_InputSDL_View.AnalogDeadzoneX_Text = analogDeadzoneX;
                        VM.Plugins_Input_InputSDL_View.AnalogDeadzoneY_Text = analogDeadzoneY;

                        // Analog Peak
                        List<string> splitAnalogPeak = new List<string>();
                        splitAnalogPeak = cfg.Read("Input-SDL-Control" + num, "AnalogPeak").Split(',').ToList();
                        string analogPeakX = string.Empty;
                        string analogPeakY = string.Empty;
                        if (splitAnalogPeak.Count > 1) // null check
                        {
                            analogPeakX = splitAnalogPeak[0];
                            analogPeakY = splitAnalogPeak[1];
                        }

                        VM.Plugins_Input_InputSDL_View.AnalogPeakX_Text = analogPeakX;
                        VM.Plugins_Input_InputSDL_View.AnalogPeakY_Text = analogPeakY;
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Problem reading mupen64plus.cfg.",
                                        "Read Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
                // Error
                else
                {
                    MessageBox.Show("Could not find mupen64plus.cfg.",
                                    "Read Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            // Path not found
            else
            {
                MessageBox.Show("Config Path is empty.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }


        /// <summary>
        /// Display All Buttons
        /// </summary>
        /// <remarks>
        /// // Displays all buttons text from ButtonDisplayer
        /// </remarks>
        public void DisplayAllButtons()
        {
            // -------------------------
            // Display Key Letters on Toggle Buttons
            // -------------------------
            //System.Windows.MessageBox.Show(DPad_R); //debug
            VM.Plugins_Input_InputSDL_View.DPad_R_Text = ButtonDisplayer(DPad_R);
            VM.Plugins_Input_InputSDL_View.DPad_L_Text = ButtonDisplayer(DPad_L);
            VM.Plugins_Input_InputSDL_View.DPad_D_Text = ButtonDisplayer(DPad_D);
            VM.Plugins_Input_InputSDL_View.DPad_U_Text = ButtonDisplayer(DPad_U);

            VM.Plugins_Input_InputSDL_View.Start_Text = ButtonDisplayer(Start);

            VM.Plugins_Input_InputSDL_View.Z_Trig_Text = ButtonDisplayer(Z_Trig);
            VM.Plugins_Input_InputSDL_View.B_Button_Text = ButtonDisplayer(B_Button);
            VM.Plugins_Input_InputSDL_View.A_Button_Text = ButtonDisplayer(A_Button);

            VM.Plugins_Input_InputSDL_View.C_Button_R_Text = ButtonDisplayer(C_Button_R);
            VM.Plugins_Input_InputSDL_View.C_Button_L_Text = ButtonDisplayer(C_Button_L);
            VM.Plugins_Input_InputSDL_View.C_Button_D_Text = ButtonDisplayer(C_Button_D);
            VM.Plugins_Input_InputSDL_View.C_Button_U_Text = ButtonDisplayer(C_Button_U);

            VM.Plugins_Input_InputSDL_View.R_Trig_Text = ButtonDisplayer(R_Trig);
            VM.Plugins_Input_InputSDL_View.L_Trig_Text = ButtonDisplayer(L_Trig);

            VM.Plugins_Input_InputSDL_View.X_Axis_R_Text = ButtonDisplayer(X_Axis_R);
            VM.Plugins_Input_InputSDL_View.X_Axis_L_Text = ButtonDisplayer(X_Axis_L);
            VM.Plugins_Input_InputSDL_View.Y_Axis_U_Text = ButtonDisplayer(Y_Axis_U);
            VM.Plugins_Input_InputSDL_View.Y_Axis_D_Text = ButtonDisplayer(Y_Axis_D);

            VM.Plugins_Input_InputSDL_View.Mempak_Text = ButtonDisplayer(MemPak);
            VM.Plugins_Input_InputSDL_View.Rumblepak_Text = ButtonDisplayer(RumblePak);
        }

        /// <summary>
        /// Controller Select
        /// </summary>
        private void cboController_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Import Key int values from mupen64plus.cfg
            ButtonCfgReader();

            // Import Control Values
            PluginCfgReader();

            // Check if Fully Automatic Mode is on and disable the controls
            FullyAutomatic();

            // Display Key Letters on Toggle Buttons
            DisplayAllButtons();

            // Reset Save ✓ Label
            VM.Plugins_Input_InputSDL_View.Save_Text = "";
        }

        /// <summary>
        /// Device
        /// </summary>
        public void DeviceMode()
        {
            // For now Gamepad must be Fully Automatic
            // Do not have a way to capture Manual Gamepad button presses
            if (VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Gamepad")
            {
                VM.Plugins_Input_InputSDL_View.Mode_SelectedItem = "Fully Automatic";
                VM.Plugins_Input_InputSDL_View.Mode_IsEnabled = false;
            }
            // Keyboard/Mouse
            else if (VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Keyboard/Mouse")
            {
                VM.Plugins_Input_InputSDL_View.Mode_IsEnabled = true;
            }
        }
        private void cboDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeviceMode();
            FullyAutomatic();
        }


        /// <summary>
        /// Fully Automatic
        /// </summary>
        public void FullyAutomatic()
        {
            // -------------------------
            // Device: Gamepad
            // Mode: Fully Automatic
            // -------------------------
            // Disable, Uncheck, Display "Auto" text
            if ((VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Gamepad" &&
                VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Fully Automatic"))
            {
                // Disable All Buttons/Controls

                VM.Plugins_Input_InputSDL_View.DPad_R_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.DPad_R_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.DPad_R_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.DPad_L_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.DPad_L_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.DPad_L_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.DPad_D_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.DPad_D_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.DPad_D_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.DPad_U_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.DPad_U_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.DPad_U_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.Start_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.Start_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Start_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.Z_Trig_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.Z_Trig_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Z_Trig_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.A_Button_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.A_Button_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.A_Button_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.B_Button_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.B_Button_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.B_Button_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.C_Button_R_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.C_Button_R_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.C_Button_R_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.C_Button_L_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.C_Button_L_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.C_Button_L_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.C_Button_D_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.C_Button_D_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.C_Button_D_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.C_Button_U_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.C_Button_U_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.C_Button_U_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.R_Trig_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.R_Trig_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.R_Trig_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.L_Trig_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.L_Trig_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.L_Trig_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.X_Axis_L_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.X_Axis_L_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.X_Axis_L_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.X_Axis_R_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.X_Axis_R_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.X_Axis_R_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.Y_Axis_U_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.Y_Axis_U_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Y_Axis_U_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.Y_Axis_D_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.Y_Axis_D_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Y_Axis_D_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneX_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneX_IsEnabled = false;

                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneY_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneY_IsEnabled = false;

                VM.Plugins_Input_InputSDL_View.AnalogPeakX_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.AnalogPeakX_IsEnabled = false;

                VM.Plugins_Input_InputSDL_View.AnalogPeakY_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.AnalogPeakY_IsEnabled = false;

                VM.Plugins_Input_InputSDL_View.Mempak_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.Mempak_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Mempak_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.Rumblepak_Text = "Auto";
                VM.Plugins_Input_InputSDL_View.Rumblepak_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Rumblepak_IsChecked = false;
            }

            // -------------------------
            // Device: Keyboard/Mouse
            // Mode: Fully Automatic
            // -------------------------
            else if ((VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Keyboard/Mouse" &&
                VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Fully Automatic"))
            {
                // Disable All Buttons/Controls

                // Key Defaults
                KeyDefaults();

                // Display All Buttons
                DisplayAllButtons();

                // Enable/Disable
                // D-Pad
                VM.Plugins_Input_InputSDL_View.DPad_R_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.DPad_R_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.DPad_L_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.DPad_L_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.DPad_D_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.DPad_D_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.DPad_U_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.DPad_U_IsChecked = false;

                // Start
                VM.Plugins_Input_InputSDL_View.Start_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Start_IsChecked = false;

                // Z Trig
                VM.Plugins_Input_InputSDL_View.Z_Trig_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Z_Trig_IsChecked = false;

                // A Button
                VM.Plugins_Input_InputSDL_View.A_Button_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.A_Button_IsChecked = false;

                // B Button
                VM.Plugins_Input_InputSDL_View.B_Button_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.B_Button_IsChecked = false;

                // C Buttons
                VM.Plugins_Input_InputSDL_View.C_Button_R_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.C_Button_R_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.C_Button_L_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.C_Button_L_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.C_Button_D_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.C_Button_D_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.C_Button_U_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.C_Button_U_IsChecked = false;

                // R Trig
                VM.Plugins_Input_InputSDL_View.R_Trig_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.R_Trig_IsChecked = false;

                // L Trig
                VM.Plugins_Input_InputSDL_View.L_Trig_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.L_Trig_IsChecked = false;

                // Analog X Axis
                VM.Plugins_Input_InputSDL_View.X_Axis_L_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.X_Axis_L_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.X_Axis_R_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.X_Axis_R_IsChecked = false;

                // Analog Y Axis
                VM.Plugins_Input_InputSDL_View.Y_Axis_U_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Y_Axis_U_IsChecked = false;

                VM.Plugins_Input_InputSDL_View.Y_Axis_D_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Y_Axis_D_IsChecked = false;

                // Analog Deadzone
                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneX_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneY_IsEnabled = false;

                // Analog Peak
                VM.Plugins_Input_InputSDL_View.AnalogPeakX_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.AnalogPeakY_IsEnabled = false;

                // Mem Pak
                VM.Plugins_Input_InputSDL_View.Mempak_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Mempak_IsChecked = false;

                // Rumble Pak
                VM.Plugins_Input_InputSDL_View.Rumblepak_IsEnabled = false;
                VM.Plugins_Input_InputSDL_View.Rumblepak_IsChecked = false;
            }

            // -------------------------
            // Device: Keyboard/Mouse
            // Mode: Manual
            // -------------------------
            // Enable
            if (VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Keyboard/Mouse" && 
                VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Manual"
                )
            {
                // Load Button values from mupen64plus.cfg
                ButtonCfgReader();

                // Displays all buttons text from ButtonDisplayer
                DisplayAllButtons();

                // Enable All Buttons/Controls
                //VM.Plugins_Input_InputSDL_View.Plugged_IsEnabled = true;
                //VM.Plugins_Input_InputSDL_View.Mouse_IsEnabled = true;

                // D-Pad
                VM.Plugins_Input_InputSDL_View.DPad_R_IsEnabled = true;
                VM.Plugins_Input_InputSDL_View.DPad_L_IsEnabled = true;
                VM.Plugins_Input_InputSDL_View.DPad_D_IsEnabled = true;
                VM.Plugins_Input_InputSDL_View.DPad_U_IsEnabled = true;

                // Start
                VM.Plugins_Input_InputSDL_View.Start_IsEnabled = true;

                // Z Trig
                VM.Plugins_Input_InputSDL_View.Z_Trig_IsEnabled = true;

                // A Button
                VM.Plugins_Input_InputSDL_View.A_Button_IsEnabled = true;

                // B Button
                VM.Plugins_Input_InputSDL_View.B_Button_IsEnabled = true;

                // C Buttons
                VM.Plugins_Input_InputSDL_View.C_Button_R_IsEnabled = true;
                VM.Plugins_Input_InputSDL_View.C_Button_L_IsEnabled = true;
                VM.Plugins_Input_InputSDL_View.C_Button_D_IsEnabled = true;
                VM.Plugins_Input_InputSDL_View.C_Button_U_IsEnabled = true;

                // L Trig
                VM.Plugins_Input_InputSDL_View.L_Trig_IsEnabled = true;

                // R Trig
                VM.Plugins_Input_InputSDL_View.R_Trig_IsEnabled = true;

                // Analog X Axis
                VM.Plugins_Input_InputSDL_View.X_Axis_L_IsEnabled = true;
                VM.Plugins_Input_InputSDL_View.X_Axis_R_IsEnabled = true;

                // Analog Y Axis
                VM.Plugins_Input_InputSDL_View.Y_Axis_U_IsEnabled = true;
                VM.Plugins_Input_InputSDL_View.Y_Axis_D_IsEnabled = true;

                // Analog Deadzone
                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneX_Text = "1024";
                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneX_IsEnabled = true;

                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneY_Text = "1024";
                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneY_IsEnabled = true;

                // Analog Peak
                VM.Plugins_Input_InputSDL_View.AnalogPeakX_Text = "12288";
                VM.Plugins_Input_InputSDL_View.AnalogPeakX_IsEnabled = true;

                VM.Plugins_Input_InputSDL_View.AnalogPeakY_Text = "12288";
                VM.Plugins_Input_InputSDL_View.AnalogPeakY_IsEnabled = true;

                // Mem Pak
                VM.Plugins_Input_InputSDL_View.Mempak_IsEnabled = true;

                // Rumble Pak
                VM.Plugins_Input_InputSDL_View.Rumblepak_IsEnabled = true; 
            }
        }


        /// <summary>
        /// Mode - SelectionChanged Event
        /// </summary>
        private void cboMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // -------------------------
            // Device: Gamepad, Keyboard/Mouse
            // Mode: Fully Automatic
            // -------------------------
            // Disable, Uncheck, Display "Auto" text
            //if ((VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Gamepad" &&
            //    VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Fully Automatic")

            //    ||

            //    (VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Keyboard/Mouse" &&
            //    VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Fully Automatic"))
            //{
                FullyAutomatic();
            //}

            // -------------------------
            // Device: Keyboard/Mouse
            // Mode: Manual
            // -------------------------
            //if (VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Keyboard/Mouse" &&
            //    VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Manual")
            //{
            //    // Key Defaults
            //    KeyDefaults();

            //    // Display All Buttons
            //    DisplayAllButtons();
            //}
        }

        //public Keys KeyEnumConverter(System.Windows.Input.KeyEventArgs e)
        //{
        //    System.Windows.Input.Key wpfKey = e.Key == Key.System ? e.SystemKey : e.Key;
        //    Keys formsKey = (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(wpfKey);

        //    return formsKey;
        //}

        
        /// <summary>
        /// Enum Extract
        /// </summary>
        /// <remarks>
        /// Extracts the numbers between parentheses
        /// </remarks>
        public String EnumExtract(string value)
        {
            try
            {
                // Empty
                if (string.IsNullOrEmpty(value))
                {
                    return "";
                }
                else
                {
                    return Regex.Match(value, @"\(([^)]*)\)").Groups[1].Value;
                }
            }
            // Error
            catch
            {
                //MessageBox.Show("Error"); //debug
                return "🚫";
            }
        }

        /// <summary>
        /// Key Reader
        /// </summary>
        /// <remarks>
        /// Import Key number values from mupen64plus.cfg
        /// </remarks>
        public void ButtonCfgReader()
        {
            // Check if Paths Config TextBox is Empty
            if (MainWindow.IsValidPath(VM.PathsView.Config_Text))
            {
                // Check if Cfg File Exists
                if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
                {
                    // Save to mupen64plus.cfg
                    Configure.ConigFile cfg = null;

                    // Controller Number
                    string num = VM.Plugins_Input_InputSDL_View.Controller_SelectedItem;

                    try
                    {
                        cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

                        // -------------------------
                        // [Input-SDL-Control1-4]
                        // -------------------------
                        DPad_R = cfg.Read("Input-SDL-Control" + num, "DPad R");
                        DPad_L = cfg.Read("Input-SDL-Control" + num, "DPad L");
                        DPad_D = cfg.Read("Input-SDL-Control" + num, "DPad D");
                        DPad_U = cfg.Read("Input-SDL-Control" + num, "DPad U");

                        Start = cfg.Read("Input-SDL-Control" + num, "Start");

                        Z_Trig = cfg.Read("Input-SDL-Control" + num, "Z Trig");
                        B_Button = cfg.Read("Input-SDL-Control" + num, "B Button");
                        A_Button = cfg.Read("Input-SDL-Control" + num, "A Button");

                        C_Button_R = cfg.Read("Input-SDL-Control" + num, "C Button R");
                        C_Button_L = cfg.Read("Input-SDL-Control" + num, "C Button L");
                        C_Button_D = cfg.Read("Input-SDL-Control" + num, "C Button D");
                        C_Button_U = cfg.Read("Input-SDL-Control" + num, "C Button U");

                        R_Trig = cfg.Read("Input-SDL-Control" + num, "R Trig");
                        L_Trig = cfg.Read("Input-SDL-Control" + num, "L Trig");

                        MemPak = cfg.Read("Input-SDL-Control" + num, "Mempak switch");
                        RumblePak = cfg.Read("Input-SDL-Control" + num, "Rumblepak switch");

                        string X_Axis_Full = cfg.Read("Input-SDL-Control" + num, "X Axis"); // Eg. axis(0-,0+), key(97,100)
                        X_Axis = EnumExtract(X_Axis_Full); // Eg. 0-/0+ 97/100
                        List<string> splitXAxis = new List<string>();
                        splitXAxis = X_Axis.Split(',').ToList();
                        if (splitXAxis.Count > 1) // null check
                        {
                            // Axis
                            if (X_Axis_Full.Contains("axis("))
                            {
                                X_Axis_L = "axis(" + splitXAxis[0] + ")"; // re-wrap it in axis() parenthese for ButtonDisplayer
                                X_Axis_R = "axis(" + splitXAxis[1] + ")"; // re-wrap it in axis() parenthese for ButtonDisplayer
                            }
                            // Key
                            else if (X_Axis_Full.Contains("key("))
                            {
                                X_Axis_L = "key(" + splitXAxis[0] + ")"; // re-wrap it in axis() parenthese for ButtonDisplayer
                                X_Axis_R = "key(" + splitXAxis[1] + ")"; // re-wrap it in axis() parenthese for ButtonDisplayer
                            }
                        }

                        string Y_Axis_Full = cfg.Read("Input-SDL-Control" + num, "Y Axis"); // Eg. axis(1-,1+), key(119,115)
                        Y_Axis = EnumExtract(Y_Axis_Full); // Eg. 1-/1+ 119/115
                        List<string> splitYAxis = new List<string>();
                        splitYAxis = Y_Axis.Split(',').ToList();
                        if (splitXAxis.Count > 1) // null check
                        {
                            // Axis
                            if (X_Axis_Full.Contains("axis("))
                            {
                                Y_Axis_U = "axis(" + splitYAxis[0] + ")"; // re-wrap it in axis() parenthese for ButtonDisplayer
                                Y_Axis_D = "axis(" + splitYAxis[1] + ")"; // re-wrap it in axis() parenthese for ButtonDisplayer
                            }
                            // Key
                            else if (X_Axis_Full.Contains("key("))
                            {
                                Y_Axis_U = "key(" + splitYAxis[0] + ")"; // re-wrap it in axis() parenthese for ButtonDisplayer
                                Y_Axis_D = "key(" + splitYAxis[1] + ")"; // re-wrap it in axis() parenthese for ButtonDisplayer
                            }
                        }
                    }
                    // Error
                    catch
                    {
                        MessageBox.Show("Problem reading mupen64plus.cfg [Input-SDL-Control].",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
                // Error
                //else
                //{
                //    MessageBox.Show("Could not find mupen64plus.cfg.",
                //                    "Error",
                //                    MessageBoxButton.OK,
                //                    MessageBoxImage.Error);
                //}
            }
            // Path not found
            else
            {
                MessageBox.Show("Config Path is empty.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }


        /// <summary>
        /// Key Displayer
        /// </summary>
        /// <remarks>
        /// Read Key number values from and convert to Letter for display
        /// </remarks>
        public String ButtonDisplayer(string input)
        {
            //MessageBox.Show(input); //debug

            // Null check to prevent .Contains() crash
            if (MainWindow.IsValidPath(input))
            {
                // int.TryParse value
                // -1 is Not an int
                int number = -1;

                // -------------------------
                // Value is a Gamepad Button (Eg. axis(0+))
                // -------------------------
                if (input.Contains("hat(") ||
                    input.Contains("button(") ||
                    input.Contains("axis(") ||
                    input.Contains("+") ||
                    input.Contains("-") ||
                    input.Contains("Select..."))
                {
                    // Return original value without converting
                    // Such as axis(0+), button(7), Select..., etc.
                    return input;
                }
                // -------------------------
                // Value is a Keyboard Key (Eg. key(275))
                // -------------------------
                else if (input.Contains("key("))
                {
                    // Extract the value between the brackets
                    // Eg. key(275) = 275
                    // Keyboard Keys will not contain + or - symbols which will cause int convert error
                    string inputEnum = EnumExtract(input);

                    // Convert Enum to int
                    int value = 9999999;
                    int.TryParse(inputEnum, out value);
                    //int.TryParse(keyValue, out value);

                    // Int Convert Error
                    if (value == 9999999)
                    {
                        //MessageBox.Show("Error"); //debug
                        return "🚫";
                    }

                    // Int To Key Converter
                    return IntToKeyConverter(value);

                }
                // -------------------------
                // Is Number (Eg. 275)
                // -------------------------
                else if (int.TryParse(input, out number))
                {
                    // Int To Key Converter
                    return IntToKeyConverter(number);
                }
                // Error
                else
                {
                    //MessageBox.Show("Error"); //debug
                    return "🚫";
                }
            }

            return input;
        }


        /// <summary>
        /// Int To Key Converter
        /// </summary>
        /// <remarks>
        /// Converts Mupen64Plus Input SDL Plugin Format int to Keyboard Letter/Symbol
        /// </remarks>
        public static String IntToKeyConverter(int value)
        {
            // Convert
            switch (value)
            {
                // F1
                case 282:
                    return "F1";
                // F2
                case 283:
                    return "F2";
                // F3
                case 284:
                    return "F3";
                // F4
                case 285:
                    return "F4";
                // F5
                case 286:
                    return "F5";
                // F6
                case 287:
                    return "F6";
                // F7
                case 288:
                    return "F7";
                // F8
                case 289:
                    return "F8";
                // F9
                case 290:
                    return "F9";
                // F10
                case 291:
                    return "F10";
                // F11
                case 292:
                    return "F11";
                // F12
                case 293:
                    return "F12";

                // Scroll
                case 302:
                    return "ScrLk";
                // Pause
                case 19:
                    return "Pause";
                // Ins
                case 277:
                    return "Ins";
                // Home
                case 278:
                    return "Home";
                // PageUp 
                case 280:
                    return "PgUp";
                // PageDown
                case 281:
                    return "PgDn";
                // Delete
                case 127:
                    return "Del";
                // End
                case 279:
                    return "End";

                // Right Arrow
                case 275:
                    return "🡆";
                // Down Arrow
                case 274:
                    return "🡇";
                // Left Arrow
                case 276:
                    return "🡄";
                // Up Arrow
                case 273:
                    return "🡅";

                // Tilde ` ~
                case 96:
                    return "`";

                // 1
                case 49:
                    return "1";
                // 2
                case 50:
                    return "2";
                // 3
                case 51:
                    return "3";
                // 4
                case 52:
                    return "4";
                // 5
                case 53:
                    return "5";
                // 6
                case 54:
                    return "6";
                // 7
                case 55:
                    return "7";
                // 8
                case 56:
                    return "8";
                // 9
                case 57:
                    return "9";
                // 0
                case 48:
                    return "0";

                // Minus - _
                case 45:
                    return "-";
                // Plus + =
                case 61:
                    return "=";
                // Q
                case 113:
                    return "Q";
                // W
                case 119:
                    return "W";
                // E
                case 101:
                    return "E";
                // R
                case 114:
                    return "R";
                // T
                case 116:
                    return "T";
                // Y
                case 121:
                    return "Y";
                // U
                case 117:
                    return "U";
                // I
                case 105:
                    return "I";
                // O
                case 111:
                    return "O";
                // P
                case 112:
                    return "P";
                // OpenBrackets [
                case 91:
                    return "[";
                // ClosedBrackets ]
                case 93:
                    return "]";
                // \
                case 92:
                    return @"\";

                // Capslock
                case 301:
                    return "Caps";
                // A
                case 97:
                    return "A";
                // S
                case 115:
                    return "S";
                // D
                case 100:
                    return "D";
                // F
                case 102:
                    return "F";
                // G
                case 103:
                    return "G";
                // H
                case 104:
                    return "H";
                // J
                case 106:
                    return "J";
                // K
                case 107:
                    return "K";
                // L
                case 108:
                    return "L";
                // ;
                case 59:
                    return ";";
                // Quotes ' "
                case 39:
                    return "'";
                // Return Enter
                case 13:
                    return "Enter";

                // Shift Left
                case 304:
                    return "Shift";
                // Shift Right
                //case 304:
                //    return "Sift";

                // Z
                case 122:
                    return "Z";
                // X
                case 120:
                    return "X";
                // C
                case 99:
                    return "C";
                // V
                case 118:
                    return "V";
                // B
                case 98:
                    return "B";
                // N
                case 110:
                    return "N";
                // M
                case 109:
                    return "M";
                // ,
                case 44:
                    return ",";
                // .
                case 46:
                    return ".";
                // /
                case 47:
                    return "/";

                // Ctrl Left
                case 306:
                    return "Ctrl";
                // Ctrl Right
                //case 306:
                //    return "Ctrl";
                // Alt Left
                case 308:
                    return "Alt";
                // Alt Right
                //case 308:
                //    return "Alt";
                // Space
                case 32:
                    return "Space";

                // Default
                default:
                    return "";
            }
        }


        /// <summary>
        /// Key Enum Converter
        /// </summary>
        /// <remarks>
        /// Converts System.Windows.Input Key Enum to Mupen64Plus Input SDL Plugin Format
        /// </remarks>
        public int KeyEnumConverter(int keyValue)
        {
            // Extract the value between the brackets
            // Eg. key(275) = 275
            // Keyboard Keys will not contain + or - symbols which will cause int convert error

            switch (keyValue)
            {
                // F1
                case 90:
                    return 282;
                // F2
                case 91:
                    return 283;
                // F3
                case 92:
                    return 284;
                // F4
                case 93:
                    return 285;
                // F5
                case 94:
                    return 286;
                // F6
                case 95:
                    return 287;
                // F7
                case 96:
                    return 288;
                // F8
                case 97:
                    return 289;
                // F9
                case 98:
                    return 290;
                // F10
                case 99:
                    return 291;
                // F11
                case 100:
                    return 292;
                // F12
                case 101:
                    return 293;

                // Scroll
                case 115:
                    return 302;
                // Pause
                case 7:
                    return 19;
                // Ins
                case 31:
                    return 277;
                // Home
                case 22:
                    return 278;
                // PageUp 
                case 19:
                    return 280;
                // PageDown
                case 20:
                    return 281;
                // Delete
                case 32:
                    return 127;
                // End
                case 21:
                    return 279;

                // Up Arrow
                case 24:
                    return 273;
                // Left Arrow
                case 23:
                    return 276;
                // Down Arrow
                case 26:
                    return 274;
                // Right Arrow
                case 25:
                    return 275;

                // Tilde ` ~
                case 146:
                    return 96;

                // 1
                case 35:
                    return 49;
                // 2
                case 36:
                    return 50;
                // 3
                case 37:
                    return 51;
                // 4
                case 38:
                    return 52;
                // 5
                case 39:
                    return 53;
                // 6
                case 40:
                    return 54;
                // 7
                case 41:
                    return 55;
                // 8
                case 42:
                    return 56;
                // 9
                case 43:
                    return 57;
                // 0
                case 34:
                    return 48;

                // Minus - _
                case 143:
                    return 45;
                // Plus + =
                case 141:
                    return 61;
                // Q
                case 60:
                    return 113;
                // W
                case 66:
                    return 119;
                // E
                case 48:
                    return 101;
                // R
                case 61:
                    return 114;
                // T
                case 63:
                    return 116;
                // Y
                case 68:
                    return 121;
                // U
                case 64:
                    return 117;
                // I
                case 52:
                    return 105;
                // O
                case 58:
                    return 111;
                // P
                case 59:
                    return 112;
                // OpenBrackets [
                case 149:
                    return 91;
                // ClosedBrackets ]
                case 151:
                    return 93;
                // \
                case 150:
                    return 92;

                // Capslock
                case 8:
                    return 301;
                // A
                case 44:
                    return 97;
                // S
                case 62:
                    return 115;
                // D
                case 47:
                    return 100;
                // F
                case 49:
                    return 102;
                // G
                case 50:
                    return 103;
                // H
                case 51:
                    return 104;
                // J
                case 53:
                    return 106;
                // K
                case 54:
                    return 107;
                // L
                case 55:
                    return 108;
                // ;
                case 140:
                    return 59;
                // Quotes ' "
                case 152:
                    return 39;
                // Return Enter
                case 6:
                    return 13;

                // Shift Left
                case 116:
                    return 304;
                // Shift Right
                case 117:
                    return 304;

                // Z
                case 69:
                    return 122;
                // X
                case 67:
                    return 120;
                // C
                case 46:
                    return 99;
                // V
                case 65:
                    return 118;
                // B
                case 45:
                    return 98;
                // N
                case 57:
                    return 110;
                // M
                case 56:
                    return 109;
                // ,
                case 142:
                    return 44;
                // .
                case 144:
                    return 46;
                // /
                case 145:
                    return 47;

                // Ctrl Left
                case 118:
                    return 306;
                // Ctrl Right
                case 119:
                    return 306;
                // Alt Left
                case 120:
                    return 308;
                // Alt Right
                case 121:
                    return 308;
                // Space
                case 18:
                    return 32;

                // Default
                default:
                    return 0;
            }
        }


        /// <summary>
        /// Untoggle All Buttons
        /// </summary>
        public static void UntoggleAllButtons(string button)
        {
            //if (button = )
            //{

            //}
        }


        /// <summary>
        /// DPad R
        /// </summary>
        private void btnDPad_R_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnDPad_R.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.DPad_R_Text = ButtonDisplayer(keyEnum.ToString());
                DPad_R = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnDPad_R.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// DPad L
        /// </summary>
        private void btnDPad_L_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnDPad_L.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.DPad_L_Text = ButtonDisplayer(keyEnum.ToString());
                DPad_L = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnDPad_L.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// DPad D
        /// </summary>
        private void btnDPad_D_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnDPad_D.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                //VM.Plugins_Input_InputSDL_View.DPad_D_Text = e.Key.ToString();
                VM.Plugins_Input_InputSDL_View.DPad_D_Text = ButtonDisplayer(keyEnum.ToString());
                DPad_D = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnDPad_D.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// DPad U
        /// </summary>
        private void btnDPad_U_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnDPad_U.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.DPad_U_Text = ButtonDisplayer(keyEnum.ToString());
                DPad_U = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnDPad_U.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// Start
        /// </summary>
        private void btnStart_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // PROBLEM - Enter/Space Key stays stuck
            if (btnStart.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.Start_Text = ButtonDisplayer(keyEnum.ToString());
                Start = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnStart.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// Z Trig
        /// </summary>
        private void btnZ_Trig_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnZ_Trig.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.Z_Trig_Text = ButtonDisplayer(keyEnum.ToString());
                Z_Trig = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnZ_Trig.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// B Button
        /// </summary>
        private void btnB_Button_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnB_Button.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.B_Button_Text = ButtonDisplayer(keyEnum.ToString());
                B_Button = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnB_Button.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// A Button
        /// </summary>
        private void btnA_Button_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnA_Button.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.A_Button_Text = ButtonDisplayer(keyEnum.ToString());
                A_Button = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnA_Button.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// C Button R
        /// </summary>
        private void btnC_Button_R_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnC_Button_R.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.C_Button_R_Text = ButtonDisplayer(keyEnum.ToString());
                C_Button_R = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnC_Button_R.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// C Button L
        /// </summary>
        private void btnC_Button_L_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnC_Button_L.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.C_Button_L_Text = ButtonDisplayer(keyEnum.ToString());
                C_Button_L = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnC_Button_L.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// C Button D
        /// </summary>
        private void btnC_Button_D_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnC_Button_D.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.C_Button_D_Text = ButtonDisplayer(keyEnum.ToString());
                C_Button_D = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnC_Button_D.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// C Button U
        /// </summary>
        private void btnC_Button_U_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnC_Button_U.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.C_Button_U_Text = ButtonDisplayer(keyEnum.ToString());
                C_Button_U = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnC_Button_U.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// R Trig
        /// </summary>
        private void btnR_Trig_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnR_Trig.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.R_Trig_Text = ButtonDisplayer(keyEnum.ToString());
                R_Trig = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnR_Trig.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// L Trig
        /// </summary>
        private void btnL_Trig_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnL_Trig.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.L_Trig_Text = ButtonDisplayer(keyEnum.ToString());
                L_Trig = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnL_Trig.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// Mem Pak
        /// </summary>
        private void btnMemPak_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnMemPak.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.Mempak_Text = ButtonDisplayer(keyEnum.ToString());
                MemPak = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnMemPak.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// Rumble Pak
        /// </summary>
        private void btnRumblePak_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnRumblePak.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.Rumblepak_Text = ButtonDisplayer(keyEnum.ToString());
                RumblePak = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnRumblePak.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// X Axis R
        /// </summary>
        private void btnX_Axis_R_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnX_Axis_R.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.X_Axis_R_Text = ButtonDisplayer(keyEnum.ToString());
                X_Axis_R = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnX_Axis_R.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// X Axis L
        /// </summary>
        private void btnX_Axis_L_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnX_Axis_L.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.X_Axis_L_Text = ButtonDisplayer(keyEnum.ToString());
                X_Axis_L = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnX_Axis_L.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// Y Axis U
        /// </summary>
        private void btnY_Axis_U_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnY_Axis_U.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.Y_Axis_U_Text = ButtonDisplayer(keyEnum.ToString());
                Y_Axis_U = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnY_Axis_U.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// Y Axis D
        /// </summary>
        private void btnY_Axis_D_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (btnY_Axis_D.IsChecked == true)
            {
                // Convert Key
                int keyEnum = KeyEnumConverter((int)e.Key);

                // Set Key
                VM.Plugins_Input_InputSDL_View.Y_Axis_D_Text = ButtonDisplayer(keyEnum.ToString());
                Y_Axis_D = "key(" + keyEnum.ToString() + ")";

                // Untoggle
                btnY_Axis_D.IsChecked = false;

                // Reset Save ✓ Label
                VM.Plugins_Input_InputSDL_View.Save_Text = "";
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Check if Paths Config TextBox is Empty
            if (!string.IsNullOrEmpty(VM.PathsView.Config_Text) &&
                !string.IsNullOrWhiteSpace(VM.PathsView.Config_Text))
            {
                // Check if Cfg File Exists
                if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
                {
                    // Save to mupen64plus.cfg
                    Configure.ConigFile cfg = null;

                    // Controller Number
                    string num = VM.Plugins_Input_InputSDL_View.Controller_SelectedItem;

                    try
                    {
                        cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

                        // -------------------------
                        // [Input-SDL-Control1-4]
                        // -------------------------
                        // -------------------------
                        // Plugin
                        // -------------------------
                        // None
                        if (VM.Plugins_Input_InputSDL_View.Plugin_SelectedItem == "None")
                        {
                            cfg.Write("Input-SDL-Control" + num, "plugin", "\"1\"");
                        }
                        // Mem Pak
                        else if (VM.Plugins_Input_InputSDL_View.Plugin_SelectedItem == "Mem Pak")
                        {
                            cfg.Write("Input-SDL-Control" + num, "plugin", "\"2\"");
                        }
                        // Rumble Pak
                        else if (VM.Plugins_Input_InputSDL_View.Plugin_SelectedItem == "Rumble Pak")
                        {
                            cfg.Write("Input-SDL-Control" + num, "plugin", "\"3\"");
                        }

                        // -------------------------
                        // Device
                        // -------------------------
                        // Keyboard/Mouse
                        if (VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Keyboard/Mouse")
                        {
                            cfg.Write("Input-SDL-Control" + num, "device", "-1");
                        }
                        // Gamepad
                        else if (VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Gamepad")
                        {
                            cfg.Write("Input-SDL-Control" + num, "device", "0");
                        }

                        // -------------------------
                        // Mode
                        // -------------------------
                        // Manual
                        if (VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Manual")
                        {
                            cfg.Write("Input-SDL-Control" + num, "mode", "0");
                        }
                        // Auto with named SDL Device
                        else if (VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Auto with named SDL Device")
                        {
                            cfg.Write("Input-SDL-Control" + num, "mode", "1");
                        }
                        // Fully Automatic
                        else if (VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Fully Automatic")
                        {
                            cfg.Write("Input-SDL-Control" + num, "mode", "2");
                        }

                        // -------------------------
                        // Controller
                        // -------------------------
                        // Plugged
                        if (VM.Plugins_Input_InputSDL_View.Plugged_IsChecked == true)
                        {
                            cfg.Write("Input-SDL-Control" + num, "plugged", "True");
                        }
                        else if (VM.Plugins_Input_InputSDL_View.Plugged_IsChecked == false)
                        {
                            cfg.Write("Input-SDL-Control" + num, "plugged", "False");
                        }

                        // Mouse
                        if (VM.Plugins_Input_InputSDL_View.Mouse_IsChecked == true)
                        {
                            cfg.Write("Input-SDL-Control" + num, "mouse", "True");
                        }
                        else if (VM.Plugins_Input_InputSDL_View.Mouse_IsChecked == false)
                        {
                            cfg.Write("Input-SDL-Control" + num, "mouse", "False");
                        }

                        // -------------------------
                        // Save Buttons/Joystick only if Manual Mode
                        // -------------------------
                        if (VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Keyboard/Mouse" &&
                            VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Manual")
                        {
                            // -------------------------
                            // Analog Stick
                            // -------------------------
                            // Analog Deadzone
                            cfg.Write("Input-SDL-Control" + num, "AnalogDeadzone", "\""
                                                          + VM.Plugins_Input_InputSDL_View.AnalogDeadzoneX_Text
                                                          + ","
                                                          + VM.Plugins_Input_InputSDL_View.AnalogDeadzoneY_Text
                                                          + "\"");

                            // Analog Peak
                            cfg.Write("Input-SDL-Control" + num, "AnalogPeak", "\""
                                                          + VM.Plugins_Input_InputSDL_View.AnalogPeakX_Text
                                                          + ","
                                                          + VM.Plugins_Input_InputSDL_View.AnalogPeakY_Text
                                                          + "\"");

                            // -------------------------
                            // Buttons
                            // -------------------------
                            // Wrap in quotes (Eg. "key(275)")
                            cfg.Write("Input-SDL-Control" + num, "DPad R", "\"" + DPad_R + "\"");
                            cfg.Write("Input-SDL-Control" + num, "DPad L", "\"" + DPad_L + "\"");
                            cfg.Write("Input-SDL-Control" + num, "DPad D", "\"" + DPad_D + "\"");
                            cfg.Write("Input-SDL-Control" + num, "DPad U", "\"" + DPad_U + "\"");

                            cfg.Write("Input-SDL-Control" + num, "Start", "\"" + Start + "\"");

                            cfg.Write("Input-SDL-Control" + num, "Z Trig", "\"" + Z_Trig + "\"");
                            cfg.Write("Input-SDL-Control" + num, "B Button", "\"" + B_Button + "\"");
                            cfg.Write("Input-SDL-Control" + num, "A Button", "\"" + A_Button + "\"");

                            cfg.Write("Input-SDL-Control" + num, "C Button R", "\"" + C_Button_R + "\"");
                            cfg.Write("Input-SDL-Control" + num, "C Button L", "\"" + C_Button_L + "\"");
                            cfg.Write("Input-SDL-Control" + num, "C Button D", "\"" + C_Button_D + "\"");
                            cfg.Write("Input-SDL-Control" + num, "C Button U", "\"" + C_Button_U + "\"");

                            cfg.Write("Input-SDL-Control" + num, "R Trig", "\"" + R_Trig + "\"");
                            cfg.Write("Input-SDL-Control" + num, "L Trig", "\"" + L_Trig + "\"");

                            cfg.Write("Input-SDL-Control" + num, "Mempak switch", "\"" + MemPak + "\"");
                            cfg.Write("Input-SDL-Control" + num, "Rumblepak switch", "\"" + RumblePak + "\"");

                            // Enum Extract and Recombine Axis
                            // "key(97),key(100)" to "key(97,100)"
                            cfg.Write("Input-SDL-Control" + num, "X Axis", X_Axis = "\"key(" + EnumExtract(X_Axis_L) + "," + EnumExtract(X_Axis_R) + ")\"");
                            cfg.Write("Input-SDL-Control" + num, "Y Axis", Y_Axis = "\"key(" + EnumExtract(Y_Axis_U) + "," + EnumExtract(Y_Axis_D) + ")\"");

                        }

                        // -------------------------
                        // Save Complete
                        // -------------------------
                        VM.Plugins_Input_InputSDL_View.Save_Text = "✓";
                    }
                    // Error
                    catch
                    {
                        VM.Plugins_Input_InputSDL_View.Save_Text = "Error";

                        MessageBox.Show("Could not save to mupen64plus.cfg.",
                                        "Write Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
                // Error
                else
                {
                    MessageBox.Show("Could not find mupen64plus.cfg.",
                                    "Write Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            // Path not found
            else
            {
                MessageBox.Show("Config Path is empty.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Defaults
        /// </summary>
        private void btnDefaults_Click(object sender, RoutedEventArgs e)
        {
            // -------------------------
            // Device: Gamepad, Keyboard/Mouse
            // Mode: Fully Automatic
            // -------------------------
            //if (VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Gamepad")
            //{

                //# Scaling factor for mouse movements.  For X, Y axes.
                //            MouseSensitivity = "2.00,2.00"
                //# The minimum absolute value of the SDL analog joystick axis to move the N64 controller axis value from 0.  For X, Y axes.
                //AnalogDeadzone = "1024,1024"
                //# An absolute value of the SDL joystick axis >= AnalogPeak will saturate the N64 controller axis value (at 80).  For X, Y axes. For each axis, this must be greater than the corresponding AnalogDeadzone value
                //AnalogPeak = "21000,21000"
                //# Digital button configuration mappings
                //DPad R = "hat(0 Right)"
                //DPad L = "hat(0 Left)"
                //DPad D = "hat(0 Down)"
                //DPad U = "hat(0 Up)"
                //Start = "button(9)"
                //Z Trig = "button(8)"
                //B Button = "button(2)"
                //A Button = "button(1)"
                //C Button R = "axis(3-)"
                //C Button L = "axis(3+)"
                //C Button D = "axis(2+)"
                //C Button U = "axis(2-)"
                //R Trig = "button(7)"
                //L Trig = "button(6)"
                //Mempak switch = "key(109)"
                //Rumblepak switch = "key(114)"
                //# Analog axis configuration mappings
                //X Axis = "axis(0-,0+)"
                //Y Axis = "axis(1-,1+)"

            //    // D-Pad
            //    DPad_R = "()"; // 
            //    DPad_L = "()"; // 
            //    DPad_D = "()"; // 
            //    DPad_U = "()"; // 

            //    // Start
            //    Start = "()"; // 

            //    // Letter Buttons
            //    Z_Trig = "()"; // 
            //    B_Button = "()"; // 
            //    A_Button = "()"; // 

            //    // C-Pad
            //    C_Button_R = "()"; // 
            //    C_Button_L = "()"; // 
            //    C_Button_D = "()"; // 
            //    C_Button_U = "()"; // 

            //    // Triggers
            //    L_Trig = "()"; // 
            //    R_Trig = "()"; // 

            //    // Analog
            //    X_Axis_R = "()"; // 
            //    X_Axis_L = "()"; // 
            //    Y_Axis_D = "()"; // 
            //    Y_Axis_U = "()"; // 

            //    // Accessories
            //    MemPak = "()"; // 
            //    RumblePak = "()"; // 
            //}

            // -------------------------
            // Device: Keyboard/Mouse
            // Mode: Manual
            // -------------------------
            if (VM.Plugins_Input_InputSDL_View.Device_SelectedItem == "Keyboard/Mouse" &&
                VM.Plugins_Input_InputSDL_View.Mode_SelectedItem == "Manual")
            {
                // Plugged
                VM.Plugins_Input_InputSDL_View.Plugged_IsChecked = true;

                // Mouse
                VM.Plugins_Input_InputSDL_View.Mouse_IsChecked = false;

                // Key Defaults
                KeyDefaults();

                // Analog Deadzone
                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneX_Text = "1024";
                VM.Plugins_Input_InputSDL_View.AnalogDeadzoneY_Text = "1024";

                // Analog Peak
                VM.Plugins_Input_InputSDL_View.AnalogPeakX_Text = "12288";
                VM.Plugins_Input_InputSDL_View.AnalogPeakY_Text = "12288";

                // Accessories
                MemPak = "key(44)"; // ,
                RumblePak = "key(46)"; // .
                //MemPak = "key(109)"; // M
                //RumblePak = "key(114)"; // R

                // Display All Buttons
                DisplayAllButtons();
            }

            // Reset Save ✓ Label
            VM.Plugins_Audio_AudioSDL_View.Save_Text = "";
        }

        /// <summary>
        /// Key Defaults
        /// </summary>
        public void KeyDefaults()
        {
            // D-Pad
            //DPad_R = "key(275)"; // 🡆
            //DPad_L = "key(276)"; // 🡄
            //DPad_D = "key(274)"; // 🡇
            //DPad_U = "key(273)"; // 🡅
            DPad_R = "key(100)"; // D
            DPad_L = "key(97)"; // A
            DPad_D = "key(115)"; // S
            DPad_U = "key(119)"; // W

            // Start
            Start = "key(13)"; // Enter

            // Letter Buttons
            Z_Trig = "key(122)"; // Z
            B_Button = "key(306)"; // Ctrl
            A_Button = "key(304)"; // Shift

            // C-Pad
            C_Button_R = "key(108)"; // L
            C_Button_L = "key(106)"; // J
            C_Button_D = "key(107)"; // K
            C_Button_U = "key(105)"; // I

            // Triggers
            L_Trig = "key(120)"; // X
            R_Trig = "key(99)"; // C

            // Analog
            //X_Axis_R = "key(100)"; // D
            //X_Axis_L = "key(97)"; // A
            //Y_Axis_D = "key(115)"; // S
            //Y_Axis_U = "key(119)"; // W
            X_Axis_R = "key(275)"; // 🡆
            X_Axis_L = "key(276)"; // 🡄
            Y_Axis_D = "key(274)"; // 🡇
            Y_Axis_U = "key(273)"; // 🡅

            // Accessories
            MemPak = "key(44)"; // ,
            RumblePak = "key(46)"; // .
        }

        /// <summary>
        /// Close
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

    
}
