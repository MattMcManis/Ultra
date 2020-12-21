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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ViewModel;

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
        public readonly static string appRootDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + @"\"; // exe directory
        public readonly static string commonProgramFilesDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles).TrimEnd('\\') + @"\";
        public readonly static string commonProgramFilesX86Dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86).TrimEnd('\\') + @"\";
        public readonly static string programFilesDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).TrimEnd('\\') + @"\";
        public readonly static string programFilesX86Dir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).TrimEnd('\\') + @"\";
        public readonly static string programFilesX64Dir = @"C:\Program Files\";
        public readonly static string programDataDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData).TrimEnd('\\') + @"\";
        public readonly static string appDataLocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).TrimEnd('\\') + @"\";
        public readonly static string appDataRoamingDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).TrimEnd('\\') + @"\";
        public readonly static string tempDir = System.IO.Path.GetTempPath().TrimEnd('\\') + @"\"; // AppData Temp Directory

        // User
        public readonly static string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).TrimEnd('\\') + @"\"; // C:\Users\User1\

        // Config
        public static readonly string ultraConfDir = appDataRoamingDir + @"Ultra UI\";
        public static readonly string ultraConfFile = Path.Combine(ultraConfDir, "ultra.conf");
        //public static string mupenCfgFile;

        // Mupen64Plus
        public static string mupen64plusExe { get; set; }
        public static string mupen64plusDll { get; set; }

        // Theme
        public static string theme { get; set; }

        // Config Read/Write Checks
        // When MainWindow initializes, conf.Read populates these global variables with imported values.
        // When MainWindow exits, conf.Write checks these variables to see if any changes have been made before writing to glow.conf.
        // This prevents writing to glow.conf every time at exit unless necessary.
        private static double top_Read { get; set; }
        private static double left_Read { get; set; }
        private static double width_Read { get; set; }
        private static double height_Read { get; set; }
        //private static bool maximized_Read { get; set; }
        private static string mupen_Text_Read { get; set; }
        private static string config_Text_Read { get; set; }
        private static string roms_Text_Read { get; set; }
        private static string theme_Read { get; set; }
        private static bool updateAutoCheck_IsChecked_Read { get; set; }

        /// <summary>
        /// Main Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            MinWidth = VM.MainView.Window_Width;
            MinHeight = VM.MainView.Window_Height;

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
            VM.MainView.TitleVersion = "Ultra ~ Mupen64Plus (" + Convert.ToString(MainViewModel.currentVersion) + "-" + MainViewModel.currentBuildPhase + ")";

            // --------------------------------------------------
            // Control Defaults
            // --------------------------------------------------
            // Tooltip Duration
            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));

            //SystemEvents.DisplaySettingsChanged +=
            //    new EventHandler(SystemEvents_DisplaySettingsChanged);

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
                    top_Read = top;

                    // Window Position Left
                    double left;
                    double.TryParse(Configure.ConigFile.conf.Read("Main Window", "Window_Position_Left"), out left);
                    this.Left = left;
                    left_Read = left;

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
                    width_Read= width;

                    // Window Height
                    double height;
                    double.TryParse(Configure.ConigFile.conf.Read("Main Window", "Window_Height"), out height);
                    this.Height = height;
                    height_Read = height;

                    // -------------------------
                    // Settings
                    // -------------------------
                    bool updateAutoCheck_IsChecked = true;
                    bool.TryParse(Configure.ConigFile.conf.Read("Settings", "UpdateAutoCheck_IsChecked").ToLower(), out updateAutoCheck_IsChecked);
                    VM.MainView.UpdateAutoCheck_IsChecked = updateAutoCheck_IsChecked;
                    updateAutoCheck_IsChecked_Read = updateAutoCheck_IsChecked;

                    theme = Configure.ConigFile.conf.Read("Settings", "Theme");
                    theme_Read = theme;

                    // -------------------------
                    // Paths
                    // -------------------------
                    // Mupen64plus Path
                    string mupen_Text = Configure.ConigFile.conf.Read("Paths", "Mupen64Plus");
                    if (!string.IsNullOrWhiteSpace(mupen_Text))
                    {
                        VM.PathsView.Mupen_Text = mupen_Text;
                    }
                    mupen_Text_Read = mupen_Text;

                    // Config Path
                    string config_Text = Configure.ConigFile.conf.Read("Paths", "Config");
                    if (!string.IsNullOrWhiteSpace(config_Text))
                    {
                        VM.PathsView.Config_Text = config_Text;
                    }
                    config_Text_Read = config_Text;

                    // Plugins Path is loaded from mupen64plus.cfg

                    // Data Path
                    //VM.PathsView.Data_Text = Configure.ConigFile.conf.Read("Paths", "Data"); //disabled for now

                    // ROMs Path
                    string roms_Text = Configure.ConigFile.conf.Read("Paths", "ROMs");
                    if (!string.IsNullOrWhiteSpace(roms_Text))
                    {
                        VM.PathsView.ROMs_Text = roms_Text;
                    }
                    roms_Text_Read = roms_Text;
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
            // Window Position Center
            // -------------------------
            if ((this.Top.ToString() == "NaN" && this.Left.ToString() == "NaN") ||
                (this.Top == 0 && this.Top == 0)
               )
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            // -------------------------
            // Theme
            // -------------------------
            SetTheme();
        }

        /// <summary>
        /// Display Settings
        /// </summary>
        /// <remarks>
        /// Get the Screen Resolution
        /// </remarks>
        //public static int screenWidth { get; set; }
        //public static int screenHeight { get; set; }
        //public void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        //{
        //    screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        //    screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        //}


        /// <summary>
        /// Set Theme
        /// </summary>
        public void SetTheme()
        {
            // Change Theme Resource

            switch (theme)
            {
                case "Ultra":
                    App.Current.Resources.MergedDictionaries.Clear();
                    App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                    {
                        Source = new Uri("Themes/Ultra.xaml", UriKind.RelativeOrAbsolute)
                    });

                    //VM.MainView.WindowIcon = "Resources/Icons/ultraui.ico";
                    SetThemeIcon(this);
                    break;

                case "N64":
                    App.Current.Resources.MergedDictionaries.Clear();
                    App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                    {
                        Source = new Uri("Themes/N64.xaml", UriKind.RelativeOrAbsolute)
                    });

                    //VM.MainView.WindowIcon = "Resources/Icons/u3d.ico";
                    SetThemeIcon(this);
                    break;

                default:
                    theme = "Ultra";

                    App.Current.Resources.MergedDictionaries.Clear();
                    App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                    {
                        Source = new Uri("Themes/Ultra.xaml", UriKind.RelativeOrAbsolute)
                    });

                    //VM.MainView.WindowIcon = "Resources/Icons/ultraui.ico";
                    SetThemeIcon(this);
                    break;
            }
        }

        public static void SetThemeIcon(Window window)
        {
            switch (MainWindow.theme)
            {
                case "Ultra":
                    Uri iconUri = new Uri("pack://application:,,,/Ultra;component/Resources/Icons/ultraui.ico");
                    window.Icon = BitmapFrame.Create(iconUri);
                    break;

                case "N64":
                    Uri iconUri2 = new Uri("pack://application:,,,/Ultra;component/Resources/Icons/u3d.ico");
                    window.Icon = BitmapFrame.Create(iconUri2);
                    break;
            }
        }

        /// <summary>
        /// Window Loaded
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

            //Task<int> initPlugins = Parse.InitPluginsAsync();
            //int count = await task;
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
                        // Updater
                        Configure.ConigFile.conf.Write("Settings", "UpdateAutoCheck_IsChecked", VM.MainView.UpdateAutoCheck_IsChecked.ToString());

                        // Theme
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

            // -------------------------
            // Check for Available Updates
            // -------------------------
            Task<int> updates = UpdateAvailableCheck();
        }


        /// <summary>
        /// On Closed (Method)
        /// </summary>
        //protected override void OnClosed(EventArgs e)
        //{
        //    // Force Exit All Executables
        //    base.OnClosed(e);
        //    System.Windows.Forms.Application.ExitThread();
        //    Application.Current.Shutdown();
        //}

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
                        Application.Current.Shutdown();
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
                Application.Current.Shutdown();
            }
            // Unknown
            else
            {
                // Exit
                SaveConfOnExit();
                System.Windows.Forms.Application.ExitThread();
                Application.Current.Shutdown();
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
            Configure.ConigFile conf = new Configure.ConigFile(ultraConfFile);

            // -------------------------
            // Save only if changes have been made
            // -------------------------
            if (// Main Window
                this.Top != top_Read ||
                this.Left != left_Read ||
                this.Width != width_Read ||
                this.Height != height_Read ||
                theme != theme_Read ||
                VM.MainView.UpdateAutoCheck_IsChecked != updateAutoCheck_IsChecked_Read ||

                // Paths
                VM.PathsView.Mupen_Text != mupen_Text_Read ||
                VM.PathsView.Config_Text != config_Text_Read ||
                //VM.PathsView.Data_Text != data_Text_Read ||
                VM.PathsView.ROMs_Text != roms_Text_Read
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
                        // Updater
                        Configure.ConigFile.conf.Write("Settings", "UpdateAutoCheck_IsChecked", VM.MainView.UpdateAutoCheck_IsChecked.ToString());
                        // Theme
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
                Configure.WriteUltraConf(ultraConfDir,  // Directory: %AppData%\Ultra UI\
                                         "ultra.conf",  // Filename
                                          actionsToWrite // Actions to write
                                        );

                //MessageBox.Show("Saved"); //debug
            }

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
                                             "mupen64plus.cfg",        // Filename
                                             actionsToWrite            // Actions to write
                                            );

                //MupenCfg.WriteMupen64PlusCfg(VM.PathsView.Config_Text);
            }
        }


        /// <summary>
        /// Is Valid Windows Path
        /// </summary>
        /// <remarks>
        ///  Check for Invalid Characters
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
        /// Save File Renamer (Method)
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
        /// Cfg File Renamer (Method)
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
        public static CheatsWindow cheatsWindow;
        private Boolean IsCheatsOpened = false;
        private void Cheats_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //if (Mupen64PlusAPI.api != null)
            //{
                //PressKey("{F2}");
                //OpenCheatsWindow();
            //}
        }
        public void OpenCheatsWindow()
        {
            // Check if Window is already open
            if (IsCheatsOpened) return;

            // Start Window
            cheatsWindow = new CheatsWindow();

            // Only allow 1 Window instance
            cheatsWindow.ContentRendered += delegate { IsCheatsOpened = true; };
            cheatsWindow.Closed += delegate { IsCheatsOpened = false; };

            // Open Window
            cheatsWindow.Show();
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

    }
}
