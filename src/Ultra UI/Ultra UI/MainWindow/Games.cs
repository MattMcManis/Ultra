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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViewModel;

namespace Ultra
{
    public partial class MainWindow : Window
    {
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
        /// Play - Button
        /// </summary>
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            PlayButton();
        }
        public void PlayButton()
        {
            if (Mupen64PlusAPI.api == null) // Only if Mupen64Plus is not already running
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
            else
            {
                MessageBox.Show("Another instance of Mupen64Plus is already running.",
                                "Notice",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }
    }
}
