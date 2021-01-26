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

using Ryukuo_Trainer_Community.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ryukuo_Trainer_Community
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private List<Tuple<int, string>> scripts;
        private CheatEngine cheatEngine;

        private GodmodeWindow godmodeWindow;
        private AutosWindow autoWindow;
        private MobControlWindow mobcontrolWindow;
        private PerformanceWindow performanceWindow;
        private CharacterWindow characterWindow;
        public MainWindow()
        {
            InitializeComponent();

            godmodeWindow = new GodmodeWindow(this);
            autoWindow = new AutosWindow(this);
            mobcontrolWindow = new MobControlWindow(this);
            performanceWindow = new PerformanceWindow(this);
            characterWindow = new CharacterWindow(this);

            scripts = new List<Tuple<int, string>>();
            cheatEngine = new CheatEngine();
            cheatEngine.loadEngine();

            refreshButton_Click(this, null);
            string mapleProc = GetMapleStoryProcess();
            while (GetMapleStoryProcess() == null)
            {
                MessageBox.Show("Cannot find MapleStory. Please ensure MapleStory is launched before clicking OK.", "Ryukuo Trainer Community", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            mapleProc = GetMapleStoryProcess();

            comboBox.Text = mapleProc;

            AttachToProcess(GetMapleStoryProcess());

        }


        private void EnableInitialScripts()
        {
            EnableScript("viewswears");
            //EnableScript("randomhwid");
            EnableScript("unlimitedattack");
            EnableScript("skipaircheck");
            EnableScript("aircheckattackonrope");
            EnableScript("logoskipper");
            EnableScript("nocrusadercodex");
			EnableScript("pictype");
        }

        private void LoadScripts()
        {
            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Script"));
            for (int i = 0; i < files.Length; ++i)
            {
                cheatEngine.iAddScript(Path.GetFileName(files[i]), File.ReadAllText(files[i]));
                cheatEngine.iActivateRecord(i, false);
                scripts.Add(new Tuple<int, string>(i, Path.GetFileName(files[i])));
            }
        }

        public int GetScriptId(string scriptName)
        {
            foreach (var T in scripts)
            {
                if (T.Item2 == scriptName)
                    return T.Item1;
            }

            return -1;
        }

        public void EnableScript(string scriptName)
        {
            int i = GetScriptId(scriptName);
            if (i != -1)
            {
                cheatEngine.iActivateRecord(i, true);
            }

        }

        public void DisableScript(string scriptName)
        {
            int i = GetScriptId(scriptName);
            if (i != -1)
            {
                cheatEngine.iActivateRecord(i, false);
            }
        }


        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try { AttachToProcess((comboBox.SelectedItem as ComboBoxItem).Content.ToString()); }
            catch { }
        }

        private string GetMapleStoryProcess()
        {
            string processes;
            cheatEngine.iGetProcessList(out processes);
            foreach (string process in Regex.Split(processes, "\r\n"))
            {
                if (process.Contains("MapleStory.exe"))
                    return process;
            }

            return null;
        }


        private bool AttachToProcess(string p)
        {
            cheatEngine.iOpenProcess(p.Substring(0, p.IndexOf('-', 0)));
            LoadScripts();
            EnableInitialScripts();
            return true;
        }



        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            comboBox.Items.Clear();

            string processes;
            cheatEngine.iGetProcessList(out processes);
            foreach (string process in Regex.Split(processes, "\r\n"))
            {
                if (process.Contains("MapleStory.exe"))
                    comboBox.Items.Add(new ComboBoxItem { Content = process });
            }
        }


        private void godmodeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            godmodeWindow.EnableHacks();
        }

        private void godmodeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            godmodeWindow.DisableHacks();
        }

        private void godmodeWindowRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            godmodeWindow.Left = Application.Current.MainWindow.Left;
            godmodeWindow.Top = Application.Current.MainWindow.Top + godmodeWindow.Height;
            godmodeWindow.Visibility = (godmodeWindow.IsVisible) ? Visibility.Hidden : Visibility.Visible;
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            godmodeWindow.Close();
            autoWindow.Close();
            mobcontrolWindow.Close();
            performanceWindow.Close();
        }

        private void mainWindow_LocationChanged(object sender, EventArgs e)
        {
            godmodeWindow.Left = Application.Current.MainWindow.Left - godmodeWindow.Width;
            godmodeWindow.Top = Application.Current.MainWindow.Top;
            autoWindow.Left = Application.Current.MainWindow.Left - autoWindow.Width;
            autoWindow.Top = Application.Current.MainWindow.Top + godmodeWindow.Height;
            mobcontrolWindow.Left = Application.Current.MainWindow.Left - mobcontrolWindow.Width;
            mobcontrolWindow.Top = Application.Current.MainWindow.Top;
            performanceWindow.Left = Application.Current.MainWindow.Left + mainWindow.Width;
            performanceWindow.Top = Application.Current.MainWindow.Top + mainWindow.Height - performanceWindow.Height;
            characterWindow.Top = Application.Current.MainWindow.Top + mainWindow.Height - characterWindow.Height;
            characterWindow.Left = Application.Current.MainWindow.Left - mainWindow.Width;
        }

        private void autosRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            autoWindow.Left = Application.Current.MainWindow.Left;
            autoWindow.Top = Application.Current.MainWindow.Top + godmodeWindow.Height;
            autoWindow.Visibility = (autoWindow.IsVisible) ? Visibility.Hidden : Visibility.Visible;
        }

        private void mobControlRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mobcontrolWindow.Left = Application.Current.MainWindow.Left;
            mobcontrolWindow.Top = Application.Current.MainWindow.Top + godmodeWindow.Height;
            mobcontrolWindow.Visibility = (mobcontrolWindow.IsVisible) ? Visibility.Hidden : Visibility.Visible;
        }

        private void mobControlCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            mobcontrolWindow.EnableHacks();
        }

        private void mobControlCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            mobcontrolWindow.DisableHacks();
        }

        private void performanceWindowRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            performanceWindow.Left = Application.Current.MainWindow.Left;
            performanceWindow.Top = Application.Current.MainWindow.Top + godmodeWindow.Height;
            performanceWindow.Visibility = (performanceWindow.IsVisible) ? Visibility.Hidden : Visibility.Visible;
        }

        private void performanceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            performanceWindow.EnableHacks();
        }

        private void performanceCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            performanceWindow.DisableHacks();
        }


        private void characterWindowRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            characterWindow.Left = Application.Current.MainWindow.Left;
            characterWindow.Top = Application.Current.MainWindow.Top + godmodeWindow.Height;
            characterWindow.Visibility = (characterWindow.IsVisible) ? Visibility.Hidden : Visibility.Visible;
        }

        private void characterCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            characterWindow.EnableHacks();
        }

        private void characterCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            characterWindow.DisableHacks();
        }

        public void autosCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            autoWindow.EnableHacks();
        }

        private void autosCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            autoWindow.DisableHacks();
        }



        /**
        
        faceleft
        fullmapattack
        fullmaploot
        itemfilter
        jumpdownanywhere
        nodelayflashjump
        skillinjectiongnd
        slideandattack
        spawnpointcontrol
        tubinolootanimation
        randomhwid


        */
    }
}
