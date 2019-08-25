/* ----------------------------------------------------------------------
The MIT License

Copyright (C) 2019 BizHawk Team

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

https://github.com/TASVideos/BizHawk/blob/master/LICENSE
https://github.com/TASVideos/BizHawk/tree/master/BizHawk.Emulation.Cores/Consoles/Nintendo/N64/NativeApi
---------------------------------------------------------------------- */
/* ----------------------------------------------------------------------
Modifications by Matt McManis for Ultra UI
https://github.com/MattMcManis/Ultra
https://ultraui.github.io
mattmcmanis@outlook.com
---------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace Ultra
{
    public class Mupen64PlusAPI : IDisposable
    {
        // API
        public static Mupen64PlusAPI api = null; // mupen64plus DLL Api

        bool disposed = false;

        Thread m64pEmulator;

        AutoResetEvent m64pFrameComplete = new AutoResetEvent(false);
        ManualResetEvent m64pStartupComplete = new ManualResetEvent(false);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GetLastError();

        // Directory other than default root
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        public enum m64p_error
        {
            M64ERR_SUCCESS = 0,
            M64ERR_NOT_INIT,        /* Function is disallowed before InitMupen64Plus() is called */
            M64ERR_ALREADY_INIT,    /* InitMupen64Plus() was called twice */
            M64ERR_INCOMPATIBLE,    /* API versions between components are incompatible */
            M64ERR_INPUT_ASSERT,    /* Invalid parameters for function call, such as ParamValue=NULL for GetCoreParameter() */
            M64ERR_INPUT_INVALID,   /* Invalid input data, such as ParamValue="maybe" for SetCoreParameter() to set a BOOL-type value */
            M64ERR_INPUT_NOT_FOUND, /* The input parameter(s) specified a particular item which was not found */
            M64ERR_NO_MEMORY,       /* Memory allocation failed */
            M64ERR_FILES,           /* Error opening, creating, reading, or writing to a file */
            M64ERR_INTERNAL,        /* Internal error (bug) */
            M64ERR_INVALID_STATE,   /* Current program state does not allow operation */
            M64ERR_PLUGIN_FAIL,     /* A plugin function returned a fatal error */
            M64ERR_SYSTEM_FAIL,     /* A system function call, such as an SDL or file operation, failed */
            M64ERR_UNSUPPORTED,     /* Function call is not supported (ie, core not built with debugger) */
            M64ERR_WRONG_TYPE       /* A given input type parameter cannot be used for desired operation */
        };

        public enum m64p_plugin_type
        {
            M64PLUGIN_NULL = 0,
            M64PLUGIN_RSP = 1,
            M64PLUGIN_GFX,
            M64PLUGIN_AUDIO,
            M64PLUGIN_INPUT,
            M64PLUGIN_CORE
        };

        private enum m64p_command
        {
            M64CMD_NOP = 0,
            M64CMD_ROM_OPEN,
            M64CMD_ROM_CLOSE,
            M64CMD_ROM_GET_HEADER,
            M64CMD_ROM_GET_SETTINGS,
            M64CMD_EXECUTE,
            M64CMD_STOP,
            M64CMD_PAUSE,
            M64CMD_RESUME,
            M64CMD_CORE_STATE_QUERY,
            M64CMD_STATE_LOAD,
            M64CMD_STATE_SAVE,
            M64CMD_STATE_SET_SLOT,
            M64CMD_SEND_SDL_KEYDOWN,
            M64CMD_SEND_SDL_KEYUP,
            M64CMD_SET_FRAME_CALLBACK,
            M64CMD_TAKE_NEXT_SCREENSHOT,
            M64CMD_CORE_STATE_SET,
            M64CMD_READ_SCREEN,
            M64CMD_RESET,
            M64CMD_ADVANCE_FRAME,
            M64CMD_SET_VI_CALLBACK,
            M64CMD_SET_RENDER_CALLBACK
        };

        public enum m64p_core_param
        {
            M64CORE_EMU_STATE = 1,
            M64CORE_VIDEO_MODE,
            M64CORE_SAVESTATE_SLOT,
            M64CORE_SPEED_FACTOR,
            M64CORE_SPEED_LIMITER,
            M64CORE_VIDEO_SIZE,
            M64CORE_AUDIO_VOLUME,
            M64CORE_AUDIO_MUTE,
            M64CORE_INPUT_GAMESHARK,
            M64CORE_STATE_LOADCOMPLETE,
            M64CORE_STATE_SAVECOMPLETE
        };

        public enum m64p_video_mode
        {
            M64VIDEO_NONE = 1,
            M64VIDEO_WINDOWED,
            M64VIDEO_FULLSCREEN
        };


        public enum m64p_emu_state
        {
            M64EMU_STOPPED = 1,
            M64EMU_RUNNING,
            M64EMU_PAUSED
        };

        public enum m64p_type
        {
            M64TYPE_INT = 1,
            M64TYPE_FLOAT,
            M64TYPE_BOOL,
            M64TYPE_STRING
        };

        public enum N64_MEMORY : uint
        {
            RDRAM = 1,
            PI_REG,
            SI_REG,
            VI_REG,
            RI_REG,
            AI_REG,

            EEPROM = 100,
            MEMPAK1,
            MEMPAK2,
            MEMPAK3,
            MEMPAK4,

            THE_ROM
        }

        public enum m64p_system_type
        {
            SYSTEM_NTSC = 0,
            SYSTEM_PAL,
            SYSTEM_MPAL
        };

        // UnmanagedType Enum 
        // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.unmanagedtype?view=netframework-4.8
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct m64p_rom_header
        {
            public byte init_PI_BSB_DOM1_LAT_REG;  /* 0x00 */
            public byte init_PI_BSB_DOM1_PGS_REG;  /* 0x01 */
            public byte init_PI_BSB_DOM1_PWD_REG;  /* 0x02 */
            public byte init_PI_BSB_DOM1_PGS_REG2; /* 0x03 */
            public uint ClockRate;                 /* 0x04 */
            public uint PC;                        /* 0x08 */
            public uint Release;                   /* 0x0C */
            public uint CRC1;                      /* 0x10 */
            public uint CRC2;                      /* 0x14 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public uint[] Unknown;                 /* 0x18 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] Name;                    /* 0x20 */
            public uint unknown;                   /* 0x34 */
            public uint Manufacturer_ID;           /* 0x38 */
            public ushort Cartridge_ID;            /* 0x3C - Game serial number  */
            public ushort Country_code;            /* 0x3E */
        };


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct m64p_rom_settings
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] goodname;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
            public char[] MD5;
            public byte savetype;
            public byte status;         /* Rom status on a scale from 0-5. */
            public byte players;        /* Local players 0-4, 2/3/4 way Netplay indicated by 5/6/7. */
            public byte rumble;         /* 0 - No, 1 - Yes boolean for rumble support. */
            public byte transferpak;    /* 0 - No, 1 - Yes boolean for transfer pak support. */
            public byte mempak;         /* 0 - No, 1 - Yes boolean for memory pak support. */
            public byte biopak;         /* 0 - No, 1 - Yes boolean for bio pak support. */
        }

        public string m64p_handle;

        public double CONFIG_PARAM_VERSION = 1;

        // Core Specifc functions

        /// <summary>
        /// Initializes the the core DLL
        /// </summary>
        /// <param name="APIVersion">Specifies what API version our app is using. Just set this to 0x20001</param>
        /// <param name="ConfigPath">Directory to have the DLL look for config data. "" seems to disable this</param>
        /// <param name="DataPath">Directory to have the DLL look for user data. "" seems to disable this</param>
        /// <param name="Context">Use "Core"</param>
        /// <param name="DebugCallback">A function to use when the core wants to output debug messages</param>
        /// <param name="context2">Use ""</param>
        /// <param name="dummy">Use IntPtr.Zero</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreStartup(int APIVersion, string ConfigPath, string DataPath, string Context, DebugCallback DebugCallback, string context2, IntPtr dummy);
        CoreStartup m64pCoreStartup;

        /// <summary>
        /// Cleans up the core
        /// </summary>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreShutdown();
        CoreShutdown m64pCoreShutdown;

        /// <summary>
        /// Connects a plugin DLL to the core DLL
        /// </summary>
        /// <param name="PluginType">The type of plugin that is being connected</param>
        /// <param name="PluginLibHandle">The DLL handle for the plugin</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreAttachPlugin(m64p_plugin_type PluginType, IntPtr PluginLibHandle);
        CoreAttachPlugin m64pCoreAttachPlugin;

        /// <summary>
        /// Disconnects a plugin DLL from the core DLL
        /// </summary>
        /// <param name="PluginType">The type of plugin to be disconnected</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDetachPlugin(m64p_plugin_type PluginType);
        CoreDetachPlugin m64pCoreDetachPlugin;

        /// <summary>
        /// Opens a section in the global config system
        /// </summary>
        /// <param name="SectionName">The name of the section to open</param>
        /// <param name="ConfigSectionHandle">A pointer to the pointer to use as the section handle</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error ConfigOpenSection(string SectionName, ref IntPtr ConfigSectionHandle);
        ConfigOpenSection m64pConfigOpenSection;

        /// <summary>
        /// Sets a parameter in the global config system
        /// </summary>
        /// <param name="ConfigSectionHandle">The handle of the section to access</param>
        /// <param name="ParamName">The name of the parameter to set</param>
        /// <param name="ParamType">The type of the parameter</param>
        /// <param name="ParamValue">A pointer to the value to use for the parameter (must be an int right now)</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error ConfigSetParameter(IntPtr ConfigSectionHandle, string ParamName, m64p_type ParamType, ref int ParamValue);
        ConfigSetParameter m64pConfigSetParameter;

        /// <summary>
        /// Sets a parameter in the global config system
        /// </summary>
        /// <param name="ConfigSectionHandle">The handle of the section to access</param>
        /// <param name="ParamName">The name of the parameter to set</param>
        /// <param name="ParamType">The type of the parameter</param>
        /// <param name="ParamValue">A pointer to the value to use for the parameter (must be an int right now)</param>
        /// <returns></returns>
        /*dont know if this even works*/
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error ConfigSetPlugins(IntPtr ConfigSectionHandle, string ParamName, m64p_type ParamType, ref string ParamValue);
        ConfigSetPlugins m64pConfigSetPlugins;

        /// <summary>
        /// Saves the mupen64plus state to the provided buffer
        /// </summary>
        /// <param name="buffer">A byte array to use to save the state. Must be at least 16788288 + 1024 bytes</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int savestates_save_bkm(byte[] buffer);
        savestates_save_bkm m64pCoreSaveState;

        /// <summary>
        /// Loads the mupen64plus state from the provided buffer
        /// </summary>
        /// <param name="buffer">A byte array filled with the state to load. Must be at least 16788288 + 1024 bytes</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int savestates_load_bkm(byte[] buffer);
        savestates_load_bkm m64pCoreLoadState;

        /// <summary>
        /// Gets a pointer to a section of the mupen64plus core
        /// </summary>
        /// <param name="mem_ptr_type">The section to get a pointer for</param>
        /// <returns>A pointer to the section requested</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr DebugMemGetPointer(N64_MEMORY mem_ptr_type);
        DebugMemGetPointer m64pDebugMemGetPointer;

        /// <summary>
        /// Gets the size of the given memory area
        /// </summary>
        /// <param name="mem_ptr_type">The section to get the size of</param>
        /// <returns>The size of the section requested</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int MemGetSize(N64_MEMORY mem_ptr_type);
        MemGetSize m64pMemGetSize;

        /// <summary>
        /// Initializes the saveram (eeprom and 4 mempacks)
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr init_saveram();
        init_saveram m64pinit_saveram;

        /// <summary>
        /// Pulls out the saveram for bizhawk to save
        /// </summary>
        /// <param name="dest">A byte array to save the saveram into</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr save_saveram(byte[] dest);
        save_saveram m64psave_saveram;

        /// <summary>
        /// Restores the saveram from bizhawk
        /// </summary>
        /// <param name="src">A byte array containing the saveram to restore</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr load_saveram(byte[] src);
        load_saveram m64pload_saveram;

        // The last parameter of CoreDoCommand is actually a void pointer, so instead we'll make a few delegates for the versions we want to use
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandByteArray(m64p_command Command, int ParamInt, byte[] ParamPtr);
        CoreDoCommandByteArray m64pCoreDoCommandByteArray;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandPtr(m64p_command Command, int ParamInt, IntPtr ParamPtr);
        CoreDoCommandPtr m64pCoreDoCommandPtr;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandStr(m64p_command Command, int ParamInt, string ParamPtr);
        CoreDoCommandStr m64pCoreDoCommandStr;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandROMHeader(m64p_command Command, int ParamInt, ref m64p_rom_header ParamPtr);
        CoreDoCommandROMHeader m64pCoreDoCommandROMHeader;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandROMSettings(m64p_command Command, m64p_rom_settings ParamInt, ref int ParamPtr);
        CoreDoCommandROMSettings m64pCoreDoCommandROMSettings;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandCoreStateQuery(m64p_command Command, m64p_core_param ParamInt, int ParamPtr);
        CoreDoCommandCoreStateQuery m64pCoreDoCommandCoreStateQuery;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandCoreStateSet(m64p_command Command, m64p_core_param ParamInt, ref int ParamPtr);
        CoreDoCommandCoreStateSet m64pCoreDoCommandCoreStateSet;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandCoreStateSetVideoMode(m64p_command Command, m64p_video_mode ParamInt, ref int ParamPtr);
        CoreDoCommandCoreStateSetVideoMode m64pCoreDoCommandCoreStateVideoMode;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandCoreStateSetRef(m64p_command Command, m64p_core_param ParamInt, ref int ParamPtr);
        CoreDoCommandCoreStateSetRef m64pCoreDoCommandCoreStateSetRef;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandRefInt(m64p_command Command, int ParamInt, ref int ParamPtr);
        CoreDoCommandRefInt m64pCoreDoCommandRefInt;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandFrameCallback(m64p_command Command, int ParamInt, FrameCallback ParamPtr);
        CoreDoCommandFrameCallback m64pCoreDoCommandFrameCallback;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandVICallback(m64p_command Command, int ParamInt, VICallback ParamPtr);
        CoreDoCommandVICallback m64pCoreDoCommandVICallback;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error CoreDoCommandRenderCallback(m64p_command Command, int ParamInt, RenderCallback ParamPtr);
        CoreDoCommandRenderCallback m64pCoreDoCommandRenderCallback;

        // Plugins Search Load
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //delegate m64p_error PluginSearchLoad(string ConfigUI);
        //PluginSearchLoad m64pPluginSearchLoad;

        // Configuration
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error ConfigSetDefaultFloat(string ConfigSectionHandle, string ParamName, double ParamValue, string ParamHelp);
        ConfigSetDefaultFloat m64pConfigSetDefaultFloat;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error ConfigSetDefaultString(string ConfigSectionHandle, string ParamName, string ParamValue, string ParamHelp);
        ConfigSetDefaultString m64pConfigSetDefaultString;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate m64p_error ConfigSaveFile();
        ConfigSaveFile m64pConfigSaveFile;

        //WARNING - RETURNS A STATIC BUFFER
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr biz_r4300_decode_op(uint instr, uint counter);
        public biz_r4300_decode_op m64p_decode_op;

        /// <summary>
        /// Reads from the "system bus"
        /// </summary>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate byte biz_read_memory(uint addr);
        public biz_read_memory m64p_read_memory_8;

        /// <summary>
        /// Writes to the "system bus"
        /// </summary>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void biz_write_memory(uint addr, byte value);
        public biz_write_memory m64p_write_memory_8;

        // These are common for all four plugins

        /// <summary>
        /// Initializes the plugin
        /// </summary>
        /// <param name="CoreHandle">The DLL handle for the core DLL</param>
        /// <param name="Context">Giving a context to the DebugCallback</param>
        /// <param name="DebugCallback">A function to use when the pluging wants to output debug messages</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate m64p_error PluginStartup(IntPtr CoreHandle, string Context, DebugCallback DebugCallback);

        /// <summary>
        /// Cleans up the plugin
        /// </summary>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate m64p_error PluginShutdown();

        // Callback functions

        /// <summary>
        /// Handles a debug message from mupen64plus
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="level"></param>
        /// <param name="Message">The message to display</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DebugCallback(IntPtr Context, int level, string Message);

        /// <summary>
        /// This will be called every time a new frame is finished
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FrameCallback();
        FrameCallback m64pFrameCallback;

        /// <summary>
        /// This will be called every time a VI occurs
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void VICallback();
        VICallback m64pVICallback;

        /// <summary>
        /// This will be called every time before the screen is drawn
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void RenderCallback();
        RenderCallback m64pRenderCallback;

        /// <summary>
        /// This will be called after the emulator is setup and is ready to be used
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StartupCallback();

        /// <summary>
        /// Type of the read/write memory callbacks
        /// </summary>
        /// <param name="address">The address which the cpu is read/writing</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MemoryCallback(uint address);

        /// <summary>
        /// Sets the memory read callback
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetReadCallback(MemoryCallback callback);
        SetReadCallback m64pSetReadCallback;

        /// <summary>
        /// Sets the memory write callback
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SetWriteCallback(MemoryCallback callback);
        SetWriteCallback m64pSetWriteCallback;

        /// <summary>
        /// Gets the CPU registers
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void GetRegisters(byte[] dest);
        GetRegisters m64pGetRegisters;

        // DLL handles
        public IntPtr CoreDll { get; private set; }
        public IntPtr VideoPluginDll { get; private set; }

        /// <summary>
        /// Mupen64Plus API
        /// </summary>
        public Mupen64PlusAPI()
        {
        }

        /// <summary>
        /// Initiate - Ultra Modified
        /// </summary>
        public void Initiate(string key,
                             byte[] romBuffer,
                             string videoPlugin,
                             string audioPlugin,
                             string inputPlugin,
                             string rspPlugin,
                             int windowWidth,
                             int windowHeight
                            )
        {
            // -------------------------
            // Load DLL
            // -------------------------
            SetDllDirectory(VM.PathsView.Mupen_Text);
            CoreDll = LoadLibrary("mupen64plus.dll");
            if (CoreDll == IntPtr.Zero)
                throw new InvalidOperationException("Failed to load mupen64plus.dll");

            // -------------------------
            // Function Pointers
            // -------------------------
            connectFunctionPointers();

            // --------------------------------------------------
            // Core Section
            // --------------------------------------------------
            // -------------------------
            // Start up the core
            // -------------------------
            m64p_error result = m64pCoreStartup(
                0x20001,    // API Version
                VM.PathsView.Config_Text, // Make sure this path is set, Default ""
                "",         // Data Path ""
                "Core",     // Context 
                null,       // DebugCallback 
                "",         // Context2 
                IntPtr.Zero // StateCallback
                );

            // -------------------------
            // Save Slot
            // -------------------------
            result = m64pCoreDoCommandPtr(m64p_command.M64CMD_STATE_SET_SLOT, GetStateSlot(), IntPtr.Zero);

            // -------------------------
            // Core Settings
            // -------------------------
            // Open the core settings section in the config system
            IntPtr core_section = IntPtr.Zero;
            m64pConfigOpenSection("Core", ref core_section);

            // Set the savetype if needed
            //if (DisableExpansionSlot)
            //{
            //    int disable = 1;
            //    m64pConfigSetParameter(core_section, "DisableExtraMem", m64p_type.M64TYPE_INT, ref disable);
            //}

            // Set the savetype if needed
            //if (SaveType != 0)
            //{
            //    m64pConfigSetParameter(core_section, "SaveType", m64p_type.M64TYPE_INT, ref SaveType);
            //}

            // -------------------------
            // Core Type
            // -------------------------
            int CoreType = 2; // default

            // Pure Interpreter
            if (VM.EmulatorView.Emulator_PureInterpreter_IsChecked == true)
            {
                CoreType = 0;
            }
            // Cached Interpreter
            else if (VM.EmulatorView.Emulator_CachedInterpreter_IsChecked == true)
            {
                CoreType = 1;
            }
            // Dynamic Recompiler
            else if (VM.EmulatorView.Emulator_DynamicRecompiler_IsChecked == true)
            {
                CoreType = 2;
            }

            m64pConfigSetParameter(core_section, "R4300Emulator", m64p_type.M64TYPE_INT, ref CoreType);

            // -------------------------
            // NoCompiledJump
            // -------------------------
            int noCompiledJump = 0;
            // Off
            if (VM.EmulatorView.Emulator_NoCompiledJump_IsChecked == false)
            {
                noCompiledJump = 0;
            }
            // On
            else if (VM.EmulatorView.Emulator_NoCompiledJump_IsChecked == true)
            {
                noCompiledJump = 1;
            }

            result = m64pConfigSetParameter(core_section, "NoCompiledJump", m64p_type.M64TYPE_INT, ref noCompiledJump);

            // -------------------------
            // DisableExtraMemory
            // -------------------------
            int disableExtraMemory = 0;
            // Off
            if (VM.EmulatorView.Emulator_DisableExtraMemory_IsChecked == false)
            {
                disableExtraMemory = 0;
            }
            // On
            else if (VM.EmulatorView.Emulator_DisableExtraMemory_IsChecked == true)
            {
                disableExtraMemory = 1;
            }

            result = m64pConfigSetParameter(core_section, "DisableExtraMem", m64p_type.M64TYPE_INT, ref disableExtraMemory);

            // -------------------------
            // DelaySI
            // -------------------------
            int delaySI = 0;
            // Off
            if (VM.EmulatorView.Emulator_DelaySI_IsChecked == false)
            {
                delaySI = 0;
            }
            // On
            else if (VM.EmulatorView.Emulator_DelaySI_IsChecked == true)
            {
                delaySI = 1;
            }

            result = m64pConfigSetParameter(core_section, "DelaySI", m64p_type.M64TYPE_INT, ref delaySI);

            // -------------------------
            // Cycles
            // -------------------------
            int cycles = 0; // default
            if (VM.EmulatorView.Emulator_Cycles_SelectedItem == "0")
            {
                cycles = 0;
            }
            else if (VM.EmulatorView.Emulator_Cycles_SelectedItem == "1")
            {
                cycles = 1;
            }
            else if (VM.EmulatorView.Emulator_Cycles_SelectedItem == "2")
            {
                cycles = 2;
            }
            else if (VM.EmulatorView.Emulator_Cycles_SelectedItem == "3")
            {
                cycles = 3;
            }
            else if (VM.EmulatorView.Emulator_Cycles_SelectedItem == "4")
            {
                cycles = 4;
            }

            result = m64pConfigSetParameter(core_section, "CountPerOp", m64p_type.M64TYPE_INT, ref cycles);

            // --------------------------------------------------
            // Enable Debugger
            // --------------------------------------------------
            //int enableDebugger = 1;
            //m64pConfigSetParameter(core_section, "EnableDebugger", m64p_type.M64TYPE_INT, ref enableDebugger);

            // --------------------------------------------------
            // UI Console Section
            // --------------------------------------------------
            IntPtr uiconsole_section = IntPtr.Zero;
            m64pConfigOpenSection("UI-Console", ref uiconsole_section);
            // -------------------------
            // Plugins
            // ------------------------ -
            /*dont know if this even works*/
            //string pluginsDir = "\"" + VM.PathsView.Plugins_Text + "\"";
            //result = m64pConfigSetPlugins(uiconsole_section, "PluginDir", m64p_type.M64TYPE_STRING, ref pluginsDir);

            //string videoPluginPath = "\"" + videoPlugin + "\"";
            //result = m64pConfigSetPlugins(uiconsole_section, "VideoPlugin", m64p_type.M64TYPE_STRING, ref videoPluginPath);

            //string audioPluginPath = "\"" + audioPlugin + "\"";
            //result = m64pConfigSetPlugins(uiconsole_section, "AudioPlugin", m64p_type.M64TYPE_STRING, ref audioPluginPath);

            //string inputPluginPath = "\"" + inputPlugin + "\"";
            //result = m64pConfigSetPlugins(uiconsole_section, "InputPlugin", m64p_type.M64TYPE_STRING, ref inputPluginPath);

            //string rspPluginPath = "\"" + rspPlugin + "\"";
            //result = m64pConfigSetPlugins(uiconsole_section, "RspPlugin", m64p_type.M64TYPE_STRING, ref rspPluginPath);

            // --------------------------------------------------
            // ROM Section
            // --------------------------------------------------
            // -------------------------
            // Pass the ROM to the Core
            // -------------------------
            //if (key == "play")
            //{
                result = m64pCoreDoCommandByteArray(m64p_command.M64CMD_ROM_OPEN, romBuffer.Length, romBuffer);

                // Get ROM Header
                int sizeHeader = Marshal.SizeOf(typeof(m64p_rom_header));
                result = m64pCoreDoCommandROMHeader(m64p_command.M64CMD_ROM_GET_HEADER, sizeHeader, ref _rom_header);

                // Get ROM Settings
                int sizeSettings = Marshal.SizeOf(typeof(m64p_rom_settings));
                result = m64pCoreDoCommandROMSettings(m64p_command.M64CMD_ROM_GET_SETTINGS, _rom_settings, ref sizeSettings);
            //}

            // --------------------------------------------------
            // Video Section
            // --------------------------------------------------
            IntPtr video_section = IntPtr.Zero;
            // Open the general video settings section in the config system
            m64pConfigOpenSection("Video-General", ref video_section);

            // -------------------------
            // Set Window Size
            // -------------------------
            // Set the desired width and height for mupen64plus
            result = m64pConfigSetParameter(video_section, "ScreenWidth", m64p_type.M64TYPE_INT, ref windowWidth/*ref video_settings.Width*/);
            result = m64pConfigSetParameter(video_section, "ScreenHeight", m64p_type.M64TYPE_INT, ref windowHeight/*ref video_settings.Height*/);

            // -------------------------
            // Set Fullscreen / Windowed
            // -------------------------
            int displayMode = 0; // default
            // Windowed
            if (VM.DisplayView.Display_View_SelectedItem == "Windowed")
            {
                displayMode = 0;
            }
            // Fullscreen
            else if (VM.DisplayView.Display_View_SelectedItem == "Fullscreen")
            {
                displayMode = 1;
            }

            result = m64pConfigSetParameter(video_section, "Fullscreen", m64p_type.M64TYPE_INT, ref displayMode);

            // -------------------------
            // Set Vsync
            // -------------------------
            int vsync = 0;
            // Off
            if (VM.DisplayView.Display_Vsync_IsChecked == false)
            {
                vsync = 0;
            }
            // On
            else if (VM.DisplayView.Display_Vsync_IsChecked == true)
            {
                vsync = 1;
            }

            result = m64pConfigSetParameter(video_section, "VerticalSync", m64p_type.M64TYPE_INT, ref vsync);

            // -------------------------
            // OnScreenDisplay
            // -------------------------
            int osd = 0;
            // Off
            if (VM.DisplayView.Display_OSD_IsChecked == false)
            {
                osd = 0;
            }
            // On
            else if (VM.DisplayView.Display_OSD_IsChecked == true)
            {
                osd = 1;
            }

            result = m64pConfigSetParameter(video_section, "OnScreenDisplay", m64p_type.M64TYPE_INT, ref osd);

            //set_video_parameters(video_settings);

            // --------------------------------------------------
            // Configuration Defaults
            // --------------------------------------------------
            //IntPtr l_ConfigUI = IntPtr.Zero;
            //result = m64pConfigSetDefaultFloat("UI-Console", "Version", CONFIG_PARAM_VERSION, "Mupen64Plus UI-Console config parameter set version number. Please don't change this version number.");
            //result = m64pConfigSetDefaultString("UI-Console", "PluginDir", "\"" + VM.PathsView.Plugins_Text + "\"", "Directory in which to search for plugins");
            //result = m64pConfigSetDefaultString("UI-Console", "VideoPlugin", videoPlugin, "Filename of video plugin");
            //result = m64pConfigSetDefaultString("UI-Console", "AudioPlugin", audioPlugin, "Filename of audio plugin");
            //result = m64pConfigSetDefaultString("UI-Console", "InputPlugin", inputPlugin, "Filename of input plugin");
            //result = m64pConfigSetDefaultString(/*l_ConfigUI*/"UI-Console", "RspPlugin", rspPlugin, "Filename of RSP plugin");

            // --------------------------------------------------
            // Attach Plugins
            // --------------------------------------------------
            //  The plugins must be attached in the following order: Video, Audio, Input, RSP. 
            //IntPtr video_plugin_section = IntPtr.Zero;

            // Video
            AttachPlugin(m64p_plugin_type.M64PLUGIN_GFX, videoPlugin); //mupen64plus-video-GLideN64.dll
            // Audio
            AttachPlugin(m64p_plugin_type.M64PLUGIN_AUDIO, audioPlugin); //mupen64plus-audio-sdl.dll
            // Input
            AttachPlugin(m64p_plugin_type.M64PLUGIN_INPUT, inputPlugin); //mupen64plus-input-sdl.dll
            // RSP
            AttachPlugin(m64p_plugin_type.M64PLUGIN_RSP, rspPlugin); //mupen64plus-rsp-hle.dll

            // Generate Defaults here

            // --------------------------------------------------
            // Initialize event invoker
            // --------------------------------------------------
            m64pFrameCallback = new FrameCallback(FireFrameFinishedEvent);
            result = m64pCoreDoCommandFrameCallback(m64p_command.M64CMD_SET_FRAME_CALLBACK, 0, m64pFrameCallback);
            m64pVICallback = new VICallback(FireVIEvent);
            result = m64pCoreDoCommandVICallback(m64p_command.M64CMD_SET_VI_CALLBACK, 0, m64pVICallback);
            m64pRenderCallback = new RenderCallback(FireRenderEvent);
            result = m64pCoreDoCommandRenderCallback(m64p_command.M64CMD_SET_RENDER_CALLBACK, 0, m64pRenderCallback);

            // -------------------------
            // Save Config
            // -------------------------
            m64pConfigSaveFile();

            //// -------------------------
            //// Start
            //// -------------------------
            //// Prepare to start the emulator in a different thread
            ////m64pEmulator = new Thread(ExecuteEmulator);
            //ExecuteEmulator();
        }


        /// <summary>
        /// Launch
        /// </summary>
        public void Launch(string key,
                           byte[] romBuffer,
                           string videoPlugin,
                           string audioPlugin,
                           string inputPlugin,
                           string rspPlugin,
                           int windowWidth,
                           int windowHeight
                          )
        {
            // -------------------------
            // Initiate
            // -------------------------
            Initiate(key,
                     romBuffer,
                     videoPlugin,
                     audioPlugin,
                     inputPlugin,
                     rspPlugin,
                     windowWidth,
                     windowHeight
                    );

            // -------------------------
            // Start
            // -------------------------
            ExecuteEmulator();
        }


        /// <summary>
        /// Generate
        /// </summary>
        //public void Generate(string key,
        //                     byte[] romBuffer,
        //                     string videoPlugin,
        //                     string audioPlugin,
        //                     string inputPlugin,
        //                     string rspPlugin,
        //                     int windowWidth,
        //                     int windowHeight
        //                    )
        //{
        //    // -------------------------
        //    // Initiate
        //    // -------------------------
        //    Initiate(key,
        //             romBuffer,
        //             videoPlugin,
        //             audioPlugin,
        //             inputPlugin,
        //             rspPlugin,
        //             windowWidth,
        //             windowHeight
        //            );
        //}

        volatile bool emulator_running = false;

        /// <summary>
        /// Starts executing the emulator asynchronously
        /// Waits until the emulator booted up and than returns
        /// </summary>
        public void AsyncExecuteEmulator()
        {
            m64pEmulator.Start();

            // Wait for the core to boot up
            m64pStartupComplete.WaitOne();
        }

        /// <summary>
        /// Starts execution of mupen64plus
        /// Does not return until the emulator stops
        /// </summary>
        private void ExecuteEmulator()
        {
            emulator_running = true;
            var cb = new StartupCallback(() => m64pStartupComplete.Set());
            m64pCoreDoCommandPtr(m64p_command.M64CMD_EXECUTE, 0,
                Marshal.GetFunctionPointerForDelegate(cb));
            emulator_running = false;
            cb.GetType();
        }

        /// <summary>
        /// Get Current State Slot Number
        /// </summary>
        /// <returns></returns>
        public int GetStateSlot()
        {
            if (VM.MainView.StateSlot0_IsChecked == true)
            {
                return 0;
            }
            else if (VM.MainView.StateSlot1_IsChecked == true)
            {
                return 1;
            }
            else if (VM.MainView.StateSlot2_IsChecked == true)
            {
                return 2;
            }
            else if (VM.MainView.StateSlot3_IsChecked == true)
            {
                return 3;
            }
            else if (VM.MainView.StateSlot4_IsChecked == true)
            {
                return 4;
            }
            else if (VM.MainView.StateSlot5_IsChecked == true)
            {
                return 5;
            }
            else if (VM.MainView.StateSlot6_IsChecked == true)
            {
                return 6;
            }
            else if (VM.MainView.StateSlot7_IsChecked == true)
            {
                return 7;
            }
            else if (VM.MainView.StateSlot8_IsChecked == true)
            {
                return 8;
            }
            else if (VM.MainView.StateSlot9_IsChecked == true)
            {
                return 9;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// State Slot
        /// </summary>
        public void StateSlot()
        {
            m64pCoreDoCommandPtr(m64p_command.M64CMD_STATE_SET_SLOT, GetStateSlot(), IntPtr.Zero);
        }

        /// <summary>
        /// Load State File
        /// </summary>
        public void LoadStateFile(string file)
        {
            // ParamInt: Ignored
            m64pCoreDoCommandStr(m64p_command.M64CMD_STATE_LOAD, 0, file);
        }

        /// <summary>
        /// Save State File
        /// </summary>
        public void SaveStateFile(string file)
        {
            // ParamInt: This parameter will only be used if ParamPtr is not NULL. 
            // If 1, a Mupen64Plus state file will be saved. 
            // If 2, a Project64 compressed state file will be saved. 
            // If 3, a Project64 uncompressed state file will be saved.
            m64pCoreDoCommandStr(m64p_command.M64CMD_STATE_SAVE, 1, file);
        }

        /// <summary>
        /// ROM Get Header
        /// </summary>
        public m64p_rom_header _rom_header;
        public String ROMGetHeader()
        {
            int size = Marshal.SizeOf(typeof(m64p_rom_header));

            m64pCoreDoCommandROMHeader(m64p_command.M64CMD_ROM_GET_HEADER, size, ref _rom_header);

            return System.Text.Encoding.Default.GetString(_rom_header.Name);
        }


        /// <summary>
        /// ROM Get Settings
        /// </summary>
        public m64p_rom_settings _rom_settings;
        public String ROMGetSettings()
        {
            int size = Marshal.SizeOf(typeof(m64p_rom_settings));

            m64pCoreDoCommandROMSettings(m64p_command.M64CMD_ROM_GET_SETTINGS, _rom_settings, ref size);

            return new string(_rom_settings.goodname);
        }


        /// <summary>
        /// Load State
        /// </summary>
        public void LoadState()
        {
            m64pCoreDoCommandPtr(m64p_command.M64CMD_STATE_LOAD, GetStateSlot(), IntPtr.Zero);
        }


        /// <summary>
        /// Save State
        /// </summary>
        public void SaveState()
        {
            m64pCoreDoCommandPtr(m64p_command.M64CMD_STATE_SAVE, GetStateSlot(), IntPtr.Zero);
        }

        /// <summary>
        /// Screenshot
        /// </summary>
        public void Screenshot()
        {
            m64pCoreDoCommandPtr(m64p_command.M64CMD_TAKE_NEXT_SCREENSHOT, 0, IntPtr.Zero);
        }

        /// <summary>
        /// Pause
        /// </summary>
        public void Pause()
        {
            m64pCoreDoCommandPtr(m64p_command.M64CMD_PAUSE, 0, IntPtr.Zero);
        }

        /// <summary>
        /// Mute
        /// </summary>
        private int mute = 0;
        public void Mute()
        {
            // Mute
            if (mute == 0)
            {
                mute = 1;
            }
            // Unmute
            else if (mute == 1)
            {
                mute = 0;
            }

            m64pCoreDoCommandCoreStateSet(
                m64p_command.M64CMD_CORE_STATE_SET,
                m64p_core_param.M64CORE_AUDIO_MUTE,
                ref mute
            );
        }

        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            //m64pCoreDoCommandPtr(m64p_command.M64CMD_STOP, 0, IntPtr.Zero);
            Dispose();
        }

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
            // ParamInt 0 to do a soft reset, 1 to do a hard reset.
            m64pCoreDoCommandPtr(m64p_command.M64CMD_RESET, 1, IntPtr.Zero);
        }

        /// <summary>
        /// Soft Reset
        /// </summary>
        public void SoftReset()
        {
            // ParamInt 0 to do a soft reset, 1 to do a hard reset.
            m64pCoreDoCommandPtr(m64p_command.M64CMD_RESET, 0, IntPtr.Zero);
        }

        /// <summary>
        /// Speed Up / Speed Down
        /// </summary>
        private int speed = 100; // Default 100%

        /// <summary>
        /// Slow Down
        /// </summary>
        public void SlowDown()
        {
            // Decrease by 25%
            speed = speed - 25;

            if (speed < 0)
            {
                speed = 0;
            }

            m64pCoreDoCommandCoreStateSet(
                m64p_command.M64CMD_CORE_STATE_SET,
                m64p_core_param.M64CORE_SPEED_FACTOR,
                ref speed
            );
        }

        /// <summary>
        /// Speed Up
        /// </summary>
        public void SpeedUp()
        {
            // Increase by 25%
            speed = speed + 25;

            if (speed > 1000)
            {
                speed = 1000;
            }

            m64pCoreDoCommandCoreStateSet(
                m64p_command.M64CMD_CORE_STATE_SET,
                m64p_core_param.M64CORE_SPEED_FACTOR,
                ref speed
            ); 
        }

        /// <summary>
        /// Fullscreen
        /// </summary>
        int mode = 3; // default fullscreen
        public void Fullscreen()
        {
            // Fullscreen
            if (mode == 3)
            {
                m64pCoreDoCommandCoreStateSet(
                    m64p_command.M64CMD_CORE_STATE_SET,
                    m64p_core_param.M64CORE_VIDEO_MODE,
                    ref mode
                );

                mode = 2;
            }
            // Windowed
            else if (mute == 2)
            {
                m64pCoreDoCommandCoreStateSet(
                    m64p_command.M64CMD_CORE_STATE_SET,
                    m64p_core_param.M64CORE_VIDEO_MODE,
                    ref mode
                );

                mode = 3;
            }     
        }

        /// <summary>
        /// Look up function pointers in the dlls
        /// </summary>
        void connectFunctionPointers()
        {
            m64pCoreStartup = (CoreStartup)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreStartup"), typeof(CoreStartup));
            m64pCoreShutdown = (CoreShutdown)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreShutdown"), typeof(CoreShutdown));
            m64pCoreDoCommandByteArray = (CoreDoCommandByteArray)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandByteArray));
            m64pCoreDoCommandPtr = (CoreDoCommandPtr)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandPtr));
            // Custom
            m64pCoreDoCommandStr = (CoreDoCommandStr)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandStr));
            m64pCoreDoCommandROMHeader = (CoreDoCommandROMHeader)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandROMHeader));
            m64pCoreDoCommandROMSettings = (CoreDoCommandROMSettings)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandROMSettings));
            m64pCoreDoCommandCoreStateSet = (CoreDoCommandCoreStateSet)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandCoreStateSet));
            m64pCoreDoCommandCoreStateVideoMode = (CoreDoCommandCoreStateSetVideoMode)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandCoreStateSetVideoMode));
            m64pCoreDoCommandCoreStateSetRef = (CoreDoCommandCoreStateSetRef)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandCoreStateSetRef));
            m64pCoreDoCommandCoreStateQuery = (CoreDoCommandCoreStateQuery)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandCoreStateQuery));
            m64pConfigSetDefaultFloat = (ConfigSetDefaultFloat)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "ConfigSetDefaultFloat"), typeof(ConfigSetDefaultFloat));
            m64pConfigSetDefaultString = (ConfigSetDefaultString)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "ConfigSetDefaultString"), typeof(ConfigSetDefaultString));
            m64pConfigSaveFile = (ConfigSaveFile)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "ConfigSaveFile"), typeof(ConfigSaveFile));
            // End Custom
            m64pCoreDoCommandRefInt = (CoreDoCommandRefInt)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandRefInt));
            m64pCoreDoCommandFrameCallback = (CoreDoCommandFrameCallback)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandFrameCallback));
            m64pCoreDoCommandVICallback = (CoreDoCommandVICallback)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandVICallback));
            m64pCoreDoCommandRenderCallback = (CoreDoCommandRenderCallback)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDoCommand"), typeof(CoreDoCommandRenderCallback));
            m64pCoreAttachPlugin = (CoreAttachPlugin)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreAttachPlugin"), typeof(CoreAttachPlugin));
            m64pCoreDetachPlugin = (CoreDetachPlugin)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "CoreDetachPlugin"), typeof(CoreDetachPlugin));
            m64pConfigOpenSection = (ConfigOpenSection)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "ConfigOpenSection"), typeof(ConfigOpenSection));
            m64pConfigSetParameter = (ConfigSetParameter)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "ConfigSetParameter"), typeof(ConfigSetParameter));
            /*dont know if this even works*/m64pConfigSetPlugins = (ConfigSetPlugins)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "ConfigSetParameter"), typeof(ConfigSetPlugins));
            //m64pCoreSaveState = (savestates_save_bkm)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "savestates_save_bkm"), typeof(savestates_save_bkm));
            //m64pCoreLoadState = (savestates_load_bkm)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "savestates_load_bkm"), typeof(savestates_load_bkm));
            m64pDebugMemGetPointer = (DebugMemGetPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "DebugMemGetPointer"), typeof(DebugMemGetPointer));
            //m64pMemGetSize = (MemGetSize)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "MemGetSize"), typeof(MemGetSize));
            //m64pinit_saveram = (init_saveram)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "init_saveram"), typeof(init_saveram));
            //m64psave_saveram = (save_saveram)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "save_saveram"), typeof(save_saveram));
            //m64pload_saveram = (load_saveram)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "load_saveram"), typeof(load_saveram));
            //m64pSetReadCallback = (SetReadCallback)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "SetReadCallback"), typeof(SetReadCallback));
            //m64pSetWriteCallback = (SetWriteCallback)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "SetWriteCallback"), typeof(SetWriteCallback));
            //m64pGetRegisters = (GetRegisters)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "GetRegisters"), typeof(GetRegisters));
            //m64p_read_memory_8 = (biz_read_memory)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "biz_read_memory"), typeof(biz_read_memory));
            //m64p_write_memory_8 = (biz_write_memory)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "biz_write_memory"), typeof(biz_write_memory));
            //m64p_decode_op = (biz_r4300_decode_op)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "biz_r4300_decode_op"), typeof(biz_r4300_decode_op));
           
            // Newer Version
            //m64pConfigSetParameterStr = (ConfigSetParameterStr)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "ConfigSetParameter"), typeof(ConfigSetParameterStr));
            //m64pDebugSetCallbacks = (DebugSetCallbacks)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "DebugSetCallbacks"), typeof(DebugSetCallbacks));
            //m64pDebugBreakpointLookup = (DebugBreakpointLookup)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "DebugBreakpointLookup"), typeof(DebugBreakpointLookup));
            //m64pDebugBreakpointCommand = (DebugBreakpointCommand)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "DebugBreakpointCommand"), typeof(DebugBreakpointCommand));
            //m64pDebugGetState = (DebugGetState)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "DebugGetState"), typeof(DebugGetState));
            //m64pDebugSetRunState = (DebugSetRunState)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "DebugSetRunState"), typeof(DebugSetRunState));
            //m64pDebugStep = (DebugStep)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "DebugStep"), typeof(DebugStep));
            //m64pSetTraceCallback = (SetTraceCallback)Marshal.GetDelegateForFunctionPointer(GetProcAddress(CoreDll, "SetTraceCallback"), typeof(SetTraceCallback));
        }

        /// <summary>
        /// Puts plugin settings of EmuHawk into mupen64plus
        /// </summary>
        /// <param name="video_settings">Settings to put into mupen64plus</param>
        public void set_video_parameters(/*VideoPluginSettings video_settings*/)
        {
            IntPtr video_plugin_section = IntPtr.Zero;

            // GLideN64
            if (VM.PluginsView.Video_SelectedItem == "mupen64plus-video-GLideN64.dll")
            {
                m64pConfigOpenSection("Video-GLideN64", ref video_plugin_section);
            }
            // Glide64mk2
            else if (VM.PluginsView.Video_SelectedItem == "mupen64plus-video-glide64mk2.dll")
            {
                m64pConfigOpenSection("Video-Glide64mk2", ref video_plugin_section);
            }
            // Glide64
            else if (VM.PluginsView.Video_SelectedItem == "mupen64plus-video-glide64.dll")
            {
                m64pConfigOpenSection("Video-Glide64", ref video_plugin_section);
            }
            // Rice
            else if (VM.PluginsView.Video_SelectedItem == "mupen64plus-video-rice.dll")
            {
                m64pConfigOpenSection("Video-Rice", ref video_plugin_section);
            }
            // z64
            else if (VM.PluginsView.Video_SelectedItem == "mupen64plus-video-z64.dll")
            {
                m64pConfigOpenSection("Video-z64", ref video_plugin_section);
            }
            // Arachnoid
            else if (VM.PluginsView.Video_SelectedItem == "mupen64plus-video-arachnoid.dll")
            {
                m64pConfigOpenSection("Video-Arachnoid", ref video_plugin_section);
            }
            else
            {
                return;
            }

            //foreach (string Parameter in video_settings.Parameters.Keys)
            //{
            //    int value = 0;
            //    if (video_settings.Parameters[Parameter].GetType() == typeof(int))
            //    {
            //        value = (int)video_settings.Parameters[Parameter];
            //    }
            //    else if (video_settings.Parameters[Parameter].GetType() == typeof(bool))
            //    {
            //        value = (bool)video_settings.Parameters[Parameter] ? 1 : 0;
            //    }
            //    else if (video_settings.Parameters[Parameter] is Enum)
            //    {
            //        value = (int)video_settings.Parameters[Parameter];
            //    }
            //    m64pConfigSetParameter(video_plugin_section, Parameter, m64p_type.M64TYPE_INT, ref value);
            //}
        }

        public int get_memory_size(N64_MEMORY id)
        {
            return m64pMemGetSize(id);
        }

        public IntPtr get_memory_ptr(N64_MEMORY id)
        {
            return m64pDebugMemGetPointer(id);
        }

        //public void soft_reset()
        //{
        //    m64pCoreDoCommandPtr(m64p_command.M64CMD_RESET, 0, IntPtr.Zero);
        //}

        //public void hard_reset()
        //{
        //    m64pCoreDoCommandPtr(m64p_command.M64CMD_RESET, 1, IntPtr.Zero);
        //}

        public void frame_advance()
        {
            m64pCoreDoCommandPtr(m64p_command.M64CMD_ADVANCE_FRAME, 0, IntPtr.Zero);

            //the way we should be able to do it:
            //m64pFrameComplete.WaitOne();

            //however. since this is probably an STAThread, this call results in message pumps running.
            //those message pumps are only supposed to respond to critical COM stuff, but in fact they interfere with other things.
            //so here are two workaround methods.

            //method 1.
            //BizHawk.Common.Win32ThreadHacks.HackyPinvokeWaitOne(m64pFrameComplete);

            //method 2.
            //BizHawk.Common.Win32ThreadHacks.HackyComWaitOne(m64pFrameComplete);
        }

        public int SaveState(byte[] buffer)
        {
            return m64pCoreSaveState(buffer);
        }

        public void LoadState(byte[] buffer)
        {
            m64pCoreLoadState(buffer);
        }

        byte[] saveram_backup;

        public void InitSaveram()
        {
            m64pinit_saveram();
        }

        public const int kSaveramSize = 0x800 + 4 * 0x8000 + 0x20000 + 0x8000;

        public byte[] SaveSaveram()
        {
            if (disposed)
            {
                if (saveram_backup != null)
                {
                    return (byte[])saveram_backup.Clone();
                }
                else
                {
                    // This shouldn't happen!!
                    return new byte[kSaveramSize];
                }
            }
            else
            {
                byte[] dest = new byte[kSaveramSize];
                m64psave_saveram(dest);
                return dest;
            }
        }

        public void LoadSaveram(byte[] src)
        {
            m64pload_saveram(src);
        }

        public void setReadCallback(MemoryCallback callback)
        {
            m64pSetReadCallback(callback);
        }

        public void setWriteCallback(MemoryCallback callback)
        {
            m64pSetWriteCallback(callback);
        }

        public void getRegisters(byte[] dest)
        {
            m64pGetRegisters(dest);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                // Stop the core, and wait for it to end
                while (emulator_running)
                {
                    // Repeatedly send the stop command, because sometimes sending it just once doesn't work
                    m64pCoreDoCommandPtr(m64p_command.M64CMD_STOP, 0, IntPtr.Zero);
                }

                // Backup the saveram in case bizhawk wants to get at is after we've freed the libraries
                //saveram_backup = SaveSaveram();

                DetachPlugin(m64p_plugin_type.M64PLUGIN_GFX);
                DetachPlugin(m64p_plugin_type.M64PLUGIN_AUDIO);
                DetachPlugin(m64p_plugin_type.M64PLUGIN_INPUT);
                DetachPlugin(m64p_plugin_type.M64PLUGIN_RSP);

                m64pCoreDoCommandPtr(m64p_command.M64CMD_ROM_CLOSE, 0, IntPtr.Zero);
                m64pCoreShutdown();
                FreeLibrary(CoreDll);

                disposed = true;
            }
        }

        struct AttachedPlugin
        {
            public PluginStartup dllStartup;
            public PluginShutdown dllShutdown;
            public IntPtr dllHandle;
        }
        Dictionary<m64p_plugin_type, AttachedPlugin> plugins = new Dictionary<m64p_plugin_type, AttachedPlugin>();

        public IntPtr AttachPlugin(m64p_plugin_type type, string PluginName)
        {
            //SetDllDirectory(VM.PathsView.Plugins_Text);

            if (plugins.ContainsKey(type))
                DetachPlugin(type);

            AttachedPlugin plugin;
            plugin.dllHandle = LoadLibrary(PluginName);
            if (plugin.dllHandle == IntPtr.Zero)
                throw new InvalidOperationException(string.Format("Failed to load plugin {0}, error code: 0x{1:X}", PluginName, GetLastError()));

            plugin.dllStartup = (PluginStartup)Marshal.GetDelegateForFunctionPointer(GetProcAddress(plugin.dllHandle, "PluginStartup"), typeof(PluginStartup));
            plugin.dllShutdown = (PluginShutdown)Marshal.GetDelegateForFunctionPointer(GetProcAddress(plugin.dllHandle, "PluginShutdown"), typeof(PluginShutdown));
            plugin.dllStartup(CoreDll, null, null);

            m64p_error result = m64pCoreAttachPlugin(type, plugin.dllHandle);
            if (result != m64p_error.M64ERR_SUCCESS)
            {
                FreeLibrary(plugin.dllHandle);
                throw new InvalidOperationException(string.Format("Error during attaching plugin {0}", PluginName));
            }

            plugins.Add(type, plugin);
            return plugin.dllHandle;
        }

        public void DetachPlugin(m64p_plugin_type type)
        {
            AttachedPlugin plugin;
            if (plugins.TryGetValue(type, out plugin))
            {
                plugins.Remove(type);
                m64pCoreDetachPlugin(type);
                plugin.dllShutdown();
                FreeLibrary(plugin.dllHandle);
            }
        }

        public event Action FrameFinished;
        public event Action VInterrupt;
        public event Action BeforeRender;

        private void FireFrameFinishedEvent()
        {
            // Execute Frame Callback functions
            if (FrameFinished != null)
                FrameFinished();
        }

        private void FireVIEvent()
        {
            // Execute VI Callback functions
            if (VInterrupt != null)
                VInterrupt();
            m64pFrameComplete.Set();
        }

        private void FireRenderEvent()
        {
            if (BeforeRender != null)
                BeforeRender();
        }

        private void CompletedFrameCallback()
        {
            m64pFrameComplete.Set();
        }
    }
}
