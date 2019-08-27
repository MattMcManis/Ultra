/* ----------------------------------------------------------------------
Ultra UI
https://github.com/MattMcManis/Ultra
https://ultraui.github.io
mattmcmanis@outlook.com

The MIT License

Copyright (C) 2019 Matt McManis

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

namespace Ultra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // $ mupen64plus --help
        // Usage: mupen64plus[parameter(s)] rom

        //Parameters:
        //    --noosd               : disable onscreen display
        //    --osd                 : enable onscreen display
        //    --fullscreen          : use fullscreen display mode
        //    --windowed            : use windowed display mode
        //    --resolution(res)     : display resolution(640x480, 800x600, 1024x768, etc)
        //    --nospeedlimit        : disable core speed limiter(should be used with dummy audio plugin)
        //    --cheats(cheat-spec)  : enable or list cheat codes for the given rom file
        //    --corelib(filepath)   : use core library(filepath) (can be only filename or full path)
        //    --configdir(dir)      : force configation directory to(dir); should contain mupen64plus.cfg
        //    --datadir(dir)        : search for shared data files(.ini files, languages, etc) in (dir)
        //    --debug               : launch console-based debugger(requires core lib built for debugging)
        //    --plugindir(dir)      : search for plugins in (dir)
        //    --sshotdir(dir)       : set screenshot directory to(dir)
        //    --gfx(plugin-spec)    : use gfx plugin given by(plugin-spec)
        //    --audio(plugin-spec)  : use audio plugin given by(plugin-spec)
        //    --input(plugin-spec)  : use input plugin given by(plugin-spec)
        //    --rsp(plugin-spec)    : use rsp plugin given by(plugin-spec)
        //    --emumode(mode)       : set emu mode to: 0=Pure Interpreter 1=Interpreter 2=DynaRec
        //    --testshots(list)     : take screenshots at frames given in comma-separated(list), then quit
        //    --set(param-spec)     : set a configuration variable, format: ParamSection[ParamName]=Value
        //    --gb-rom-{1,2,3,4}    : define GB cart rom to load inside transferpak {1,2,3,4}"
        //    --gb-ram-{1,2,3,4}    : define GB cart ram to load inside transferpak {1,2,3,4}"
        //    --core-compare-send   : use the Core Comparison debugging feature, in data sending mode
        //    --core-compare-recv   : use the Core Comparison debugging feature, in data receiving mode
        //    --nosaveoptions       : do not save the given command-line options in configuration file
        //    --verbose             : print lots of information
        //    --help                : see this help message

        //(plugin-spec):
        //    (pluginname)          : filename (without path) of plugin to find in plugin directory
        //    (pluginpath)          : full path and filename of plugin
        //    'dummy'               : use dummy plugin

        //(cheat-spec):
        //    'list'                : show all of the available cheat codes
        //    'all'                 : enable all of the available cheat codes
        //    (codelist)            : a comma-separated list of cheat code numbers to enable,
        //                            with dashes to use code variables (ex 1-2 to use cheat 1 option 2)

        // System
        public static string appRootDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + @"\"; // exe directory
        public static string commonProgramFilesDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles).TrimEnd('\\') + @"\";
        public static string commonProgramFilesX86Dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86).TrimEnd('\\') + @"\";
        public static string programFilesDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).TrimEnd('\\') + @"\";
        public static string programFilesX86Dir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).TrimEnd('\\') + @"\";
        public static string programFilesX64Dir = @"C:\Program Files\";
        public static string programDataDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData).TrimEnd('\\') + @"\";
        public static string appDataLocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).TrimEnd('\\') + @"\";
        public static string appDataRoamingDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).TrimEnd('\\') + @"\";
        public static string tempDir = System.IO.Path.GetTempPath().TrimEnd('\\') + @"\"; // AppData Temp Directory

        // User
        public static string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).TrimEnd('\\') + @"\"; // C:\Users\User1\

        // Config
        public static string ultraConfDir = appDataRoamingDir + @"Ultra UI\";
        public static string ultraConfFile = Path.Combine(ultraConfDir, "ultra.conf");
        //public static string mupenCfgFile;

        // Mupen64Plus
        public static string mupen64plusExe;
        public static string mupen64plusDll;

        // Theme
        public static string theme { get; set; }

        /// <summary>
        /// Main Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            MinWidth = 480;
            MinHeight = 500;

            // -------------------------
            // Set Current Version to Assembly Version
            // -------------------------
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string assemblyVersion = fvi.FileVersion;
            MainViewModel.currentVersion = new Version(assemblyVersion);

            // -------------------------
            // Title + Version
            // -------------------------
            VM.MainView.TitleVersion = "Ultra UI ~ Mupen64Plus (" + Convert.ToString(MainViewModel.currentVersion) + "-" + MainViewModel.currentBuildPhase + ")";

            // -------------------------
            // Set Paths
            // -------------------------
            // Mupen64Plus Folder
            // Folder with mupen64plus.dll (Hardcoded for now, can't change location)
            //VM.PathsView.Mupen_Text = appRootDir;

            // Config Folder
            // Mupen Config File mupen64plus.cfg (Hardcoded for now, can't change location)
            VM.PathsView.Config_Text = appDataRoamingDir + @"Mupen64Plus\";

            // Plugins Folder
            //VM.PathsView.Plugins_Text = appRootDir;

            // -------------------------
            // ultra.conf actions to read
            // -------------------------
            List<Action> actionsToRead = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // Main Window
                    // -------------------------
                    // Window Position Top
                    double top;
                    double.TryParse(Configure.ConigFile.conf.Read("Main Window", "Window_Position_Top"), out top);
                    this.Top = top;

                    // Window Position Left
                    double left;
                    double.TryParse(Configure.ConigFile.conf.Read("Main Window", "Window_Position_Left"), out left);
                    this.Left = left;

                    // Center
                    if (top == 0 && left == 0)
                    {
                        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    }

                    // Window Maximized
                    bool mainwindow_WindowState_Maximized;
                    bool.TryParse(Configure.ConigFile.conf.Read("Main Window", "WindowState_Maximized").ToLower(), out mainwindow_WindowState_Maximized);
                    if (mainwindow_WindowState_Maximized == true)
                    {
                        //VM.MainView.Window_State = WindowState.Maximized;
                        this.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        //VM.MainView.Window_State = WindowState.Normal;
                        this.WindowState = WindowState.Normal;
                    }

                    // Window Width
                    double width;
                    double.TryParse(Configure.ConigFile.conf.Read("Main Window", "Window_Width"), out width);
                    this.Width = width;

                    // Window Height
                    double height;
                    double.TryParse(Configure.ConigFile.conf.Read("Main Window", "Window_Height"), out height);
                    this.Height = height;

                    // -------------------------
                    // Settings
                    // -------------------------
                    theme = Configure.ConigFile.conf.Read("Settings", "Theme");

                    // -------------------------
                    // Paths
                    // -------------------------

                    // Mupen64plus Path
                    VM.PathsView.Mupen_Text = Configure.ConigFile.conf.Read("Paths", "Mupen64Plus");

                    // Config Path
                    VM.PathsView.Config_Text = Configure.ConigFile.conf.Read("Paths", "Config");

                    // Plugins Path is loaded from mupen64plus.cfg

                    // Data Path
                    //VM.PathsView.Data_Text = Configure.ConigFile.conf.Read("Paths", "Data"); //disabled for now

                    // ROMs Path
                    VM.PathsView.ROMs_Text = Configure.ConigFile.conf.Read("Paths", "ROMs");
                }),
            };

            // -------------------------
            // Read ultra.conf
            // -------------------------
            if (File.Exists(ultraConfFile))
            {
                //Configure.ReadUltraConf(this);

                Configure.ReadUltraConf(ultraConfDir,  // Directory: %AppData%\Ultra UI\
                                        "ultra.conf",  // Filename
                                        actionsToRead  // Actions to read
                                       );
            }

            // -------------------------
            // Theme
            // -------------------------
            SetTheme();
        }

        /// <summary>
        /// Set Theme
        /// </summary>
        public void SetTheme()
        {
            // Change Theme Resource

            // Ultra
            if (theme == "Ultra")
            {
                App.Current.Resources.MergedDictionaries.Clear();
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri("Themes/Ultra.xaml", UriKind.RelativeOrAbsolute)
                });
            }
            // N64
            else if (theme == "N64")
            {
                App.Current.Resources.MergedDictionaries.Clear();
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri("Themes/N64.xaml", UriKind.RelativeOrAbsolute)
                });
            }
            // Default
            else
            {
                theme = "Ultra";

                App.Current.Resources.MergedDictionaries.Clear();
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri("Themes/Ultra.xaml", UriKind.RelativeOrAbsolute)
                });
            }
        }

        /// <summary>
        ///    Window Loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;

            // --------------------------------------------------
            // Event Handlers
            // --------------------------------------------------
            // Attach SelectionChanged Handlers
            // Prevent Bound ComboBox from firing SelectionChanged Event at application startup

            // -------------------------
            // Emulator
            // -------------------------
            // CPU
            cboCPU.SelectionChanged += cboCPU_SelectionChanged;
            // PureInterpreter
            //cbxPureInterpreter.Checked += cbxPureInterpreter_Checked;
            // Cycles
            cboEmulator_Cycles.SelectionChanged += cboEmulator_Cycles_SelectionChanged;
            // Cached Interpreter
            //cbxCachedInterpreter.Checked += cbxCachedInterpreter_Checked;
            //cbxCachedInterpreter.Unchecked += cbxCachedInterpreter_Checked;
            // DynamicRecompiler
            //cbxDynamicRecompiler.Checked += cbxDynamicRecompiler_Checked;
            //cbxDynamicRecompiler.Unchecked += cbxDynamicRecompiler_Checked;

            // DisableSpecRecomp
            cbxDisableSpecRecomp.Checked += cbxDisableSpecRecomp_Checked;
            cbxDisableSpecRecomp.Unchecked += cbxDisableSpecRecomp_Checked;
            // RandomizeInterrupt
            cbxRandomizeInterrupt.Checked += cbxRandomizeInterrupt_Checked;
            cbxRandomizeInterrupt.Unchecked += cbxRandomizeInterrupt_Checked;
            // NoCompiledJump
            cbxNoCompiledJump.Checked += cbxNoCompiledJump_Checked;
            cbxNoCompiledJump.Unchecked += cbxNoCompiledJump_Checked;
            // DisableExtraMemory
            cbxDisableExtraMemory.Checked += cbxDisableExtraMemory_Checked;
            cbxDisableExtraMemory.Unchecked += cbxDisableExtraMemory_Checked;
            // DelaySI
            cbxDelaySI.Checked += cbxDelaySI_Checked;
            cbxDelaySI.Unchecked += cbxDelaySI_Checked;

            // -------------------------
            // Display
            // -------------------------
            // View
            cboView.SelectionChanged += cboView_SelectionChanged;
            // Resolution
            cboResolution.SelectionChanged += cboResolution_SelectionChanged;
            // Vsync
            cbxVsync.Checked += cbxVsync_Checked;
            cbxVsync.Unchecked += cbxVsync_Checked;
            // OSD
            cbxOSD.Checked += cbxOSD_Checked;
            cbxOSD.Unchecked += cbxOSD_Checked;
            // Screensaver
            cbxScreensaver.Checked += cbxScreensaver_Checked;
            cbxScreensaver.Unchecked += cbxScreensaver_Checked;

            // -------------------------
            // Plugin
            // -------------------------
            // Video
            cboPlugin_Video.SelectionChanged += cboPlugin_Video_SelectionChanged;
            // Audio
            cboPlugin_Audio.SelectionChanged += cboPlugin_Audio_SelectionChanged;
            // Input
            cboPlugin_Input.SelectionChanged += cboPlugin_Input_SelectionChanged;
            // RSP
            cboPlugin_RSP.SelectionChanged += cboPlugin_RSP_SelectionChanged;

            // -------------------------
            // Read mupen64plus.cfg
            // -------------------------
            MupenCfg.ReadMupen64PlusCfg();

            // -------------------------
            // Scan Plugins
            // -------------------------
            Parse.ScanPlugins();

            // -------------------------
            // Load Plugins
            // -------------------------
            MupenCfg.LoadPlugins();

            // -------------------------
            // Parse ROMs List
            // -------------------------
            Parse.ParseGamesList();

            // -------------------------
            // utlra.conf initialize
            // Create a default config file to be populated
            // -------------------------
            // Put this here so when the games list is written it will not be above the Main Window section

            // Create only if file does not already exist
            if (!File.Exists(ultraConfFile))
            {
                // ultra.conf actions to write
                List<Action> actionsToWrite = new List<Action>
                {
                    new Action(() =>
                    {
                        // -------------------------
                        // Main Window
                        // -------------------------
                        // Window Position Top
                        Configure.ConigFile.conf.Write("Main Window", "Window_Position_Top", this.Top.ToString());
                        // Window Position Left
                        Configure.ConigFile.conf.Write("Main Window", "Window_Position_Left", this.Left.ToString());
                        // Window Width
                        Configure.ConigFile.conf.Write("Main Window", "Window_Width", this.Width.ToString());
                        // Window Height
                        Configure.ConigFile.conf.Write("Main Window", "Window_Height", this.Height.ToString());
                        // Window Maximized
                        Configure.ConigFile.conf.Write("Main Window", "WindowState_Maximized", "false");

                        // -------------------------
                        // Settings
                        // -------------------------
                        Configure.ConigFile.conf.Write("Settings", "Theme", theme);

                        // -------------------------
                        // Paths (Default empty. Do not add trailing slash to empty.)
                        // -------------------------
                        // Mupen64Plus
                        Configure.ConigFile.conf.Write("Paths", "Mupen64Plus", VM.PathsView.Mupen_Text);

                        // Config
                        Configure.ConigFile.conf.Write("Paths", "Config", VM.PathsView.Config_Text);

                        // Plugins
                        // This is Read/Saved to mupen64plus.cfg, not ultra.conf.

                        // Data
                        Configure.ConigFile.conf.Write("Paths", "Data", VM.PathsView.Data_Text);

                        // ROMs
                        Configure.ConigFile.conf.Write("Paths", "ROMs", VM.PathsView.ROMs_Text);
                    }),
                };

                // -------------------------
                // Save ultra.conf
                // -------------------------
                Configure.WriteUltraConf(ultraConfDir,  // Directory: %AppData%\Ultra UI\
                                         "ultra.conf",  // Filename
                                         actionsToWrite // Actions to write
                                         );
            }
        }


        /// <summary>
        /// On Closed (Method)
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            // Force Exit All Executables
            base.OnClosed(e);
            System.Windows.Forms.Application.ExitThread();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Window Closing (Method)
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
            // Mupen64Plus is running
            if (Mupen64PlusAPI.api != null)
            {
                // Yes/No Dialog Confirmation
                //
                MessageBoxResult resultExport = MessageBox.Show("Exit Ultra and quit emulation?",
                                                                "Exit",
                                                                MessageBoxButton.YesNo,
                                                                MessageBoxImage.Question);
                switch (resultExport)
                {
                    // Quit
                    case MessageBoxResult.Yes:
                        // Exit
                        SaveConfOnExit();
                        Mupen64PlusAPI.api.Stop();
                        System.Windows.Forms.Application.ExitThread();
                        Environment.Exit(0);
                        break;
                    // Cancel
                    case MessageBoxResult.No:
                        e.Cancel = true;
                        break;
                }
            }
            // Mupen64Plus is not running
            else if (Mupen64PlusAPI.api == null)
            {
                // Exit
                SaveConfOnExit();
                System.Windows.Forms.Application.ExitThread();
                Environment.Exit(0);
            }
            // Unknown
            else
            {
                // Exit
                SaveConfOnExit();
                System.Windows.Forms.Application.ExitThread();
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Save Conf on Exit (Method)
        /// </summary>
        /// <remarks>
        /// Saves ultra.conf
        /// </remarks>
        public void SaveConfOnExit()
        {
            // -------------------------
            // Save ultra.conf
            // -------------------------
            // do not use VM.PathsView.Config_Text, ultra.conf uses it's own %AppData% folder
            //WriteUltraConf(appDataRoamingDir + @"Ultra UI\");
            Configure.ConigFile conf = null;

            double top = Top;
            double left = Left;
            double width = this.Width;
            double height = this.Height;
            string selectedTheme = string.Empty;

            string pathsMupen64Plus = string.Empty;
            string pathsConfig = string.Empty;
            string pathsROMs = string.Empty;

            try
            {
                conf = new Configure.ConigFile(ultraConfFile);

                // -------------------------
                // Window
                // -------------------------
                //double top = 0;
                double.TryParse(conf.Read("Main Window", "Window_Position_Top"), out top);
                //double left;
                double.TryParse(conf.Read("Main Window", "Window_Position_Left"), out left);
                //double width;
                double.TryParse(conf.Read("Main Window", "Window_Width"), out width);
                //double height;
                double.TryParse(conf.Read("Main Window", "Window_Height"), out height);

                // -------------------------
                // Settings
                // -------------------------
                selectedTheme = conf.Read("Settings", "Theme");

                // -------------------------
                // Paths
                // -------------------------
                // Mupen64plus
                pathsMupen64Plus = conf.Read("Paths", "Mupen64Plus");
                // Config
                pathsConfig = conf.Read("Paths", "Config");
                // Plugins Path is loaded from mupen64plus.cfg
                //// Data Path - Disabled for now
                //VM.PathsView.Data_Text = conf.Read("Paths", "Data");
                // ROMs Path
                pathsROMs = conf.Read("Paths", "ROMs");
            }
            catch
            {
                MessageBox.Show("Could not read ultra.conf on exit.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            // -------------------------
            // Save only if changes have been made
            // -------------------------
            if (// Main Window
                this.Top != top ||
                this.Left != left ||
                this.Width != width ||
                this.Height != height ||

                theme != selectedTheme ||

                VM.PathsView.Mupen_Text != pathsMupen64Plus ||
                VM.PathsView.Config_Text != pathsConfig ||
                //VM.PathsView.Data_Text != pathsMupen64Plus || // disabled for now
                VM.PathsView.ROMs_Text != pathsROMs
                )
            {
                // -------------------------
                // ultra.conf actions to write
                // -------------------------
                List<Action> actionsToWrite = new List<Action>
                {
                    // -------------------------
                    // Main Window
                    // -------------------------
                    new Action(() =>
                    {
                        // -------------------------
                        // Main Window
                        // -------------------------
                        // Window Position Top
                        Configure.ConigFile.conf.Write("Main Window", "Window_Position_Top", this.Top.ToString());
                        // Window Position Left
                        Configure.ConigFile.conf.Write("Main Window", "Window_Position_Left", this.Left.ToString());
                        // Window Width
                        Configure.ConigFile.conf.Write("Main Window", "Window_Width", this.Width.ToString());
                        // Window Height
                        Configure.ConigFile.conf.Write("Main Window", "Window_Height", this.Height.ToString());
                        // Window Maximized
                        if (this.WindowState == WindowState.Maximized)
                        {
                            Configure.ConigFile.conf.Write("Main Window", "WindowState_Maximized", "true");
                        }
                        else
                        {
                            Configure.ConigFile.conf.Write("Main Window", "WindowState_Maximized", "false");
                        }

                        // -------------------------
                        // Settings
                        // -------------------------
                        Configure.ConigFile.conf.Write("Settings", "Theme", theme);

                        // -------------------------
                        // Mupen64Plus Path
                        // -------------------------
                        // Contains mupen64plus.dll
                        if (IsValidPath(VM.PathsView.Mupen_Text))
                        {
                            Configure.ConigFile.conf.Write("Paths", "Mupen64Plus", VM.PathsView.Mupen_Text);
                        }
                        else
                        {
                            Configure.ConigFile.conf.Write("Paths", "Mupen64Plus", "");
                        }

                        // -------------------------
                        // Config Path
                        // -------------------------
                        // Contains mupen64plus.cfg
                        if (IsValidPath(VM.PathsView.Config_Text))
                        {
                            Configure.ConigFile.conf.Write("Paths", "Config", VM.PathsView.Config_Text.TrimEnd('\\') + @"\");
                        }
                        else
                        {
                            Configure.ConigFile.conf.Write("Paths", "Config", "");
                        }

                        // -------------------------
                        // Plugins
                        // -------------------------
                        // This is Read/Saved to mupen64plus.cfg, not ultra.conf.

                        // -------------------------
                        // Data Path
                        // -------------------------
                        //if (IsValidPath(VM.PathsView.Data_Text))
                        //{
                        //    Configure.ConigFile.conf.Write("Paths", "Data", VM.PathsView.Data_Text.TrimEnd('\\') + @"\");
                        //}
                        //else
                        //{
                        //    Configure.ConigFile.conf.Write("Paths", "Data", "");
                        //}

                        // -------------------------
                        // ROMs Path
                        // -------------------------
                        if (IsValidPath(VM.PathsView.ROMs_Text))
                        {
                            Configure.ConigFile.conf.Write("Paths", "ROMs", VM.PathsView.ROMs_Text.TrimEnd('\\') + @"\");
                        }
                        else
                        {
                            Configure.ConigFile.conf.Write("Paths", "ROMs", "");
                        }
                    }),

                    //new Action(() => { ; }),
                };

                // -------------------------
                // Save Config
                // -------------------------
                Configure.WriteUltraConf(ultraConfDir,     // Directory: %AppData%\Ultra UI\
                                         "ultra.conf",  // Filename
                                         actionsToWrite // Actions to write
                                        );
                //Configure.WriteUltraConf(this/*, ultraConfFile*/);
            }
            //}
            //catch
            //{
            //    MessageBox.Show("Could not create ultra.conf",
            //                    "Error",
            //                    MessageBoxButton.OK,
            //                    MessageBoxImage.Error);
            //}

            // -------------------------
            // Save Plugins Path to mupen64plus.cfg
            // Only if mupen64plus.cfg already exist
            // -------------------------
            if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
            {
                //Configure.ConigFile cfg = null;

                // -------------------------
                // mupen64plus.cfg actions to write
                // -------------------------
                List<Action> actionsToWrite = new List<Action>
                {
                    // Plugins Path
                    new Action(() => { Configure.ConigFile.cfg.Write("UI-Console", "PluginDir", "\"" + VM.PathsView.Plugins_Text.TrimEnd('\\') + @"\" + "\"" ); }),
                };

                // Write Actions
                MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                              "mupen64plus.cfg",       // Filename
                                              actionsToWrite           // Actions to write
                                             );

                //MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text);
            }
        }


        /// <summary>
        ///    Is Valid Windows Path
        /// </summary>
        /// <remarks>
        ///     Check for Invalid Characters
        /// </remarks>
        public static bool IsValidPath(string path)
        {
            if (!string.IsNullOrEmpty(path) &&
                !string.IsNullOrWhiteSpace(path))
            {
                // Not Valid
                string invalidChars = new string(Path.GetInvalidPathChars());
                Regex regex = new Regex("[" + Regex.Escape(invalidChars) + "]");

                if (regex.IsMatch(path)) { return false; };
            }

            // Empty
            else
            {
                return false;
            }

            // Is Valid
            return true;
        }


        /// <summary>
        /// Folder Write Access Check (Method)
        /// </summary>
        public static bool hasWriteAccessToFolder(string path)
        {
            try
            {
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(path);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }


        /// <summary>
        ///    File Renamer (Method)
        /// </summary>
        public static String SaveFileRenamer(string directory, string fileName, string ext)
        {
            string output = Path.Combine(directory, fileName + ext);

            string outputNewFileName = string.Empty;

            int count = 1;

            if (File.Exists(output))
            {
                while (File.Exists(output))
                {
                    outputNewFileName = string.Format("{0}({1})", fileName + " ", count++);
                    output = Path.Combine(directory, outputNewFileName + ext);
                }
            }
            else
            {
                // stay default
                outputNewFileName = fileName;
            }

            return outputNewFileName;
        }

        /// <summary>
        ///    Menu Item - Update
        /// </summary>
        public static UpdateWindow updatewindow;
        private Boolean IsUpdateWindowOpened = false;
        private void Update_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // -------------------------
            // Proceed if Internet Connection
            // -------------------------
            if (UpdateWindow.CheckForInternetConnection() == true)
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                WebClient wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.UserAgent, "Ultra (https://github.com/MattMcManis/Ultra) " + " v" + MainViewModel.currentVersion + "-" + MainViewModel.currentBuildPhase + " Update Check");
                wc.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                wc.Headers.Add("accept-language", "en-US,en;q=0.9");
                wc.Headers.Add("dnt", "1");
                wc.Headers.Add("upgrade-insecure-requests", "1");
                //wc.Headers.Add("accept-encoding", "gzip, deflate, br"); //error

                // -------------------------
                // Parse GitHub .version file
                // -------------------------
                string parseLatestVersion = string.Empty;

                try
                {
                    parseLatestVersion = wc.DownloadString("https://raw.githubusercontent.com/MattMcManis/Ultra/master/.version");
                }
                catch
                {
                    MessageBox.Show("GitHub version file not found.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation);

                    return;
                }

                // -------------------------
                // Split Version & Build Phase by dash
                // -------------------------
                if (!string.IsNullOrEmpty(parseLatestVersion) &&
                    !string.IsNullOrWhiteSpace(parseLatestVersion))
                {
                    try
                    {
                        // Split Version and Build Phase
                        MainViewModel.splitVersionBuildPhase = Convert.ToString(parseLatestVersion).Split('-');

                        // Set Version Number
                        MainViewModel.latestVersion = new Version(MainViewModel.splitVersionBuildPhase[0]); //number
                        MainViewModel.latestBuildPhase = MainViewModel.splitVersionBuildPhase[1]; //alpha
                    }
                    catch
                    {
                        MessageBox.Show("Problem reading version.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);

                        return;
                    }

                    // Debug
                    //MessageBox.Show(Convert.ToString(latestVersion));
                    //MessageBox.Show(latestBuildPhase);

                    // -------------------------
                    // Check if Ultra is the Latest Version
                    // -------------------------
                    // Update Available
                    if (MainViewModel.latestVersion > MainViewModel.currentVersion)
                    {
                        // Yes/No Dialog Confirmation
                        //
                        MessageBoxResult result = MessageBox.Show("v" + Convert.ToString(MainViewModel.latestVersion) + "-" + MainViewModel.latestBuildPhase + "\n\nDownload Update?",
                                                                  "Update Available",
                                                                  MessageBoxButton.YesNo
                                                                  );
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                // Check if Window is already open
                                if (IsUpdateWindowOpened) return;

                                // Start Window
                                updatewindow = new UpdateWindow();

                                // Keep in Front
                                updatewindow.Owner = Window.GetWindow(this);

                                // Only allow 1 Window instance
                                updatewindow.ContentRendered += delegate { IsUpdateWindowOpened = true; };
                                updatewindow.Closed += delegate { IsUpdateWindowOpened = false; };

                                // Position Relative to MainWindow
                                // Keep from going off screen
                                updatewindow.Left = Math.Max((Left + (Width - updatewindow.Width) / 2), Left);
                                updatewindow.Top = Math.Max((Top + (Height - updatewindow.Height) / 2), Top);

                                // Open Window
                                updatewindow.Show();
                                break;
                            case MessageBoxResult.No:
                                break;
                        }
                    }

                    // Update Not Available
                    //
                    else if (MainViewModel.latestVersion <= MainViewModel.currentVersion)
                    {
                        MessageBox.Show("This version is up to date.",
                                        "Notice",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);

                        return;
                    }

                    // Unknown
                    //
                    else // null
                    {
                        MessageBox.Show("Could not find download. Try updating manually.",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);

                        return;
                    }
                }

                // Version is Null
                //
                else
                {
                    MessageBox.Show("GitHub version file returned empty.",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);

                    return;
                }
            }

            // No Internet Connection
            //
            else
            {
                MessageBox.Show("Could not detect Internet Connection.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }
        }


        /// <summary>
        ///    File Renamer (Method)
        /// </summary>
        public static String CfgFileRenamer(string outputDir, string filename)
        {
            string output = Path.Combine(outputDir, filename + ".cfg.bak");
            string outputNewFileName = string.Empty;

            int count = 1;

            if (File.Exists(output))
            {
                while (File.Exists(output))
                {
                    outputNewFileName = string.Format("{0}({1})", filename + " (" + DateTime.Now.ToString("yyyy-MM-dd h.mmtt") + ")" + " ", count++);
                    output = Path.Combine(outputDir, outputNewFileName + ".cfg.bak");
                }
            }
            else
            {
                // stay default
                outputNewFileName = filename;
            }

            return outputNewFileName;
        }


        /// <summary>
        /// Menu Bar
        /// </summary>
        /// <summary>
        /// Open ROM
        /// </summary>
        private void OpenROM_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Open Select File Window
            Microsoft.Win32.OpenFileDialog selectFile = new Microsoft.Win32.OpenFileDialog();

            // Default Path
            selectFile.InitialDirectory = VM.PathsView.ROMs_Text;
            selectFile.RestoreDirectory = true;

            // File Extension Filter
            selectFile.Filter = "N64 (*.n64,*.v64,*.z64)|*.n64;*.v64;*.z64";

            // Show Dialog Box
            Nullable<bool> result = selectFile.ShowDialog();

            // Process Dialog Box
            if (result == true)
            {
                Game.Play(selectFile.FileName);            
            }
        }

        /// <summary>
        /// Website
        /// </summary>
        private void Website_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Open Ultra UI Website URL in Default Browser
            Process.Start("https://ultraui.github.io");
        }


        /// <summary>
        /// Info Window
        /// </summary>
        public static InfoWindow infoWindow;
        private Boolean IsInfoWindowOpened = false;
        private void Info_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenInfoWindow();
        }
        public void OpenInfoWindow()
        {
            // Check if Window is already open
            if (IsInfoWindowOpened) return;

            // Start Window
            infoWindow = new InfoWindow();

            // Only allow 1 Window instance
            infoWindow.ContentRendered += delegate { IsInfoWindowOpened = true; };
            infoWindow.Closed += delegate { IsInfoWindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (infoWindow.Width > Width)
            {
                infoWindow.Left = Left - ((infoWindow.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                infoWindow.Left = Math.Max((Left + (Width - infoWindow.Width) / 2), Left);
            }

            infoWindow.Top = Math.Max((Top + (Height - infoWindow.Height) / 2), Top);

            // Open Window
            infoWindow.Show();
        }

        /// <summary>
        /// Debug Window
        /// </summary>
        public static DebugWindow debugWindow;
        private Boolean IsDebugWindowOpened = false;
        private void Debug_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenDebugWindow();
        }
        public void OpenDebugWindow()
        {
            // Check if Window is already open
            if (IsDebugWindowOpened) return;

            // Start Window
            debugWindow = new DebugWindow();

            // Only allow 1 Window instance
            debugWindow.ContentRendered += delegate { IsDebugWindowOpened = true; };
            debugWindow.Closed += delegate { IsDebugWindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (debugWindow.Width > Width)
            {
                debugWindow.Left = Left - ((debugWindow.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                debugWindow.Left = Math.Max((Left + (Width - debugWindow.Width) / 2), Left);
            }

            debugWindow.Top = Math.Max((Top + (Height - debugWindow.Height) / 2), Top);

            // Open Window
            debugWindow.Show();
        }


        /// <summary>
        /// Explore Path
        /// </summary>
        public static void ExplorePath(string path)
        {
            if (IsValidPath(path))
            {
                if (Directory.Exists(path))
                {
                    Process.Start("explorer.exe", path);
                }
                else
                {
                    MessageBox.Show(path + " does not exist.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Enable Menu Itema
        /// </summary>
        public static void EnableMenuItems()
        {

        }
        /// <summary>
        /// Disable Menu Itema
        /// </summary>
        public static void DisableMenuItems()
        {

        }

        /// <summary>
        /// Menu Item - Load State
        /// </summary>
        private void LoadState_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.LoadState();
            }
        }

        /// <summary>
        /// Menu Item - Save State
        /// </summary>
        private void SaveState_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.SaveState();
            }
        }

        /// <summary>
        /// Menu Item - Load Save State File
        /// </summary>
        private void LoadStateFile_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                // Open Select File Window
                Microsoft.Win32.OpenFileDialog selectFile = new Microsoft.Win32.OpenFileDialog();

                //File Extension Filter
                selectFile.InitialDirectory = VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + @"save\";
                selectFile.Filter = "M64P (*.m64p)|*.m64p|PJ64 (*.pj)|*.pj|PJ64 Compressed (*.zip)|*.zip|State (*.st*)|*.st*";

                // Show Dialog Box
                Nullable<bool> result = selectFile.ShowDialog();

                // Default Directory
                if (Directory.Exists(VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + @"save\"))
                {
                    selectFile.InitialDirectory = VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + @"save\";
                }

                // Process Dialog Box
                if (result == true)
                {
                    if (File.Exists(selectFile.FileName))
                    {
                        Mupen64PlusAPI.api.LoadStateFile(selectFile.FileName);
                    }
                }
            }
        }

        /// <summary>
        /// Menu Item - Save Save State File
        /// </summary>
        private void SaveStateFile_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                // -------------------------
                // Check if Config Path is empty
                // -------------------------
                if (IsValidPath(VM.PathsView.Config_Text))
                {
                    // Default Save Directory
                    string saveDir = VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + @"save\";

                    // -------------------------
                    // If Save Directory does not exist, create it
                    // -------------------------
                    if (!Directory.Exists(saveDir))
                    {
                        if (hasWriteAccessToFolder(Path.GetDirectoryName(saveDir)) == true)
                        {
                            Directory.CreateDirectory(saveDir);
                        }
                    }

                    // -------------------------
                    // Open 'Save File'
                    // -------------------------
                    Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();

                    // -------------------------
                    // 'Save File' Default Path same as Input Directory
                    // -------------------------
                    string initialDirectory = string.Empty;
                    if (Directory.Exists(saveDir))
                    {
                        saveFile.InitialDirectory = saveDir;
                        initialDirectory = saveDir;
                    }

                    // -------------------------
                    // Defaults
                    // -------------------------
                    // Remember Last Dir
                    //saveFile.RestoreDirectory = true;
                    saveFile.Filter = "M64P (*.m64p)|*.m64p|PJ64 (*.pj)|*.pj|PJ64 Compressed (*.zip)|*.zip";
                    string ext = ".m64p";
                    saveFile.DefaultExt = ext;

                    // -------------------------
                    // File Name
                    // -------------------------
                    if (Mupen64PlusAPI.api != null)
                    {
                        saveFile.FileName = System.Text.Encoding.Default.GetString(Mupen64PlusAPI.api._rom_header.Name);

                        //saveFile.FileName = new string(Mupen64PlusAPI.api._rom_settings.goodname);
                    }
                    else
                    {
                        saveFile.FileName = "save";
                    }

                    // -------------------------
                    // Show Dialog Box
                    // -------------------------
                    Nullable<bool> result = saveFile.ShowDialog();

                    // Process Dialog Box
                    if (result == true)
                    {
                        // Access
                        if (hasWriteAccessToFolder(Path.GetDirectoryName(saveFile.FileName)) == true)
                        {
                            Mupen64PlusAPI.api.SaveStateFile(saveFile.FileName);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Menu Item - Screenshot
        /// </summary>
        private void Screenshot_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.Screenshot();
            }
        }

        /// <summary>
        /// Menu Item - Exit
        /// </summary>
        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Force Exit All Executables
            base.OnClosed(e);
            System.Windows.Forms.Application.ExitThread();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Menu Item - Pause
        /// </summary>
        private void Pause_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.Pause();
            }
        }

        /// <summary>
        /// Menu Item - Mute
        /// </summary>
        private void Mute_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.Mute();
            }
        }

        /// <summary>
        /// Menu Item - Stop
        /// </summary>
        //[SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        //private void KillTheThread()
        //{
        //    Game.m64pEmulator.Abort();
        //}
        public static bool stopped = false;
        private void Stop_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                stopped = true;
                Mupen64PlusAPI.api.Stop(); // Calls Dispose()
                GC.Collect();
            }
        }

        /// <summary>
        /// Menu Item - Reset
        /// </summary>
        private void Reset_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // API reset crashes, do it manually
            if (Mupen64PlusAPI.api != null)
            {
                stopped = true;
                Mupen64PlusAPI.api.Stop();
                GC.Collect();
            }

            PlayButton();

            //if (Mupen64PlusAPI.api != null)
            //{
            //    Mupen64PlusAPI.api.HardReset();
            //}
        }

        /// <summary>
        /// Menu Item - Soft Reset
        /// </summary>
        private void SoftReset_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.SoftReset();
            }
        }

        /// <summary>
        /// Menu Item - Slow Down
        /// </summary>
        private void SlowDown_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.SlowDown();
            }
        }

        /// <summary>
        /// Menu Item - Speed Up
        /// </summary>
        private void SpeedUp_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.SpeedUp();
            }
        }

        /// <summary>
        /// Menu Item - Cheats
        /// </summary>
        private void Cheats_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                //PressKey("{F2}");
            }
        }

        /// <summary>
        /// Menu Item - Reload Plugins
        /// </summary>
        public void PluginsReload()
        {
            // -------------------------
            // Save Plugins Path
            // -------------------------
            // Creates simple mupen64plus.cfg file that will later be populated when game is run

            // mupen64plus.cfg actions to write
            List<Action> actionsToWrite = new List<Action>
            {
                // Plugins Path
                new Action(() => { Configure.ConigFile.cfg.Write("UI-Console", "PluginDir", "\"" + VM.PathsView.Plugins_Text.TrimEnd('\\') + @"\" + "\""); }),
            };

            // Write Actions
            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );

            // -------------------------
            // Reset Missing missing mupen64plus.cfg Label Notice
            // -------------------------
            if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
            {
                VM.MainView.CfgErrorNotice_Text = "";
            }

            // -------------------------
            // Read mupen64plus.cfg
            // -------------------------
            //MupenCfg.ReadMupen64PlusCfg();

            // -------------------------
            // Plugins
            // -------------------------
            Parse.ScanPlugins();

            // -------------------------
            // Plugins Defaults
            // -------------------------
            PluginDefaults();
        }
        private void btnPluginReload_Click(object sender, RoutedEventArgs e)
        {
            PluginsReload();
        }

        /// <summary>
        /// Generate
        /// </summary>
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            Generate.CfgDefaults(
                Path.Combine(VM.PathsView.Mupen_Text, "m64p_test_rom.v64")
                );
        }

        /// <summary>
        /// Menu Item - Edit mupen64plus.cfg
        /// </summary>
        public static CfgEditorWindow cfgEditorWindow;
        private Boolean IsCfgEditorOpened = false;
        private void EditCfg_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenEditCfgWindow();
        }
        public void OpenEditCfgWindow()
        {
            // Check if Window is already open
            if (IsCfgEditorOpened) return;

            // Start Window
            cfgEditorWindow = new CfgEditorWindow();

            // Only allow 1 Window instance
            cfgEditorWindow.ContentRendered += delegate { IsCfgEditorOpened = true; };
            cfgEditorWindow.Closed += delegate { IsCfgEditorOpened = false; };

            //// Position Relative to MainWindow
            //// MainWindow Smaller
            //if (cfgEditorWindow.Width > Width)
            //{
            //    cfgEditorWindow.Left = Left - ((cfgEditorWindow.Width - Width) / 2);
            //}
            //// MainWindow Larger
            //else
            //{
            //    cfgEditorWindow.Left = Math.Max((Left + (Width - cfgEditorWindow.Width) / 2), Left);
            //}

            //cfgEditorWindow.Top = Math.Max((Top + (Height - cfgEditorWindow.Height) / 2), Top);

            // Open Window
            cfgEditorWindow.Show();
        }


        /// <summary>
        /// Menu Item - Backup mupen64plus.cfg
        /// </summary>
        private void BackupCfg_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Check if mupen64plus.cfg exists
            if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
            {
                try
                {
                    // Rename File mupen64plus (1).cfg.bak
                    string newFile = CfgFileRenamer(VM.PathsView.Config_Text.TrimEnd('\\') + @"\", "mupen64plus") + ".cfg.bak";

                    // Create Backup
                    File.Copy(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"), VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + newFile);

                    //MessageBox.Show(newFile); //debug

                    // Complete
                    if (File.Exists(Path.Combine(VM.PathsView.Config_Text.TrimEnd('\\') + @"\", newFile)))
                    {
                        MessageBox.Show("Backup complete.\n\n" + newFile,
                                        "Notice",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }
                    // Create File Error
                    else
                    {
                        MessageBox.Show("Could not create " + newFile,
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
                // Backup Error
                catch
                {
                    MessageBox.Show("Could not backup mupen64plus.cfg",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            // Cfg Not Found
            else
            {
                MessageBox.Show("Could not find mupen64plus.cfg",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }          
        }

        /// <summary>
        /// Menu Item - Fullscreen
        /// </summary>
        private void Fullscreen_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.Fullscreen();
            }
        }

        /// <summary>
        /// Menu Item - ROMs Folder
        /// </summary>
        private void ROMs_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidPath(VM.PathsView.ROMs_Text))
            {
                ExplorePath(VM.PathsView.ROMs_Text);
            }
        }

        /// <summary>
        /// Menu Item - Ultra Folder
        /// </summary>
        private void UltraFolder_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ExplorePath(appRootDir);
        }

        /// <summary>
        /// Menu Item - Ultra Conf Folder
        /// </summary>
        private void UltraConfFolder_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(ultraConfDir))
            {
                Process.Start("explorer.exe", ultraConfDir);
            }
            else
            {
                MessageBox.Show(ultraConfDir + " does not yet exist.\n\nPlease run restart Ultra.exe to automatically create it.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Menu Item - Mupen Folder
        /// </summary>
        private void MupenFolder_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidPath(VM.PathsView.Mupen_Text))
            {
                ExplorePath(VM.PathsView.Mupen_Text);
            }
        }

        /// <summary>
        /// Menu Item - Mupen Config Folder
        /// </summary>
        private void ConfigFolder_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidPath(VM.PathsView.Config_Text))
            {
                if (Directory.Exists(VM.PathsView.Config_Text))
                {
                    Process.Start("explorer.exe", VM.PathsView.Config_Text);
                }
                else
                {
                    MessageBox.Show(VM.PathsView.Config_Text + " does not yet exist.\n\nPlease run mupen64plus-ui-console.exe or launch a game to automatically create it.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Menu Item - Save Folder
        /// </summary>
        private void Saves_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidPath(VM.PathsView.Config_Text))
            {
                if (Directory.Exists(VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + "save"))
                {
                    Process.Start("explorer.exe", VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + "save");
                }
                else
                {
                    MessageBox.Show(VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + @"save\" + " does not yet exist.\n\nSave while playing a game and it will automatically be created.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Menu Item - Screenshots Folder
        /// </summary>
        private void Screenshots_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidPath(VM.PathsView.Config_Text))
            {
                if (Directory.Exists(VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + "screenshot"))
                {
                    Process.Start("explorer.exe", VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + "screenshot");
                }
                else
                {
                    MessageBox.Show(VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + @"screenshot\" + " does not yet exist.\n\nPress F12 to take a screenshot while playing a game and it will automatically be created.",
                                    "Notice",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Save Slot Uncheck
        /// </summary>
        /// <remarks>
        /// Uncheck all Save Slots but the one selected
        /// </remarks>
        public void SlotCheckUncheck(int slot)
        {
            // 0
            if (slot == 0)
            {
                VM.MainView.StateSlot0_IsChecked = true;
            }
            else
            {
                VM.MainView.StateSlot0_IsChecked = false;
            }

            // 1
            if (slot == 1)
            {
                VM.MainView.StateSlot1_IsChecked = true;
            }
            else
            {
                VM.MainView.StateSlot1_IsChecked = false;
            }

            // 2
            if (slot == 2)
            {
                VM.MainView.StateSlot2_IsChecked = true;
            }
            else
            {
                VM.MainView.StateSlot2_IsChecked = false;
            }

            // 3
            if (slot == 3)
            {
                VM.MainView.StateSlot3_IsChecked = true;
            }
            else
            {
                VM.MainView.StateSlot3_IsChecked = false;
            }

            // 4
            if (slot == 4)
            {
                VM.MainView.StateSlot4_IsChecked = true;
            }
            else
            {
                VM.MainView.StateSlot4_IsChecked = false;
            }

            // 5
            if (slot == 5)
            {
                VM.MainView.StateSlot5_IsChecked = true;
            }
            else
            {
                VM.MainView.StateSlot5_IsChecked = false;
            }

            // 6
            if (slot == 6)
            {
                VM.MainView.StateSlot6_IsChecked = true;
            }
            else
            {
                VM.MainView.StateSlot6_IsChecked = false;
            }

            // 7
            if (slot == 7)
            {
                VM.MainView.StateSlot7_IsChecked = true;
            }
            else
            {
                VM.MainView.StateSlot7_IsChecked = false;
            }

            // 8
            if (slot == 8)
            {
                VM.MainView.StateSlot8_IsChecked = true;
            }
            else
            {
                VM.MainView.StateSlot8_IsChecked = false;
            }

            // 9
            if (slot == 9)
            {
                VM.MainView.StateSlot9_IsChecked = true;
            }
            else
            {
                VM.MainView.StateSlot9_IsChecked = false;
            }
        }

        /// <summary>
        /// Menu Item - State State Slots
        /// </summary>
        private void StateSlot0_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.StateSlot();
            }

            // Stay Checked
            SlotCheckUncheck(0);
        }

        private void StateSlot1_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.StateSlot();
            }

            // Stay Checked
            SlotCheckUncheck(1);
        }
        private void StateSlot2_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.StateSlot();
            }

            // Stay Checked
            SlotCheckUncheck(2);
        }
        private void StateSlot3_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.StateSlot();
            }

            // Stay Checked
            SlotCheckUncheck(3);
        }
        private void StateSlot4_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.StateSlot();
            }

            // Stay Checked
            SlotCheckUncheck(4);
        }
        private void StateSlot5_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.StateSlot();
            }

            // Stay Checked
            SlotCheckUncheck(5);
        }
        private void StateSlot6_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.StateSlot();
            }

            // Stay Checked
            SlotCheckUncheck(6);
        }
        private void StateSlot7_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.StateSlot();
            }

            // Stay Checked
            SlotCheckUncheck(7);
        }
        private void StateSlot8_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.StateSlot();
            }

            // Stay Checked
            SlotCheckUncheck(8);
        }
        private void StateSlot9_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Mupen64PlusAPI.api != null)
            {
                Mupen64PlusAPI.api.StateSlot(); // state 9 not working
            }

            // Stay Checked
            SlotCheckUncheck(9);
        }

        /// <summary>
        /// Video - Menu Item
        /// </summary>
        private void Video_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenVideoConfigureWindow();
        }

        /// <summary>
        /// Audio - Menu Item
        /// </summary>
        private void Audio_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenAudioConfigureWindow();
        }

        /// <summary>
        /// Controls - Menu Item
        /// </summary>
        private void Controls_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenInputConfigureWindow();
        }

        /// <summary>
        /// RSP - Menu Item
        /// </summary>
        private void RSP_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenRSPConfigureWindow();
        }

        /// <summary>
        /// Theme Ultra - Menu Item
        /// </summary>
        private void ThemeUltra_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            theme = "Ultra";

            SetTheme();
        }

        /// <summary>
        /// Theme N64 - Menu Item
        /// </summary>
        private void ThemeN64_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            theme = "N64";

            SetTheme();
        }



        /// <summary>
        /// Games List View
        /// </summary>
        private void listViewGames_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //// Clear before adding new selected items
            //if (VM.MainView.Games_SelectedItems != null &&
            //    VM.MainView.Games_SelectedItems.Count > 0)
            //{
            //    VM.MainView.Games_SelectedItems.Clear();
            //    VM.MainView.Games_SelectedItems.TrimExcess();
            //}
        }


        /// <summary>
        /// Games List View - Double Click
        /// </summary>
        private void listViewGames_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get Selected Item
            int index = 0;
            if (listViewGames.SelectedItems.Count > 0)
            {
                index = listViewGames.Items.IndexOf(listViewGames.SelectedItems[0]);

                // Get ROM Path
                string rom = VM.MainView.Games_Items.Select(item => item.FullPath).ElementAt(index);

                // Play
                Game.Play(rom);
            }
        }


        /// <summary>
        /// Reload Games List
        /// </summary>
        private void btnRebuildList_Click(object sender, RoutedEventArgs e)
        {
            // ROMs List
            Parse.ScanGameFiles();
            Parse.ParseGamesList();
        }


        /// <summary>
        /// Display - View
        /// </summary>
        private void cboView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // Fullscreen
                    if (VM.DisplayView.Display_View_SelectedItem == "Fullscreen")
                    {
                        Configure.ConigFile.cfg.Write("Video-General", "Fullscreen", "True");
                    }
                    // Windowed
                    else if (VM.DisplayView.Display_View_SelectedItem == "Windowed")
                    {
                        Configure.ConigFile.cfg.Write("Video-General", "Fullscreen", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }


        /// <summary>
        /// Resolution
        /// </summary>
        private void cboResolution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Video-General]
                    // -------------------------
                    List<string> splitRes = new List<string>();
                    splitRes = VM.DisplayView.Display_Resolution_SelectedItem.Split('x').ToList();
                    string width = string.Empty;
                    string height = string.Empty;
                    if (splitRes.Count > 1) // null check
                    {
                        width = splitRes[0];
                        height = splitRes[1];
                    }

                    Configure.ConigFile.cfg.Write("Video-General", "ScreenWidth", width);
                    Configure.ConigFile.cfg.Write("Video-General", "ScreenHeight", height);
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }


        /// <summary>
        /// Plugin Video - Button
        /// </summary>
        private void btnPlugin_Video_Configure_Click(object sender, RoutedEventArgs e)
        {
            OpenVideoConfigureWindow();
        }

        /// <summary>
        /// Open Video Configure Window
        /// </summary>
        public void OpenVideoConfigureWindow()
        {
            // -------------------------
            // Open
            // -------------------------
            if (VM.PluginsView.Video_SelectedItem == "mupen64plus-video-GLideN64.dll")
            {
                OpenGLideN64Window();
            }
            // -------------------------
            // Deny
            // -------------------------
            else
            {
                MessageBox.Show("Cannot currently configure " + VM.PluginsView.Video_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        // GLideN64
        public static GLideN64Window gliden64Window;
        private Boolean IsGLideN64WindowOpened = false;
        public void OpenGLideN64Window()
        {
            // Check if Window is already open
            if (IsGLideN64WindowOpened) return;

            // Start Window
            gliden64Window = new GLideN64Window();

            // Only allow 1 Window instance
            gliden64Window.ContentRendered += delegate { IsGLideN64WindowOpened = true; };
            gliden64Window.Closed += delegate { IsGLideN64WindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (gliden64Window.Width > Width)
            {
                gliden64Window.Left = Left - ((gliden64Window.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                gliden64Window.Left = Math.Max((Left + (Width - gliden64Window.Width) / 2), Left);
            }

            gliden64Window.Top = Math.Max((Top + (Height - gliden64Window.Height) / 2), Top);

            // Open Window
            gliden64Window.Show();
        }

        // Glide64mk2
        public static Glide64mk2Window glide64mk2Window;
        private Boolean IsGlide64mk2WindowOpened = false;
        public void OpenGlide64mk2Window()
        {
            // Check if Window is already open
            if (IsGlide64mk2WindowOpened) return;

            // Start Window
            glide64mk2Window = new Glide64mk2Window();

            // Only allow 1 Window instance
            glide64mk2Window.ContentRendered += delegate { IsGlide64mk2WindowOpened = true; };
            glide64mk2Window.Closed += delegate { IsGlide64mk2WindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (glide64mk2Window.Width > Width)
            {
                glide64mk2Window.Left = Left - ((glide64mk2Window.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                glide64mk2Window.Left = Math.Max((Left + (Width - glide64mk2Window.Width) / 2), Left);
            }

            glide64mk2Window.Top = Math.Max((Top + (Height - glide64mk2Window.Height) / 2), Top);

            // Open Window
            glide64mk2Window.Show();
        }

        /// <summary>
        /// Plugin Audio - Button
        /// </summary>
        private void btnPlugin_Audio_Configure_Click(object sender, RoutedEventArgs e)
        {
            OpenAudioConfigureWindow();
        }

        /// <summary>
        /// Open Audio Configure Window
        /// </summary>
        public void OpenAudioConfigureWindow()
        {
            // -------------------------
            // Open
            // -------------------------
            if (VM.PluginsView.Audio_SelectedItem == "mupen64plus-audio-sdl.dll")
            {
                OpenAudioSDLWindow();
            }
            // -------------------------
            // Deny
            // -------------------------
            else
            {
                MessageBox.Show("Cannot currently configure " + VM.PluginsView.Audio_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        // Audio SDL
        public static AudioSDLWindow audioSDLWindow;
        private Boolean IsAudioSDLWindowOpened = false;
        public void OpenAudioSDLWindow()
        {
            // Check if Window is already open
            if (IsAudioSDLWindowOpened) return;

            // Start Window
            audioSDLWindow = new AudioSDLWindow();

            // Only allow 1 Window instance
            audioSDLWindow.ContentRendered += delegate { IsAudioSDLWindowOpened = true; };
            audioSDLWindow.Closed += delegate { IsAudioSDLWindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (audioSDLWindow.Width > Width)
            {
                audioSDLWindow.Left = Left - ((audioSDLWindow.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                audioSDLWindow.Left = Math.Max((Left + (Width - audioSDLWindow.Width) / 2), Left);
            }

            audioSDLWindow.Top = Math.Max((Top + (Height - audioSDLWindow.Height) / 2), Top);

            // Open Window
            audioSDLWindow.Show();
        }

        /// <summary>
        ///    Plugin RSP - Button
        /// </summary>
        private void btnPlugin_RSP_Configure_Click(object sender, RoutedEventArgs e)
        {
            OpenRSPConfigureWindow();
        }

        /// <summary>
        ///    Open RSP Configure Window
        /// </summary>
        public void OpenRSPConfigureWindow()
        {
            // -------------------------
            // Open
            // -------------------------
            // RSP HLE
            if (VM.PluginsView.RSP_SelectedItem == "mupen64plus-rsp-hle.dll")
            {
                OpenRSPHLEWindow();
            }
            // cxd4 SSSE3
            else if (VM.PluginsView.RSP_SelectedItem == "mupen64plus-rsp-cxd4-ssse3.dll")
            {
                OpenRSPcxd4SSSE3Window();
            }
            // -------------------------
            // Deny
            // -------------------------
            else
            {
                MessageBox.Show("Cannot currently configure " + VM.PluginsView.RSP_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// RSPHLE Window
        /// </summary>
        public static RSPHLEWindow rsphleWindow;
        private Boolean IsRSPHLEWindowOpened = false;
        public void OpenRSPHLEWindow()
        {
            // Check if Window is already open
            if (IsRSPHLEWindowOpened) return;

            // Start Window
            rsphleWindow = new RSPHLEWindow();

            // Only allow 1 Window instance
            rsphleWindow.ContentRendered += delegate { IsRSPHLEWindowOpened = true; };
            rsphleWindow.Closed += delegate { IsRSPHLEWindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (rsphleWindow.Width > Width)
            {
                rsphleWindow.Left = Left - ((rsphleWindow.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                rsphleWindow.Left = Math.Max((Left + (Width - rsphleWindow.Width) / 2), Left);
            }

            rsphleWindow.Top = Math.Max((Top + (Height - rsphleWindow.Height) / 2), Top);

            // Open Window
            rsphleWindow.Show();
        }

        /// <summary>
        /// RSP cxd4 SSSE3 Window
        /// </summary>
        public static RSPcxd4SSSE3Window cxd4SSSE3;
        private Boolean IsRSPcxd4SSSE3WindowOpened = false;
        public void OpenRSPcxd4SSSE3Window()
        {
            // Check if Window is already open
            if (IsRSPcxd4SSSE3WindowOpened) return;

            // Start Window
            cxd4SSSE3 = new RSPcxd4SSSE3Window();

            // Only allow 1 Window instance
            cxd4SSSE3.ContentRendered += delegate { IsRSPcxd4SSSE3WindowOpened = true; };
            cxd4SSSE3.Closed += delegate { IsRSPcxd4SSSE3WindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (cxd4SSSE3.Width > Width)
            {
                cxd4SSSE3.Left = Left - ((cxd4SSSE3.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                cxd4SSSE3.Left = Math.Max((Left + (Width - cxd4SSSE3.Width) / 2), Left);
            }

            cxd4SSSE3.Top = Math.Max((Top + (Height - cxd4SSSE3.Height) / 2), Top);

            // Open Window
            cxd4SSSE3.Show();
        }


        /// <summary>
        ///    Plugin Input - Button
        /// </summary>
        private void btnPlugin_Input_Configure_Click(object sender, RoutedEventArgs e)
        {
            OpenInputConfigureWindow();
        }

        /// <summary>
        ///    Open Input Configure Window
        /// </summary>
        public void OpenInputConfigureWindow()
        {
            // -------------------------
            // Open
            // -------------------------
            if (VM.PluginsView.Input_SelectedItem == "mupen64plus-input-sdl.dll")
            {
                OpenInputWindow();
            }
            // -------------------------
            // Deny
            // -------------------------
            else
            {
                MessageBox.Show("Cannot currently configure " + VM.PluginsView.Input_SelectedItem + " at this time.\n\nPlease edit mupen64plus.cfg manually.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        public static InputSDLWindow inputSDLWindow;
        private Boolean IsInputSDLWindowOpened = false;
        public void OpenInputWindow()
        {
            // Check if Window is already open
            if (IsInputSDLWindowOpened) return;

            // Start Window
            inputSDLWindow = new InputSDLWindow();

            // Only allow 1 Window instance
            inputSDLWindow.ContentRendered += delegate { IsInputSDLWindowOpened = true; };
            inputSDLWindow.Closed += delegate { IsInputSDLWindowOpened = false; };

            // Position Relative to MainWindow
            // MainWindow Smaller
            if (inputSDLWindow.Width > Width)
            {
                inputSDLWindow.Left = Left - ((inputSDLWindow.Width - Width) / 2);
            }
            // MainWindow Larger
            else
            {
                inputSDLWindow.Left = Math.Max((Left + (Width - inputSDLWindow.Width) / 2), Left);
            }

            inputSDLWindow.Top = Math.Max((Top + (Height - inputSDLWindow.Height) / 2), Top);

            // Open Window
            inputSDLWindow.Show();
        }


        

        /// <summary>
        /// Play - Button
        /// </summary>
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            PlayButton();
        }
        public void PlayButton()
        {
            // Get Selected Item
            int index = 0;
            if (listViewGames.SelectedItems.Count > 0)
            {
                index = listViewGames.Items.IndexOf(listViewGames.SelectedItems[0]);

                // Get ROM Path
                string rom = VM.MainView.Games_Items.Select(item => item.FullPath).ElementAt(index);

                // Play
                Game.Play(rom);
            }
            // None Selected
            else
            {
                MessageBox.Show("Please select a game from the list.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Set Mupen64Plus Exe
        /// </summary>
        public static string SetMupen64PlusExe()
        {
            // mupen64plus-ui-console.exe
            if (File.Exists(Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus-ui-console.exe")))
            {
                return Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus-ui-console.exe");
            }
            // mupen64plus.exe
            else if (File.Exists(Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus.exe")))
            {
                return Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus.exe");
            }
            // mupen64.exe
            else if (File.Exists(Path.Combine(VM.PathsView.Mupen_Text, "mupen64.exe")))
            {
                return Path.Combine(VM.PathsView.Mupen_Text, "mupen64.exe");
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Set Mupen64Plus dll
        /// </summary>
        public static string SetMupen64PlusDll()
        {
            // mupen64plus-ui-console.exe
            if (File.Exists(Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus.dll")))
            {
                return Path.Combine(VM.PathsView.Mupen_Text, "mupen64plus.dll");
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Path Mupen64Plus - Browse
        /// </summary>
        private void btnBrowsePathMupen_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.PathsView.Mupen_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.PathsView.Mupen_Text = VM.PathsView.Mupen_Text.Replace(@"\\", @"\");
            }

            //// Open Select File Window
            //Microsoft.Win32.OpenFileDialog selectFile = new Microsoft.Win32.OpenFileDialog();

            //// Show Dialog Box
            //Nullable<bool> result = selectFile.ShowDialog();

            //// Process Dialog Box
            //if (result == true)
            //{
            //    if (File.Exists(selectFile.FileName))
            //    {
            //        VM.PathsView.Mupen_Text = selectFile.FileName;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Could not find " + selectFile.FileName,
            //                        "Error",
            //                        MessageBoxButton.OK,
            //                        MessageBoxImage.Error);
            //    }

            //}
        }


        /// <summary>
        /// Path Config - Browse
        /// </summary>
        private void btnBrowsePathConfig_Click(object sender, RoutedEventArgs e)
        {
            // Config Path cannot be changed from %AppData%

            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.PathsView.Config_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.PathsView.Config_Text = VM.PathsView.Config_Text.TrimEnd('\\') + @"\".Replace(@"\\", @"\");
            }
        }

        /// <summary>
        /// Path Plugins
        /// </summary>
        private void btnBrowsePathPlugins_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.PathsView.Plugins_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.PathsView.Plugins_Text = VM.PathsView.Plugins_Text.Replace(@"\\", @"\");
            }
        }

        /// <summary>
        /// Path Data
        /// </summary>
        private void btnBrowsePathData_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.PathsView.Data_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.PathsView.Data_Text = VM.PathsView.Data_Text.Replace(@"\\", @"\");
            }
        }

        /// <summary>
        /// Path ROMs
        /// </summary>
        private void btnBrowsePathROMs_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.PathsView.ROMs_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.PathsView.ROMs_Text = VM.PathsView.ROMs_Text.Replace(@"\\", @"\");
            }
        }

        /// <summary>
        /// Open Mupen64Plus Path
        /// </summary>
        private void btnOpenMupen64PlusExePath_Click(object sender, RoutedEventArgs e)
        {
            //ExplorePath(Path.GetDirectoryName(VM.PathsView.Mupen_Text));
            ExplorePath(VM.PathsView.Mupen_Text);
        }

        /// <summary>
        /// Open Config Path
        /// </summary>
        private void btnOpenConfigPath_Click(object sender, RoutedEventArgs e)
        {
            //ExplorePath(VM.PathsView.Config_Text);

            if (Directory.Exists(VM.PathsView.Config_Text))
            {
                Process.Start("explorer.exe", VM.PathsView.Config_Text);
            }
            else
            {
                MessageBox.Show(VM.PathsView.Config_Text + " does not yet exist.\n\nPlease run mupen64plus-ui-console.exe or launch a game to automatically create it.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Open Plugins Path
        /// </summary>
        private void btnOpenPluginsPath_Click(object sender, RoutedEventArgs e)
        {
            ExplorePath(VM.PathsView.Plugins_Text);
        }

        /// <summary>
        /// Open Data Path
        /// </summary>
        private void btnOpenDataPath_Click(object sender, RoutedEventArgs e)
        {
            ExplorePath(VM.PathsView.Data_Text);
        }

        /// <summary>
        /// Open ROMs Path
        /// </summary>
        private void btnOpenROMsPath_Click(object sender, RoutedEventArgs e)
        {
            ExplorePath(VM.PathsView.ROMs_Text);
        }

        /// <summary>
        /// Defaults all - Button
        /// </summary>
        private void btnPathsDefaultsAll_Click(object sender, RoutedEventArgs e)
        {
            // Paths
            PathDefaults();

            // Plugins
            // Reload Contains PluginsDefaults()
            PluginsReload(); 

            // Emulator
            EmulatorDefaults();

            // Display
            DisplayDefaults();
        }

        /// <summary>
        /// Path Defaults - Button
        /// </summary>
        private void btnPathsDefaults_Click(object sender, RoutedEventArgs e)
        {
            PathDefaults();
        }
        public void PathDefaults()
        {
            //string mupen64plusExeFolder = string.Empty;

            // -------------------------
            // Mupen64Plus Exe Path
            // -------------------------
            // -------------------------
            // Path TextBox is Empty
            // Check if Ultra.exe is in the Mupen64Plus folder 
            // -------------------------
            if (File.Exists(Path.Combine(appRootDir, "mupen64plus-ui-console.exe")) ||
                File.Exists(Path.Combine(appRootDir, "mupen64plus.exe")) ||
                File.Exists(Path.Combine(appRootDir, "mupen64.exe")))
            {
                VM.PathsView.Mupen_Text = appRootDir;
            }
            // -------------------------
            // Exe Not Found
            // -------------------------
            else
            {
                MessageBox.Show("Could not find Mupen64Plus exe.\n\nPlease place Ultra.exe in the Mupen64Plus folder.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }

            // -------------------------
            // Cofig Path
            // -------------------------
            // Hardcoded %AppData%\Mupen64Plus
            VM.PathsView.Config_Text = appDataRoamingDir + @"Mupen64Plus\";

            // -------------------------
            // Plugins Path
            // -------------------------
            if (IsValidPath(VM.PathsView.Mupen_Text))
            {
                // Check for 'plugins' folder
                if (Directory.Exists(VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\" + "plugins"))
                {
                    VM.PathsView.Plugins_Text = VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\" + @"plugins\";
                }
                // Use mupen64plus-ui-console.exe root folder for Plugins
                else
                {
                    VM.PathsView.Plugins_Text = VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\";
                }
            }

            // -------------------------
            // Data
            // -------------------------
            if (IsValidPath(VM.PathsView.Mupen_Text))
            {
                // Check for 'data' folder
                if (Directory.Exists(VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\" + "data"))
                {
                    VM.PathsView.Data_Text = VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\" + @"data\";
                }
                // Use mupen64plus-ui-console.exe root folder for Data
                else
                {
                    VM.PathsView.Data_Text = VM.PathsView.Mupen_Text.TrimEnd('\\') + @"\";
                }
            }
        }


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

            // Pure Interpreter
            if (VM.EmulatorView.CPU_SelectedItem == "Pure Interpreter")
            {
                cpu = "0";
            }
            // Cached Interpreter
            else if (VM.EmulatorView.CPU_SelectedItem == "Cached Interpreter")
            {
                cpu = "1";
            }
            // Dynamic Recompiler
            else if (VM.EmulatorView.CPU_SelectedItem == "Dynamic Recompiler")
            {
                cpu = "2";
            }
            // Safe Default
            else
            {
                cpu = "2";
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


        /// <summary>
        /// Display - Vsync
        /// </summary>
        private void cbxVsync_Checked(object sender, RoutedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Video-General]
                    // -------------------------
                    if (VM.DisplayView.Display_Vsync_IsChecked == true)
                    {
                        Configure.ConigFile.cfg.Write("Video-General", "VerticalSync", "True");
                    }
                    else if (VM.DisplayView.Display_Vsync_IsChecked == false)
                    {
                        Configure.ConigFile.cfg.Write("Video-General", "VerticalSync", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Display - Video Extension
        /// </summary>
        private void cbxVideoExtension_Checked(object sender, RoutedEventArgs e)
        {
            // ???
        }

        /// <summary>
        /// On Screen Display
        /// </summary>
        private void cbxOSD_Checked(object sender, RoutedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    // -------------------------
                    // [Core]
                    // -------------------------
                    if (VM.DisplayView.Display_OSD_IsChecked == true)
                    {
                        Configure.ConigFile.cfg.Write("Core", "OnScreenDisplay", "True");
                    }
                    else if (VM.DisplayView.Display_OSD_IsChecked == false)
                    {
                        Configure.ConigFile.cfg.Write("Core", "OnScreenDisplay", "False");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Disable Screensaver
        /// </summary>
        private void cbxScreensaver_Checked(object sender, RoutedEventArgs e)
        {
            // ???
        }

        /// <summary>
        /// Display Defaults - Button
        /// </summary>
        private void btnDisplayDefaults_Click(object sender, RoutedEventArgs e)
        {
            DisplayDefaults();
        }
        public void DisplayDefaults()
        {
            // Resolution
            VM.DisplayView.Display_Resolution_SelectedItem = "960x720";

            // Vsync
            VM.DisplayView.Display_Vsync_IsChecked = true;

            // Video Extension
            VM.DisplayView.Display_VideoExtension_IsChecked = false;

            // View
            VM.DisplayView.Display_View_SelectedItem = "Windowed";

            // OSD
            VM.DisplayView.Display_OSD_IsChecked = true;

            // Screensaver
            //VM.DisplayView.Display_Screensaver_IsChecked = true;
        }

        /// <summary>
        /// Plugins Defaults - Button
        /// </summary>
        // Button
        private void btnPluginsDefaults_Click(object sender, RoutedEventArgs e)
        {
            PluginDefaults();
        }

        public static void PluginDefaults()
        {
            // -------------------------
            // Video
            // -------------------------
            if (VM.PluginsView.Video_Items != null &&
                VM.PluginsView.Video_Items.Count > 0)
            {
                List<string> videoPluginNames = VM.PluginsView.Video_Items.Select(item => item.Name).ToList();

                // GLideN64
                if (videoPluginNames.Contains("mupen64plus-video-GLideN64.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-GLideN64.dll";
                }
                // Glide64mk2
                else if (videoPluginNames.Contains("mupen64plus-video-glide64mk2.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-glide64mk2.dll";
                }
                // Glide64
                else if (videoPluginNames.Contains("mupen64plus-video-glide64.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-glide64.dll";
                }
                // Rice
                else if (videoPluginNames.Contains("mupen64plus-video-rice.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-rice.dll";
                }
                // z64
                else if (videoPluginNames.Contains("mupen64plus-video-z64.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-z64.dll";
                }
                // Arachnoid
                else if (videoPluginNames.Contains("mupen64plus-video-arachnoid.dll"))
                {
                    VM.PluginsView.Video_SelectedItem = "mupen64plus-video-arachnoid.dll";
                }
                // First
                else
                {
                    VM.PluginsView.Video_SelectedIndex = 0;
                }
            }

            // -------------------------
            // Audio
            // -------------------------
            if (VM.PluginsView.Audio_Items != null &&
                VM.PluginsView.Audio_Items.Count > 0)
            {
                List<string> audioPluginNames = VM.PluginsView.Audio_Items.Select(item => item.Name).ToList();

                // Audio SDL
                if (audioPluginNames.Contains("mupen64plus-audio-sdl.dll"))
                {
                    VM.PluginsView.Audio_SelectedItem = "mupen64plus-audio-sdl.dll";
                }
                // First
                else
                {
                    VM.PluginsView.Audio_SelectedIndex = 0;
                }
            }

            // -------------------------
            // Input
            // -------------------------
            if (VM.PluginsView.Input_Items != null &&
                VM.PluginsView.Input_Items.Count > 0)
            {
                List<string> inputPluginNames = VM.PluginsView.Input_Items.Select(item => item.Name).ToList();

                // Input SDL
                if (inputPluginNames.Contains("mupen64plus-input-sdl.dll"))
                {
                    VM.PluginsView.Input_SelectedItem = "mupen64plus-input-sdl.dll";
                }
                // First
                else
                {
                    VM.PluginsView.Input_SelectedIndex = 0;
                }
            }

            // -------------------------
            // RSP
            // -------------------------
            if (VM.PluginsView.RSP_Items != null &&
                VM.PluginsView.RSP_Items.Count > 0)
            {
                List<string> rspPluginNames = VM.PluginsView.RSP_Items.Select(item => item.Name).ToList();

                // RSP HLE
                if (rspPluginNames.Contains("mupen64plus-rsp-hle.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-hle.dll";
                }
                // cxd4 SSSE3
                else if (rspPluginNames.Contains("mupen64plus-rsp-cxd4-ssse3.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-cxd4-ssse3.dll";
                }
                // cxd4 SSE2
                else if (rspPluginNames.Contains("mupen64plus-rsp-cxd4-sse2.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-cxd4-sse2.dll";
                }
                // cxd4
                else if (rspPluginNames.Contains("mupen64plus-rsp-cxd4.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-cxd4.dll";
                }
                // z64
                else if (rspPluginNames.Contains("mupen64plus-rsp-z64.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-z64.dll";
                }
                // z64 HLE
                else if (rspPluginNames.Contains("mupen64plus-rsp-z64-hlevideo.dll"))
                {
                    VM.PluginsView.RSP_SelectedItem = "mupen64plus-rsp-z64-hlevideo.dll";
                }
                // First
                else
                {
                    VM.PluginsView.RSP_SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Plugin - Video
        /// </summary>
        private void cboPlugin_Video_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    if (!string.IsNullOrEmpty(VM.PluginsView.Video_SelectedItem))
                    {
                        // -------------------------
                        // [UI-Console]
                        // -------------------------
                        Configure.ConigFile.cfg.Write("UI-Console", "VideoPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.Video_SelectedItem)/*VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + VM.PluginsView.Video_SelectedItem*/ + "\"");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Plugin - Audio
        /// </summary>
        private void cboPlugin_Audio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    if (!string.IsNullOrEmpty(VM.PluginsView.Audio_SelectedItem))
                    {
                        // -------------------------
                        // [UI-Console]
                        // -------------------------
                        Configure.ConigFile.cfg.Write("UI-Console", "AudioPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.Audio_SelectedItem)/*VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + VM.PluginsView.Audio_SelectedItem*/ + "\"");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// Input
        /// </summary>
        private void cboPlugin_Input_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    if (!string.IsNullOrEmpty(VM.PluginsView.Input_SelectedItem))
                    {
                        // -------------------------
                        // [UI-Console]
                        // -------------------------
                        Configure.ConigFile.cfg.Write("UI-Console", "InputPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.Input_SelectedItem)/*VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + VM.PluginsView.Audio_SelectedItem*/ + "\"");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

        /// <summary>
        /// RSP
        /// </summary>
        private void cboPlugin_RSP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Write to mupen64plus.cfg
            List<Action> actionsToWrite = new List<Action>
            {
                new Action(() =>
                {
                    if (!string.IsNullOrEmpty(VM.PluginsView.RSP_SelectedItem))
                    {
                        // -------------------------
                        // [UI-Console]
                        // -------------------------
                        Configure.ConigFile.cfg.Write("UI-Console", "RspPlugin", "\"" + Path.Combine(VM.PathsView.Config_Text, VM.PluginsView.RSP_SelectedItem)/*VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + VM.PluginsView.Audio_SelectedItem*/ + "\"");
                    }
                }),
            };

            MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text, // Directory: %AppData%\Mupen64Plus\
                                         "mupen64plus.cfg",        // Filename
                                         actionsToWrite            // Actions to write
                                        );
        }

    }
}
