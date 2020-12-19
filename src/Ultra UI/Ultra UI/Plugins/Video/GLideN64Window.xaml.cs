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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing.Text;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using ViewModel;

namespace Ultra
{
    /// <summary>
    /// Interaction logic for GLideN64Window.xaml
    /// </summary>
    public partial class GLideN64Window : Window
    {
        public GLideN64Window()
        {
            InitializeComponent();

            MinWidth = 650;
            MinHeight = 505;

            // Load Control Values
            PluginCfgReader();

            // -------------------------
            // Load Fonts
            // -------------------------
            //PluginsVideoView.installedFonts = Directory
            //                                 .EnumerateFiles(@"C:\Windows\Fonts\")
            //                                 .Select(System.IO.Path.GetFileName)
            //                                 .ToList();

            //foreach (System.Drawing.FontFamily font in VM.PluginsVideoView.installedFonts.Families)
            //{
            //    if (!string.IsNullOrEmpty(font.Name))
            //    {
            //        VM.Plugins_Video_GLideN64_View.FontName_Items.Add(font.Name);
            //    }
            //}
        }

        /// <summary>
        /// Window Loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load Fonts
            Plugins_Video_GLideN64_ViewModel.installedFonts = Directory
                                            .EnumerateFiles(@"C:\Windows\Fonts\")
                                            .Select(System.IO.Path.GetFileName)
                                            .ToList();

            VM.Plugins_Video_GLideN64_View.Fonts_Items = new ObservableCollection<string>(Plugins_Video_GLideN64_ViewModel.installedFonts);
        }

        /// <summary>
        /// Window Closing
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
            // Reset Save ✓ Label
            VM.Plugins_Video_GLideN64_View.Save_Text = "";
        }


