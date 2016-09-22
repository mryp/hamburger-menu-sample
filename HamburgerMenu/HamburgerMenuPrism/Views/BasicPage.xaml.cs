using HamburgerMenuPrism.ViewModels;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    /// コンテンツのみ表示するページ
    /// </summary>
    public sealed partial class BasicPage : SessionStateAwarePage
    {
        public BasicPage()
        {
            this.InitializeComponent();
        }

        private void ButtonAppFrame_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(typeof(BasicPageDetail));
        }

        private void ButtonFrame_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.Frame.Navigate(typeof(BasicPageDetail));
        }
    }
}
