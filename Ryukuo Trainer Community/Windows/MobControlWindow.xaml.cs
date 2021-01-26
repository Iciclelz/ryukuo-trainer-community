/*
Copyright 2016 Ryukuo Trainer Developers

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Ryukuo_Trainer_Community.Windows
{
    /// <summary>
    /// Interaction logic for MobControlWindow.xaml
    /// </summary>
    public partial class MobControlWindow : Window
    {
        private MainWindow mainWindow;
        public MobControlWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
        }

        public void EnableHacks()
        {
            if (noSuckRadioButton.IsChecked == true)
            {
                mainWindow.DisableScript("mobsuckleft");
                mainWindow.DisableScript("mobsuckright");
            }

            if (suckRightRadioButton.IsChecked == true)
            {
                mainWindow.EnableScript("mobsuckright");
            }

            if (suckLeftRadioButton.IsChecked == true)
            {
                mainWindow.EnableScript("mobsuckleft");
            }


            if (suckBothRadioButton.IsChecked == true)
            {
                mainWindow.EnableScript("mobsuckleft");
                mainWindow.EnableScript("mobsuckright");
            }

            if (noMagnusBallCheckBox.IsChecked == true)
            {
                mainWindow.EnableScript("nomagnusballs");
            }

            if (mobSpeedUpCheckBox.IsChecked == true)
            {
                mainWindow.EnableScript("fastermobs");
            }

            if (mobFuckupCheckBox.IsChecked == true)
            {
                mainWindow.EnableScript("crazymobs");
            }

            if (autoAggroCheckBox.IsChecked == true)
            {
                mainWindow.EnableScript("mobaggro");
            }

            if (mobDisarmCheckBox.IsChecked == true)
            {
                mainWindow.EnableScript("mobdisarm");
            }
            if (mobFreezeCheckBox.IsChecked == true)
            {
                mainWindow.EnableScript("mobfreeze");
            }
        }

        public void DisableHacks()
        {
            mainWindow.DisableScript("nomagnusballs");
            mainWindow.DisableScript("fastermobs");
            mainWindow.DisableScript("crazymobs");
            mainWindow.DisableScript("mobaggro");
            mainWindow.DisableScript("mobdisarm");
            mainWindow.DisableScript("mobfreeze");
            mainWindow.DisableScript("mobsuckleft");
            mainWindow.DisableScript("mobsuckright");
        }

        private void backRectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Hide();
        }
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private void mobControlWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
    }
}
