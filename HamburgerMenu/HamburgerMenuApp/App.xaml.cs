using HamburgerMenuApp.Utils;
using HamburgerMenuApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace HamburgerMenuApp
{
    /// <summary>
    /// アプリ起動時処理クラス
    /// </summary>
    sealed partial class App : Application
    {
        private Frame m_rootFrame;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// アプリケーションがエンド ユーザーによって正常に起動されたときに呼び出されます。他のエントリ ポイントは、
        /// アプリケーションが特定のファイルを開くために起動されたときなどに使用されます。
        /// </summary>
        /// <param name="e">起動の要求とプロセスの詳細を表示します。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
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
                m_rootFrame.NavigationFailed += OnNavigationFailed;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 休止からの復旧データがあれば読み込み
                }
            }
            if (m_rootFrame.Content == null)
            {
                m_rootFrame.Navigate(typeof(AppShell), e.Arguments);
            }
            Window.Current.Content = m_rootFrame;
            
            //アクティブにする
            Window.Current.Activate();
        }

        /// <summary>
        /// 特定のページへの移動が失敗したときに呼び出されます
        /// </summary>
        /// <param name="sender">移動に失敗したフレーム</param>
        /// <param name="e">ナビゲーション エラーの詳細</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("ページ遷移処理失敗 " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// アプリケーションの実行が中断されたときに呼び出されます。
        /// アプリケーションが終了されるか、メモリの内容がそのままで再開されるかに
        /// かかわらず、アプリケーションの状態が保存されます。
        /// </summary>
        /// <param name="sender">中断要求の送信元。</param>
        /// <param name="e">中断要求の詳細。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            //TODO: 休止時はここでデータを保存する

            deferral.Complete();
        }
    }
}
