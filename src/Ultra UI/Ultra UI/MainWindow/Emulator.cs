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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModel;

namespace Ultra
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Pure Interpreter
        /// </summary>
        private void cboCPU_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cpu = string.Empty;

            switch (VM.EmulatorView.CPU_SelectedItem)
            {
                case "Pure Interpreter":
                    cpu = "0";
                    break;

                case "Cached Interpreter":
                    cpu = "1";
                    break;

                case "Dynamic Recompiler":
                    cpu = "2";
                    break;

                default:
                    cpu = "2";
                    break;
            }

            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Core]
                    // -------------------------
                    Configure.ConigFile.cfg.Write("Core", "R4300Emulator", cpu);
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Disable Spec Recomp
        /// </summary>
        private void cbxDisableSpecRecomp_Checked(object sender, RoutedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Core]
                    // -------------------------
                    if (VM.EmulatorView.Emulator_DisableSpecRecomp_IsChecked == true)
                    {
                        Configure.ConigFile.cfg.Write("Core", "DisableSpecRecomp", "True");
                    }
                    else if (VM.EmulatorView.Emulator_DisableSpecRecomp_IsChecked == false)
                    {
                        Configure.ConigFile.cfg.Write("Core", "DisableSpecRecomp", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Randomize Interrupt
        /// </summary>
        private void cbxRandomizeInterrupt_Checked(object sender, RoutedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Core]
                    // -------------------------
                    if (VM.EmulatorView.Emulator_RandomizeInterrupt_IsChecked == true)
                    {
                        Configure.ConigFile.cfg.Write("Core", "RandomizeInterrupt", "True");
                    }
                    else if (VM.EmulatorView.Emulator_RandomizeInterrupt_IsChecked == false)
                    {
                        Configure.ConigFile.cfg.Write("Core", "RandomizeInterrupt", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// No Compiled Jump
        /// </summary>
        private void cbxNoCompiledJump_Checked(object sender, RoutedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Core]
                    // -------------------------
                    if (VM.EmulatorView.Emulator_NoCompiledJump_IsChecked == true)
                    {
                        Configure.ConigFile.cfg.Write("Core", "NoCompiledJump", "True");
                    }
                    else if (VM.EmulatorView.Emulator_NoCompiledJump_IsChecked == false)
                    {
                        Configure.ConigFile.cfg.Write("Core", "NoCompiledJump", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Disable Extra Memory
        /// </summary>
        private void cbxDisableExtraMemory_Checked(object sender, RoutedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Core]
                    // -------------------------
                    if (VM.EmulatorView.Emulator_DisableExtraMemory_IsChecked == true)
                    {
                        Configure.ConigFile.cfg.Write("Core", "DisableExtraMem", "True");
                    }
                    else if (VM.EmulatorView.Emulator_DisableExtraMemory_IsChecked == false)
                    {
                        Configure.ConigFile.cfg.Write("Core", "DisableExtraMem", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Delay SI
        /// </summary>
        private void cbxDelaySI_Checked(object sender, RoutedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Core]
                    // -------------------------
                    if (VM.EmulatorView.Emulator_DelaySI_IsChecked == true)
                    {
                        Configure.ConigFile.cfg.Write("Core", "DelaySI", "True");
                    }
                    else if (VM.EmulatorView.Emulator_DelaySI_IsChecked == false)
                    {
                        Configure.ConigFile.cfg.Write("Core", "DelaySI", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Cycles
        /// </summary>
        private void cboEmulator_Cycles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    if (!string.IsNullOrEmpty(VM.EmulatorView.Emulator_Cycles_SelectedItem))
                    {
                        Configure.ConigFile.cfg.Write("Core", "CountPerOp", VM.EmulatorView.Emulator_Cycles_SelectedItem);
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }


        /// <summary>
        /// Emulator Defaults - Button
        /// </summary>
        private void btnEmulatorDefaults_Click(object sender, RoutedEventArgs e)
        {
            EmulatorDefaults();
        }
        public void EmulatorDefaults()
        {
            // CPU
            //VM.EmulatorView.Emulator_DynamicRecompiler_IsChecked = true;
            VM.EmulatorView.CPU_SelectedItem = "Dynamic Recompiler";

            // DisableSpecRecomp
            VM.EmulatorView.Emulator_DisableSpecRecomp_IsChecked = true;

            // RandomizeInterrupt
            VM.EmulatorView.Emulator_RandomizeInterrupt_IsChecked = true;

            // No Compiled Jump
            VM.EmulatorView.Emulator_NoCompiledJump_IsChecked = false;

            // Disable Extra Memory
            VM.EmulatorView.Emulator_DisableExtraMemory_IsChecked = false;

            // Delay SI
            VM.EmulatorView.Emulator_DelaySI_IsChecked = true;

            // Cycles
            VM.EmulatorView.Emulator_Cycles_SelectedItem = "0";
        }
    }
}
