﻿using HamburgerMenuPrism.Utils;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HamburgerMenuPrism.Views
{
    /// <summary>
    /// コマンドバーを表示するページ
    /// </summary>
    public sealed partial class CommandBarPage : SessionStateAwarePage
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandBarPage()
        {
            this.InitializeComponent();
            this.Loaded += CommandBarPage_Loaded;
        }

        /// <summary>
        /// 画面表示時イベント
        /// </summary>
        private void CommandBarPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemInformationHelpers.IsSmartPhoneDisplaySize)
            {
                mainCommandBar.Visibility = Visibility.Collapsed;
                titleOnlyBar.Visibility = Visibility.Visible;
                bottomCommandBar.Visibility = Visibility.Visible;
            }
            else
            {
                mainCommandBar.Visibility = Visibility.Visible;
                titleOnlyBar.Visibility = Visibility.Collapsed;
                bottomCommandBar.Visibility = Visibility.Collapsed;
            }
        }
    }
}
