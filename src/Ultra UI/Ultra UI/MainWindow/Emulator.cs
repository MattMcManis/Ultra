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

namespace Ultra
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Pure Interpreter
        /// </summary>
        //private void cbxPureInterpreter_Checked(object sender, RoutedEventArgs e)
        //{
        //    // Write to mupen64plus.cfg
        //    List<Action> actionsToWrite = new List<Action>
        //    {
        //        new Action(() => 
        //        {
        //            // -------------------------
        //            // [Core]
        //            // -------------------------
        //            Configure.ConigFile.cfg.Write("Core", "R4300Emulator", "0");
        //        }),
        //    };

        //    MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
        //                                 "mupen64plus.cfg",        // Filename
        //                                 actionsToWrite            // Actions to write
        //                                );
        //}
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
        /// Cached Interpreter
        /// </summary>
        //private void cbxCachedInterpreter_Checked(object sender, RoutedEventArgs e)
        //{
        //    // Write to mupen64plus.cfg
        //    List<Action> actionsToWrite = new List<Action>
        //    {
        //        new Action(() => 
        //        {
        //            // -------------------------
        //            // [Core]
        //            // -------------------------
        //            Configure.ConigFile.cfg.Write("Core", "R4300Emulator", "1");
        //        }),
        //    };

        //    MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
        //                                 "mupen64plus.cfg",        // Filename
        //                                 actionsToWrite            // Actions to write
        //                                );
        //}

        /// <summary>
        /// Dynamic Recompiler
        /// </summary>
        //private void cbxDynamicRecompiler_Checked(object sender, RoutedEventArgs e)
        //{
        //    // Write to mupen64plus.cfg
        //    List<Action> actionsToWrite = new List<Action>
        //    {
        //        new Action(() =>
        //        {
        //            // -------------------------
        //            // [Core]
        //            // -------------------------
        //            Configure.ConigFile.cfg.Write("Core", "R4300Emulator", "2");
        //        }),
        //    };

        //    MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
        //                                 "mupen64plus.cfg",        // Filename
        //                                 actionsToWrite            // Actions to write
        //                                );
        //}

        /// <summary>
        /// DisableSpecRecomp
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
        /// RandomizeInterrupt
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
