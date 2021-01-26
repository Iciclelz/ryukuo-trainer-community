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
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;


namespace Ryukuo_Trainer_Community.Windows
{
    /// <summary>
    /// Interaction logic for AutosWindow.xaml
    /// </summary>
    public partial class AutosWindow : Window
    {

        private MainWindow mainWindow;
        public AutosWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }
        private void backRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
        }
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private void autoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        [DllImport("user32.dll", EntryPoint= "PostMessageW")]
        private static extern bool PostMessage(int hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        [DllImport("user32.dll")]
        private static extern int MapVirtualKey(uint uCode, uint uMapType);

        public void OnKey(byte key)
        {
            int hwnd = FindWindow("MapleStoryClass", null);
            int lParam = (MapVirtualKey(key, 0) << 16) + 2;
            PostMessage(hwnd, WM_KEYDOWN, key, lParam);
            PostMessage(hwnd, WM_KEYUP, key, lParam);
        }

        Stopwatch timeKey = new Stopwatch();
        private List<byte> skillCheck = new List<byte>
        {0x00,0x00,0x00,0x00,0x00,0x00};
        private long[] skillDelay = new long[6];
        private long[] timedSkill = new long[6];
        private int[] goFlag = new int[6];
        private bool plzStop = false;
        
        private bool autoStart = false;
        
        private int skillCount = 0;

        private int delay = 250;
        private bool bAutoAttack = false;

        private int uAutoAttack = 250;
        private long timedAttack = 250;

        private bool bAutoLoot = false;
        private int uAutoLoot = 50;
        private long timedLoot = 50;





        private void autoGo()
        {
            while (autoStart == true)
            {
                foreach (byte id in skillCheck)
                {
                    if (id != 0x00)
                    {
                        skillCount++;
                    }
                }


                if (skillCount != 0)
                {
                    skillCount = -1;
                    foreach (byte id in skillCheck)
                    {
                        skillCount++;
                        if (goFlag[skillCount] == 1)
                        {
                            OnKey(id);
                            Thread.Sleep(delay);
                            OnKey(id);
                            Thread.Sleep(delay);
                            OnKey(id);
                            Thread.Sleep(600);
                            goFlag[skillCount] = 0;
                            
                        }

                        if (id != 0x00 && (timeKey.ElapsedMilliseconds - timedSkill[skillCount]) >= skillDelay[skillCount])
                        {

                            timedSkill[skillCount] = timeKey.ElapsedMilliseconds;
                            goFlag[skillCount] = 1;
                        }
                        
                    }

                }
                plzStop = false;

                foreach (int flag in goFlag)
                {
                    if (goFlag[flag] == 0)
                    {
                        if (flag != 0)
                        { plzStop = true; }
                    }
                    
                }

                if (plzStop == false) 
                {
                    if (bAutoAttack == true && (timeKey.ElapsedMilliseconds - timedAttack) >= uAutoAttack)
                    {
                        timedAttack = timeKey.ElapsedMilliseconds;
                        OnKey(0x11); //VK_CONTROL
                    }

                    if (bAutoLoot == true && (timeKey.ElapsedMilliseconds - timedLoot) >= uAutoLoot)
                    {
                        timedLoot = timeKey.ElapsedMilliseconds;
                        OnKey(0x60);
                    }
                }

                if (timeKey.ElapsedMilliseconds >= 3372036854775807)
                {
                    timeKey.Restart();
                }
            }

        }

        public void EnableHacks()
        {

           
                skillCheck[0] = 0x00;
                skillCheck[1] = 0x00;
                skillCheck[2] = 0x00;
                skillCheck[3] = 0x00;
                skillCheck[4] = 0x00;
                skillCheck[5] = 0x00;
            

            if (autoSkillOneCheckBox.IsChecked == true)
                {
                    
                    skillCheck[1] = 0x31;
                    skillDelay[1] = Int32.Parse(autoSkillOneTextBox.Text);
                    goFlag[1] = 1;
              
                }

                if (autoSkillTwoCheckBox.IsChecked == true)
                {
                    
                    skillCheck[2] = 0x32;
                    skillDelay[2] = Int32.Parse(autoSkillTwoTextBox.Text);
                    goFlag[2] = 1;
                }

                if (autoSkillThreeCheckBox.IsChecked == true)
                {
                 
                    skillCheck[3] = 0x33;
                    skillDelay[3] = Int32.Parse(autoSkillThreeTextBox.Text);
                    goFlag[3] = 1;
                }

                if (autoSkillFourCheckBox.IsChecked == true)
                {
                   
                    skillCheck[4] = 0x34;
                    skillDelay[4] = Int32.Parse(autoSkillFourTextBox.Text);
                goFlag[4] = 1;
                }

            if (autoSkillFiveCheckBox.IsChecked == true)
                {
                    
                    skillCheck[5] = 0x35;
                    skillDelay[5] = Int32.Parse(autoSkillFiveTextBox.Text);
                goFlag[5] = 1;
                }


            if (autoAttackCheckBox.IsChecked == true)
                {
                    uAutoAttack = Int32.Parse(autoAttackTextBox.Text);
                    bAutoAttack = true;
                }


                if (autoLootCheckBox.IsChecked == true)
                {
                    uAutoLoot = Int32.Parse(autoLootTextBox.Text);
                    bAutoLoot = true;
                                    }
            
            goFlag[0] = 0;
            timedSkill[0] = 300000;
            skillDelay[0] = 300000;
            autoStart = true;
            timeKey.Start();
            new Thread(autoGo).Start();



        }


        public void DisableHacks()
        {
            autoStart = false;
            timeKey.Reset();
            bAutoAttack = false;
            bAutoLoot = false;
            foreach (int flag in goFlag)
            {
                goFlag[flag] = 0;
            }
            
            skillCount = 0;
            skillCheck[0] = 0x00;
            skillCheck[1] = 0x00;
            skillCheck[2] = 0x00;
            skillCheck[3] = 0x00;
            skillCheck[4] = 0x00;
            skillCheck[5] = 0x00;

        }

    }
}
