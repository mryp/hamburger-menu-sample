using HamburgerMenuApp.Controls;
using HamburgerMenuApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HamburgerMenuApp
{

    /// <summary>
    /// ナビゲーションメニューを表示する親ページ
    /// </summary>
    public sealed partial class AppShell : Page
    {
        /// <summary>
        /// ナビゲーションメニューリスト
        /// </summary>
        private List<NavMenuItem> navlist = new List<NavMenuItem>(
            new[]
            {
                new NavMenuItem()
                {
                    Symbol = Symbol.Contact,
                    Label = "コンテンツのみビュー",
                    DestPage = typeof(BasicPage)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Edit,
                    Label = "コマンドバーありビュー",
                    DestPage = typeof(CommandBarPage)
                },
                /*
                new NavMenuItem()
                {
                    Symbol = Symbol.Favorite,
                    Label = "Drill In Page",
                    DestPage = typeof(BasicPage)
                },
                */
            });

        /// <summary>
        /// インスタンス取得オブジェクト
        /// </summary>
        public static AppShell Current = null;

        /// <summary>
        /// メインコンテンツ部のフレームオブジェクト
        /// </summary>
        public Frame AppFrame
        {
            get { return this.frame; }
        }

        /// <summary>
        /// メニューの表示領域
        /// </summary>
        public Rect TogglePaneButtonRect
        {
            get;
            private set;
        }

        /// <summary>
        /// ナビゲーションメニュー開閉イベント
        /// </summary>
        public event TypedEventHandler<AppShell, Rect> TogglePaneButtonRectChanged;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AppShell()
        {
            this.InitializeComponent();

            //起動時の初期化
            this.Loaded += (sender, args) =>
            {
                Current = this;

                this.CheckTogglePaneButtonSizeChanged();
            };

            //ディスプレイモード変更イベント発生時
            this.RootSplitView.RegisterPropertyChangedCallback(SplitView.DisplayModeProperty, (s, a) => {
                this.CheckTogglePaneButtonSizeChanged();
            });

            //タイトルバーの戻るボタン処理
            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            
            //ナビゲーションメニュー一覧設定
            NavMenuList.ItemsSource = navlist;
            NavMenuList.SelectedIndex = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
            if (this.AppFrame.Content == null)
            {
                //初期画面設定
                this.AppFrame.Navigate(typeof(BasicPage), null, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
            }
        }

        /// <summary>
        /// キーボードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppShell_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            FocusNavigationDirection direction = FocusNavigationDirection.None;
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                case Windows.System.VirtualKey.GamepadLeftThumbstickLeft:
                case Windows.System.VirtualKey.NavigationLeft:
                    direction = FocusNavigationDirection.Left;
                    break;
                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.GamepadDPadRight:
                case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                case Windows.System.VirtualKey.NavigationRight:
                    direction = FocusNavigationDirection.Right;
                    break;
                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.GamepadDPadUp:
                case Windows.System.VirtualKey.GamepadLeftThumbstickUp:
                case Windows.System.VirtualKey.NavigationUp:
                    direction = FocusNavigationDirection.Up;
                    break;
                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                case Windows.System.VirtualKey.NavigationDown:
                    direction = FocusNavigationDirection.Down;
                    break;
            }

            if (direction != FocusNavigationDirection.None)
            {
                //フォーカス変更
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                if (control != null)
                {
                    control.Focus(FocusState.Keyboard);
                    e.Handled = true;
                }
            }
        }

        #region BackRequested Handlers

        /// <summary>
        /// 戻るボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            bool handled = e.Handled;
            this.BackRequested(ref handled);
            e.Handled = handled;
        }

        /// <summary>
        /// 戻るボタン押下処理
        /// </summary>
        /// <param name="handled">処理を行ったかどうか</param>
        private void BackRequested(ref bool handled)
        {
            if (this.AppFrame == null)
            {
                return;
            }

            if (this.AppFrame.CanGoBack && !handled)
            {
                //戻れるときはコンテンツフレームのほうを戻す
                handled = true;
                this.AppFrame.GoBack();
            }
        }

        #endregion

        #region Navigation

        /// <summary>
        /// メニューアイテムを選択したとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listViewItem"></param>
        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            foreach (var i in navlist)
            {
                i.IsSelected = false;
            }

            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);
            if (item != null)
            {
                item.IsSelected = true;
                if (item.DestPage != null &&
                    item.DestPage != this.AppFrame.CurrentSourcePageType)
                {
                    this.AppFrame.Navigate(item.DestPage, item.Arguments);
                }
            }
        }

        /// <summary>
        /// コンテンツフレームで画面遷移を行うときのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                //違うページを探す
                var item = (from p in this.navlist where p.DestPage == e.SourcePageType select p).SingleOrDefault();
                if (item == null && this.AppFrame.BackStackDepth > 0)
                {
                    foreach (var entry in this.AppFrame.BackStack.Reverse())
                    {
                        item = (from p in this.navlist where p.DestPage == entry.SourcePageType select p).SingleOrDefault();
                        if (item != null)
                        {
                            break;
                        }
                    }
                }

                //選択状態を切り替える
                foreach (var i in navlist)
                {
                    i.IsSelected = false;
                }
                if (item != null)
                {
                    item.IsSelected = true;
                }

                //選択切り替え
                var container = (ListViewItem)NavMenuList.ContainerFromItem(item);
                if (container != null) container.IsTabStop = false;
                NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            }
        }

        #endregion


        /// <summary>
        /// メニューを開く
        /// </summary>
        public void OpenNavePane()
        {
            TogglePaneButton.IsChecked = true;
        }

        /// <summary>
        /// メニューが閉じられたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void RootSplitView_PaneClosed(SplitView sender, object args)
        {
        }

        /// <summary>
        /// メニューを閉じるときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        /// メニューを開くときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Checked(object sender, RoutedEventArgs e)
        {
            this.CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        /// メニューの開閉とディスプレイによって表示状態を決定する
        /// </summary>
        private void CheckTogglePaneButtonSizeChanged()
        {
            if (this.RootSplitView.DisplayMode == SplitViewDisplayMode.Inline ||
                this.RootSplitView.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                var transform = this.TogglePaneButton.TransformToVisual(this);
                var rect = transform.TransformBounds(new Rect(0, 0, this.TogglePaneButton.ActualWidth, this.TogglePaneButton.ActualHeight));
                this.TogglePaneButtonRect = rect;
            }
            else
            {
                this.TogglePaneButtonRect = new Rect();
            }

            var handler = this.TogglePaneButtonRectChanged;
            if (handler != null)
            {
                handler.DynamicInvoke(this, this.TogglePaneButtonRect);
            }
        }

        /// <summary>
        /// ナビゲーションメニューに表示する項目変更イベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavMenuItemContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is NavMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((NavMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }
    }
}
