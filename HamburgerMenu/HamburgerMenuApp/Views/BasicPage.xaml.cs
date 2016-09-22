using HamburgerMenuApp.ViewModels;
using System;
using System.Collections.Generic;
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

namespace HamburgerMenuApp.Views
{
    /// <summary>
    /// コンテンツのみ表示するページ
    /// </summary>
    public sealed partial class BasicPage : Page
    {
        public BasicPageViewModel ViewModel
        {
            get { return this.DataContext as BasicPageViewModel; }
        }

        public BasicPage()
        {
            this.InitializeComponent();
            this.DataContext = new BasicPageViewModel();
        }
    }
}