        /// <summary>
        /// TxPath Browse
        /// </summary>
        private void btnTxPathBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.Plugins_Video_GLideN64_View.TxPath_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.Plugins_Video_GLideN64_View.TxPath_Text = VM.Plugins_Video_GLideN64_View.TxPath_Text.Replace(@"\\", @"\");
            }
        }

        /// <summary>
        /// TxCachePath Browse
        /// </summary>
        private void btnTxCachePathBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.Plugins_Video_GLideN64_View.TxCachePath_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.Plugins_Video_GLideN64_View.TxCachePath_Text = VM.Plugins_Video_GLideN64_View.TxCachePath_Text.Replace(@"\\", @"\");
            }
        }

        /// <summary>
        /// TxDumpPath Browse
        /// </summary>
        private void btnTxDumpPathBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Open 'Select Folder'
            System.Windows.Forms.FolderBrowserDialog outputFolder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = outputFolder.ShowDialog();

            // Process Dialog Box
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Display path and file in Output Textbox
                VM.Plugins_Video_GLideN64_View.TxDumpPath_Text = outputFolder.SelectedPath.TrimEnd('\\') + @"\";

                // Remove Double Slash in Root Dir, such as C:\
                VM.Plugins_Video_GLideN64_View.TxDumpPath_Text = VM.Plugins_Video_GLideN64_View.TxDumpPath_Text.Replace(@"\\", @"\");
            }
        }


        /// <summary>
        /// Plugin Cfg Reader
        /// </summary>
        /// <remarks>
        /// Import Control Values from mupen64plus.cfg
        /// </remarks>
        private static void PluginCfgReader()
        {
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

                        // --------------------------------------------------
                        // [Video-GLideN64]
                        // --------------------------------------------------

                        // -------------------------
                        // General
                        // -------------------------
                        // Version
                        // # Settings version. Don't touch it.
                        VM.Plugins_Video_GLideN64_View.Version_Text = cfg.Read("Video-GLideN64", "configVersion");

                        // Multisampling
                        // # Enable/Disable MultiSampling (0=off, 2,4,8,16=quality)
                        VM.Plugins_Video_GLideN64_View.MultiSampling_SelectedItem = cfg.Read("Video-GLideN64", "MultiSampling");

                        // Aspect Ratio
                        // # Screen aspect ratio (0=stretch, 1=force 4:3, 2=force 16:9, 3=adjust)
                        string aspectRatio = cfg.Read("Video-GLideN64", "AspectRatio");
                        switch (aspectRatio)
                        {
                            case "0":
                                VM.Plugins_Video_GLideN64_View.AspectRatio_SelectedItem = "Stretch";
                                break;

                            case "1":
                                VM.Plugins_Video_GLideN64_View.AspectRatio_SelectedItem = "Force 4:3";
                                break;

                            case "2":
                                VM.Plugins_Video_GLideN64_View.AspectRatio_SelectedItem = "Force 16:9";
                                break;

                            case "3":
                                VM.Plugins_Video_GLideN64_View.AspectRatio_SelectedItem = "Adjust";
                                break;
                        }

                        // Bilinear Mode
                        // # Bilinear filtering mode (0=N64 3point, 1=standard)
                        bool bilinearMode = true;
                        bool.TryParse(cfg.Read("Video-GLideN64", "bilinearMode").ToLower(), out bilinearMode);
                        switch (bilinearMode)
                        {
                            case false:
                                VM.Plugins_Video_GLideN64_View.BilinearMode_SelectedItem = "N64 3-Point";
                                break;

                            case true:
                                VM.Plugins_Video_GLideN64_View.BilinearMode_SelectedItem = "Standard";
                                break;
                        }

                        // Max Anisotropy
                        // # Max level of Anisotropic Filtering, 0 for off
                        VM.Plugins_Video_GLideN64_View.MaxAnisotropy_SelectedItem = cfg.Read("Video-GLideN64", "MaxAnisotropy");
                        //bool maxAnisotropy = false;
                        //bool.TryParse(cfg.Read("Video-GLideN64", "MaxAnisotropy").ToLower(), out maxAnisotropy);
                        //if (maxAnisotropy == false)
                        //{
                        //    VM.Plugins_Video_GLideN64_View.MaxAnisotropy_SelectedItem = "Off";
                        //}
                        //else if (maxAnisotropy == true)
                        //{
                        //    VM.Plugins_Video_GLideN64_View.MaxAnisotropy_SelectedItem = "On";
                        //}

                        // ShowFPS
                        bool showFPS = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "ShowFPS").ToLower(), out showFPS);
                        VM.Plugins_Video_GLideN64_View.ShowFPS_IsChecked = showFPS;

                        // ShowVIS
                        bool showVIS = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "ShowVIS").ToLower(), out showVIS);
                        VM.Plugins_Video_GLideN64_View.ShowVIS_IsChecked = showVIS;

                        // FXAA
                        bool fxaa = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "FXAA").ToLower(), out fxaa);
                        VM.Plugins_Video_GLideN64_View.FXAA_IsChecked = fxaa;


                        // -------------------------
                        // Render
                        // -------------------------
                        // Enable Fog
                        //bool enableFog = true;
                        //bool.TryParse(cfg.Read("Video-GLideN64", "EnableFog").ToLower(), out enableFog);
                        //VM.Plugins_Video_GLideN64_View.EnableFog_IsChecked = enableFog;

                        // Enable Noise
                        bool enableNoise = true;
                        bool.TryParse(cfg.Read("Video-GLideN64", "EnableNoise").ToLower(), out enableNoise);
                        VM.Plugins_Video_GLideN64_View.EnableNoise_IsChecked = enableNoise;

                        // EnableHalosRemoval
                        bool enableHalosRemoval = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "EnableHalosRemoval").ToLower(), out enableHalosRemoval);
                        VM.Plugins_Video_GLideN64_View.EnableHalosRemoval_IsChecked = enableHalosRemoval;

                        // EnableLOD
                        bool enableLOD = true;
                        bool.TryParse(cfg.Read("Video-GLideN64", "EnableLOD").ToLower(), out enableLOD);
                        VM.Plugins_Video_GLideN64_View.EnableLOD_IsChecked = enableLOD;

                        // EnableHWLighting
                        bool enableHWLighting = true;
                        bool.TryParse(cfg.Read("Video-GLideN64", "EnableHWLighting").ToLower(), out enableHWLighting);
                        VM.Plugins_Video_GLideN64_View.EnableHWLighting_IsChecked = enableHWLighting;

                        // CorrectTexrectCoords
                        // # Make texrect coordinates continuous to avoid black lines between them. (0=Off, 1=Auto, 2=Force)
                        string correctTexrectCoords = cfg.Read("Video-GLideN64", "CorrectTexrectCoords");
                        switch (correctTexrectCoords)
                        {
                            case "0":
                                VM.Plugins_Video_GLideN64_View.CorrectTexrectCoords_IsChecked = false;
                                break;

                            case "2":
                                VM.Plugins_Video_GLideN64_View.CorrectTexrectCoords_IsChecked = true;
                                break;

                            //default:
                            //    VM.Plugins_Video_GLideN64_View.CorrectTexrectCoords_IsChecked = false;
                            //    break;
                        }

                        // EnableShadersStorage
                        bool enableShadersStorage = true;
                        bool.TryParse(cfg.Read("Video-GLideN64", "EnableShadersStorage").ToLower(), out enableShadersStorage);
                        VM.Plugins_Video_GLideN64_View.EnableShadersStorage_IsChecked = enableShadersStorage;

                        //bool correctTexrectCoords = true;
                        //bool.TryParse(cfg.Read("Video-GLideN64", "CorrectTexrectCoords").ToLower(), out correctTexrectCoords);
                        //VM.Plugins_Video_GLideN64_View.CorrectTexrectCoords_IsChecked = correctTexrectCoords;

                        //if (correctTexrectCoords == false)
                        //{
                        //    VM.Plugins_Video_GLideN64_View.CorrectTexrectCoords_IsChecked = "0";
                        //}
                        //else if (correctTexrectCoords == true)
                        //{
                        //    VM.Plugins_Video_GLideN64_View.CorrectTexrectCoords_Text = "2";
                        //}

                        // -------------------------
                        // Bloom
                        // -------------------------
                        // Bloom Threshold Level
                        // # Brightness threshold level for bloom. Values [2, 6]
                        //VM.Plugins_Video_GLideN64_View.BloomThresholdLevel_Text = cfg.Read("Video-GLideN64", "bloomThresholdLevel");

                        // Bloom Blend Mode
                        // # (0=Strong, 1=Mild, 2=Light)
                        //string bloomBlendMode = cfg.Read("Video-GLideN64", "bloomBlendMode");
                        //if (bloomBlendMode == "0")
                        //{
                        //    VM.Plugins_Video_GLideN64_View.BloomBlendMode_SelectedItem = "Strong";
                        //}
                        //else if (bloomBlendMode == "1")
                        //{
                        //    VM.Plugins_Video_GLideN64_View.BloomBlendMode_SelectedItem = "Mild";
                        //}
                        //else if (bloomBlendMode == "2")
                        //{
                        //    VM.Plugins_Video_GLideN64_View.BloomBlendMode_SelectedItem = "Light";
                        //}

                        // Blur Amount
                        // # Blur radius. Values [2, 10]
                        //VM.Plugins_Video_GLideN64_View.BlurAmount_Text = cfg.Read("Video-GLideN64", "blurAmount");

                        //// Blur Strength
                        //// # Blur strength. Values [10, 100]
                        //VM.Plugins_Video_GLideN64_View.BlurStrength_Text = cfg.Read("Video-GLideN64", "blurStrength");

                        //// EnableBloom
                        //bool enableBloom = true;
                        //bool.TryParse(cfg.Read("Video-GLideN64", "EnableBloom").ToLower(), out enableBloom);
                        //VM.Plugins_Video_GLideN64_View.EnableBloom_IsChecked = enableBloom;


                        // -------------------------
                        // Other
                        // -------------------------
                        // EnableFBEmulation
                        bool enableFBEmulation = true;
                        bool.TryParse(cfg.Read("Video-GLideN64", "EnableFBEmulation").ToLower(), out enableFBEmulation);
                        VM.Plugins_Video_GLideN64_View.EnableFBEmulation_IsChecked = enableFBEmulation;

                        // DisableFBInfo
                        bool disableFBInfo = true;
                        bool.TryParse(cfg.Read("Video-GLideN64", "DisableFBInfo").ToLower(), out disableFBInfo);
                        VM.Plugins_Video_GLideN64_View.DisableFBInfo_IsChecked = disableFBInfo;

                        // ForceDepthBufferClear
                        bool forceDepthBufferClear = true;
                        bool.TryParse(cfg.Read("Video-GLideN64", "ForceDepthBufferClear").ToLower(), out forceDepthBufferClear);
                        VM.Plugins_Video_GLideN64_View.ForceDepthBufferClear_IsChecked = forceDepthBufferClear;

                        // FBInfoReadColorChunk
                        bool fbInfoReadColorChunk = true;
                        bool.TryParse(cfg.Read("Video-GLideN64", "FBInfoReadColorChunk").ToLower(), out fbInfoReadColorChunk);
                        VM.Plugins_Video_GLideN64_View.FBInfoReadColorChunk_IsChecked = fbInfoReadColorChunk;

                        // FBInfoReadDepthChunk
                        bool fbInfoReadDepthChunk = true;
                        bool.TryParse(cfg.Read("Video-GLideN64", "FBInfoReadDepthChunk").ToLower(), out fbInfoReadDepthChunk);
                        VM.Plugins_Video_GLideN64_View.FBInfoReadDepthChunk_IsChecked = fbInfoReadDepthChunk;

                        // EnableCopyColorToRDRAM
                        // # Enable color buffer copy to RDRAM (0=do not copy, 1=copy in sync mode, 2=Double Buffer, 3=Triple Buffer)
                        string enableCopyColorToRDRAM = cfg.Read("Video-GLideN64", "EnableCopyColorToRDRAM");
                        switch (enableCopyColorToRDRAM)
                        {
                            case "0":
                                VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_SelectedItem = "Do Not Copy";
                                break;

                            case "1":
                                VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_SelectedItem = "Copy in Sync Mode";
                                break;

                            case "2":
                                VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_SelectedItem = "Double Buffer";
                                break;

                            case "3":
                                VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_SelectedItem = "Triple Buffer";
                                break;
                        }

                        //bool enableCopyColorToRDRAM = true;
                        //bool.TryParse(cfg.Read("Video-GLideN64", "EnableCopyColorToRDRAM").ToLower(), out enableCopyColorToRDRAM);
                        //VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_IsChecked = enableCopyColorToRDRAM;

                        // EnableCopyDepthToRDRAM
                        // # Enable depth buffer copy to RDRAM  (0=do not copy, 1=copy from video memory, 2=use software render)
                        string enableCopyDepthToRDRAM = cfg.Read("Video-GLideN64", "EnableCopyDepthToRDRAM");
                        switch (enableCopyDepthToRDRAM)
                        {
                            case "0":
                                VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_SelectedItem = "Do Not Copy";
                                break;

                            case "1":
                                VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_SelectedItem = "Copy From Video Memory";
                                break;

                            case "2":
                                VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_SelectedItem = "Use Software Render";
                                break;
                        }

                        //bool enableCopyDepthToRDRAM = true;
                        //bool.TryParse(cfg.Read("Video-GLideN64", "EnableCopyDepthToRDRAM").ToLower(), out enableCopyDepthToRDRAM);
                        //VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_IsChecked = enableCopyDepthToRDRAM;

                        // EnableCopyAuxiliaryToRDRAM
                        bool enableCopyAuxiliaryToRDRAM = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "EnableCopyAuxiliaryToRDRAM").ToLower(), out enableCopyAuxiliaryToRDRAM);
                        VM.Plugins_Video_GLideN64_View.EnableCopyAuxiliaryToRDRAM_IsChecked = enableCopyAuxiliaryToRDRAM;

                        // EnableCopyColorFromRDRAM
                        bool enableCopyColorFromRDRAM = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "EnableCopyColorFromRDRAM").ToLower(), out enableCopyColorFromRDRAM);
                        VM.Plugins_Video_GLideN64_View.EnableCopyColorFromRDRAM_IsChecked = enableCopyColorFromRDRAM;

                        // EnableDetectCFB
                        //bool enableDetectCFB = false;
                        //bool.TryParse(cfg.Read("Video-GLideN64", "EnableDetectCFB").ToLower(), out enableDetectCFB);
                        //VM.Plugins_Video_GLideN64_View.EnableDetectCFB_IsChecked = enableDetectCFB;

                        // EnableN64DepthCompare
                        bool enableN64DepthCompare = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "EnableN64DepthCompare").ToLower(), out enableN64DepthCompare);
                        VM.Plugins_Video_GLideN64_View.EnableN64DepthCompare_IsChecked = enableN64DepthCompare;


                        // -------------------------
                        // Texture
                        // -------------------------
                        // Cache Size
                        //VM.Plugins_Video_GLideN64_View.CacheSize_Text = cfg.Read("Video-GLideN64", "CacheSize");

                        // Tx Cache Size
                        VM.Plugins_Video_GLideN64_View.TxCacheSize_Text = cfg.Read("Video-GLideN64", "txCacheSize");

                        // Tx Filter Mode
                        string txFilterMode = cfg.Read("Video-GLideN64", "txFilterMode");
                        switch (txFilterMode)
                        {
                            case "0":
                                VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem = "None";
                                break;

                            case "1":
                                VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem = "Smooth 1";
                                break;

                            case "2":
                                VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem = "Smooth 2";
                                break;

                            case "3":
                                VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem = "Smooth 3";
                                break;

                            case "4":
                                VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem = "Smooth 4";
                                break;

                            case "5":
                                VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem = "Sharp 1";
                                break;

                            case "6":
                                VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem = "Sharp 2";
                                break;
                        }

                        // Tx Enhancement Mode
                        // # Texture Enhancement (0=none, 1=store as is, 2=X2, 3=X2SAI, 4=HQ2X, 5=HQ2XS, 6=LQ2X, 7=LQ2XS, 8=HQ4X, 9=2xBRZ, 10=3xBRZ, 11=4xBRZ, 12=5xBRZ)
                        string txEnhancementMode = cfg.Read("Video-GLideN64", "txEnhancementMode");
                        switch (txEnhancementMode)
                        {
                            case "0":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "None";
                                break;

                            case "1":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "Store";
                                break;

                            case "2":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "X2";
                                break;

                            case "3":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "X2SAI";
                                break;

                            case "4":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "HQ2X";
                                break;

                            case "5":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "HQ2XS";
                                break;

                            case "6":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "LQ2X";
                                break;

                            case "7":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "LQ2XS";
                                break;

                            case "8":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "HQ4X";
                                break;

                            case "9":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "2xBRZ";
                                break;

                            case "10":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "3xBRZ";
                                break;

                            case "11":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "4xBRZ";
                                break;

                            case "12":
                                VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "5xBRZ";
                                break;
                        }

                        // TxFilterIgnoreBG
                        bool txFilterIgnoreBG = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "txFilterIgnoreBG").ToLower(), out txFilterIgnoreBG);
                        VM.Plugins_Video_GLideN64_View.TxFilterIgnoreBG_IsChecked = txFilterIgnoreBG;

                        // TxDeposterize
                        bool txDeposterize = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "txDeposterize").ToLower(), out txDeposterize);
                        VM.Plugins_Video_GLideN64_View.TxDeposterize_IsChecked = txDeposterize;

                        // TxHiresEnable
                        bool txHiresEnable = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "txHiresEnable").ToLower(), out txHiresEnable);
                        VM.Plugins_Video_GLideN64_View.TxHiresEnable_IsChecked = txHiresEnable;

                        // TxHiresFullAlphaChannel
                        bool txHiresFullAlphaChannel = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "txHiresFullAlphaChannel").ToLower(), out txHiresFullAlphaChannel);
                        VM.Plugins_Video_GLideN64_View.TxHiresFullAlphaChannel_IsChecked = txHiresFullAlphaChannel;

                        // TxHresAltCRC
                        bool txHresAltCRC = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "txHresAltCRC").ToLower(), out txHresAltCRC);
                        VM.Plugins_Video_GLideN64_View.TxHresAltCRC_IsChecked = txHresAltCRC;

                        // TxDump
                        bool txDump = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "txDump").ToLower(), out txDump);
                        VM.Plugins_Video_GLideN64_View.TxDump_IsChecked = txDump;

                        // TxCacheCompression
                        bool txCacheCompression = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "txCacheCompression").ToLower(), out txCacheCompression);
                        VM.Plugins_Video_GLideN64_View.TxCacheCompression_IsChecked = txCacheCompression;

                        // TxForce16bpp
                        bool txForce16bpp = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "txForce16bpp").ToLower(), out txForce16bpp);
                        VM.Plugins_Video_GLideN64_View.TxForce16bpp_IsChecked = txForce16bpp;

                        // TxSaveCache
                        bool txSaveCache = false;
                        bool.TryParse(cfg.Read("Video-GLideN64", "txSaveCache").ToLower(), out txSaveCache);
                        VM.Plugins_Video_GLideN64_View.TxSaveCache_IsChecked = txSaveCache;

                        // TxPath
                        // # Path to folder with hi-res texture packs.
                        VM.Plugins_Video_GLideN64_View.TxPath_Text = cfg.Read("Video-GLideN64", "txPath");

                        // TxCachePath
                        // # Path to folder where plugin saves texture cache files.
                        VM.Plugins_Video_GLideN64_View.TxCachePath_Text = cfg.Read("Video-GLideN64", "txCachePath");

                        // TxDumpPath
                        // # Path to folder where plugin saves dumped textures.
                        VM.Plugins_Video_GLideN64_View.TxDumpPath_Text = cfg.Read("Video-GLideN64", "txDumpPath");

                        // ValidityCheckMethod
                        //bool validityCheckMethod = false;
                        //bool.TryParse(cfg.Read("Video-GLideN64", "validityCheckMethod").ToLower(), out validityCheckMethod);
                        //VM.Plugins_Video_GLideN64_View.ValidityCheckMethod_IsChecked = validityCheckMethod;

                        // Font Name
                        VM.Plugins_Video_GLideN64_View.Fonts_SelectedItem = cfg.Read("Video-GLideN64", "fontName");

                        // Font Size
                        VM.Plugins_Video_GLideN64_View.FontSize_Text = cfg.Read("Video-GLideN64", "fontSize");

                        // Font Color
                        VM.Plugins_Video_GLideN64_View.FontColor_Text = cfg.Read("Video-GLideN64", "fontColor");

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
        /// Save
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Check if Paths Config TextBox is Empty
            if (MainWindow.IsValidPath(VM.PathsView.Config_Text))
            {
                // Check if Cfg File Exists
                if (File.Exists(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg")))
                {
                    // Save to mupen64plus.cfg
                    Configure.ConigFile cfg = null;

                    try
                    {
                        cfg = new Configure.ConigFile(Path.Combine(VM.PathsView.Config_Text, "mupen64plus.cfg"));

                        // --------------------------------------------------
                        // [Video-GLideN64]
                        // --------------------------------------------------
                        // -------------------------
                        // General
                        // -------------------------
                        // Version
                        // # Settings version. Don't touch it.
                        cfg.Write("Video-GLideN64", "configVersion", VM.Plugins_Video_GLideN64_View.Version_Text);

                        // Multisampling
                        // # Enable/Disable MultiSampling (0=off, 2,4,8,16=quality)
                        cfg.Write("Video-GLideN64", "MultiSampling", VM.Plugins_Video_GLideN64_View.MultiSampling_SelectedItem);

                        // AspectRatio
                        // # Screen aspect ratio (0=stretch, 1=force 4:3, 2=force 16:9, 3=adjust)
                        switch (VM.Plugins_Video_GLideN64_View.AspectRatio_SelectedItem)
                        {
                            case "Stretch":
                                cfg.Write("Video-GLideN64", "AspectRatio", "0");
                                break;

                            case "Force 4:3":
                                cfg.Write("Video-GLideN64", "AspectRatio", "1");
                                break;

                            case "Force 16:9":
                                cfg.Write("Video-GLideN64", "AspectRatio", "2");
                                break;

                            case "Adjust":
                                cfg.Write("Video-GLideN64", "AspectRatio", "3");
                                break;
                        }

                        // BilinearMode
                        // # Bilinear filtering mode (0=N64 3point, 1=standard)
                        switch (VM.Plugins_Video_GLideN64_View.BilinearMode_SelectedItem)
                        {
                            case "N64 3-Point":
                                cfg.Write("Video-GLideN64", "bilinearMode", "False");
                                break;

                            case "Standard":
                                cfg.Write("Video-GLideN64", "bilinearMode", "True");
                                break;
                        }

                        // MaxAnisotropy
                        // # Max level of Anisotropic Filtering, 0 for off
                        cfg.Write("Video-GLideN64", "MaxAnisotropy", VM.Plugins_Video_GLideN64_View.MaxAnisotropy_SelectedItem);
                        //if (VM.Plugins_Video_GLideN64_View.MaxAnisotropy_SelectedItem == "Off")
                        //{
                        //    cfg.Write("Video-GLideN64", "MaxAnisotropy", "False");
                        //}
                        //else if (VM.Plugins_Video_GLideN64_View.MaxAnisotropy_SelectedItem == "On")
                        //{
                        //    cfg.Write("Video-GLideN64", "MaxAnisotropy", "True");
                        //}

                        // ShowFPS
                        switch (VM.Plugins_Video_GLideN64_View.ShowFPS_IsChecked)
                        {
                            case true:
                                cfg.Write("Video-GLideN64", "ShowFPS", "True");
                                break;

                            case false:
                                cfg.Write("Video-GLideN64", "ShowFPS", "False");
                                break;
                        }

                        // ShowVIS
                        switch (VM.Plugins_Video_GLideN64_View.ShowVIS_IsChecked)
                        {
                            case true:
                                cfg.Write("Video-GLideN64", "ShowVIS", "True");
                                break;

                            case false:
                                cfg.Write("Video-GLideN64", "ShowVIS", "False");
                                break;
                        }

                        // FXAA
                        switch (VM.Plugins_Video_GLideN64_View.FXAA_IsChecked)
                        {
                            case true:
                                cfg.Write("Video-GLideN64", "FXAA", "True");
                                break;

                            case false:
                                cfg.Write("Video-GLideN64", "FXAA", "False");
                                break;
                        }

                        // -------------------------
                        // Render
                        // -------------------------
                        //// Enable Fog
                        //if (VM.Plugins_Video_GLideN64_View.EnableFog_IsChecked == true)
                        //{
                        //    cfg.Write("Video-GLideN64", "EnableFog", "True");
                        //}
                        //else if (VM.Plugins_Video_GLideN64_View.EnableFog_IsChecked == false)
                        //{
                        //    cfg.Write("Video-GLideN64", "EnableFog", "False");
                        //}
                        // EnableHalosRemoval
                        switch (VM.Plugins_Video_GLideN64_View.EnableHalosRemoval_IsChecked)
                        {
                            case true:
                                cfg.Write("Video-GLideN64", "EnableHalosRemoval", "True");
                                break;

                            case false:
                                cfg.Write("Video-GLideN64", "EnableHalosRemoval", "False");
                                break;
                        }


                        // EnableNoise
                        switch (VM.Plugins_Video_GLideN64_View.EnableNoise_IsChecked)
                        {
                            case true:
                                cfg.Write("Video-GLideN64", "EnableNoise", "True");
                                break;

                            case false:
                                cfg.Write("Video-GLideN64", "EnableNoise", "False");
                                break;
                        }

                        // EnableLOD
                        switch (VM.Plugins_Video_GLideN64_View.EnableLOD_IsChecked)
                        {
                            case true:
                                cfg.Write("Video-GLideN64", "EnableLOD", "True");
                                break;

                            case false:
                                cfg.Write("Video-GLideN64", "EnableLOD", "False");
                                break;
                        }

                        // EnableHWLighting
                        switch (VM.Plugins_Video_GLideN64_View.EnableHWLighting_IsChecked)
                        {
                            case true:
                                cfg.Write("Video-GLideN64", "EnableHWLighting", "True");
                                break;

                            case false:
                                cfg.Write("Video-GLideN64", "EnableHWLighting", "False");
                                break;
                        }

                        // CorrectTexrectCoords
                        // # Make texrect coordinates continuous to avoid black lines between them. (0=Off, 1=Auto, 2=Force)
                        switch (VM.Plugins_Video_GLideN64_View.CorrectTexrectCoords_IsChecked)
                        {
                            case true:
                                cfg.Write("Video-GLideN64", "CorrectTexrectCoords", "2");
                                break;

                            case false:
                                cfg.Write("Video-GLideN64", "CorrectTexrectCoords", "0");
                                break;
                        }

                        // EnableShadersStorage
                        switch (VM.Plugins_Video_GLideN64_View.EnableShadersStorage_IsChecked)
                        {
                            case true:
                                cfg.Write("Video-GLideN64", "EnableShadersStorage", "True");
                                break;

                            case false:
                                cfg.Write("Video-GLideN64", "EnableShadersStorage", "False");
                                break;
                        }

                        // -------------------------
                        // Bloom
                        // -------------------------
                        // Bloom Threshold Level
                        // # Brightness threshold level for bloom. Values [2, 6]
                        //cfg.Write("Video-GLideN64", "bloomThresholdLevel", VM.Plugins_Video_GLideN64_View.BloomThresholdLevel_Text);

                        //// Bloom Blend Mode
                        //// # (0=Strong, 1=Mild, 2=Light)
                        //if (VM.Plugins_Video_GLideN64_View.BloomBlendMode_SelectedItem == "Strong")
                        //{
                        //    cfg.Write("Video-GLideN64", "bloomBlendMode", "0");
                        //}
                        //else if (VM.Plugins_Video_GLideN64_View.BloomBlendMode_SelectedItem == "Mild")
                        //{
                        //    cfg.Write("Video-GLideN64", "bloomBlendMode", "1");
                        //}
                        //else if (VM.Plugins_Video_GLideN64_View.BloomBlendMode_SelectedItem == "Light")
                        //{
                        //    cfg.Write("Video-GLideN64", "bloomBlendMode", "0");
                        //}

                        //// Blur Amount
                        //// # Blur radius. Values [2, 10]
                        //cfg.Write("Video-GLideN64", "blurAmount", VM.Plugins_Video_GLideN64_View.BlurAmount_Text);

                        //// Blur Strength
                        //// # Blur strength. Values [10, 100]
                        //cfg.Write("Video-GLideN64", "blurStrength", VM.Plugins_Video_GLideN64_View.BlurStrength_Text);

                        //// EnableBloom
                        //if (VM.Plugins_Video_GLideN64_View.EnableBloom_IsChecked == true)
                        //{
                        //    cfg.Write("Video-GLideN64", "EnableBloom", "1");
                        //}
                        //else if (VM.Plugins_Video_GLideN64_View.EnableBloom_IsChecked == false)
                        //{
                        //    cfg.Write("Video-GLideN64", "EnableBloom", "0");
                        //}


                        // -------------------------
                        // Other
                        // -------------------------
                        // EnableFBEmulation
                        switch (VM.Plugins_Video_GLideN64_View.EnableFBEmulation_IsChecked)
                        {
                            case true:
                                cfg.Write("Video-GLideN64", "EnableFBEmulation", "True");
                                break;

                            case false:
                                cfg.Write("Video-GLideN64", "EnableFBEmulation", "False");
                                break;
                        }

                        // DisableFBInfo
                        if (VM.Plugins_Video_GLideN64_View.DisableFBInfo_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "DisableFBInfo", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.DisableFBInfo_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "DisableFBInfo", "False");
                        }

                        // ForceDepthBufferClear
                        if (VM.Plugins_Video_GLideN64_View.ForceDepthBufferClear_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "ForceDepthBufferClear", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.ForceDepthBufferClear_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "ForceDepthBufferClear", "False");
                        }

                        // FBInfoReadColorChunk
                        if (VM.Plugins_Video_GLideN64_View.FBInfoReadColorChunk_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "FBInfoReadColorChunk", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.FBInfoReadColorChunk_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "FBInfoReadColorChunk", "False");
                        }

                        // FBInfoReadDepthChunk
                        if (VM.Plugins_Video_GLideN64_View.FBInfoReadDepthChunk_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "FBInfoReadDepthChunk", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.FBInfoReadDepthChunk_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "FBInfoReadDepthChunk", "False");
                        }


                        // EnableCopyColorToRDRAM
                        // # Enable color buffer copy to RDRAM (0=do not copy, 1=copy in sync mode, 2=Double Buffer, 3=Triple Buffer)
                        if (VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_SelectedItem == "Do Not Copy")
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyColorToRDRAM", "0");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_SelectedItem == "Copy in Sync Mode")
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyColorToRDRAM", "1");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_SelectedItem == "Double Buffer")
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyColorToRDRAM", "2");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_SelectedItem == "Triple Buffer")
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyColorToRDRAM", "3");
                        }
                        //if (VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_IsChecked == true)
                        //{
                        //    cfg.Write("Video-GLideN64", "EnableCopyColorToRDRAM", "True");
                        //}
                        //else if (VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_IsChecked == false)
                        //{
                        //    cfg.Write("Video-GLideN64", "EnableCopyColorToRDRAM", "False");
                        //}

                        // EnableCopyDepthToRDRAM
                        // # Enable depth buffer copy to RDRAM  (0=do not copy, 1=copy from video memory, 2=use software render)
                        if (VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_SelectedItem == "Do Not Copy")
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyDepthToRDRAM", "0");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_SelectedItem == "Copy From Video Memory")
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyDepthToRDRAM", "1");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_SelectedItem == "Use Software Render")
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyDepthToRDRAM", "2");
                        }
                        //if (VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_IsChecked == true)
                        //{
                        //    cfg.Write("Video-GLideN64", "EnableCopyDepthToRDRAM", "True");
                        //}
                        //else if (VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_IsChecked == false)
                        //{
                        //    cfg.Write("Video-GLideN64", "EnableCopyDepthToRDRAM", "False");
                        //}

                        // EnableCopyAuxiliaryToRDRAM
                        if (VM.Plugins_Video_GLideN64_View.EnableCopyAuxiliaryToRDRAM_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyAuxiliaryToRDRAM", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.EnableCopyAuxiliaryToRDRAM_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyAuxiliaryToRDRAM", "False");
                        }

                        // EnableCopyColorFromRDRAM
                        if (VM.Plugins_Video_GLideN64_View.EnableCopyColorFromRDRAM_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyDepthToRDRAM", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.EnableCopyColorFromRDRAM_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "EnableCopyColorFromRDRAM", "False");
                        }

                        // EnableDetectCFB
                        //if (VM.Plugins_Video_GLideN64_View.EnableDetectCFB_IsChecked == true)
                        //{
                        //    cfg.Write("Video-GLideN64", "EnableDetectCFB", "True");
                        //}
                        //else if (VM.Plugins_Video_GLideN64_View.EnableDetectCFB_IsChecked == false)
                        //{
                        //    cfg.Write("Video-GLideN64", "EnableDetectCFB", "False");
                        //}

                        // EnableN64DepthCompare
                        if (VM.Plugins_Video_GLideN64_View.EnableN64DepthCompare_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "EnableN64DepthCompare", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.EnableN64DepthCompare_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "EnableN64DepthCompare", "False");
                        }


                        // -------------------------
                        // Texture
                        // -------------------------
                        // Cache Size
                        //cfg.Write("Video-GLideN64", "CacheSize", VM.Plugins_Video_GLideN64_View.CacheSize_Text);

                        // txCacheSize
                        cfg.Write("Video-GLideN64", "txCacheSize", VM.Plugins_Video_GLideN64_View.TxCacheSize_Text);

                        // txFilterMode
                        if (VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem == "None")
                        {
                            cfg.Write("Video-GLideN64", "txFilterMode", "0");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem == "Smooth 1")
                        {
                            cfg.Write("Video-GLideN64", "txFilterMode", "1");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem == "Smooth 2")
                        {
                            cfg.Write("Video-GLideN64", "txFilterMode", "2");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem == "Smooth 3")
                        {
                            cfg.Write("Video-GLideN64", "txFilterMode", "3");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem == "Smooth 4")
                        {
                            cfg.Write("Video-GLideN64", "txFilterMode", "4");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem == "Sharp 1")
                        {
                            cfg.Write("Video-GLideN64", "txFilterMode", "5");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem == "Sharp 2")
                        {
                            cfg.Write("Video-GLideN64", "txFilterMode", "6");
                        }

                        // txEnhancementMode
                        // # Texture Enhancement (0=none, 1=store as is, 2=X2, 3=X2SAI, 4=HQ2X, 5=HQ2XS, 6=LQ2X, 7=LQ2XS, 8=HQ4X, 9=2xBRZ, 10=3xBRZ, 11=4xBRZ, 12=5xBRZ)
                        if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "None")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "0");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "Store")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "1");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "X2")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "2");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "X2SAI")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "3");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "HQ2X")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "4");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "HQ2XS")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "5");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "LQ2X")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "6");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "LQ2XS")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "7");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "HQ4X")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "8");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "2xBRZ")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "9");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "3xBRZ")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "10");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "4xBRZ")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "11");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem == "5xBRZ")
                        {
                            cfg.Write("Video-GLideN64", "txEnhancementMode", "12");
                        }

                        // txFilterIgnoreBG
                        if (VM.Plugins_Video_GLideN64_View.TxFilterIgnoreBG_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "txFilterIgnoreBG", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxFilterIgnoreBG_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "txFilterIgnoreBG", "False");
                        }

                        // txDeposterize
                        if (VM.Plugins_Video_GLideN64_View.TxDeposterize_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "txDeposterize", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxDeposterize_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "txDeposterize", "False");
                        }

                        // txHiresEnable
                        if (VM.Plugins_Video_GLideN64_View.TxHiresEnable_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "txHiresEnable", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxHiresEnable_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "txHiresEnable", "False");
                        }

                        // txHiresFullAlphaChannel
                        if (VM.Plugins_Video_GLideN64_View.TxHiresFullAlphaChannel_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "txHiresFullAlphaChannel", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxHiresFullAlphaChannel_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "txHiresFullAlphaChannel", "False");
                        }

                        // txHresAltCRC
                        if (VM.Plugins_Video_GLideN64_View.TxHresAltCRC_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "txHresAltCRC", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxHresAltCRC_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "txHresAltCRC", "False");
                        }

                        // txDump
                        if (VM.Plugins_Video_GLideN64_View.TxDump_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "txDump", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxDump_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "txDump", "False");
                        }

                        // txCacheCompression
                        if (VM.Plugins_Video_GLideN64_View.TxCacheCompression_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "txCacheCompression", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxCacheCompression_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "txCacheCompression", "False");
                        }

                        // txForce16bpp
                        if (VM.Plugins_Video_GLideN64_View.TxForce16bpp_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "txForce16bpp", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxForce16bpp_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "txForce16bpp", "False");
                        }

                        // txSaveCache
                        if (VM.Plugins_Video_GLideN64_View.TxSaveCache_IsChecked == true)
                        {
                            cfg.Write("Video-GLideN64", "txSaveCache", "True");
                        }
                        else if (VM.Plugins_Video_GLideN64_View.TxSaveCache_IsChecked == false)
                        {
                            cfg.Write("Video-GLideN64", "txSaveCache", "False");
                        }

                        // txPath
                        // # Path to folder with hi-res texture packs.
                        cfg.Write("Video-GLideN64", "txPath", "\"" + VM.Plugins_Video_GLideN64_View.TxPath_Text + "\"");

                        // txCachePath
                        // # Path to folder where plugin saves texture cache files.
                        cfg.Write("Video-GLideN64", "txCachePath", "\"" + VM.Plugins_Video_GLideN64_View.TxCachePath_Text + "\"");

                        // txDumpPath
                        // # Path to folder where plugin saves dumped textures.
                        cfg.Write("Video-GLideN64", "txDumpPath", "\"" + VM.Plugins_Video_GLideN64_View.TxDumpPath_Text + "\"");

                        // ValidityCheckMethod
                        //if (VM.Plugins_Video_GLideN64_View.ValidityCheckMethod_IsChecked == true)
                        //{
                        //    cfg.Write("Video-GLideN64", "validityCheckMethod", "True");
                        //}
                        //else if (VM.Plugins_Video_GLideN64_View.ValidityCheckMethod_IsChecked == false)
                        //{
                        //    cfg.Write("Video-GLideN64", "validityCheckMethod", "False");
                        //}


                        // -------------------------
                        // Font
                        // -------------------------
                        // Font Name
                        cfg.Write("Video-GLideN64", "fontName", "\"" + VM.Plugins_Video_GLideN64_View.Fonts_SelectedItem + "\"");

                        // Font Size
                        cfg.Write("Video-GLideN64", "fontSize", VM.Plugins_Video_GLideN64_View.FontSize_Text);

                        // Font Color
                        cfg.Write("Video-GLideN64", "fontColor", "\"" + VM.Plugins_Video_GLideN64_View.FontColor_Text + "\"");


                        // -------------------------
                        // Save Complete
                        // -------------------------
                        VM.Plugins_Video_GLideN64_View.Save_Text = "✓";
                    }
                    // Error
                    catch
                    {
                        VM.Plugins_Video_GLideN64_View.Save_Text = "Error";

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
            // General
            //VM.Plugins_Video_GLideN64_View.Version_Text = "";
            VM.Plugins_Video_GLideN64_View.MultiSampling_SelectedItem = "0";
            VM.Plugins_Video_GLideN64_View.AspectRatio_SelectedItem = "Force 4:3";
            VM.Plugins_Video_GLideN64_View.BilinearMode_SelectedItem = "Standard";
            VM.Plugins_Video_GLideN64_View.MaxAnisotropy_SelectedItem = "16";
            VM.Plugins_Video_GLideN64_View.ShowFPS_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.ShowVIS_IsChecked = false;

            // Render
            VM.Plugins_Video_GLideN64_View.FXAA_IsChecked = false;
            //VM.Plugins_Video_GLideN64_View.EnableFog_IsChecked = true;
            VM.Plugins_Video_GLideN64_View.EnableNoise_IsChecked = true;
            VM.Plugins_Video_GLideN64_View.EnableHalosRemoval_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.EnableLOD_IsChecked = true;
            VM.Plugins_Video_GLideN64_View.EnableHWLighting_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.CorrectTexrectCoords_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.EnableShadersStorage_IsChecked = true;

            // Bloom
            //VM.Plugins_Video_GLideN64_View.BloomThresholdLevel_Text = "4";
            //VM.Plugins_Video_GLideN64_View.BloomBlendMode_SelectedItem = "Strong";
            //VM.Plugins_Video_GLideN64_View.BlurAmount_Text = "10";
            //VM.Plugins_Video_GLideN64_View.BlurStrength_Text = "20";
            //VM.Plugins_Video_GLideN64_View.EnableBloom_IsChecked = false;

            //VM.Plugins_Video_GLideN64_View.EnableDetectCFB_IsChecked = false;
            //VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_IsChecked = true;
            //VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_IsChecked = true;
            VM.Plugins_Video_GLideN64_View.EnableCopyColorToRDRAM_SelectedItem = "Double Buffer";
            VM.Plugins_Video_GLideN64_View.EnableCopyDepthToRDRAM_SelectedItem = "Use Software Render";

            // Texture
            //VM.Plugins_Video_GLideN64_View.CacheSize_Text = "500";
            VM.Plugins_Video_GLideN64_View.TxCacheSize_Text = "100";
            VM.Plugins_Video_GLideN64_View.TxFilterMode_SelectedItem = "None";
            VM.Plugins_Video_GLideN64_View.TxEnhancementMode_SelectedItem = "None";
            VM.Plugins_Video_GLideN64_View.TxFilterIgnoreBG_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.TxDeposterize_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.TxHiresEnable_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.TxHiresFullAlphaChannel_IsChecked = true;
            VM.Plugins_Video_GLideN64_View.TxHresAltCRC_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.TxDump_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.TxCacheCompression_IsChecked = true;
            VM.Plugins_Video_GLideN64_View.TxForce16bpp_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.TxSaveCache_IsChecked = true;
            VM.Plugins_Video_GLideN64_View.ValidityCheckMethod_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.TxPath_Text = VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + "hires_texture";
            VM.Plugins_Video_GLideN64_View.TxCachePath_Text = VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + "cache";
            VM.Plugins_Video_GLideN64_View.TxDumpPath_Text = VM.PathsView.Config_Text.TrimEnd('\\') + @"\" + "texture_dump";

            // Other
            VM.Plugins_Video_GLideN64_View.EnableFBEmulation_IsChecked = true;
            VM.Plugins_Video_GLideN64_View.DisableFBInfo_IsChecked = true;
            VM.Plugins_Video_GLideN64_View.ForceDepthBufferClear_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.FBInfoReadColorChunk_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.FBInfoReadDepthChunk_IsChecked = true;
            VM.Plugins_Video_GLideN64_View.EnableN64DepthCompare_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.EnableCopyAuxiliaryToRDRAM_IsChecked = false;
            VM.Plugins_Video_GLideN64_View.EnableCopyColorFromRDRAM_IsChecked = false;

            // Font
            VM.Plugins_Video_GLideN64_View.Fonts_SelectedItem = "arial.ttf";
            VM.Plugins_Video_GLideN64_View.FontSize_Text = "18";
            VM.Plugins_Video_GLideN64_View.FontColor_Text = "B5E61D";

            // Reset Save ✓ Label
            VM.Plugins_Video_GLideN64_View.Save_Text = "";
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
