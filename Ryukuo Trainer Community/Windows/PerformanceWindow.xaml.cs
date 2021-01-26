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
using System.Windows.Input;
using System.Windows.Interop;

namespace Ryukuo_Trainer_Community.Windows
{
    /// <summary>
    /// Interaction logic for PerformanceWindow.xaml
    /// </summary>
    public partial class PerformanceWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private MainWindow mainWindow;
        public PerformanceWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            
        }

        private void backRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
        }

        public void EnableHacks()
        {
            if (cpuHackCheckBox.IsChecked == true)
                mainWindow.EnableScript("cpuhack");

            if (noFadeStagesCheckBox.IsChecked == true)
                mainWindow.EnableScript("nofadestages");
        }

        public void DisableHacks()
        {
            mainWindow.DisableScript("cpuhack");
            mainWindow.DisableScript("nofadestages");
        }

        private void performanceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
    }
}
