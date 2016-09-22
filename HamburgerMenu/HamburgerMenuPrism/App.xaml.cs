using HamburgerMenuPrism.Utils;
using HamburgerMenuPrism.Views;
using Prism.Windows;
using Prism.Windows.Navigation;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HamburgerMenuPrism
{
    /// <summary>
    /// アプリ起動時処理クラス
    /// </summary>
    sealed partial class App : PrismApplication
    {
        private Frame m_rootFrame;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = new AppShell();
            return shell;
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            //アプリ最小サイズ設定
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 200));

            //タイトルバーの色設定
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (titleBar != null)
            {
                Color titleBarColor = (Color)App.Current.Resources["SystemAltHighColor"];
                titleBar.BackgroundColor = titleBarColor;
                titleBar.ButtonBackgroundColor = titleBarColor;
                titleBar.InactiveBackgroundColor = titleBarColor;
                titleBar.ButtonInactiveBackgroundColor = titleBarColor;
            }

            //10フィート画面の時は別のスタイルを適用する
            if (SystemInformationHelpers.IsTenFootExperience)
            {
                // Apply guidance from https://msdn.microsoft.com/windows/uwp/input-and-devices/designing-for-tv
                ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
                this.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("ms-appx:///Styles/TenFootStylesheet.xaml")
                });
            }
            
            //フレームを生成
            m_rootFrame = Window.Current.Content as Frame;
            if (m_rootFrame == null)
            {
                
                m_rootFrame = new Frame();
                //m_rootFrame.NavigationFailed += OnNavigationFailed;
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 休止からの復旧データがあれば読み込み
                }
            }
            if (m_rootFrame.Content == null)
            {
                m_rootFrame.Navigate(typeof(AppShell), args.Arguments);
            }
            Window.Current.Content = m_rootFrame;
            
            //アクティブにする
            Window.Current.Activate();

            return Task.FromResult(true);
        }
    }
}
