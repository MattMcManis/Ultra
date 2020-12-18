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
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra
{
    public class Plugins_Video_GLideN64_ViewModel : INotifyPropertyChanged
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
        /// Plugins - GLideN64 ViewModel
        /// </summary>
        public Plugins_Video_GLideN64_ViewModel()
        {
            //// Load Fonts
            //installedFonts = Directory
            //                .EnumerateFiles(@"C:\Windows\Fonts\")
            //                .Select(System.IO.Path.GetFileName)
            //                .ToList();

            //Fonts_Items = new ObservableCollection<string>(installedFonts);
        }

       
        // ----------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------
        /// <summary>
        /// GLideN64 Plugin
        /// </summary>
        // ----------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------

        // --------------------------------------------------
        // Version
        // --------------------------------------------------
        // Text
        private string _Version_Text = string.Empty;
        public string Version_Text
        {
            get { return _Version_Text; }
            set
            {
                if (_Version_Text == value)
                {
                    return;
                }

                _Version_Text = value;
                OnPropertyChanged("Version_Text");
            }
        }
        // Controls Enable
        private bool _Version_IsEnabled = false;
        public bool Version_IsEnabled
        {
            get { return _Version_IsEnabled; }
            set
            {
                if (_Version_IsEnabled == value)
                {
                    return;
                }

                _Version_IsEnabled = value;
                OnPropertyChanged("Version_IsEnabled");
            }
        }

        // --------------------------------------------------
        // MultiSampling
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _MultiSampling_Items = new ObservableCollection<string>
        {
            "0",
            "2",
            "4",
            "8",
            "16"
        };
        public ObservableCollection<string> MultiSampling_Items
        {
            get { return _MultiSampling_Items; }
            set
            {
                _MultiSampling_Items = value;
                OnPropertyChanged("MultiSampling_Items");
            }
        }

        // Selected Index
        private int _MultiSampling_SelectedIndex { get; set; }
        public int MultiSampling_SelectedIndex
        {
            get { return _MultiSampling_SelectedIndex; }
            set
            {
                if (_MultiSampling_SelectedIndex == value)
                {
                    return;
                }

                _MultiSampling_SelectedIndex = value;
                OnPropertyChanged("MultiSampling_SelectedIndex");
            }
        }

        // Selected Item
        private string _MultiSampling_SelectedItem { get; set; }
        public string MultiSampling_SelectedItem
        {
            get { return _MultiSampling_SelectedItem; }
            set
            {
                if (_MultiSampling_SelectedItem == value)
                {
                    return;
                }

                _MultiSampling_SelectedItem = value;
                OnPropertyChanged("MultiSampling_SelectedItem");
            }
        }

        // Controls Enable
        private bool _MultiSampling_IsEnabled = true;
        public bool MultiSampling_IsEnabled
        {
            get { return _MultiSampling_IsEnabled; }
            set
            {
                if (_MultiSampling_IsEnabled == value)
                {
                    return;
                }

                _MultiSampling_IsEnabled = value;
                OnPropertyChanged("MultiSampling_IsEnabled");
            }
        }

        // --------------------------------------------------
        // AspectRatio
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _AspectRatio_Items = new ObservableCollection<string>
        {
            "Stretch",
            "Force 4:3",
            "Force 16:9",
            "Adjust"
        };
        public ObservableCollection<string> AspectRatio_Items
        {
            get { return _AspectRatio_Items; }
            set
            {
                _AspectRatio_Items = value;
                OnPropertyChanged("AspectRatio_Items");
            }
        }

        // Selected Index
        private int _AspectRatio_SelectedIndex { get; set; }
        public int AspectRatio_SelectedIndex
        {
            get { return _AspectRatio_SelectedIndex; }
            set
            {
                if (_AspectRatio_SelectedIndex == value)
                {
                    return;
                }

                _AspectRatio_SelectedIndex = value;
                OnPropertyChanged("AspectRatio_SelectedIndex");
            }
        }

        // Selected Item
        private string _AspectRatio_SelectedItem { get; set; }
        public string AspectRatio_SelectedItem
        {
            get { return _AspectRatio_SelectedItem; }
            set
            {
                if (_AspectRatio_SelectedItem == value)
                {
                    return;
                }

                _AspectRatio_SelectedItem = value;
                OnPropertyChanged("AspectRatio_SelectedItem");
            }
        }

        // Controls Enable
        private bool _AspectRatio_IsEnabled = true;
        public bool AspectRatio_IsEnabled
        {
            get { return _AspectRatio_IsEnabled; }
            set
            {
                if (_AspectRatio_IsEnabled == value)
                {
                    return;
                }

                _AspectRatio_IsEnabled = value;
                OnPropertyChanged("AspectRatio_IsEnabled");
            }
        }

        // --------------------------------------------------
        // BilinearMode
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _BilinearMode_Items = new ObservableCollection<string>
        {
            "N64 3-Point",
            "Standard"
        };
        public ObservableCollection<string> BilinearMode_Items
        {
            get { return _BilinearMode_Items; }
            set
            {
                _BilinearMode_Items = value;
                OnPropertyChanged("BilinearMode_Items");
            }
        }

        // Selected Index
        private int _BilinearMode_SelectedIndex { get; set; }
        public int BilinearMode_SelectedIndex
        {
            get { return _BilinearMode_SelectedIndex; }
            set
            {
                if (_BilinearMode_SelectedIndex == value)
                {
                    return;
                }

                _BilinearMode_SelectedIndex = value;
                OnPropertyChanged("BilinearMode_SelectedIndex");
            }
        }

        // Selected Item
        private string _BilinearMode_SelectedItem { get; set; }
        public string BilinearMode_SelectedItem
        {
            get { return _BilinearMode_SelectedItem; }
            set
            {
                if (_BilinearMode_SelectedItem == value)
                {
                    return;
                }

                _BilinearMode_SelectedItem = value;
                OnPropertyChanged("BilinearMode_SelectedItem");
            }
        }

        // Controls Enable
        private bool _BilinearMode_IsEnabled = true;
        public bool BilinearMode_IsEnabled
        {
            get { return _BilinearMode_IsEnabled; }
            set
            {
                if (_BilinearMode_IsEnabled == value)
                {
                    return;
                }

                _BilinearMode_IsEnabled = value;
                OnPropertyChanged("BilinearMode_IsEnabled");
            }
        }

        // --------------------------------------------------
        // MaxAnisotropy
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _MaxAnisotropy_Items = new ObservableCollection<string>
        {
            "0",
            "2",
            "4",
            "6",
            "8",
            "12",
            "16"
        };
        public ObservableCollection<string> MaxAnisotropy_Items
        {
            get { return _MaxAnisotropy_Items; }
            set
            {
                _MaxAnisotropy_Items = value;
                OnPropertyChanged("MaxAnisotropy_Items");
            }
        }

        // Selected Index
        private int _MaxAnisotropy_SelectedIndex { get; set; }
        public int MaxAnisotropy_SelectedIndex
        {
            get { return _MaxAnisotropy_SelectedIndex; }
            set
            {
                if (_MaxAnisotropy_SelectedIndex == value)
                {
                    return;
                }

                _MaxAnisotropy_SelectedIndex = value;
                OnPropertyChanged("MaxAnisotropy_SelectedIndex");
            }
        }

        // Selected Item
        private string _MaxAnisotropy_SelectedItem { get; set; }
        public string MaxAnisotropy_SelectedItem
        {
            get { return _MaxAnisotropy_SelectedItem; }
            set
            {
                if (_MaxAnisotropy_SelectedItem == value)
                {
                    return;
                }

                _MaxAnisotropy_SelectedItem = value;
                OnPropertyChanged("MaxAnisotropy_SelectedItem");
            }
        }

        // Controls Enable
        private bool _MaxAnisotropy_IsEnabled = true;
        public bool MaxAnisotropy_IsEnabled
        {
            get { return _MaxAnisotropy_IsEnabled; }
            set
            {
                if (_MaxAnisotropy_IsEnabled == value)
                {
                    return;
                }

                _MaxAnisotropy_IsEnabled = value;
                OnPropertyChanged("MaxAnisotropy_IsEnabled");
            }
        }

        // -------------------------
        // Enable Fog
        // -------------------------
        // Checked
        private bool _EnableFog_IsChecked;
        public bool EnableFog_IsChecked
        {
            get { return _EnableFog_IsChecked; }
            set
            {
                if (_EnableFog_IsChecked != value)
                {
                    _EnableFog_IsChecked = value;
                    OnPropertyChanged("EnableFog_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableFog_IsEnabled = true;
        public bool EnableFog_IsEnabled
        {
            get { return _EnableFog_IsEnabled; }
            set
            {
                if (_EnableFog_IsEnabled == value)
                {
                    return;
                }

                _EnableFog_IsEnabled = value;
                OnPropertyChanged("EnableFog_IsEnabled");
            }
        }


        // -------------------------
        // ShowFPS
        // -------------------------
        // Checked
        private bool _ShowFPS_IsChecked;
        public bool ShowFPS_IsChecked
        {
            get { return _ShowFPS_IsChecked; }
            set
            {
                if (_ShowFPS_IsChecked != value)
                {
                    _ShowFPS_IsChecked = value;
                    OnPropertyChanged("ShowFPS_IsChecked");
                }
            }
        }
        // Enabled
        private bool _ShowFPS_IsEnabled = true;
        public bool ShowFPS_IsEnabled
        {
            get { return _ShowFPS_IsEnabled; }
            set
            {
                if (_ShowFPS_IsEnabled == value)
                {
                    return;
                }

                _ShowFPS_IsEnabled = value;
                OnPropertyChanged("ShowFPS_IsEnabled");
            }
        }

        // -------------------------
        // ShowVIS
        // -------------------------
        // Checked
        private bool _ShowVIS_IsChecked;
        public bool ShowVIS_IsChecked
        {
            get { return _ShowVIS_IsChecked; }
            set
            {
                if (_ShowVIS_IsChecked != value)
                {
                    _ShowVIS_IsChecked = value;
                    OnPropertyChanged("ShowVIS_IsChecked");
                }
            }
        }
        // Enabled
        private bool _ShowVIS_IsEnabled = true;
        public bool ShowVIS_IsEnabled
        {
            get { return _ShowVIS_IsEnabled; }
            set
            {
                if (_ShowVIS_IsEnabled == value)
                {
                    return;
                }

                _ShowVIS_IsEnabled = value;
                OnPropertyChanged("ShowVIS_IsEnabled");
            }
        }

        // -------------------------
        // FXAA
        // -------------------------
        // Checked
        private bool _FXAA_IsChecked;
        public bool FXAA_IsChecked
        {
            get { return _FXAA_IsChecked; }
            set
            {
                if (_FXAA_IsChecked != value)
                {
                    _FXAA_IsChecked = value;
                    OnPropertyChanged("FXAA_IsChecked");
                }
            }
        }
        // Enabled
        private bool _FXAA_IsEnabled = true;
        public bool FXAA_IsEnabled
        {
            get { return _FXAA_IsEnabled; }
            set
            {
                if (_FXAA_IsEnabled == value)
                {
                    return;
                }

                _FXAA_IsEnabled = value;
                OnPropertyChanged("FXAA_IsEnabled");
            }
        }


        // -------------------------
        // Enable Noise
        // -------------------------
        // Checked
        private bool _EnableNoise_IsChecked;
        public bool EnableNoise_IsChecked
        {
            get { return _EnableNoise_IsChecked; }
            set
            {
                if (_EnableNoise_IsChecked != value)
                {
                    _EnableNoise_IsChecked = value;
                    OnPropertyChanged("EnableNoise_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableNoise_IsEnabled = true;
        public bool EnableNoise_IsEnabled
        {
            get { return _EnableNoise_IsEnabled; }
            set
            {
                if (_EnableNoise_IsEnabled == value)
                {
                    return;
                }

                _EnableNoise_IsEnabled = value;
                OnPropertyChanged("EnableNoise_IsEnabled");
            }
        }

        // -------------------------
        // EnableHalosRemoval
        // -------------------------
        // Checked
        private bool _EnableHalosRemoval_IsChecked;
        public bool EnableHalosRemoval_IsChecked
        {
            get { return _EnableHalosRemoval_IsChecked; }
            set
            {
                if (_EnableHalosRemoval_IsChecked != value)
                {
                    _EnableHalosRemoval_IsChecked = value;
                    OnPropertyChanged("EnableHalosRemoval_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableHalosRemoval_IsEnabled = true;
        public bool EnableHalosRemoval_IsEnabled
        {
            get { return _EnableHalosRemoval_IsEnabled; }
            set
            {
                if (_EnableHalosRemoval_IsEnabled == value)
                {
                    return;
                }

                _EnableHalosRemoval_IsEnabled = value;
                OnPropertyChanged("EnableHalosRemoval_IsEnabled");
            }
        }

        // -------------------------
        // Enable LOD
        // -------------------------
        // Checked
        private bool _EnableLOD_IsChecked;
        public bool EnableLOD_IsChecked
        {
            get { return _EnableLOD_IsChecked; }
            set
            {
                if (_EnableLOD_IsChecked != value)
                {
                    _EnableLOD_IsChecked = value;
                    OnPropertyChanged("EnableLOD_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableLOD_IsEnabled = true;
        public bool EnableLOD_IsEnabled
        {
            get { return _EnableLOD_IsEnabled; }
            set
            {
                if (_EnableLOD_IsEnabled == value)
                {
                    return;
                }

                _EnableLOD_IsEnabled = value;
                OnPropertyChanged("EnableLOD_IsEnabled");
            }
        }

        // -------------------------
        // Enable HW Lighting
        // -------------------------
        // Checked
        private bool _EnableHWLighting_IsChecked;
        public bool EnableHWLighting_IsChecked
        {
            get { return _EnableHWLighting_IsChecked; }
            set
            {
                if (_EnableHWLighting_IsChecked != value)
                {
                    _EnableHWLighting_IsChecked = value;
                    OnPropertyChanged("EnableHWLighting_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableHWLighting_IsEnabled = true;
        public bool EnableHWLighting_IsEnabled
        {
            get { return _EnableHWLighting_IsEnabled; }
            set
            {
                if (_EnableHWLighting_IsEnabled == value)
                {
                    return;
                }

                _EnableHWLighting_IsEnabled = value;
                OnPropertyChanged("EnableHWLighting_IsEnabled");
            }
        }

        // -------------------------
        // CorrectTexrectCoords
        // -------------------------
        // Checked
        private bool _CorrectTexrectCoords_IsChecked;
        public bool CorrectTexrectCoords_IsChecked
        {
            get { return _CorrectTexrectCoords_IsChecked; }
            set
            {
                if (_CorrectTexrectCoords_IsChecked != value)
                {
                    _CorrectTexrectCoords_IsChecked = value;
                    OnPropertyChanged("CorrectTexrectCoords_IsChecked");
                }
            }
        }
        // Enabled
        private bool _CorrectTexrectCoords_IsEnabled = true;
        public bool CorrectTexrectCoords_IsEnabled
        {
            get { return _CorrectTexrectCoords_IsEnabled; }
            set
            {
                if (_CorrectTexrectCoords_IsEnabled == value)
                {
                    return;
                }

                _CorrectTexrectCoords_IsEnabled = value;
                OnPropertyChanged("CorrectTexrectCoords_IsEnabled");
            }
        }

        // -------------------------
        // EnableShadersStorage
        // -------------------------
        // Checked
        private bool _EnableShadersStorage_IsChecked;
        public bool EnableShadersStorage_IsChecked
        {
            get { return _EnableShadersStorage_IsChecked; }
            set
            {
                if (_EnableShadersStorage_IsChecked != value)
                {
                    _EnableShadersStorage_IsChecked = value;
                    OnPropertyChanged("EnableShadersStorage_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableShadersStorage_IsEnabled = true;
        public bool EnableShadersStorage_IsEnabled
        {
            get { return _EnableShadersStorage_IsEnabled; }
            set
            {
                if (_EnableShadersStorage_IsEnabled == value)
                {
                    return;
                }

                _EnableShadersStorage_IsEnabled = value;
                OnPropertyChanged("EnableShadersStorage_IsEnabled");
            }
        }


        // --------------------------------------------------
        // BloomThresholdLevel
        // --------------------------------------------------
        // Text
        private string _BloomThresholdLevel_Text = string.Empty;
        public string BloomThresholdLevel_Text
        {
            get { return _BloomThresholdLevel_Text; }
            set
            {
                if (_BloomThresholdLevel_Text == value)
                {
                    return;
                }

                _BloomThresholdLevel_Text = value;
                OnPropertyChanged("BloomThresholdLevel_Text");
            }
        }
        // Controls Enable
        private bool _BloomThresholdLevel_IsEnabled = true;
        public bool BloomThresholdLevel_IsEnabled
        {
            get { return _BloomThresholdLevel_IsEnabled; }
            set
            {
                if (_BloomThresholdLevel_IsEnabled == value)
                {
                    return;
                }

                _BloomThresholdLevel_IsEnabled = value;
                OnPropertyChanged("BloomThresholdLevel_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Bloom Blend Mode
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _BloomBlendMode_Items = new ObservableCollection<string>
        {
            "Light",
            "Mild",
            "Strong"
        };
        public ObservableCollection<string> BloomBlendMode_Items
        {
            get { return _BloomBlendMode_Items; }
            set
            {
                _BloomBlendMode_Items = value;
                OnPropertyChanged("BloomBlendMode_Items");
            }
        }

        // Selected Index
        private int _BloomBlendMode_SelectedIndex { get; set; }
        public int BloomBlendMode_SelectedIndex
        {
            get { return _BloomBlendMode_SelectedIndex; }
            set
            {
                if (_BloomBlendMode_SelectedIndex == value)
                {
                    return;
                }

                _BloomBlendMode_SelectedIndex = value;
                OnPropertyChanged("BloomBlendMode_SelectedIndex");
            }
        }

        // Selected Item
        private string _BloomBlendMode_SelectedItem { get; set; }
        public string BloomBlendMode_SelectedItem
        {
            get { return _BloomBlendMode_SelectedItem; }
            set
            {
                if (_BloomBlendMode_SelectedItem == value)
                {
                    return;
                }

                _BloomBlendMode_SelectedItem = value;
                OnPropertyChanged("BloomBlendMode_SelectedItem");
            }
        }

        // Controls Enable
        private bool _BloomBlendMode_IsEnabled = true;
        public bool BloomBlendMode_IsEnabled
        {
            get { return _BloomBlendMode_IsEnabled; }
            set
            {
                if (_BloomBlendMode_IsEnabled == value)
                {
                    return;
                }

                _BloomBlendMode_IsEnabled = value;
                OnPropertyChanged("BloomBlendMode_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Blur Amount
        // --------------------------------------------------
        // Text
        private string _BlurAmount_Text = string.Empty;
        public string BlurAmount_Text
        {
            get { return _BlurAmount_Text; }
            set
            {
                if (_BlurAmount_Text == value)
                {
                    return;
                }

                _BlurAmount_Text = value;
                OnPropertyChanged("BlurAmount_Text");
            }
        }
        // Controls Enable
        private bool _BlurAmount_IsEnabled = true;
        public bool BlurAmount_IsEnabled
        {
            get { return _BlurAmount_IsEnabled; }
            set
            {
                if (_BlurAmount_IsEnabled == value)
                {
                    return;
                }

                _BlurAmount_IsEnabled = value;
                OnPropertyChanged("BlurAmount_IsEnabled");
            }
        }

        // --------------------------------------------------
        // BlurStrength
        // --------------------------------------------------
        // Text
        private string _BlurStrength_Text = string.Empty;
        public string BlurStrength_Text
        {
            get { return _BlurStrength_Text; }
            set
            {
                if (_BlurStrength_Text == value)
                {
                    return;
                }

                _BlurStrength_Text = value;
                OnPropertyChanged("BlurStrength_Text");
            }
        }
        // Controls Enable
        private bool _BlurStrength_IsEnabled = true;
        public bool BlurStrength_IsEnabled
        {
            get { return _BlurStrength_IsEnabled; }
            set
            {
                if (_BlurStrength_IsEnabled == value)
                {
                    return;
                }

                _BlurStrength_IsEnabled = value;
                OnPropertyChanged("BlurStrength_IsEnabled");
            }
        }

        // -------------------------
        // Enable Bloom
        // -------------------------
        // Checked
        private bool _EnableBloom_IsChecked;
        public bool EnableBloom_IsChecked
        {
            get { return _EnableBloom_IsChecked; }
            set
            {
                if (_EnableBloom_IsChecked != value)
                {
                    _EnableBloom_IsChecked = value;
                    OnPropertyChanged("EnableBloom_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableBloom_IsEnabled = true;
        public bool EnableBloom_IsEnabled
        {
            get { return _EnableBloom_IsEnabled; }
            set
            {
                if (_EnableBloom_IsEnabled == value)
                {
                    return;
                }

                _EnableBloom_IsEnabled = value;
                OnPropertyChanged("EnableBloom_IsEnabled");
            }
        }

        // -------------------------
        // EnableFBEmulation
        // -------------------------
        // Checked
        private bool _EnableFBEmulation_IsChecked;
        public bool EnableFBEmulation_IsChecked
        {
            get { return _EnableFBEmulation_IsChecked; }
            set
            {
                if (_EnableFBEmulation_IsChecked != value)
                {
                    _EnableFBEmulation_IsChecked = value;
                    OnPropertyChanged("EnableFBEmulation_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableFBEmulation_IsEnabled = true;
        public bool EnableFBEmulation_IsEnabled
        {
            get { return _EnableFBEmulation_IsEnabled; }
            set
            {
                if (_EnableFBEmulation_IsEnabled == value)
                {
                    return;
                }

                _EnableFBEmulation_IsEnabled = value;
                OnPropertyChanged("EnableFBEmulation_IsEnabled");
            }
        }

        // -------------------------
        // DisableFBInfo
        // -------------------------
        // Checked
        private bool _DisableFBInfo_IsChecked;
        public bool DisableFBInfo_IsChecked
        {
            get { return _DisableFBInfo_IsChecked; }
            set
            {
                if (_DisableFBInfo_IsChecked != value)
                {
                    _DisableFBInfo_IsChecked = value;
                    OnPropertyChanged("DisableFBInfo_IsChecked");
                }
            }
        }
        // Enabled
        private bool _DisableFBInfo_IsEnabled = true;
        public bool DisableFBInfo_IsEnabled
        {
            get { return _DisableFBInfo_IsEnabled; }
            set
            {
                if (_DisableFBInfo_IsEnabled == value)
                {
                    return;
                }

                _DisableFBInfo_IsEnabled = value;
                OnPropertyChanged("DisableFBInfo_IsEnabled");
            }
        }

        // -------------------------
        // ForceDepthBufferClear
        // -------------------------
        // Checked
        private bool _ForceDepthBufferClear_IsChecked;
        public bool ForceDepthBufferClear_IsChecked
        {
            get { return _ForceDepthBufferClear_IsChecked; }
            set
            {
                if (_ForceDepthBufferClear_IsChecked != value)
                {
                    _ForceDepthBufferClear_IsChecked = value;
                    OnPropertyChanged("ForceDepthBufferClear_IsChecked");
                }
            }
        }
        // Enabled
        private bool _ForceDepthBufferClear_IsEnabled = true;
        public bool ForceDepthBufferClear_IsEnabled
        {
            get { return _ForceDepthBufferClear_IsEnabled; }
            set
            {
                if (_ForceDepthBufferClear_IsEnabled == value)
                {
                    return;
                }

                _ForceDepthBufferClear_IsEnabled = value;
                OnPropertyChanged("ForceDepthBufferClear_IsEnabled");
            }
        }

        // -------------------------
        // FBInfoReadColorChunk
        // -------------------------
        // Checked
        private bool _FBInfoReadColorChunk_IsChecked;
        public bool FBInfoReadColorChunk_IsChecked
        {
            get { return _FBInfoReadColorChunk_IsChecked; }
            set
            {
                if (_FBInfoReadColorChunk_IsChecked != value)
                {
                    _FBInfoReadColorChunk_IsChecked = value;
                    OnPropertyChanged("FBInfoReadColorChunk_IsChecked");
                }
            }
        }
        // Enabled
        private bool _FBInfoReadColorChunk_IsEnabled = true;
        public bool FBInfoReadColorChunk_IsEnabled
        {
            get { return _FBInfoReadColorChunk_IsEnabled; }
            set
            {
                if (_FBInfoReadColorChunk_IsEnabled == value)
                {
                    return;
                }

                _FBInfoReadColorChunk_IsEnabled = value;
                OnPropertyChanged("FBInfoReadColorChunk_IsEnabled");
            }
        }

        // -------------------------
        // FBInfoReadDepthChunk
        // -------------------------
        // Checked
        private bool _FBInfoReadDepthChunk_IsChecked;
        public bool FBInfoReadDepthChunk_IsChecked
        {
            get { return _FBInfoReadDepthChunk_IsChecked; }
            set
            {
                if (_FBInfoReadDepthChunk_IsChecked != value)
                {
                    _FBInfoReadDepthChunk_IsChecked = value;
                    OnPropertyChanged("FBInfoReadDepthChunk_IsChecked");
                }
            }
        }
        // Enabled
        private bool _FBInfoReadDepthChunk_IsEnabled = true;
        public bool FBInfoReadDepthChunk_IsEnabled
        {
            get { return _FBInfoReadDepthChunk_IsEnabled; }
            set
            {
                if (_FBInfoReadDepthChunk_IsEnabled == value)
                {
                    return;
                }

                _FBInfoReadDepthChunk_IsEnabled = value;
                OnPropertyChanged("FBInfoReadDepthChunk_IsEnabled");
            }
        }

        // -------------------------
        // EnableCopyAuxiliaryToRDRAM
        // -------------------------
        // Checked
        private bool _EnableCopyAuxiliaryToRDRAM_IsChecked;
        public bool EnableCopyAuxiliaryToRDRAM_IsChecked
        {
            get { return _EnableCopyAuxiliaryToRDRAM_IsChecked; }
            set
            {
                if (_EnableCopyAuxiliaryToRDRAM_IsChecked != value)
                {
                    _EnableCopyAuxiliaryToRDRAM_IsChecked = value;
                    OnPropertyChanged("EnableCopyAuxiliaryToRDRAM_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableCopyAuxiliaryToRDRAM_IsEnabled = true;
        public bool EnableCopyAuxiliaryToRDRAM_IsEnabled
        {
            get { return _EnableCopyAuxiliaryToRDRAM_IsEnabled; }
            set
            {
                if (_EnableCopyAuxiliaryToRDRAM_IsEnabled == value)
                {
                    return;
                }

                _EnableCopyAuxiliaryToRDRAM_IsEnabled = value;
                OnPropertyChanged("EnableCopyAuxiliaryToRDRAM_IsEnabled");
            }
        }

        // -------------------------
        // EnableCopyColorToRDRAM
        // -------------------------
        // Items Source
        private ObservableCollection<string> _EnableCopyColorToRDRAM_Items = new ObservableCollection<string>
        {
            "Do Not Copy",
            "Copy in Sync Mode",
            "Double Buffer",
            "Triple Buffer"
        };
        public ObservableCollection<string> EnableCopyColorToRDRAM_Items
        {
            get { return _EnableCopyColorToRDRAM_Items; }
            set
            {
                _EnableCopyColorToRDRAM_Items = value;
                OnPropertyChanged("EnableCopyColorToRDRAM_Items");
            }
        }

        // Selected Index
        private int _EnableCopyColorToRDRAM_SelectedIndex { get; set; }
        public int EnableCopyColorToRDRAM_SelectedIndex
        {
            get { return _EnableCopyColorToRDRAM_SelectedIndex; }
            set
            {
                if (_EnableCopyColorToRDRAM_SelectedIndex == value)
                {
                    return;
                }

                _EnableCopyColorToRDRAM_SelectedIndex = value;
                OnPropertyChanged("EnableCopyColorToRDRAM_SelectedIndex");
            }
        }

        // Selected Item
        private string _EnableCopyColorToRDRAM_SelectedItem { get; set; }
        public string EnableCopyColorToRDRAM_SelectedItem
        {
            get { return _EnableCopyColorToRDRAM_SelectedItem; }
            set
            {
                if (_EnableCopyColorToRDRAM_SelectedItem == value)
                {
                    return;
                }

                _EnableCopyColorToRDRAM_SelectedItem = value;
                OnPropertyChanged("EnableCopyColorToRDRAM_SelectedItem");
            }
        }

        // Controls Enable
        private bool _EnableCopyColorToRDRAM_IsEnabled = true;
        public bool EnableCopyColorToRDRAM_IsEnabled
        {
            get { return _EnableCopyColorToRDRAM_IsEnabled; }
            set
            {
                if (_EnableCopyColorToRDRAM_IsEnabled == value)
                {
                    return;
                }

                _EnableCopyColorToRDRAM_IsEnabled = value;
                OnPropertyChanged("EnableCopyColorToRDRAM_IsEnabled");
            }
        }
        //// Checked
        //private bool _EnableCopyColorToRDRAM_IsChecked;
        //public bool EnableCopyColorToRDRAM_IsChecked
        //{
        //    get { return _EnableCopyColorToRDRAM_IsChecked; }
        //    set
        //    {
        //        if (_EnableCopyColorToRDRAM_IsChecked != value)
        //        {
        //            _EnableCopyColorToRDRAM_IsChecked = value;
        //            OnPropertyChanged("EnableCopyColorToRDRAM_IsChecked");
        //        }
        //    }
        //}
        //// Enabled
        //private bool _EnableCopyColorToRDRAM_IsEnabled = true;
        //public bool EnableCopyColorToRDRAM_IsEnabled
        //{
        //    get { return _EnableCopyColorToRDRAM_IsEnabled; }
        //    set
        //    {
        //        if (_EnableCopyColorToRDRAM_IsEnabled == value)
        //        {
        //            return;
        //        }

        //        _EnableCopyColorToRDRAM_IsEnabled = value;
        //        OnPropertyChanged("EnableCopyColorToRDRAM_IsEnabled");
        //    }
        //}

        // -------------------------
        // EnableCopyDepthToRDRAM
        // -------------------------
        // Items Source
        private ObservableCollection<string> _EnableCopyDepthToRDRAM_Items = new ObservableCollection<string>
        {
            "Do Not Copy",
            "Copy From Video Memory",
            "Use Software Render"
        };
        public ObservableCollection<string> EnableCopyDepthToRDRAM_Items
        {
            get { return _EnableCopyDepthToRDRAM_Items; }
            set
            {
                _EnableCopyDepthToRDRAM_Items = value;
                OnPropertyChanged("EnableCopyDepthToRDRAM_Items");
            }
        }

        // Selected Index
        private int _EnableCopyDepthToRDRAM_SelectedIndex { get; set; }
        public int EnableCopyDepthToRDRAM_SelectedIndex
        {
            get { return _EnableCopyDepthToRDRAM_SelectedIndex; }
            set
            {
                if (_EnableCopyDepthToRDRAM_SelectedIndex == value)
                {
                    return;
                }

                _EnableCopyDepthToRDRAM_SelectedIndex = value;
                OnPropertyChanged("EnableCopyDepthToRDRAM_SelectedIndex");
            }
        }

        // Selected Item
        private string _EnableCopyDepthToRDRAM_SelectedItem { get; set; }
        public string EnableCopyDepthToRDRAM_SelectedItem
        {
            get { return _EnableCopyDepthToRDRAM_SelectedItem; }
            set
            {
                if (_EnableCopyDepthToRDRAM_SelectedItem == value)
                {
                    return;
                }

                _EnableCopyDepthToRDRAM_SelectedItem = value;
                OnPropertyChanged("EnableCopyDepthToRDRAM_SelectedItem");
            }
        }

        // Controls Enable
        private bool _EnableCopyDepthToRDRAM_IsEnabled = true;
        public bool EnableCopyDepthToRDRAM_IsEnabled
        {
            get { return _EnableCopyDepthToRDRAM_IsEnabled; }
            set
            {
                if (_EnableCopyDepthToRDRAM_IsEnabled == value)
                {
                    return;
                }

                _EnableCopyDepthToRDRAM_IsEnabled = value;
                OnPropertyChanged("EnableCopyDepthToRDRAM_IsEnabled");
            }
        }
        //// Checked
        //private bool _EnableCopyDepthToRDRAM_IsChecked;
        //public bool EnableCopyDepthToRDRAM_IsChecked
        //{
        //    get { return _EnableCopyDepthToRDRAM_IsChecked; }
        //    set
        //    {
        //        if (_EnableCopyDepthToRDRAM_IsChecked != value)
        //        {
        //            _EnableCopyDepthToRDRAM_IsChecked = value;
        //            OnPropertyChanged("EnableCopyDepthToRDRAM_IsChecked");
        //        }
        //    }
        //}
        //// Enabled
        //private bool _EnableCopyDepthToRDRAM_IsEnabled = true;
        //public bool EnableCopyDepthToRDRAM_IsEnabled
        //{
        //    get { return _EnableCopyDepthToRDRAM_IsEnabled; }
        //    set
        //    {
        //        if (_EnableCopyDepthToRDRAM_IsEnabled == value)
        //        {
        //            return;
        //        }

        //        _EnableCopyDepthToRDRAM_IsEnabled = value;
        //        OnPropertyChanged("EnableCopyDepthToRDRAM_IsEnabled");
        //    }
        //}

        // -------------------------
        // EnableCopyColorFromRDRAM
        // -------------------------
        // Checked
        private bool _EnableCopyColorFromRDRAM_IsChecked;
        public bool EnableCopyColorFromRDRAM_IsChecked
        {
            get { return _EnableCopyColorFromRDRAM_IsChecked; }
            set
            {
                if (_EnableCopyColorFromRDRAM_IsChecked != value)
                {
                    _EnableCopyColorFromRDRAM_IsChecked = value;
                    OnPropertyChanged("EnableCopyColorFromRDRAM_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableCopyColorFromRDRAM_IsEnabled = true;
        public bool EnableCopyColorFromRDRAM_IsEnabled
        {
            get { return _EnableCopyColorFromRDRAM_IsEnabled; }
            set
            {
                if (_EnableCopyColorFromRDRAM_IsEnabled == value)
                {
                    return;
                }

                _EnableCopyColorFromRDRAM_IsEnabled = value;
                OnPropertyChanged("EnableCopyColorFromRDRAM_IsEnabled");
            }
        }

        // -------------------------
        // EnableDetectCFB
        // -------------------------
        // Checked
        private bool _EnableDetectCFB_IsChecked;
        public bool EnableDetectCFB_IsChecked
        {
            get { return _EnableDetectCFB_IsChecked; }
            set
            {
                if (_EnableDetectCFB_IsChecked != value)
                {
                    _EnableDetectCFB_IsChecked = value;
                    OnPropertyChanged("EnableDetectCFB_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableDetectCFB_IsEnabled = true;
        public bool EnableDetectCFB_IsEnabled
        {
            get { return _EnableDetectCFB_IsEnabled; }
            set
            {
                if (_EnableDetectCFB_IsEnabled == value)
                {
                    return;
                }

                _EnableDetectCFB_IsEnabled = value;
                OnPropertyChanged("EnableDetectCFB_IsEnabled");
            }
        }

        // -------------------------
        // EnableN64DepthCompare
        // -------------------------
        // Checked
        private bool _EnableN64DepthCompare_IsChecked;
        public bool EnableN64DepthCompare_IsChecked
        {
            get { return _EnableN64DepthCompare_IsChecked; }
            set
            {
                if (_EnableN64DepthCompare_IsChecked != value)
                {
                    _EnableN64DepthCompare_IsChecked = value;
                    OnPropertyChanged("EnableN64DepthCompare_IsChecked");
                }
            }
        }
        // Enabled
        private bool _EnableN64DepthCompare_IsEnabled = true;
        public bool EnableN64DepthCompare_IsEnabled
        {
            get { return _EnableN64DepthCompare_IsEnabled; }
            set
            {
                if (_EnableN64DepthCompare_IsEnabled == value)
                {
                    return;
                }

                _EnableN64DepthCompare_IsEnabled = value;
                OnPropertyChanged("EnableN64DepthCompare_IsEnabled");
            }
        }

        // --------------------------------------------------
        // CacheSize
        // --------------------------------------------------
        // Text
        private string _CacheSize_Text = string.Empty;
        public string CacheSize_Text
        {
            get { return _CacheSize_Text; }
            set
            {
                if (_CacheSize_Text == value)
                {
                    return;
                }

                _CacheSize_Text = value;
                OnPropertyChanged("CacheSize_Text");
            }
        }
        // Controls Enable
        private bool _CacheSize_IsEnabled = true;
        public bool CacheSize_IsEnabled
        {
            get { return _CacheSize_IsEnabled; }
            set
            {
                if (_CacheSize_IsEnabled == value)
                {
                    return;
                }

                _CacheSize_IsEnabled = value;
                OnPropertyChanged("CacheSize_IsEnabled");
            }
        }

        // --------------------------------------------------
        // TxCacheSize
        // --------------------------------------------------
        // Text
        private string _TxCacheSize_Text = string.Empty;
        public string TxCacheSize_Text
        {
            get { return _TxCacheSize_Text; }
            set
            {
                if (_TxCacheSize_Text == value)
                {
                    return;
                }

                _TxCacheSize_Text = value;
                OnPropertyChanged("TxCacheSize_Text");
            }
        }
        // Controls Enable
        private bool _TxCacheSize_IsEnabled = true;
        public bool TxCacheSize_IsEnabled
        {
            get { return _TxCacheSize_IsEnabled; }
            set
            {
                if (_TxCacheSize_IsEnabled == value)
                {
                    return;
                }

                _TxCacheSize_IsEnabled = value;
                OnPropertyChanged("TxCacheSize_IsEnabled");
            }
        }

        // --------------------------------------------------
        // TxFilterMode
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _TxFilterMode_Items = new ObservableCollection<string>
        {
            "None",
            "Smooth 1",
            "Smooth 2",
            "Smooth 3",
            "Smooth 4",
            "Sharp 1",
            "Sharp 2",
        };
        public ObservableCollection<string> TxFilterMode_Items
        {
            get { return _TxFilterMode_Items; }
            set
            {
                _TxFilterMode_Items = value;
                OnPropertyChanged("TxFilterMode_Items");
            }
        }

        // Selected Index
        private int _TxFilterMode_SelectedIndex { get; set; }
        public int TxFilterMode_SelectedIndex
        {
            get { return _TxFilterMode_SelectedIndex; }
            set
            {
                if (_TxFilterMode_SelectedIndex == value)
                {
                    return;
                }

                _TxFilterMode_SelectedIndex = value;
                OnPropertyChanged("TxFilterMode_SelectedIndex");
            }
        }

        // Selected Item
        private string _TxFilterMode_SelectedItem { get; set; }
        public string TxFilterMode_SelectedItem
        {
            get { return _TxFilterMode_SelectedItem; }
            set
            {
                if (_TxFilterMode_SelectedItem == value)
                {
                    return;
                }

                _TxFilterMode_SelectedItem = value;
                OnPropertyChanged("TxFilterMode_SelectedItem");
            }
        }

        // Controls Enable
        private bool _TxFilterMode_IsEnabled = true;
        public bool TxFilterMode_IsEnabled
        {
            get { return _TxFilterMode_IsEnabled; }
            set
            {
                if (_TxFilterMode_IsEnabled == value)
                {
                    return;
                }

                _TxFilterMode_IsEnabled = value;
                OnPropertyChanged("TxFilterMode_IsEnabled");
            }
        }

        // --------------------------------------------------
        // TxEnhancementMode
        // --------------------------------------------------
        // Items Source
        private ObservableCollection<string> _TxEnhancementMode_Items = new ObservableCollection<string>
        {
            "None",
            "Store",
            "X2",
            "X2SAI",
            "HQ2X",
            "HQ2X",
            "HQ2XS",
            "LQ2X",
            "LQ2XS",
            "HQ4X",
            "2xBRZ",
            "3xBRZ",
            "4xBRZ",
            "5xBRZ",
        };
        public ObservableCollection<string> TxEnhancementMode_Items
        {
            get { return _TxEnhancementMode_Items; }
            set
            {
                _TxEnhancementMode_Items = value;
                OnPropertyChanged("TxEnhancementMode_Items");
            }
        }

        // Selected Index
        private int _TxEnhancementMode_SelectedIndex { get; set; }
        public int TxEnhancementMode_SelectedIndex
        {
            get { return _TxEnhancementMode_SelectedIndex; }
            set
            {
                if (_TxEnhancementMode_SelectedIndex == value)
                {
                    return;
                }

                _TxEnhancementMode_SelectedIndex = value;
                OnPropertyChanged("TxEnhancementMode_SelectedIndex");
            }
        }

        // Selected Item
        private string _TxEnhancementMode_SelectedItem { get; set; }
        public string TxEnhancementMode_SelectedItem
        {
            get { return _TxEnhancementMode_SelectedItem; }
            set
            {
                if (_TxEnhancementMode_SelectedItem == value)
                {
                    return;
                }

                _TxEnhancementMode_SelectedItem = value;
                OnPropertyChanged("TxEnhancementMode_SelectedItem");
            }
        }

        // Controls Enable
        private bool _TxEnhancementMode_IsEnabled = true;
        public bool TxEnhancementMode_IsEnabled
        {
            get { return _TxEnhancementMode_IsEnabled; }
            set
            {
                if (_TxEnhancementMode_IsEnabled == value)
                {
                    return;
                }

                _TxEnhancementMode_IsEnabled = value;
                OnPropertyChanged("TxEnhancementMode_IsEnabled");
            }
        }

        // -------------------------
        // TxFilterIgnoreBG
        // -------------------------
        // Checked
        private bool _TxFilterIgnoreBG_IsChecked;
        public bool TxFilterIgnoreBG_IsChecked
        {
            get { return _TxFilterIgnoreBG_IsChecked; }
            set
            {
                if (_TxFilterIgnoreBG_IsChecked != value)
                {
                    _TxFilterIgnoreBG_IsChecked = value;
                    OnPropertyChanged("TxFilterIgnoreBG_IsChecked");
                }
            }
        }
        // Enabled
        private bool _TxFilterIgnoreBG_IsEnabled = true;
        public bool TxFilterIgnoreBG_IsEnabled
        {
            get { return _TxFilterIgnoreBG_IsEnabled; }
            set
            {
                if (_TxFilterIgnoreBG_IsEnabled == value)
                {
                    return;
                }

                _TxFilterIgnoreBG_IsEnabled = value;
                OnPropertyChanged("TxFilterIgnoreBG_IsEnabled");
            }
        }

        // -------------------------
        // TxDeposterize
        // -------------------------
        // Checked
        private bool _TxDeposterize_IsChecked;
        public bool TxDeposterize_IsChecked
        {
            get { return _TxDeposterize_IsChecked; }
            set
            {
                if (_TxDeposterize_IsChecked != value)
                {
                    _TxDeposterize_IsChecked = value;
                    OnPropertyChanged("TxDeposterize_IsChecked");
                }
            }
        }
        // Enabled
        private bool _TxDeposterize_IsEnabled = true;
        public bool TxDeposterize_IsEnabled
        {
            get { return _TxDeposterize_IsEnabled; }
            set
            {
                if (_TxDeposterize_IsEnabled == value)
                {
                    return;
                }

                _TxDeposterize_IsEnabled = value;
                OnPropertyChanged("TxDeposterize_IsEnabled");
            }
        }

        // -------------------------
        // TxHiresEnable
        // -------------------------
        // Checked
        private bool _TxHiresEnable_IsChecked;
        public bool TxHiresEnable_IsChecked
        {
            get { return _TxHiresEnable_IsChecked; }
            set
            {
                if (_TxHiresEnable_IsChecked != value)
                {
                    _TxHiresEnable_IsChecked = value;
                    OnPropertyChanged("TxHiresEnable_IsChecked");
                }
            }
        }
        // Enabled
        private bool _TxHiresEnable_IsEnabled = true;
        public bool TxHiresEnable_IsEnabled
        {
            get { return _TxHiresEnable_IsEnabled; }
            set
            {
                if (_TxHiresEnable_IsEnabled == value)
                {
                    return;
                }

                _TxHiresEnable_IsEnabled = value;
                OnPropertyChanged("TxHiresEnable_IsEnabled");
            }
        }

        // -------------------------
        // TxHiresFullAlphaChannel
        // -------------------------
        // Checked
        private bool _TxHiresFullAlphaChannel_IsChecked;
        public bool TxHiresFullAlphaChannel_IsChecked
        {
            get { return _TxHiresFullAlphaChannel_IsChecked; }
            set
            {
                if (_TxHiresFullAlphaChannel_IsChecked != value)
                {
                    _TxHiresFullAlphaChannel_IsChecked = value;
                    OnPropertyChanged("TxHiresFullAlphaChannel_IsChecked");
                }
            }
        }
        // Enabled
        private bool _TxHiresFullAlphaChannel_IsEnabled = true;
        public bool TxHiresFullAlphaChannel_IsEnabled
        {
            get { return _TxHiresFullAlphaChannel_IsEnabled; }
            set
            {
                if (_TxHiresFullAlphaChannel_IsEnabled == value)
                {
                    return;
                }

                _TxHiresFullAlphaChannel_IsEnabled = value;
                OnPropertyChanged("TxHiresFullAlphaChannel_IsEnabled");
            }
        }

        // -------------------------
        // TxHresAltCRC
        // -------------------------
        // Checked
        private bool _TxHresAltCRC_IsChecked;
        public bool TxHresAltCRC_IsChecked
        {
            get { return _TxHresAltCRC_IsChecked; }
            set
            {
                if (_TxHresAltCRC_IsChecked != value)
                {
                    _TxHresAltCRC_IsChecked = value;
                    OnPropertyChanged("TxHresAltCRC_IsChecked");
                }
            }
        }
        // Enabled
        private bool _TxHresAltCRC_IsEnabled = true;
        public bool TxHresAltCRC_IsEnabled
        {
            get { return _TxHresAltCRC_IsEnabled; }
            set
            {
                if (_TxHresAltCRC_IsEnabled == value)
                {
                    return;
                }

                _TxHresAltCRC_IsEnabled = value;
                OnPropertyChanged("TxHresAltCRC_IsEnabled");
            }
        }

        // -------------------------
        // TxDump
        // -------------------------
        // Checked
        private bool _TxDump_IsChecked;
        public bool TxDump_IsChecked
        {
            get { return _TxDump_IsChecked; }
            set
            {
                if (_TxDump_IsChecked != value)
                {
                    _TxDump_IsChecked = value;
                    OnPropertyChanged("TxDump_IsChecked");
                }
            }
        }
        // Enabled
        private bool _TxDump_IsEnabled = true;
        public bool TxDump_IsEnabled
        {
            get { return _TxDump_IsEnabled; }
            set
            {
                if (_TxDump_IsEnabled == value)
                {
                    return;
                }

                _TxDump_IsEnabled = value;
                OnPropertyChanged("TxDump_IsEnabled");
            }
        }

        // -------------------------
        // TxCacheCompression
        // -------------------------
        // Checked
        private bool _TxCacheCompression_IsChecked;
        public bool TxCacheCompression_IsChecked
        {
            get { return _TxCacheCompression_IsChecked; }
            set
            {
                if (_TxCacheCompression_IsChecked != value)
                {
                    _TxCacheCompression_IsChecked = value;
                    OnPropertyChanged("TxCacheCompression_IsChecked");
                }
            }
        }
        // Enabled
        private bool _TxCacheCompression_IsEnabled = true;
        public bool TxCacheCompression_IsEnabled
        {
            get { return _TxCacheCompression_IsEnabled; }
            set
            {
                if (_TxCacheCompression_IsEnabled == value)
                {
                    return;
                }

                _TxCacheCompression_IsEnabled = value;
                OnPropertyChanged("TxCacheCompression_IsEnabled");
            }
        }

        // -------------------------
        // TxForce16bpp
        // -------------------------
        // Checked
        private bool _TxForce16bpp_IsChecked;
        public bool TxForce16bpp_IsChecked
        {
            get { return _TxForce16bpp_IsChecked; }
            set
            {
                if (_TxForce16bpp_IsChecked != value)
                {
                    _TxForce16bpp_IsChecked = value;
                    OnPropertyChanged("TxForce16bpp_IsChecked");
                }
            }
        }
        // Enabled
        private bool _TxForce16bpp_IsEnabled = true;
        public bool TxForce16bpp_IsEnabled
        {
            get { return _TxForce16bpp_IsEnabled; }
            set
            {
                if (_TxForce16bpp_IsEnabled == value)
                {
                    return;
                }

                _TxForce16bpp_IsEnabled = value;
                OnPropertyChanged("TxForce16bpp_IsEnabled");
            }
        }

        // -------------------------
        // TxSaveCache
        // -------------------------
        // Checked
        private bool _TxSaveCache_IsChecked;
        public bool TxSaveCache_IsChecked
        {
            get { return _TxSaveCache_IsChecked; }
            set
            {
                if (_TxSaveCache_IsChecked != value)
                {
                    _TxSaveCache_IsChecked = value;
                    OnPropertyChanged("TxSaveCache_IsChecked");
                }
            }
        }
        // Enabled
        private bool _TxSaveCache_IsEnabled = true;
        public bool TxSaveCache_IsEnabled
        {
            get { return _TxSaveCache_IsEnabled; }
            set
            {
                if (_TxSaveCache_IsEnabled == value)
                {
                    return;
                }

                _TxSaveCache_IsEnabled = value;
                OnPropertyChanged("TxSaveCache_IsEnabled");
            }
        }

        // --------------------------------------------------
        // TxPath
        // --------------------------------------------------
        // Text
        private string _TxPath_Text = string.Empty;
        public string TxPath_Text
        {
            get { return _TxPath_Text; }
            set
            {
                if (_TxPath_Text == value)
                {
                    return;
                }

                _TxPath_Text = value;
                OnPropertyChanged("TxPath_Text");
            }
        }
        // Controls Enable
        private bool _TxPath_IsEnabled = true;
        public bool TxPath_IsEnabled
        {
            get { return _TxPath_IsEnabled; }
            set
            {
                if (_TxPath_IsEnabled == value)
                {
                    return;
                }

                _TxPath_IsEnabled = value;
                OnPropertyChanged("TxPath_IsEnabled");
            }
        }

        // --------------------------------------------------
        // TxCachePath
        // --------------------------------------------------
        // Text
        private string _TxCachePath_Text = string.Empty;
        public string TxCachePath_Text
        {
            get { return _TxCachePath_Text; }
            set
            {
                if (_TxCachePath_Text == value)
                {
                    return;
                }

                _TxCachePath_Text = value;
                OnPropertyChanged("TxCachePath_Text");
            }
        }
        // Controls Enable
        private bool _TxCachePath_IsEnabled = true;
        public bool TxCachePath_IsEnabled
        {
            get { return _TxCachePath_IsEnabled; }
            set
            {
                if (_TxCachePath_IsEnabled == value)
                {
                    return;
                }

                _TxCachePath_IsEnabled = value;
                OnPropertyChanged("TxCachePath_IsEnabled");
            }
        }

        // --------------------------------------------------
        // TxDumpPath
        // --------------------------------------------------
        // Text
        private string _TxDumpPath_Text = string.Empty;
        public string TxDumpPath_Text
        {
            get { return _TxDumpPath_Text; }
            set
            {
                if (_TxDumpPath_Text == value)
                {
                    return;
                }

                _TxDumpPath_Text = value;
                OnPropertyChanged("TxDumpPath_Text");
            }
        }
        // Controls Enable
        private bool _TxDumpPath_IsEnabled = true;
        public bool TxDumpPath_IsEnabled
        {
            get { return _TxDumpPath_IsEnabled; }
            set
            {
                if (_TxDumpPath_IsEnabled == value)
                {
                    return;
                }

                _TxDumpPath_IsEnabled = value;
                OnPropertyChanged("TxDumpPath_IsEnabled");
            }
        }

        // -------------------------
        // ValidityCheckMethod
        // -------------------------
        // Checked
        private bool _ValidityCheckMethod_IsChecked;
        public bool ValidityCheckMethod_IsChecked
        {
            get { return _ValidityCheckMethod_IsChecked; }
            set
            {
                if (_ValidityCheckMethod_IsChecked != value)
                {
                    _ValidityCheckMethod_IsChecked = value;
                    OnPropertyChanged("ValidityCheckMethod_IsChecked");
                }
            }
        }
        // Enabled
        private bool _ValidityCheckMethod_IsEnabled = true;
        public bool ValidityCheckMethod_IsEnabled
        {
            get { return _ValidityCheckMethod_IsEnabled; }
            set
            {
                if (_ValidityCheckMethod_IsEnabled == value)
                {
                    return;
                }

                _ValidityCheckMethod_IsEnabled = value;
                OnPropertyChanged("ValidityCheckMethod_IsEnabled");
            }
        }

        // --------------------------------------------------
        // FontName
        // --------------------------------------------------
        // Items Source
        public static List<string> installedFonts = new List<string>();

        private static ObservableCollection<string> _Fonts_Items = new ObservableCollection<string>();
        //public InstalledFontCollection installedFonts = new InstalledFontCollection();
        public ObservableCollection<string> Fonts_Items
        {
            get { return _Fonts_Items; }
            set
            {
                _Fonts_Items = value;
                OnPropertyChanged("Fonts_Items");
            }
        }

        // Selected Index
        private int _Fonts_SelectedIndex { get; set; }
        public int Fonts_SelectedIndex
        {
            get { return _Fonts_SelectedIndex; }
            set
            {
                if (_Fonts_SelectedIndex == value)
                {
                    return;
                }

                _Fonts_SelectedIndex = value;
                OnPropertyChanged("Fonts_SelectedIndex");
            }
        }

        // Selected Item
        private string _Fonts_SelectedItem { get; set; }
        public string Fonts_SelectedItem
        {
            get { return _Fonts_SelectedItem; }
            set
            {
                if (_Fonts_SelectedItem == value)
                {
                    return;
                }

                _Fonts_SelectedItem = value;
                OnPropertyChanged("Fonts_SelectedItem");
            }
        }

        // Controls Enable
        private bool _Fonts_IsEnabled = true;
        public bool Fonts_IsEnabled
        {
            get { return _Fonts_IsEnabled; }
            set
            {
                if (_Fonts_IsEnabled == value)
                {
                    return;
                }

                _Fonts_IsEnabled = value;
                OnPropertyChanged("Fonts_IsEnabled");
            }
        }

        // --------------------------------------------------
        // FontSize
        // --------------------------------------------------
        // Text
        private string _FontSize_Text = string.Empty;
        public string FontSize_Text
        {
            get { return _FontSize_Text; }
            set
            {
                if (_FontSize_Text == value)
                {
                    return;
                }

                _FontSize_Text = value;
                OnPropertyChanged("FontSize_Text");
            }
        }
        // Controls Enable
        private bool _FontSize_IsEnabled = true;
        public bool FontSize_IsEnabled
        {
            get { return _FontSize_IsEnabled; }
            set
            {
                if (_FontSize_IsEnabled == value)
                {
                    return;
                }

                _FontSize_IsEnabled = value;
                OnPropertyChanged("FontSize_IsEnabled");
            }
        }

        // --------------------------------------------------
        // FontColor
        // --------------------------------------------------
        // Text
        private string _FontColor_Text = string.Empty;
        public string FontColor_Text
        {
            get { return _FontColor_Text; }
            set
            {
                if (_FontColor_Text == value)
                {
                    return;
                }

                _FontColor_Text = value;
                OnPropertyChanged("FontColor_Text");
            }
        }
        // Controls Enable
        private bool _FontColor_IsEnabled = true;
        public bool FontColor_IsEnabled
        {
            get { return _FontColor_IsEnabled; }
            set
            {
                if (_FontColor_IsEnabled == value)
                {
                    return;
                }

                _FontColor_IsEnabled = value;
                OnPropertyChanged("FontColor_IsEnabled");
            }
        }

        // --------------------------------------------------
        // Save
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
